using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{

    public SceneFader sceneFader;

    public void Play() {
        Debug.Log("clicked play");
        sceneFader.FadeTo("GameScene");
    }

    public void Instructions() {
        Debug.Log("clicked instructions");
        sceneFader.FadeTo("GameScene");
    }

    public void ReturnToMenu() {
        sceneFader.FadeTo("MenuScene");
    }

    public void Quit () {
        Application.Quit();
    }
}
