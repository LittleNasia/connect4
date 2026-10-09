using Connect4.Interop;

namespace Connect4.Tests;

public class MakeMoveTests : EngineTestBase
{
    [Test]
    public void FirstPiece_LandsOnTheBottomRow([Values(0, 1, 2, 3, 4, 5, 6)] int col)
    {
        var played = Engine.MakeMove(Player.Red, col);

        Assert.That(played, Is.True);
        Assert.That(CellAt(Bottom, col), Is.EqualTo(Player.Red));
        Assert.That(PieceCount(), Is.EqualTo(1));
    }

    [Test]
    public void Pieces_StackUpInTheSameColumn()
    {
        Play("3 3 3");

        Assert.That(CellAt(5, 3), Is.EqualTo(Player.Red));
        Assert.That(CellAt(4, 3), Is.EqualTo(Player.Yellow));
        Assert.That(CellAt(3, 3), Is.EqualTo(Player.Red));
        Assert.That(PieceCount(), Is.EqualTo(3));
    }

    [Test]
    public void Pieces_InDifferentColumns_EachLandOnTheBottomRow()
    {
        Play("0 6 3");

        Assert.That(CellAt(Bottom, 0), Is.EqualTo(Player.Red));
        Assert.That(CellAt(Bottom, 6), Is.EqualTo(Player.Yellow));
        Assert.That(CellAt(Bottom, 3), Is.EqualTo(Player.Red));
        Assert.That(PieceCount(), Is.EqualTo(3));
    }

    [Test]
    public void Column_CanBeFilledUpToTheTopRow()
    {
        Play("0 0 0 0 0 0");

        Assert.That(CellAt(5, 0), Is.EqualTo(Player.Red));
        Assert.That(CellAt(4, 0), Is.EqualTo(Player.Yellow));
        Assert.That(CellAt(3, 0), Is.EqualTo(Player.Red));
        Assert.That(CellAt(2, 0), Is.EqualTo(Player.Yellow));
        Assert.That(CellAt(1, 0), Is.EqualTo(Player.Red));
        Assert.That(CellAt(0, 0), Is.EqualTo(Player.Yellow));
        Assert.That(PieceCount(), Is.EqualTo(6));
    }

    [Test]
    public void FullColumn_RejectsFurtherPieces()
    {
        Play("0 0 0 0 0 0");
        var before = Cells();

        var played = Engine.MakeMove(Player.Red, 0);

        Assert.That(played, Is.False);
        Assert.That(Cells(), Is.EqualTo(before));
    }

    [Test]
    public void FullColumn_DoesNotUseUpTheTurn()
    {
        Play("0 0 0 0 0 0");

        Assert.That(Engine.MakeMove(Player.Red, 0), Is.False);
        Assert.That(Engine.MakeMove(Player.Red, 1), Is.True);

        Assert.That(CellAt(Bottom, 1), Is.EqualTo(Player.Red));
    }

    [Test]
    public void ColumnOutsideTheBoard_IsIgnored([Values(-1, 7, 100, int.MinValue, int.MaxValue)] int col)
    {
        var played = Engine.MakeMove(Player.Red, col);

        Assert.That(played, Is.False);
        Assert.That(Cells(), Is.All.EqualTo(Player.None));
    }

    [Test]
    public void ColumnOutsideTheBoard_DoesNotUseUpTheTurn()
    {
        Assert.That(Engine.MakeMove(Player.Red, 7), Is.False);
        Assert.That(Engine.MakeMove(Player.Red, 3), Is.True);

        Assert.That(CellAt(Bottom, 3), Is.EqualTo(Player.Red));
    }

    [Test]
    public void UnknownSide_IsIgnored([Values(0, 3, -1, 42)] int side)
    {
        var played = Engine.MakeMove(side, 3);

        Assert.That(played, Is.False);
        Assert.That(Cells(), Is.All.EqualTo(Player.None));
    }

    [Test]
    public void UnknownSide_DoesNotUseUpTheTurn()
    {
        Assert.That(Engine.MakeMove(0, 3), Is.False);
        Assert.That(Engine.MakeMove(Player.Red, 3), Is.True);

        Assert.That(CellAt(Bottom, 3), Is.EqualTo(Player.Red));
    }

    [Test]
    public void PlayfieldIsOnlyChangedByTheMove()
    {
        Play("2 4");
        var before = Cells();

        var played = Engine.MakeMove(Player.Red, 4);
        var after = Cells();

        Assert.That(played, Is.True);
        var changed = Enumerable.Range(0, before.Length).Where(i => before[i] != after[i]).ToArray();
        Assert.That(changed, Has.Length.EqualTo(1));
        Assert.That(after[changed[0]], Is.EqualTo(Player.Red));
        Assert.That(changed[0], Is.EqualTo(4 * Board.Columns + 4));
    }
}
