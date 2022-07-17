using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneFader : MonoBehaviour
{
    public Image img;
    public AnimationCurve fadeCurve;
    public float timeScale = 1f;

    private float fadeColor = 0.08627451f;

    void Start() {
        StartCoroutine(FadeIn());
    }

    public void FadeTo (string scene) {
        StartCoroutine(FadeOut(scene));
    }
    IEnumerator FadeIn () {
        float t = 1f;

        while (t > 0f) {
            t -= Time.deltaTime * timeScale;
            float a = fadeCurve.Evaluate(t);
            img.color = new Color(fadeColor, fadeColor, fadeColor, a);
            yield return 0;
        }
    }
    
    IEnumerator FadeOut (string scene) {
        float t = 0f;

        while (t < 1f) {
            t += Time.deltaTime * timeScale;
            float a = fadeCurve.Evaluate(t);
            img.color = new Color(fadeColor, fadeColor, fadeColor, a);
            yield return 0;
        }

        SceneManager.LoadScene(scene);
    }
}
