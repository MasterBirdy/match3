using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

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
                                AnimalTile currentAnimalTileComponent = currentAnimal.GetComponent<AnimalTile>();
                                AnimalTile leftAnimalTileComponent = leftAnimal.GetComponent<AnimalTile>();
                                AnimalTile rightAnimalTileComponent = rightAnimal.GetComponent<AnimalTile>();

                                if (currentAnimal.GetComponent<AnimalTile>().isRowBomb ||
                                    leftAnimal.GetComponent<AnimalTile>().isRowBomb ||
                                    rightAnimal.GetComponent<AnimalTile>().isRowBomb)
                                {
                                    currentMatches.Union(GetRowPieces(j));
                                }

                                if (currentAnimalTileComponent.isColumnBomb)
                                {
                                    currentMatches.Union(GetColumnPieces(i));
                                }

                                if (leftAnimalTileComponent.isColumnBomb)
                                {
                                    currentMatches.Union(GetColumnPieces(i - 1));
                                }

                                if (rightAnimalTileComponent.isColumnBomb)
                                {
                                    currentMatches.Union(GetColumnPieces(i - 1));
                                }

                                if (!currentMatches.Contains(leftAnimal))
                                {
                                    currentMatches.Add(leftAnimal);
                                }
                                leftAnimalTileComponent.isMatched = true;

                                if (!currentMatches.Contains(rightAnimal))
                                {
                                    currentMatches.Add(rightAnimal);
                                }
                                rightAnimalTileComponent.isMatched = true;

                                if (!currentMatches.Contains(currentAnimal))
                                {
                                    currentMatches.Add(currentAnimal);
                                }
                                currentAnimalTileComponent.isMatched = true;

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
                                AnimalTile currentAnimalTileComponent = currentAnimal.GetComponent<AnimalTile>();
                                AnimalTile upAnimalTileComponent = upAnimal.GetComponent<AnimalTile>();
                                AnimalTile downAnimalTileComponent = downAnimal.GetComponent<AnimalTile>();

                                if (currentAnimalTileComponent.isColumnBomb ||
                                    upAnimalTileComponent.isColumnBomb ||
                                    downAnimalTileComponent.isColumnBomb)
                                {
                                    currentMatches.Union(GetColumnPieces(i));
                                }

                                if (currentAnimalTileComponent.isRowBomb)
                                {
                                    currentMatches.Union(GetRowPieces(j));
                                }

                                if (upAnimalTileComponent.isRowBomb)
                                {
                                    currentMatches.Union(GetRowPieces(j + 1));
                                }

                                if (downAnimalTileComponent.isRowBomb)
                                {
                                    currentMatches.Union(GetRowPieces(j - 1));
                                }

                                if (!currentMatches.Contains(upAnimal))
                                {
                                    currentMatches.Add(upAnimal);
                                }
                                upAnimalTileComponent.isMatched = true;

                                if (!currentMatches.Contains(downAnimal))
                                {
                                    currentMatches.Add(downAnimal);
                                }
                                downAnimalTileComponent.isMatched = true;

                                if (!currentMatches.Contains(currentAnimal))
                                {
                                    currentMatches.Add(currentAnimal);
                                }
                                currentAnimalTileComponent.isMatched = true;

                            }
                        }
                    }
                }
            }
        }
    }

    private List<GameObject> GetColumnPieces(int column)
    {
        List<GameObject> animals = new List<GameObject>();
        for (int i = 0; i < board.height; i++)
        {
            if (board.allAnimals[column, i] != null)
            {
                animals.Add(board.allAnimals[column, i]);
                board.allAnimals[column, i].GetComponent<AnimalTile>().isMatched = true;
            }
        }
        return animals;
    }

    private List<GameObject> GetRowPieces(int row)
    {
        List<GameObject> animals = new List<GameObject>();
        for (int i = 0; i < board.width; i++)
        {
            if (board.allAnimals[i, row] != null)
            {
                animals.Add(board.allAnimals[i, row]);
                board.allAnimals[i, row].GetComponent<AnimalTile>().isMatched = true;
            }
        }
        return animals;
    }
}
