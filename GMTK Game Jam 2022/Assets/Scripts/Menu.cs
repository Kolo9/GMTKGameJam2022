using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{

    public SceneFader sceneFader;

    public void Play() {
        sceneFader.FadeTo("GameScene");
    }

    public void Instructions() {
        sceneFader.FadeTo("GameScene");
    }

    public void ReturnToMenu() {
        sceneFader.FadeTo("MenuScene");
    }

    public void Quit () {
        Application.Quit();
    }
}
