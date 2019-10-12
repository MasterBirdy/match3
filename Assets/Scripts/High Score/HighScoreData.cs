using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class HighScoreData
{
    public int[] highScores;
    public string[] animalClass;
    public int currentScore;

    public HighScoreData (int score, string animal)
    {
        highScores = new int[5];
        animalClass = new string[5];
        highScores[0] = score;
        animalClass[0] = animal;
        currentScore = score;
    }

    public bool AddData(int score, string animal)
    {
        currentScore = score;
        int tempScore = 0;
        string tempAnimal = "";
        bool hasBeenAdded = false;
        for (int i = 0; i < highScores.Length; i++)
        {
            if (highScores[i] == 0 && !hasBeenAdded)
            {
                highScores[i] = score;
                animalClass[i] = animal;
                hasBeenAdded = true;
            }
            else if (highScores[i] < score && !hasBeenAdded)
            {
                tempScore = highScores[i];
                if (animalClass[i] != null)
                    tempAnimal = animalClass[i];
                highScores[i] = score;
                animalClass[i] = animal;
                hasBeenAdded = true;
            }
            else if (highScores[i] < tempScore && hasBeenAdded)
            {
                string tempTempAnimal = "";
                int tempTempScore;
                tempTempScore = highScores[i];
                if (animalClass[i] != null)
                    tempTempAnimal = animalClass[i];
                highScores[i] = tempScore;
                animalClass[i] = tempAnimal;
                tempScore = tempTempScore;
                tempAnimal = tempTempAnimal;
            }
        }
        return hasBeenAdded;
    }
 
}
