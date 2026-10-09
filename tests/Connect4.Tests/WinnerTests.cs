using Connect4.Interop;

namespace Connect4.Tests;

public class WinnerTests : EngineTestBase
{
    private const string DrawnGame =
        "0 2 2 0 0 2 2 0 0 2 2 0 " +
        "1 3 3 1 1 3 3 1 1 3 3 1 " +
        "4 6 6 4 4 6 6 4 4 6 6 4 " +
        "5 5 5 5 5 5";

    [Test]
    public void EmptyBoard_HasNoWinner()
    {
        Assert.That(Engine.GetWinner(), Is.EqualTo(Player.None));
    }

    [TestCase("0 6 1 6 2", TestName = "ThreeInARowHorizontally_IsNotAWin")]
    [TestCase("0 1 0 1 0", TestName = "ThreeInARowVertically_IsNotAWin")]
    [TestCase("0 2 1 4 3", TestName = "HorizontalFourBrokenByTheOpponent_IsNotAWin")]
    [TestCase("0 1 1 2 3 2 2", TestName = "ThreeInARowDiagonally_IsNotAWin")]
    public void NoWinner(string moves)
    {
        Play(moves);

        Assert.That(Engine.GetWinner(), Is.EqualTo(Player.None));
    }

    [TestCase("0 0 1 1 2 2 3", Player.Red, TestName = "RedWinsHorizontally_OnTheBottomRow")]
    [TestCase("6 0 6 1 5 2 5 3", Player.Yellow, TestName = "YellowWinsHorizontally_OnTheBottomRow")]
    [TestCase("0 1 2 3 0 4 1 4 2 4 3", Player.Red, TestName = "RedWinsHorizontally_OnTheSecondRow")]
    [TestCase("0 0 1 1 3 3 4 4 2", Player.Red, TestName = "RedWinsWithFiveInARow_ByFillingTheGap")]
    [TestCase("0 1 0 1 0 1 0", Player.Red, TestName = "RedWinsVertically")]
    [TestCase("0 1 0 1 0 1 2 1", Player.Yellow, TestName = "YellowWinsVertically")]
    [TestCase("0 1 1 2 3 2 2 3 6 3 3", Player.Red, TestName = "RedWinsDiagonally_RisingToTheRight")]
    [TestCase("6 5 5 4 3 4 4 3 0 3 3", Player.Red, TestName = "RedWinsDiagonally_RisingToTheLeft")]
    [TestCase("6 0 1 1 2 3 2 2 3 6 3 3", Player.Yellow, TestName = "YellowWinsDiagonally_RisingToTheRight")]
    [TestCase("0 6 5 5 4 3 4 4 3 0 3 3", Player.Yellow, TestName = "YellowWinsDiagonally_RisingToTheLeft")]
    public void Win(string moves, int expectedWinner)
    {
        Play(moves);

        Assert.That(Engine.GetWinner(), Is.EqualTo(expectedWinner));
    }

    [Test]
    public void NoWinnerYet_UntilTheFinalMove()
    {
        Play("0 1 1 2 3 2 2 3 6 3");
        Assert.That(Engine.GetWinner(), Is.EqualTo(Player.None));

        var played = Engine.MakeMove(Player.Red, 3);

        Assert.That(played, Is.True);
        Assert.That(Engine.GetWinner(), Is.EqualTo(Player.Red));
    }

    [Test]
    public void MovesAfterTheWin_AreIgnored()
    {
        Play("0 1 0 1 0 1 0");
        var before = Cells();

        var played = Engine.MakeMove(Player.Yellow, 3);

        Assert.That(played, Is.False);
        Assert.That(Cells(), Is.EqualTo(before));
        Assert.That(Engine.GetWinner(), Is.EqualTo(Player.Red));
    }

    [Test]
    public void TheWinner_CannotBeChangedByTheLoserKeepingOnPlaying()
    {
        Play("0 1 0 1 0 1 0");

        Assert.That(Engine.MakeMove(Player.Yellow, 1), Is.False);
        Assert.That(Engine.MakeMove(Player.Yellow, 1), Is.False);
        Assert.That(Engine.MakeMove(Player.Red, 2), Is.False);

        Assert.That(Engine.GetWinner(), Is.EqualTo(Player.Red));
        Assert.That(PieceCount(), Is.EqualTo(7));
    }

    [Test]
    public void FullBoardWithoutFourInARow_HasNoWinner()
    {
        Play(DrawnGame);

        Assert.That(Cells(), Is.All.Not.EqualTo(Player.None));
        Assert.That(PieceCount(Player.Red), Is.EqualTo(21));
        Assert.That(PieceCount(Player.Yellow), Is.EqualTo(21));
        Assert.That(Engine.GetWinner(), Is.EqualTo(Player.None));
    }

    [Test]
    public void FullBoard_RejectsEveryFurtherMove([Values(0, 1, 2, 3, 4, 5, 6)] int col)
    {
        Play(DrawnGame);
        var before = Cells();

        Assert.That(Engine.MakeMove(Player.Red, col), Is.False);
        Assert.That(Engine.MakeMove(Player.Yellow, col), Is.False);

        Assert.That(Cells(), Is.EqualTo(before));
    }
}
