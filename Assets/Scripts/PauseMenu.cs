using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public static bool gameIsPaused = false;
    public GameObject pauseMenuUI;
    private DataTracker dataTracker;
    private SceneLoader sceneLoader;
    // Start is called before the first frame update
    void Start()
    {
        dataTracker = FindObjectOfType<DataTracker>();
        sceneLoader = FindObjectOfType<SceneLoader>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (gameIsPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    private void Pause()
    {
        pauseMenuUI.SetActive(true);
        dataTracker.PauseMusic();
        Time.timeScale = 0f;
        gameIsPaused = true;
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        dataTracker.UnpauseMusic();
        gameIsPaused = false;
    }

    public void LoadMenu()
    {
        Time.timeScale = 1f;
        sceneLoader.LoadStartScene();
    }
}
