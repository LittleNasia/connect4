#include "connect4_engine.h"

bool C4_CALL MakeMove(int side, int col)
{
	// random nonsense to disable warning about unused parameters
	bool validMove = side * col;
    return validMove;
}

void C4_CALL GetPlayfield(int* cells)
{
    for (int i = 0; i < PLAYFIELD_ROWS * PLAYFIELD_COLS; ++i)
    {
        cells[i] = CELL_EMPTY;
    }
}

void C4_CALL NewGame()
{
}

int C4_CALL GetWinner()
{
    return 0;
}

int C4_CALL GetSideToMove()
{
    return 0;
}
