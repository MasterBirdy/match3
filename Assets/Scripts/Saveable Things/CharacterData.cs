using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CharacterData
{
    //snake = 0, penguin = 1, giraffe = 2, panda = 3, monkey = 4
    public int[] levels;
    public int[] experience;
    public int currentCharacter;
    static public int Level2 = 200000;
    static public int Level3 = 500000;

    // Start is called before the first frame update
    public CharacterData()
    {
        experience = new int[5];
        currentCharacter = 0;
        levels = new int[5];
        for (int i = 0; i < levels.Length; i++)
        {
            levels[i] = 1;
        }
    }

    public bool AddExperience(int score)
    {
        bool levelGained = false;
        if (levels[currentCharacter] < 3)
        {
            experience[currentCharacter] += score;
            if ((levels[currentCharacter] == 1 && experience[currentCharacter] > Level2) ||
                (levels[currentCharacter] == 2 && experience[currentCharacter] > Level3))
            {
                levelGained = true;
                levels[currentCharacter]++;
                experience[currentCharacter] = 0;
            }
        }
        return levelGained;
    }

    public void SaveCharacter(int i)
    {
        currentCharacter = i;
    }

    public int LoadCharacter()
    {
        return currentCharacter;
    }
}
