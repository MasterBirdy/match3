using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monkey: MonoBehaviour, AnimalClass
{
    FindMatches findMatches;
    DataTracker dataTracker;
    Board board;
    [SerializeField] Sprite sprite;
    public void ActivatePower(int level)
    {
        if (board == null)
            board = FindObjectOfType<Board>();
        if (findMatches == null)
            findMatches = FindObjectOfType<FindMatches>();
        for (int j = 0; j < 3 + level; j++)
        {
            for (int i = 0; i < board.width; i++)
            {
                GameObject testGameObject = board.allAnimals[i, j];
                if (j == 0 || j == 2 + level)
                {
                    if (i == 3 || i == 4)
                    {
                        testGameObject.GetComponent<AnimalTile>().isMatched = true;
                        findMatches.currentMatches.Add(testGameObject);
                    }
                }
                else if (j > 0 && j < 2 + level)
                {
                    if (i == 4 || i == 5)
                    {
                        testGameObject.GetComponent<AnimalTile>().isMatched = true;
                        findMatches.currentMatches.Add(testGameObject);
                    }
                }
            }
        }
        board.StartDestroyAllNow();
    }

    public bool HasTimeExtension()
    {
        return true;
    }

    public string ReturnName()
    {
        return "Monkey";
    }

    public Sprite ReturnSprite()
    {
        return sprite;
    }
}
