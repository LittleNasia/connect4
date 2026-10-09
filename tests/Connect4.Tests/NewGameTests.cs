using Connect4.Interop;

namespace Connect4.Tests;

public class NewGameTests : EngineTestBase
{
    [Test]
    public void Playfield_HasSixRowsOfSevenColumns()
    {
        var playfield = Engine.GetPlayfield();

        Assert.That(playfield, Has.Length.EqualTo(6));
        Assert.That(playfield, Has.All.Length.EqualTo(7));
    }

    [Test]
    public void Playfield_IsEmpty()
    {
        Assert.That(Cells(), Is.All.EqualTo(Player.None));
    }

    [Test]
    public void Winner_IsNeutral()
    {
        Assert.That(Engine.GetWinner(), Is.EqualTo(Player.None));
    }

    [Test]
    public void ClearsPiecesOfThePreviousGame()
    {
        Play("3 3 4 4 5");
        Assert.That(PieceCount(), Is.EqualTo(5));

        Engine.NewGame();

        Assert.That(Cells(), Is.All.EqualTo(Player.None));
    }

    [Test]
    public void ResetsTheWinner()
    {
        Play("0 1 0 1 0 1 0");
        Assert.That(Engine.GetWinner(), Is.EqualTo(Player.Red));

        Engine.NewGame();

        Assert.That(Engine.GetWinner(), Is.EqualTo(Player.None));
    }

    [Test]
    public void AllowsMovesAfterAFinishedGame()
    {
        Play("0 1 0 1 0 1 0");

        Engine.NewGame();
        var played = Engine.MakeMove(Player.Red, 3);

        Assert.That(played, Is.True);
        Assert.That(CellAt(Bottom, 3), Is.EqualTo(Player.Red));
    }

    [Test]
    public void GivesTheTurnBackToRed()
    {
        Engine.MakeMove(Player.Red, 3);

        Engine.NewGame();
        var played = Engine.MakeMove(Player.Red, 3);

        Assert.That(played, Is.True);
        Assert.That(CellAt(Bottom, 3), Is.EqualTo(Player.Red));
        Assert.That(PieceCount(), Is.EqualTo(1));
    }

    [Test]
    public void CanBeCalledRepeatedly()
    {
        Engine.NewGame();
        Engine.NewGame();
        Engine.NewGame();

        Assert.That(Cells(), Is.All.EqualTo(Player.None));
        Assert.That(Engine.GetWinner(), Is.EqualTo(Player.None));
    }
}
