using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI : MonoBehaviour {

    [Header("Game Play")]
    [SerializeField] List<Button> boardButtons;
    [SerializeField] List<GameObject> winLines;
    [SerializeField] Sprite x;
    [SerializeField] Sprite o;

    [Header("Text Mesh Pros")]
    [SerializeField] GameObject aiTurn;
    [SerializeField] GameObject playerTurn;
    [SerializeField] GameObject aiWon;
    [SerializeField] GameObject playerWon;
    [SerializeField] GameObject drawText;

    private SoundManager soundManager;

    private void Start() {
        soundManager = FindObjectOfType<SoundManager>();
    }

    /// <summary>
    /// The sprite on the button will be changed to the value passed as argument
    /// </summary>
    public void DrawOnBoard(int buttonIndex, int player) {
        var child = boardButtons[buttonIndex].transform.GetChild(0).GetComponent<Image>();

        if (player == GameManager.PLAYER) {
            child.sprite = x;
            soundManager.PlayPlayerSound();
        } else if (player == GameManager.AI) {
            child.sprite = o;
            soundManager.PlayAISound();
        }

        IncreaseAlphaValue(child);

        // Disable interaction with that button
        boardButtons[buttonIndex].interactable = false;       
    }

    private void IncreaseAlphaValue(Image child) {
        var color = child.color;
        color.a = 1f;
        child.color = color;
    }

    public void IndicateAITurn(bool state) {
        aiTurn.gameObject.SetActive(state);
    }

    public void IndicatePlayerTurn(bool state) {
        playerTurn.gameObject.SetActive(state);
    }

    /// <summary>
    /// Display draw message on the screen
    /// </summary>
    public void ShowDraw() {
        drawText.gameObject.SetActive(true);
        soundManager.PlayGameDrawSound();
    }

    private void ShowPlayerWon(bool state) {
        playerWon.gameObject.SetActive(state);
    }

    private void ShowAIWon(bool state) {
        aiWon.gameObject.SetActive(state);
    }

    /// <summary>
    /// This function draws a line on 3 consecutive X's or O's
    /// </summary>
    public void DrawWinLine(int index, string rowOrColumn, int player) {

        StartCoroutine(DrawAfterDelay());

        if (player == GameManager.AI) {
            ShowAIWon(true);
            soundManager.PlayAIWon();
        } else if (player == GameManager.PLAYER) {
            ShowPlayerWon(true);
            soundManager.PlayPlayerWon();
        }

        if (rowOrColumn.Equals("row")) {
            winLines[index].SetActive(true);
        } else if (rowOrColumn.Equals("column")) {
            winLines[index + 3].gameObject.SetActive(true);
        } else if (rowOrColumn.Equals("diagonal")) {
            winLines[index + 6].gameObject.SetActive(true);
        }
    }

    private IEnumerator DrawAfterDelay() {
        yield return null; // Wait for end of frame
        IndicatePlayerTurn(false);
        IndicateAITurn(false);
        EnableInput(false, true);

    }

    /// <summary>
    /// Call this function to enable and disable input from user
    /// </summary>
    public void EnableInput(bool state, bool forceAssign) {
        foreach (Button button in boardButtons) {
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
    }
 }
