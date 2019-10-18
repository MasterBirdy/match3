using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Panda : MonoBehaviour, AnimalClass
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

        List<string> randomList = new List<string>();

        for (int i = 0; i < level - 1; i++)
        {
            int numToAdd = Random.Range(0, 3);
            while (randomList.Contains(helperFunction(numToAdd)))
            {
                numToAdd = Random.Range(0, 3);
            }
            randomList.Add(helperFunction(numToAdd));
        }


        for (int j = 0; j < board.height; j++)
        {
            for (int i = 0; i < board.width; i++)
            {
                GameObject testGameObject = board.allAnimals[i, j];
                if (testGameObject.tag == "Panda" || randomList.Contains(testGameObject.tag))
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

    private string helperFunction(int i)
    {
        switch(i)
        {
            case 0:
                return "Snake";
            case 1:
                return "Penguin";
            case 2:
                return "Giraffe";
            case 3:
                return "Monkey";
            default:
                return "";
        }
    }
}
