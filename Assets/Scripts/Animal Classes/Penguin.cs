using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Penguin : MonoBehaviour, AnimalClass
{
    [SerializeField] GameObject findMatchesObject;
    [SerializeField] GameObject dataTrackerObject;
    [SerializeField] GameObject boardObject;
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
        for (int j = 0; j < level * 2; j++)
        {
            for (int i = 0; i < board.width; i++)
            {
                GameObject testGameObject = board.allAnimals[i, j];
                if (j % 2 == 0)
                {
                    if (i % 2 == 1)
                        testGameObject.GetComponent<AnimalTile>().isMatched = true;
                }
                else
                {
                    if (i % 2 == 0)
                        testGameObject.GetComponent<AnimalTile>().isMatched = true;
                }
                findMatches.currentMatches.Add(testGameObject);
            }
        }
        board.StartDestroyAllNow();
    }

    public bool HasTimeExtension()
    {
        return true;
    }

    public Sprite ReturnSprite()
    {
        return sprite;
    }

    public string ReturnName()
    {
        return "Penguin";
    }
}
