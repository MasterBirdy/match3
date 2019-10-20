using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public enum SessionState
{
    NOTSTARTED,
    STARTED,
    FINISHED
}

public class DataTracker : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI scoreText;
    [SerializeField] TextMeshProUGUI timeText;
    [SerializeField] TextMeshProUGUI countdownText;
    [SerializeField] TextMeshProUGUI timesUpText;
    public int currentScore;
    public float time;
    private Board board;
    public float startCountdown;
    private bool isGameGoing;
    private SessionState sessionState;
    private SceneLoader sceneLoader;
    private PowerBar powerBar;

    // Start is called before the first frame update
    void Start()
    {
        timesUpText.enabled = false;
        board = FindObjectOfType<Board>();
        sceneLoader = FindObjectOfType<SceneLoader>();
        powerBar = FindObjectOfType<PowerBar>();
        sessionState = SessionState.NOTSTARTED;

        NewGame();
    }

    // Update is called once per frame
    void Update()
    {
        Timer();
    }

    public void NewGame()
    {
        currentScore = 0;
        time = 60;
        StartCoroutine(CountDown());
    }

    public void UpdateScore(int number)
    {
        currentScore += number;
        scoreText.text = "Score: " + currentScore;
    }

    public void ExtendTime(int number)
    {
        time += number;
    }

    private void Timer()
    {
        if (sessionState == SessionState.STARTED)
        {
            time -= Time.deltaTime;
            timeText.text = "Time: " + Mathf.Round(time);
            if (time <= 0f)
            {
                sessionState = SessionState.FINISHED;
                timesUpText.enabled = true;
                StartCoroutine(EndGame());
            }
        }
    }

    public SessionState GetSessionState()
    {
        return sessionState;
    }

    private IEnumerator EndGame()
    {
        yield return new WaitForSeconds(2f);
        HighScoreData highScoreData = SaveSystem.LoadHighScore();
        if (highScoreData == null)
            highScoreData = new HighScoreData(currentScore, powerBar.ReturnAnimalName());
        else
            highScoreData.AddData(currentScore, powerBar.ReturnAnimalName());
        SaveSystem.SaveHighScore(highScoreData);
        sceneLoader.LoadExpScene();
    }

    private IEnumerator CountDown()
    {
        for (int i = 3; i > 0; i--)
        {
            countdownText.text = "" + i;
            AudioManager.instance.PlayCountdownSound();
            yield return new WaitForSeconds(1f);
        }
        sessionState = SessionState.STARTED;
        countdownText.enabled = false;
        board.StartBoard();
        AudioManager.instance.PlayMusic(1);
    }

}
