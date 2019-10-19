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
        AudioManager.instance.PlayMusic(0);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void LoadCreditsScene()
    {
        SceneManager.LoadScene("Credits Scene");
        AudioManager.instance.StopMusic();
    }

    public void StartGame()
    {
        SceneManager.LoadScene("Main Game");
        AudioManager.instance.StopMusic();
    }

    public void LoadHighScoreScene()
    {
        SceneManager.LoadScene("High Score Scene");
    }

    public void LoadCharacterScene()
    {
        SceneManager.LoadScene("Character Scene");
        AudioManager.instance.StopMusic();
    }

    public void LoadExpScene()
    {
        SceneManager.LoadScene("Exp Scene");
        AudioManager.instance.StopMusic();
    }

}
