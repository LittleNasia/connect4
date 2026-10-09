# Connect 4

This project is a Connect 4 game. It has two parts:

- The **DLL** (`Connect4Engine.dll`) must contain all of the game rules. It is written in C++.
- The **frontend** is a window that shows the playfield. It has no game rules. It is written in C#.

The frontend sends each click to the DLL. Then it asks the DLL for the new state of the game. It draws the answer.

## Terms

| Term | Meaning |
| --- | --- |
| DLL | A file that contains code. Other programs can call the functions in it. |
| Native code | Code that the computer runs directly. The C++ code in the DLL is native code. |
| Managed code | Code that runs inside .NET. .NET looks after the computer memory for the code. The C# code is managed code. |
| .NET | A Microsoft platform that runs C# programs. |
| WPF | A .NET tool that makes windows. The frontend uses it. |
| Playfield | The board. It has 6 rows and 7 columns. |
| Cell | One place in the playfield. It is empty, red, or yellow. |
| Side | A player. The side is red or yellow. |

## Build and run

1. Open `Connect4.slnx` in Visual Studio.
2. Select the `Debug` configuration or the `Release` configuration.
3. Select the `x64` platform. No other platform works.
4. In the Solution Explorer, right-click `Connect4.Wpf`. Select **Set as Startup Project**.
5. Select **Build > Build Solution**. Visual Studio builds the C++ project first. Then it copies `Connect4Engine.dll` next to the frontend and next to the tests.
6. Press **F5** to start the frontend.

Native debugging is on. A breakpoint in the C++ code stops the frontend while you play.

To build from the command line, open a Developer Command Prompt and enter this command:

`msbuild Connect4.slnx -restore -p:Configuration=Release -p:Platform=x64`

Do not use `dotnet build`. It cannot build the C++ project.

## Run the tests

In Visual Studio, select **Test > Test Explorer**. Then select **Run All Tests**.

To run the tests from the command line, build the solution first. Then enter this command:

`dotnet test tests/Connect4.Tests/bin/<Configuration>/net10.0/Connect4.Tests.dll`

Replace `<Configuration>` with `Debug` or `Release`.

## The DLL functions

```cpp
bool MakeMove(int side, int col);
void GetPlayfield(int* cells);
void NewGame(void);
int  GetWinner(void);
int  GetSideToMove(void);
```

Cells, sides, and the winner use the same numbers:

| Number | Meaning |
| --- | --- |
| `0` | Empty cell, or nobody |
| `1` | Red |
| `2` | Yellow |

Each function uses the `__cdecl` calling convention. A calling convention is a rule for how a function receives its data. The C++ code and the C# code must use the same rule. In the header, `C4_CALL` means `__cdecl`. The C# code uses `CallingConvention.Cdecl`, which is the same rule.

The C# code calls the DLL through the class `NativeConnect4Engine`. The C# interface `IConnect4Engine` lists the same functions. In C#, `GetPlayfield` returns `int[][]`. This is an array of rows. Each row is an array of 7 numbers.

## How the frontend works

When the player clicks a column, the frontend does these steps:

1. It calls `MakeMove(side, col)`. The value of `side` is the last value that `GetSideToMove()` returned.
2. It calls `GetPlayfield()`. It draws the pieces.
3. It calls `GetSideToMove()`. It shows this color next to "Next move:".
4. It calls `GetWinner()`. It shows the result at the top of the window.

The frontend does not know the turn order. It only shows what the DLL says.
