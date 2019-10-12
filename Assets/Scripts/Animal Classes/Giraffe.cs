using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Giraffe : MonoBehaviour, AnimalClass
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
        int j = Random.Range(0, 8);
        for (int i = 0; i < board.height; i++)
        {
            GameObject testGameObject = board.allAnimals[j, i];
            testGameObject.GetComponent<AnimalTile>().isMatched = true;
            findMatches.currentMatches.Add(testGameObject);
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
