using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindMatches : MonoBehaviour
{

    private Board board;
    public List<GameObject> currentMatches = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        board = FindObjectOfType<Board>();
    }

    public void FindAllMatches()
    {
        StartCoroutine(FindAllMatchesCo());
    }

    private IEnumerator FindAllMatchesCo()
    {
        yield return new WaitForSeconds(.2f);
        for (int i = 0; i < board.width; i++)
        {
            for (int j = 0; j < board.height; j++)
            {
                GameObject currentAnimal = board.allAnimals[i, j];
                if (currentAnimal != null)
                {
                    if (i > 0 && i < board.width - 1)
                    {
                        GameObject leftAnimal = board.allAnimals[i - 1, j];
                        GameObject rightAnimal = board.allAnimals[i + 1, j];
                        // null check
                        if (leftAnimal != null && rightAnimal != null)
                        {
                            if (leftAnimal.tag == currentAnimal.tag && rightAnimal.tag == currentAnimal.tag)
                            {
                                if (!currentMatches.Contains(leftAnimal))
                                {
                                    currentMatches.Add(leftAnimal);
                                }
                                leftAnimal.GetComponent<AnimalTile>().isMatched = true;
                                if (!currentMatches.Contains(rightAnimal))
                                {
                                    currentMatches.Add(rightAnimal);
                                }
                                rightAnimal.GetComponent<AnimalTile>().isMatched = true;
                                if (!currentMatches.Contains(currentAnimal))
                                {
                                    currentMatches.Add(currentAnimal);
                                }
                                currentAnimal.GetComponent<AnimalTile>().isMatched = true;
                            }
                        }
                    }
                     if (j > 0 && j < board.height - 1)
                    {
                        GameObject upAnimal = board.allAnimals[i, j + 1];
                        GameObject downAnimal = board.allAnimals[i, j - 1];
                        if (upAnimal != null && downAnimal != null)
                        {
                            if (upAnimal.tag == currentAnimal.tag && downAnimal.tag == currentAnimal.tag)
                            {
                                if (!currentMatches.Contains(upAnimal))
                                {
                                    currentMatches.Add(upAnimal);
                                }
                                upAnimal.GetComponent<AnimalTile>().isMatched = true;
                                if (!currentMatches.Contains(downAnimal))
                                {
                                    currentMatches.Add(downAnimal);
                                }
                                downAnimal.GetComponent<AnimalTile>().isMatched = true;
                                if (!currentMatches.Contains(currentAnimal))
                                {
                                    currentMatches.Add(currentAnimal);
                                }
                                currentAnimal.GetComponent<AnimalTile>().isMatched = true;
                            }
                        }
                    }
                }
            }
        }
    }
}
