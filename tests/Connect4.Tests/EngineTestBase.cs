using Connect4.Interop;

namespace Connect4.Tests;

public abstract class EngineTestBase
{
    protected IConnect4Engine Engine = null!;

    [SetUp]
    public void StartNewGame()
    {
        Engine = new NativeConnect4Engine();
        Engine.NewGame();
    }

    protected void Play(string columns)
    {
        var side = Player.Red;
        foreach (var token in columns.Split(' ', StringSplitOptions.RemoveEmptyEntries))
        {
            var column = int.Parse(token);
            Assert.That(Engine.MakeMove(side, column), Is.True, $"setup move of side {side} in column {column} was rejected");
            side = side == Player.Red ? Player.Yellow : Player.Red;
        }
    }

    protected int[] Cells() => Engine.GetPlayfield().SelectMany(row => row).ToArray();

    protected int CellAt(int row, int col) => Engine.GetPlayfield()[row][col];

    protected int PieceCount() => Cells().Count(cell => cell != Player.None);

    protected int PieceCount(int side) => Cells().Count(cell => cell == side);

    protected const int Bottom = Board.Rows - 1;
}
