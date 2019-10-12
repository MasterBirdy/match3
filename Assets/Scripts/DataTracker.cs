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
    public float countdownTimer;
    public float startCountdown;
    private bool isGameGoing;
    private SessionState sessionState;
    private SceneLoader sceneLoader;
    private AudioSource audioSource;
    private PowerBar powerBar;
    // Start is called before the first frame update
    void Start()
    {
        timesUpText.enabled = false;
        board = FindObjectOfType<Board>();
        sceneLoader = FindObjectOfType<SceneLoader>();
        audioSource = GetComponent<AudioSource>();
        powerBar = FindObjectOfType<PowerBar>();
        sessionState = SessionState.NOTSTARTED;
        countdownTimer = 3f;
        startCountdown = countdownTimer;
        countdownTimer += .49f;
        NewGame();
    }

    // Update is called once per frame
    void Update()
    {
        Timer();
        CountdownTimer();
    }

    public void NewGame()
    {
        currentScore = 0;
        time = 10;
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

    private void CountdownTimer()
    {
        if (sessionState == SessionState.NOTSTARTED)
        {
            countdownTimer -= Time.deltaTime;
            if (countdownTimer <= 0.51f)
            {
                sessionState = SessionState.STARTED;
                countdownText.enabled = false;
                board.StartBoard();
                audioSource.Play();
            }
            else
            {
                countdownText.text = "" + Mathf.Round(countdownTimer);
            } 
        }
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

    public bool IsGameGoing()
    {
        return isGameGoing;
    }

    public SessionState GetSessionState()
    {
        return sessionState;
    }

    private IEnumerator EndGame()
    {
        HighScoreData highScoreData = SaveSystem.LoadHighScore();
        if (highScoreData == null)
            highScoreData = new HighScoreData(currentScore, powerBar.ReturnAnimalName());
        else
            highScoreData.AddData(currentScore, powerBar.ReturnAnimalName());
        SaveSystem.SaveHighScore(highScoreData);
        yield return new WaitForSeconds(2f);
        sceneLoader.LoadHighScoreScene();
    }
}
