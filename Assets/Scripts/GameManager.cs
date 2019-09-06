using UnityEngine;

public class GameManager : MonoBehaviour {

    [SerializeField] private WaitingTextAnimator m_waitingAnimator;

    public readonly static int EMPTY = 0;
    public readonly static int PLAYER_ONE = 1;
    public readonly static int PLAYER_TWO = 2;

    private int m_turn = PLAYER_ONE;
    private bool m_isPlayerOne = false; // This value will be set by server
    private int[,] m_board;
    private int m_winScore;

    private int m_currentPlayer = -1;

    private UI ui;
    private NetworkManager networkManager;

    private void Start() {
        m_board = new int[3, 3] {
            { EMPTY, EMPTY, EMPTY },
            { EMPTY, EMPTY, EMPTY },
            { EMPTY, EMPTY, EMPTY }
        };

        ui = FindObjectOfType<UI>();
        networkManager = FindObjectOfType<NetworkManager>();
    }

    public void SetIsPlayerOne(bool state) {
        m_isPlayerOne = state;
        SetPlayerNumber();
    }

    private void SetPlayerNumber() {
        if (m_isPlayerOne) {
            m_currentPlayer = PLAYER_ONE;
        } else {
            m_currentPlayer = PLAYER_TWO;
        }
    }

    private void DrawAndUpdate(int buttonIndex) {
        ui.DrawOnBoard(buttonIndex, m_turn);
        Vector2 rowColumn = MapButtonIndexTo2DArray(buttonIndex);
        int row = (int)rowColumn.x;
        int column = (int)rowColumn.y;
        UpdateBoardValues(row, column, m_turn);
    }

    /// <summary>
    /// Registers user input on the matrix
    /// </summary>
    public void UserInput(int buttonIndex) {
        Debug.Log("TURN: " + m_turn);
        DrawAndUpdate(buttonIndex);

        if (m_turn == m_currentPlayer) {
            Debug.Log("Calling sendInput()");
            networkManager.SendInput(buttonIndex);
        }

        int state = CheckForWinners();

        if (state == 0) {
            // Instruct other player to play
            if (m_turn == PLAYER_ONE) {

                if (m_currentPlayer == PLAYER_ONE) {
                    ui.IndicateWaitingForInput(true);
                    ui.IndicatePlayerTurn(false);
                } else {
                    ui.IndicatePlayerTurn(true);
                    ui.IndicateWaitingForInput(false);
                }

            } else {

                if (m_currentPlayer == PLAYER_TWO) {
                    ui.IndicateWaitingForInput(false);
                    ui.IndicatePlayerTurn(true);
                } else {
                    ui.IndicateWaitingForInput(true);
                    ui.IndicatePlayerTurn(false);
                }

            }

            // Disable input from other player
            if (m_currentPlayer == m_turn) {
                ui.EnableInput(false, false);
            } else {
                ui.EnableInput(true, false);
            }
        }
        ChangeTurn();
    }

    private int CheckForWinners() {
        int score = CheckForWins();
        if (score == 10) {
            // PlayerOne won
            ui.IndicatePlayerTurn(false);
            ui.IndicateWaitingForInput(false);
            return 1;
        } else if (score == -10) {
            // PlayerTwo won
            ui.IndicatePlayerTurn(false);
            ui.IndicateWaitingForInput(false);
            return -1;
        }

        if (!CheckForMovesLeft()) {
            ui.IndicatePlayerTurn(false);
            ui.IndicateWaitingForInput(false);
            ui.ShowDraw();
            return -2;
        }
        return 0;
    }

    public Vector2 MapButtonIndexTo2DArray(int buttonIndex) {
        int row = buttonIndex / 3;
        int column = buttonIndex - (3 * row);
        return new Vector2(row, column);
    }

    public void ChangeTurn() {
        Debug.Log("m_turn before increment: " + m_turn);
        m_turn = (m_turn % 2) + 1;
        Debug.Log("m_turn after increment: " + m_turn);
    }

    /// <summary>
    /// Use this function to check if any player has won or not.
    /// </summary>
    /// <param name="board"></param>
    /// <returns>Returns 10 if AI has won, -10 if player has won, 0 if no winners,</returns>
    public int CheckForWins(int[,] board = null) {
        // Use this board when checking for win, to draw the line on the screen
        if (board == null) {
            board = this.m_board;
        }

        m_winScore = CheckForWinRow();
        if (m_winScore != -1) {
            return m_winScore;
        }

        m_winScore = CheckForWinColumn();
        if (m_winScore != -1) {
            return m_winScore;
        }

        m_winScore = CheckForWinDiagonals();
        if (m_winScore != -1) {
            return m_winScore;
        }
        // None of them have won.
        return 0;
    }

    private int CheckForWinRow() {
        // Check row for win.
        for (int row = 0; row < 3; row++) {
            if (m_board[row, 0] == m_board[row, 1] && m_board[row, 1] == m_board[row, 2]) {
                if (m_board[row, 0] == PLAYER_TWO) {
                    ui.DrawWinLine(row, "row", PLAYER_TWO);
                    return 10;
                } else if (m_board[row, 0] == PLAYER_ONE) {
                    ui.DrawWinLine(row, "row", PLAYER_ONE);
                    return -10;
                }
            }
        }
        return -1;
    }

    private int CheckForWinColumn() {
        // Check column for win.
        for (int column = 0; column < 3; column++) {
            if (m_board[0, column] == m_board[1, column] && m_board[1, column] == m_board[2, column]) {
                if (m_board[0, column] == GameManager.PLAYER_TWO) {
                    ui.DrawWinLine(column, "column", GameManager.PLAYER_TWO);
                    return 10;
                } else if (m_board[0, column] == GameManager.PLAYER_ONE) {
                    ui.DrawWinLine(column, "column", GameManager.PLAYER_ONE);
                    return -10;
                }
            }
        }
        return -1;
    }

    private int CheckForWinDiagonals() {
        // Check Diagonal - Left to right
        if (m_board[0, 0] == m_board[1, 1] && m_board[1, 1] == m_board[2, 2]) {
            if (m_board[0, 0] == GameManager.PLAYER_TWO) {
                ui.DrawWinLine(0, "diagonal", GameManager.PLAYER_TWO);
                return 10;
            } else if (m_board[0, 0] == GameManager.PLAYER_ONE) {
                ui.DrawWinLine(0, "diagonal", GameManager.PLAYER_ONE);
                return -10;
            }
        }

        // Check Diagonal - Right to left
        if (m_board[0, 2] == m_board[1, 1] && m_board[1, 1] == m_board[2, 0]) {
            if (m_board[0, 2] == GameManager.PLAYER_TWO) {
                ui.DrawWinLine(1, "diagonal", GameManager.PLAYER_TWO);
                return 10;
            } else if (m_board[0, 2] == GameManager.PLAYER_ONE) {
                ui.DrawWinLine(1, "diagonal", GameManager.PLAYER_ONE);
                return -10;
            }
        }
        return -1;
    }

    public bool CheckForMovesLeft() {
        for (int row = 0; row < 3; row++) {
            for (int column = 0; column < 3; column++) {
                if (m_board[row, column] == GameManager.EMPTY) {
                    return true;
                }
            }
        }
        return false;
    }

    public void UpdateBoardValues(int row, int column, int player) {
        if (row != -1 && column != -1) {
            m_board[row, column] = player;
        }
    }
}
