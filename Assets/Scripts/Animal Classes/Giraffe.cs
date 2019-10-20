using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Giraffe : MonoBehaviour, AnimalClass
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

        List<int> randomList = new List<int>();

        for (int i = 0; i < level; i++)
        {
            int numToAdd = Random.Range(0, 7);
            while (randomList.Contains(numToAdd))
            {
                numToAdd = Random.Range(0, 7);
            }
            randomList.Add(numToAdd);
        }

        foreach (int j in randomList)
        {
            for (int i = 0; i < board.height; i++)
            {
                GameObject testGameObject = board.allAnimals[j, i];
                testGameObject.GetComponent<AnimalTile>().isMatched = true;
                findMatches.currentMatches.Add(testGameObject);
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
        return "Giraffe";
    }

    public Sprite ReturnSprite()
    {
        return sprite;
    }

}
