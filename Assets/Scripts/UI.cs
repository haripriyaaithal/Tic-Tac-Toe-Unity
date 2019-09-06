using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI : MonoBehaviour {

    [SerializeField] GameObject m_loading;
    [SerializeField] GameObject m_board;
    [SerializeField] GameObject m_playerInstructionText;

    [Header("Game Play")]
    [SerializeField] List<Button> m_boardButtons;
    [SerializeField] List<GameObject> m_winLines;
    [SerializeField] Sprite x;
    [SerializeField] Sprite o;

    [Header("Text Mesh Pros")]
    [SerializeField] GameObject m_playerTurn;
    [SerializeField] GameObject m_waitingForInput;
    [SerializeField] GameObject m_playerOneWon;
    [SerializeField] GameObject m_playerTwoWon;
    [SerializeField] GameObject m_drawText;

    private SoundManager m_soundManager;

    private void Start() {
        m_soundManager = FindObjectOfType<SoundManager>();
    }

    /// <summary>
    /// The sprite on the button will be changed to the value passed as argument
    /// </summary>
    public void DrawOnBoard(int buttonIndex, int player) {
        var child = m_boardButtons[buttonIndex].transform.GetChild(0).GetComponent<Image>();

        if (player == GameManager.PLAYER_ONE) {
            child.sprite = x;
            m_soundManager.PlayPlayerOneSound();
        } else if (player == GameManager.PLAYER_TWO) {
            child.sprite = o;
            m_soundManager.PlayPlayerTwoSound();
        }

        IncreaseAlphaValue(child);

        // Disable interaction with that button
        m_boardButtons[buttonIndex].interactable = false;
    }

    private void IncreaseAlphaValue(Image child) {
        var color = child.color;
        color.a = 1f;
        child.color = color;
    }

    private void DecreaseAlphaValue(Image child) {
        var color = child.color;
        color.a = 0f;
        child.color = color;
    }

    public void IndicateWaitingForInput(bool state) {
        m_waitingForInput.gameObject.SetActive(state);
    }

    public void IndicatePlayerTurn(bool state) {
        m_playerTurn.gameObject.SetActive(state);
    }

    /// <summary>
    /// Display draw message on the screen
    /// </summary>
    public void ShowDraw(bool state) {
        IndicatePlayerTurn(false);
        IndicateWaitingForInput(false);
        m_drawText.gameObject.SetActive(state);

        if (state) {
            m_soundManager.PlayGameDrawSound();
        }
    }

    public void ShowPlayerOneWon(bool state) {
        IndicatePlayerTurn(false);
        IndicateWaitingForInput(false);
        m_playerOneWon.gameObject.SetActive(state);
    }

    public void ShowPlayerTwoWon(bool state) {
        IndicatePlayerTurn(false);
        IndicateWaitingForInput(false);
        m_playerTwoWon.gameObject.SetActive(state);
    }

    /// <summary>
    /// This function draws a line on 3 consecutive X's or O's
    /// </summary>
    public void DrawWinLine(int index, string rowOrColumn, int player) {

        StartCoroutine(DrawAfterDelay());

        if (player == GameManager.PLAYER_TWO) {
            ShowPlayerTwoWon(true);
            m_soundManager.PlayPlayerTwoWon();
        } else if (player == GameManager.PLAYER_ONE) {
            ShowPlayerOneWon(true);
            m_soundManager.PlayPlayerWon();
        }

        if (rowOrColumn.Equals("row")) {
            m_winLines[index].SetActive(true);
        } else if (rowOrColumn.Equals("column")) {
            m_winLines[index + 3].gameObject.SetActive(true);
        } else if (rowOrColumn.Equals("diagonal")) {
            m_winLines[index + 6].gameObject.SetActive(true);
        }
    }

    private IEnumerator DrawAfterDelay() {
        yield return null; // Wait for end of frame
        IndicatePlayerTurn(false);
        IndicateWaitingForInput(false);
        EnableInput(false, true);

    }

    /// <summary>
    /// Call this function to enable and disable input from user
    /// </summary>
    public void EnableInput(bool state, bool forceAssign) {
        foreach (Button button in m_boardButtons) {
            if (forceAssign) {
                button.interactable = state;
            } else {
                if (button.transform.GetChild(0).GetComponent<Image>().sprite != null) {
                    button.interactable = false;
                } else {
                    button.interactable = state;
                }
            }
        }

        if (!forceAssign && state == true) {
            IndicateWaitingForInput(false);
            IndicatePlayerTurn(true);
        } else if (!forceAssign) {
            IndicateWaitingForInput(true);
            IndicatePlayerTurn(false);
        }

    }

    public void ResetBoard() {
        foreach (var button in m_boardButtons) {
            var child = button.transform.GetChild(0).GetComponent<Image>();
            DecreaseAlphaValue(child);
            child.sprite = null;
        }
        foreach (var lines in m_winLines) {
            lines.gameObject.SetActive(false);
        }
        EnableInput(true, true);
    }

    public void ShowLoading(bool show) {
        if (show) {
            m_board.SetActive(false);
            m_playerInstructionText.SetActive(false);
            m_loading.SetActive(true);
        } else {
            m_loading.SetActive(false);
            m_board.SetActive(true);
            m_playerInstructionText.SetActive(true);
        }
    }
}
