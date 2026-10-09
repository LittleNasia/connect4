using System.Runtime.InteropServices;

namespace Connect4.Interop;

public sealed class NativeConnect4Engine : IConnect4Engine
{
    public bool MakeMove(int side, int col) => Native.MakeMove(side, col);

    public int[][] GetPlayfield()
    {
        var cells = new int[Board.Rows * Board.Columns];
        Native.GetPlayfield(cells);

        var rows = new int[Board.Rows][];
        for (var row = 0; row < Board.Rows; row++)
        {
            rows[row] = new int[Board.Columns];
            Array.Copy(cells, row * Board.Columns, rows[row], 0, Board.Columns);
        }
        return rows;
    }

    public void NewGame() => Native.NewGame();

    public int GetWinner() => Native.GetWinner();

    public int GetSideToMove() => Native.GetSideToMove();

    private static class Native
    {
        private const string Dll = "Connect4Engine";

        [DllImport(Dll, CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.U1)]
        public static extern bool MakeMove(int side, int col);

        [DllImport(Dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern void GetPlayfield([Out] int[] cells);

        [DllImport(Dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern void NewGame();

        [DllImport(Dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern int GetWinner();

        [DllImport(Dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern int GetSideToMove();
    }
}
