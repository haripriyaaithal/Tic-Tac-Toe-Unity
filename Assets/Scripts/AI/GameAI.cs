/*using System.Collections;
using UnityEngine;

public class GameAI : MonoBehaviour {

    private int[,] board;
    private UI ui;
    private GameManager gameManager;
    private bool markWin = false;
    private int winScore;

    private bool isFirstTurn = true;

    private void Start() {
        board = new int[3, 3] {
            { GameManager.EMPTY, GameManager.EMPTY, GameManager.EMPTY},
            { GameManager.EMPTY, GameManager.EMPTY, GameManager.EMPTY},
            { GameManager.EMPTY, GameManager.EMPTY, GameManager.EMPTY}
        };

        ui = FindObjectOfType<UI>();
        gameManager = FindObjectOfType<GameManager>();
    }

    /// <summary>
    /// Call this function when AI has to play
    /// </summary>
    public void PlayerTwoTurn() {

       
    }

    private int CheckForWinningMove() {
        for (int row = 0; row < 3; row++) {
            if (Sum(board[row, 0], board[row, 1], board[row, 2]) == (GameManager.PLAYER_TWO + GameManager.PLAYER_TWO) && HasWinningChance("row", row, out int winRow, out int winColumn)) {
                UpdateBoardValues(winRow, winColumn, GameManager.PLAYER_TWO);
                return Map2DArrayToButtonIndex(winRow, winColumn);
            }
        }

        for (int column = 0; column < 3; column++) {
            if (Sum(board[0, column], board[1, column], board[2, column]) == (GameManager.PLAYER_TWO + GameManager.PLAYER_TWO) && HasWinningChance("column", column, out int winRow, out int winColumn)) {
                UpdateBoardValues(winRow, winColumn, GameManager.PLAYER_TWO);
                return Map2DArrayToButtonIndex(winRow, winColumn);
            }
        }

        if (Sum(board[0, 0], board[1, 1], board[2, 2]) == (GameManager.PLAYER_TWO + GameManager.PLAYER_TWO) && HasWinningChance("diagonal1", -1, out int winningRow, out int winningColumn)) {
            UpdateBoardValues(winningRow, winningColumn, GameManager.PLAYER_TWO);
            return Map2DArrayToButtonIndex(winningRow, winningColumn);
        }

        if (Sum(board[0, 2], board[1, 1], board[2, 0]) == (GameManager.PLAYER_TWO + GameManager.PLAYER_TWO) && HasWinningChance("diagonal2", -1, out winningRow, out winningColumn)) {
            UpdateBoardValues(winningRow, winningColumn, GameManager.PLAYER_TWO);
            return Map2DArrayToButtonIndex(winningRow, winningColumn);
        }
        return -1;
    }

    private bool HasWinningChance(string state, int index, out int winRow, out int winColumn) {
        if (state.Equals("row")) {
            for (int column = 0; column < 3; column++) {
                if (board[index, column] == GameManager.EMPTY) {
                    winRow = index;
                    winColumn = column;
                    return true;
                }
            }
        } else if (state.Equals("column")) {
            for (int row = 0; row < 3; row++) {
                if (board[row, index] == GameManager.EMPTY) {
                    winRow = row;
                    winColumn = index;
                    return true;
                }
            }
        } else if (state.Equals("diagonal1")) {
            if (board[0, 0] == GameManager.EMPTY) {
                winRow = 0;
                winColumn = 0;
                return true;
            } else if (board[1, 1] == GameManager.EMPTY) {
                winRow = 1;
                winColumn = 1;
                return true;
            } else if (board[2, 2] == GameManager.EMPTY) {
                winRow = 2;
                winColumn = 2;
                return true;
            }
        } else if (state.Equals("diagonal2")) {
            if (board[0, 2] == GameManager.EMPTY) {
                winRow = 0;
                winColumn = 2;
                return true;
            } else if (board[1, 1] == GameManager.EMPTY) {
                winRow = 1;
                winColumn = 1;
                return true;
            } else if (board[2, 0] == GameManager.EMPTY) {
                winRow = 2;
                winColumn = 0;
                return true;
            }
        }
        winRow = -1;
        winColumn = -1;
        return false;
    }

    private int Sum(int a, int b, int c) {
        return a + b + c;
    }

    *//*IEnumerator DrawWithRandomDelay(int buttonIndex) {
        float seconds = Random.Range(0.1f, 3f);
        yield return new WaitForSeconds(seconds);
        MarkOnBoard(buttonIndex);

        SetMarkWin(true);
        CheckForWins();
        SetMarkWin(false);

        ui.IndicatePlayerTwoTurn(false);
        ui.IndicatePlayerOneTurn(true);
        ui.EnableInput(true, false);
    }*//*

    private void MarkOnBoard(int buttonIndex) {
        ui.DrawOnBoard(buttonIndex, GameManager.PLAYER_TWO);
        gameManager.ChangeTurn();
    }

    private int FindBestMove() {
        int bestValue = int.MinValue;
        int rowSelected = -1;
        int columnSelected = -1;

        for (int row = 0; row < 3; row++) {
            for (int column = 0; column < 3; column++) {
                if (board[row, column] == GameManager.EMPTY) {
                    // Make move
                    board[row, column] = GameManager.PLAYER_TWO;

                    int moveValue = minimax(board, 0, false); // 0 -> depth, false -> maximizing

                    // Undo move
                    board[row, column] = GameManager.EMPTY;

                    if (moveValue > bestValue) {
                        rowSelected = row;
                        columnSelected = column;
                        bestValue = moveValue;
                    }
                }
            }
        }

        UpdateBoardValues(rowSelected, columnSelected, GameManager.PLAYER_TWO);
        return Map2DArrayToButtonIndex(rowSelected, columnSelected);
    }

    /// <summary>
    /// Updates the board array to current game state.
    /// </summary>
    public void UpdateBoardValues(int row, int column, int player) {
        if (row != -1 && column != -1) {
            board[row, column] = player;
        }
    }

    private int Map2DArrayToButtonIndex(int row, int column) {
        return (3 * row) + column;
    }

    private int minimax(int[,] board, int depth, bool isMaximizing) {
        int score = CheckForWins(board);

        // Game Won, 10 -> PlayerTwo, -10 -> PlayerOne
        if (score == 10 || score == -10) {
            return score;
        }

        // Tie
        if (CheckForMovesLeft() == false) {
            return 0;
        }

        if (isMaximizing) {
            int best = int.MinValue;

            for (int row = 0; row < 3; row++) {
                for (int column = 0; column < 3; column++) {
                    if (board[row, column] == GameManager.EMPTY) {
                        // Make move
                        board[row, column] = GameManager.PLAYER_TWO;

                        best = Max(best, minimax(board, depth + 1, !isMaximizing));

                        // Undo move
                        board[row, column] = GameManager.EMPTY;
                    }
                }
            }
            return best;
        } else {
            int best = int.MaxValue;

            for (int row = 0; row < 3; row++) {
                for (int column = 0; column < 3; column++) {
                    if (board[row, column] == GameManager.EMPTY) {
                        // Make move
                        board[row, column] = GameManager.PLAYER_ONE;

                        best = Min(best, minimax(board, depth + 1, !isMaximizing));

                        // Undo move
                        board[row, column] = GameManager.EMPTY;
                    }
                }
            }
            return best;
        }
    }

    private int Min(int best, int newValue) {
        return best < newValue ? best : newValue;
    }

    private int Max(int best, int newValue) {
        return best > newValue ? best : newValue;
    }

    public bool CheckForMovesLeft() {
        for (int row = 0; row < 3; row++) {
            for (int column = 0; column < 3; column++) {
                if (board[row, column] == GameManager.EMPTY) {
                    return true;
                }
            }
        }
        return false;
    }

    /// <summary>
    /// Use this function to check if any player has won or not.
    /// </summary>
    /// <param name="board"></param>
    /// <returns>Returns 10 if AI has won, -10 if player has won, 0 if no winners,</returns>
    public int CheckForWins(int[,] board = null) {
        // Use this board when checking for win, to draw the line on the screen
        if (board == null) {
            board = this.board;
        }

        winScore = CheckForWinRow();
        if (winScore != -1) {
            return winScore;
        }

        winScore = CheckForWinColumn();
        if (winScore != -1) {
            return winScore;
        }

        winScore = CheckForWinDiagonals();
        if (winScore != -1) {
            return winScore;
        }
        // None of them have won.
        return 0;
    }

    private int CheckForWinRow() {
        // Check row for win.
        for (int row = 0; row < 3; row++) {
            if (board[row, 0] == board[row, 1] && board[row, 1] == board[row, 2]) {
                if (board[row, 0] == GameManager.PLAYER_TWO) {
                    if (markWin) {
                        ui.DrawWinLine(row, "row", GameManager.PLAYER_TWO);
                    }
                    return 10;
                } else if (board[row, 0] == GameManager.PLAYER_ONE) {
                    if (markWin) {
                        ui.DrawWinLine(row, "row", GameManager.PLAYER_ONE);
                    }
                    return -10;
                }
            }
        }
        return -1;
    }

    private int CheckForWinColumn() {
        // Check column for win.
        for (int column = 0; column < 3; column++) {
            if (board[0, column] == board[1, column] && board[1, column] == board[2, column]) {
                if (board[0, column] == GameManager.PLAYER_TWO) {
                    if (markWin) {
                        ui.DrawWinLine(column, "column", GameManager.PLAYER_TWO);
                    }
                    return 10;
                } else if (board[0, column] == GameManager.PLAYER_ONE) {
                    if (markWin) {
                        ui.DrawWinLine(column, "column", GameManager.PLAYER_ONE);
                    }
                    return -10;
                }
            }
        }
        return -1;
    }

    private int CheckForWinDiagonals() {
        // Check Diagonal - Left to right
        if (board[0, 0] == board[1, 1] && board[1, 1] == board[2, 2]) {
            if (board[0, 0] == GameManager.PLAYER_TWO) {
                if (markWin) {
                    ui.DrawWinLine(0, "diagonal", GameManager.PLAYER_TWO);
                }
                return 10;
            } else if (board[0, 0] == GameManager.PLAYER_ONE) {
                if (markWin) {
                    ui.DrawWinLine(0, "diagonal", GameManager.PLAYER_ONE);
                }
                return -10;
            }
        }

        // Check Diagonal - Right to left
        if (board[0, 2] == board[1, 1] && board[1, 1] == board[2, 0]) {
            if (board[0, 2] == GameManager.PLAYER_TWO) {
                if (markWin) {
                    ui.DrawWinLine(1, "diagonal", GameManager.PLAYER_TWO);
                }
                return 10;
            } else if (board[0, 2] == GameManager.PLAYER_ONE) {
                if (markWin) {
                    ui.DrawWinLine(1, "diagonal", GameManager.PLAYER_ONE);
                }
                return -10;
            }
        }
        return -1;
    }

    /// <summary>
    /// Draws win lines when set to true
    /// </summary>
    public void SetMarkWin(bool state) {
        markWin = state;
    }
}*/