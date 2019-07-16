using UnityEngine;
using TMPro;
using System.Collections;

public class AIThinkingAnimator : MonoBehaviour {

    private TextMeshProUGUI text;
    private WaitForSeconds waitForSeconds;

    private void OnEnable() {
        if (text == null) {
            text = GetComponent<TextMeshProUGUI>();
        }
        if (waitForSeconds == null) {
            waitForSeconds = new WaitForSeconds(0.45f);
        }

        StartCoroutine(Animate());
    }

    private IEnumerator Animate() {
        text.text = "AI is thinking";
        yield return waitForSeconds;
        text.text = "AI is thinking.  ";
        yield return waitForSeconds;
        text.text = "AI is thinking.. ";
        yield return waitForSeconds;
        text.text = "AI is thinking...";
        yield return waitForSeconds;
        yield return StartCoroutine(Animate());
    }
}
