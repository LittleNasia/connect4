namespace Connect4.Wpf.ViewModels;

public sealed class CellViewModel : ViewModelBase
{
    private int _owner;

    public int Owner
    {
        get => _owner;
        set => Set(ref _owner, value);
    }
}

public sealed class ColumnViewModel(int index, int rows)
{
    public int Index { get; } = index;

    public IReadOnlyList<CellViewModel> Cells { get; } =
        Enumerable.Range(0, rows).Select(_ => new CellViewModel()).ToArray();
}
