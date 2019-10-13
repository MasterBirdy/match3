using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Panda : MonoBehaviour, AnimalClass
{
    FindMatches findMatches;
    DataTracker dataTracker;
    Board board;
    [SerializeField] Sprite sprite;
    public void ActivatePower()
    {
        if (board == null)
            board = FindObjectOfType<Board>();
        if (findMatches == null)
            findMatches = FindObjectOfType<FindMatches>();
        for (int j = 0; j < board.height; j++)
        {
            for (int i = 0; i < board.width; i++)
            {
                GameObject testGameObject = board.allAnimals[i, j];
                if (testGameObject.tag == "Panda")
                {
                    testGameObject.GetComponent<AnimalTile>().isMatched = true;
                    findMatches.currentMatches.Add(testGameObject);
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
        return "Panda";
    }

    public Sprite ReturnSprite()
    {
        return sprite;
    }
}
