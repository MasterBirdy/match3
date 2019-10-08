using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DataTracker : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI scoreText;
    [SerializeField] TextMeshProUGUI timeText;
    [SerializeField] TextMeshProUGUI countdownText;
    public int currentScore;
    public float time;
    private Board board;
    public float countdownTimer;
    public float startCountdown;
    private bool isGameGoing;
    // Start is called before the first frame update
    void Start()
    {

        board = FindObjectOfType<Board>();
        isGameGoing = false;
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
        time = 60;
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
        if (!isGameGoing)
        {
            countdownTimer -= Time.deltaTime;
            if (countdownTimer <= 0.51f)
            {
                isGameGoing = true;
                countdownText.enabled = false;
                board.StartBoard();
            }
            else
            {
                countdownText.text = "" + Mathf.Round(countdownTimer);
            } 
        }
    }

    private void Timer()
    {
        if (isGameGoing)
        {
            time -= Time.deltaTime;
            timeText.text = "Time: " + Mathf.Round(time);
        }
    }

    public bool IsGameGoing()
    {
        return isGameGoing;
    }
}
