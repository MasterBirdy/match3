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
    static public int Level2 = 20000;
    static public int Level3 = 50000;

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
        return false;
    }
}
