using Connect4.Interop;

namespace Connect4.Tests;

public class SideToMoveTests : EngineTestBase
{
    [Test]
    public void NewGame_RedMovesFirst()
    {
        Assert.That(Engine.GetSideToMove(), Is.EqualTo(Player.Red));
    }

    [Test]
    public void SideChangesAfterEveryMove()
    {
        Engine.MakeMove(Player.Red, 0);
        Assert.That(Engine.GetSideToMove(), Is.EqualTo(Player.Yellow));

        Engine.MakeMove(Player.Yellow, 0);
        Assert.That(Engine.GetSideToMove(), Is.EqualTo(Player.Red));

        Engine.MakeMove(Player.Red, 1);
        Assert.That(Engine.GetSideToMove(), Is.EqualTo(Player.Yellow));
    }

    [Test]
    public void IgnoredMoves_DoNotChangeTheSide()
    {
        Play("3");

        Assert.That(Engine.MakeMove(Player.Red, 4), Is.False);
        Assert.That(Engine.MakeMove(Player.Yellow, 7), Is.False);
        Assert.That(Engine.MakeMove(Player.Yellow, -1), Is.False);
        Assert.That(Engine.MakeMove(Player.None, 4), Is.False);

        Assert.That(Engine.GetSideToMove(), Is.EqualTo(Player.Yellow));
    }

    [Test]
    public void MoveInAFullColumn_DoesNotChangeTheSide()
    {
        Play("0 0 0 0 0 0");
        Assert.That(Engine.GetSideToMove(), Is.EqualTo(Player.Red));

        Assert.That(Engine.MakeMove(Player.Red, 0), Is.False);

        Assert.That(Engine.GetSideToMove(), Is.EqualTo(Player.Red));
    }

    [Test]
    public void NewGame_GivesTheTurnBackToRed()
    {
        Play("3");
        Assert.That(Engine.GetSideToMove(), Is.EqualTo(Player.Yellow));

        Engine.NewGame();

        Assert.That(Engine.GetSideToMove(), Is.EqualTo(Player.Red));
    }

    [Test]
    public void AfterAWin_NobodyIsToMove()
    {
        Play("0 1 0 1 0 1 0");
        Assert.That(Engine.GetWinner(), Is.EqualTo(Player.Red));

        Assert.That(Engine.GetSideToMove(), Is.EqualTo(Player.None));
    }

    [Test]
    public void NewGame_AfterAWin_RedMovesAgain()
    {
        Play("0 1 0 1 0 1 0");

        Engine.NewGame();

        Assert.That(Engine.GetSideToMove(), Is.EqualTo(Player.Red));
    }

    [Test]
    public void ReportedSide_IsAlwaysAcceptedByMakeMove([Values(0, 1, 2, 3, 4, 5)] int moves)
    {
        for (var col = 0; col < moves; col++)
        {
            Assert.That(Engine.MakeMove(Engine.GetSideToMove(), col), Is.True, $"move {col + 1}");
        }

        var side = Engine.GetSideToMove();

        Assert.That(side, Is.AnyOf(Player.Red, Player.Yellow));
        Assert.That(Engine.MakeMove(side, 6), Is.True);
    }
}
