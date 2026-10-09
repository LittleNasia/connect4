namespace Connect4.Interop;

public static class Player
{
    public const int None = 0;
    public const int Red = 1;
    public const int Yellow = 2;
}

public static class Board
{
    public const int Rows = 6;
    public const int Columns = 7;
}

public interface IConnect4Engine
{
    bool MakeMove(int side, int col);

    int[][] GetPlayfield();

    void NewGame();

    int GetWinner();

    int GetSideToMove();
}
