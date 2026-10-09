#pragma once

#include <stdbool.h>

#ifdef __cplusplus
extern "C" {
#endif

#if defined(_WIN32)
  #if defined(CONNECT4_ENGINE_EXPORTS)
    #define C4_API __declspec(dllexport)
  #else
    #define C4_API __declspec(dllimport)
  #endif
  #define C4_CALL __cdecl
#else
  #define C4_API
  #define C4_CALL
#endif

enum PlayfieldDimensions
{
    PLAYFIELD_ROWS = 6,
    PLAYFIELD_COLS = 7
};

enum CellState
{
    CELL_EMPTY  = 0,
    CELL_RED    = 1,
    CELL_YELLOW = 2
};

C4_API bool C4_CALL MakeMove(int side, int col);
C4_API void C4_CALL GetPlayfield(int* cells);
C4_API void C4_CALL NewGame();
C4_API int C4_CALL GetWinner();
C4_API int C4_CALL GetSideToMove();

#ifdef __cplusplus
}
#endif
