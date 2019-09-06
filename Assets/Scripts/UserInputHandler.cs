using UnityEngine;
using UnityEngine.SceneManagement;

public class UserInputHandler : MonoBehaviour {

    [SerializeField] private GameManager gameManager;

    public void ButtonClick(int number) {
        gameManager.UserInput(number);
    }

    public void GoBack() {
        if (SceneManager.GetActiveScene().buildIndex > 0) {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
        }
    }

    public void RestartGame() {
        gameManager.ResetGame();
    }

    public void LoadNextScene() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void QuitGame() {
        Application.Quit();
    }
}
