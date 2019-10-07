using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DataTracker : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI scoreText;
    [SerializeField] TextMeshProUGUI timeText;
    public int currentScore;
    public float time;
    // Start is called before the first frame update
    void Start()
    {
        NewGame();
    }

    // Update is called once per frame
    void Update()
    {
        time -= Time.deltaTime;
        timeText.text = "Time: " + Mathf.Round(time);
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

}
