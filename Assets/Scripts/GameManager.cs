using UnityEngine;

public class GameManager : MonoBehaviour {

    [SerializeField] private AIThinkingAnimator aiTurn;

    public static int EMPTY = 0;
    public static int PLAYER = 1;
    public static int AI = 2;

    private int turn = PLAYER;

    private GameAI ai;
    private UI ui;

    private void Start() {
        ui = FindObjectOfType<UI>();
        ai = FindObjectOfType<GameAI>();
    }

    /// <summary>
    /// Registers user input on the matrix
    /// </summary>
    public void UserInput(int buttonIndex) {
        if (turn == PLAYER) {
            ui.DrawOnBoard(buttonIndex, turn);

            Vector2 rowColumn = MapButtonIndexTo2DArray(buttonIndex);
            int row = (int)rowColumn.x;
            int column = (int)rowColumn.y;

            ai.UpdateBoardValues(row, column, GameManager.PLAYER);
            ui.IndicatePlayerTurn(false);
        }
        ChangeTurn();
        int state = CheckForWinners();

        if (state == 0) {
            ui.IndicateAITurn(true);
            ui.EnableInput(false, false);
            ai.AITurn();
        }
    }

    private int CheckForWinners() {
        ai.SetMarkWin(true);
        int score = ai.CheckForWins();
        ai.SetMarkWin(false);
        if (score == 10) {
            // AI won
            ui.IndicatePlayerTurn(false);
            ui.IndicateAITurn(false);
            return 1;
        } else if (score == -10) {
            // Player won
            ui.IndicatePlayerTurn(false);
            ui.IndicateAITurn(false);
            return -1;
        }

        if (!ai.CheckForMovesLeft()) {
            ui.IndicatePlayerTurn(false);
            ui.IndicateAITurn(false);
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
        turn = (++turn % 2);
    }
}
