using System.Windows.Input;
using Connect4.Interop;

namespace Connect4.Wpf.ViewModels;

public sealed class MainViewModel : ViewModelBase
{
    private readonly IConnect4Engine _engine;
    private int _sideToMove;
    private string _statusText = "";

    public MainViewModel(IConnect4Engine engine)
    {
        _engine = engine;
        Columns = Enumerable.Range(0, Board.Columns).Select(c => new ColumnViewModel(c, Board.Rows)).ToArray();
        DropCommand = new RelayCommand<int>(Drop);
        NewGameCommand = new RelayCommand(NewGame);

        NewGame();
    }

    public IReadOnlyList<ColumnViewModel> Columns { get; }

    public ICommand DropCommand { get; }

    public ICommand NewGameCommand { get; }

    public string StatusText
    {
        get => _statusText;
        private set => Set(ref _statusText, value);
    }

    public int SideToMove
    {
        get => _sideToMove;
        private set
        {
            if (Set(ref _sideToMove, value))
            {
                OnPropertyChanged(nameof(SideToMoveText));
            }
        }
    }

    public string SideToMoveText => _sideToMove switch
    {
        Player.Red => "Red",
        Player.Yellow => "Yellow",
        _ => "-",
    };

    private void Drop(int col) => Guarded(() =>
    {
        _engine.MakeMove(_sideToMove, col);
        Refresh();
    });

    private void NewGame() => Guarded(() =>
    {
        _engine.NewGame();
        Refresh();
    });

    private void Refresh()
    {
        var playfield = _engine.GetPlayfield();
        for (var col = 0; col < Board.Columns; col++)
        {
            for (var row = 0; row < Board.Rows; row++)
            {
                Columns[col].Cells[row].Owner = playfield[row][col];
            }
        }

        SideToMove = _engine.GetSideToMove();

        StatusText = _engine.GetWinner() switch
        {
            Player.Red => "Red wins!",
            Player.Yellow => "Yellow wins!",
            _ => "No winner yet",
        };
    }

    private void Guarded(Action action)
    {
        try
        {
            action();
        }
        catch (Exception ex) when (ex is DllNotFoundException or BadImageFormatException or EntryPointNotFoundException)
        {
            StatusText = $"Cannot use Connect4Engine.dll: {ex.Message}";
        }
    }
}
