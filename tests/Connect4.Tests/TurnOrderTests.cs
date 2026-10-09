using Connect4.Interop;

namespace Connect4.Tests;

public class TurnOrderTests : EngineTestBase
{
    [Test]
    public void YellowCannotMoveFirst()
    {
        var played = Engine.MakeMove(Player.Yellow, 3);

        Assert.That(played, Is.False);
        Assert.That(Cells(), Is.All.EqualTo(Player.None));
    }

    [Test]
    public void RedCanMoveFirst()
    {
        var played = Engine.MakeMove(Player.Red, 3);

        Assert.That(played, Is.True);
        Assert.That(CellAt(Bottom, 3), Is.EqualTo(Player.Red));
    }

    [Test]
    public void SameSideTwiceInARow_SecondMoveIsIgnored()
    {
        Assert.That(Engine.MakeMove(Player.Red, 3), Is.True);
        Assert.That(Engine.MakeMove(Player.Red, 4), Is.False);

        Assert.That(CellAt(Bottom, 3), Is.EqualTo(Player.Red));
        Assert.That(CellAt(Bottom, 4), Is.EqualTo(Player.None));
        Assert.That(PieceCount(), Is.EqualTo(1));
    }

    [Test]
    public void SidesAlternate()
    {
        Play("0 1 2 3");

        Assert.That(CellAt(Bottom, 0), Is.EqualTo(Player.Red));
        Assert.That(CellAt(Bottom, 1), Is.EqualTo(Player.Yellow));
        Assert.That(CellAt(Bottom, 2), Is.EqualTo(Player.Red));
        Assert.That(CellAt(Bottom, 3), Is.EqualTo(Player.Yellow));
    }

    [Test]
    public void IgnoredMove_DoesNotChangeWhoseTurnItIs()
    {
        Assert.That(Engine.MakeMove(Player.Red, 3), Is.True);
        Assert.That(Engine.MakeMove(Player.Red, 4), Is.False);
        Assert.That(Engine.MakeMove(Player.Yellow, 4), Is.True);

        Assert.That(CellAt(Bottom, 4), Is.EqualTo(Player.Yellow));
        Assert.That(PieceCount(), Is.EqualTo(2));
    }
}
