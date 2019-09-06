using UnityEngine;
using TMPro;
using System.Collections;

public class WaitingTextAnimator : MonoBehaviour {

    private TextMeshProUGUI m_text;
    private WaitForSeconds m_waitForSeconds;

    private void OnEnable() {
        if (m_text == null) {
            m_text = GetComponent<TextMeshProUGUI>();
        }
        if (m_waitForSeconds == null) {
            m_waitForSeconds = new WaitForSeconds(0.45f);
        }

        StartCoroutine(Animate());
    }

    private IEnumerator Animate() {
        m_text.text = "Waiting for input   ";
        yield return m_waitForSeconds;
        m_text.text = "Waiting for input.  ";
        yield return m_waitForSeconds;
        m_text.text = "Waiting for input.. ";
        yield return m_waitForSeconds;
        m_text.text = "Waiting for input...";
        yield return m_waitForSeconds;
        yield return StartCoroutine(Animate());
    }
}
