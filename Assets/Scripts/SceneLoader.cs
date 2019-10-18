using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    // Start is called before the first frame update
    public void LoadStartScene()
    {
        SceneManager.LoadScene(0);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void LoadCreditsScene()
    {
        SceneManager.LoadScene("Credits Scene");
    }

    public void StartGame()
    {
        SceneManager.LoadScene("Main Game");
    }

    public void LoadHighScoreScene()
    {
        SceneManager.LoadScene("High Score Scene");
    }

    public void LoadCharacterScene()
    {
        SceneManager.LoadScene("Character Scene");
    }

    public void LoadExpScene()
    {
        SceneManager.LoadScene("Exp Scene");
    }

}
