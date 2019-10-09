using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState
{
    WAIT,
    MOVE,
    FINISHED
}
public class Board : MonoBehaviour
{
    public GameState currentState;
    public int height = 7;
    public int width = 7;
    public float padding = .1f;
    [SerializeField] private GameObject[] animalChoices;
    [SerializeField] public GameObject explosion;
    public DataTracker dataTracker;
    public GameObject[,] allAnimals;
    private FindMatches findMatches;
    private PowerBar powerBar;
    public AnimalTile currentAnimal;

    // Start is called before the first frame update
    void Start()
    {
        findMatches = FindObjectOfType<FindMatches>();
        dataTracker = FindObjectOfType<DataTracker>();
        powerBar = FindObjectOfType<PowerBar>();
        allAnimals = new GameObject[width, height];
        currentState = GameState.WAIT;
        for (int j = 0; j < height; j++)
        {
            float y = j;
            for (int i = 0; i < width; i++)
            {
                float x = i;
                GameObject newAnimalTile = CreateNewAnimalTile(x, y);
                var animalTileComponent = newAnimalTile.GetComponent<AnimalTile>();
                animalTileComponent.row = (int)y;
                animalTileComponent.column = (int)x;
                allAnimals[(int)x, (int)y] = newAnimalTile;
            }
        }
    }

    private GameObject CreateNewAnimalTile(float x, float y)
    {
        Vector2 tempPosition = new Vector2(x, y)
           + new Vector2(transform.position.x - (((width - 1) / 2) + ((width - 1) / 2) * padding),
           transform.position.y - (((height - 1) / 2) + ((height - 1) / 2) * padding))
           + new Vector2(padding * x, padding * y);

        int animalToUse = Random.Range(0, animalChoices.Length);
        while (MatchesAt((int)x, (int)y, animalChoices[animalToUse]))
        {
            animalToUse = Random.Range(0, animalChoices.Length);
        }
        var newAnimalTile = Instantiate(animalChoices[animalToUse], tempPosition, Quaternion.identity) as GameObject;
        newAnimalTile.transform.parent = this.transform;
        newAnimalTile.name = (x + " " + y);
        return newAnimalTile;
    }

    private bool MatchesAt(int row, int column, GameObject piece)
    {
        if (column > 1)
            if (allAnimals[row, column - 1].tag == piece.tag && allAnimals[row, column - 2].tag == piece.tag)
            {
                return true;
            }
        if (row > 1)
        {
            if (allAnimals[row - 1, column].tag == piece.tag && allAnimals[row - 2, column].tag == piece.tag)
            {
                return true;
            }
        }

        return false;
    }



    private bool DestroyMatchesAt(int column, int row)
    {
        AnimalTile testAnimal = allAnimals[column, row].GetComponent<AnimalTile>();
        if (testAnimal.isMatched)
        {
            //how many elements are in the matched pieces list
            if (findMatches.currentMatches.Count == 4 && currentAnimal != null && !(testAnimal.isColumnBomb || testAnimal.isRowBomb))
            {
                if (findMatches.CheckBombs())
                    findMatches.formedBombs.Add(currentAnimal);
            }
            findMatches.currentMatches.Remove(allAnimals[column, row]);
            GameObject explode = Instantiate(explosion, allAnimals[column, row].transform.position, Quaternion.identity);
            Destroy(explode, 1f);
            if (testAnimal.isMatched)
            {
                Destroy(allAnimals[column, row]);
                allAnimals[column, row] = null;
            }
            return true;
        }
        else if (findMatches.formedBombs.Contains(testAnimal))
            return true;
        else
            return false;
    }

    private IEnumerator FadeToBlack(SpriteRenderer s)
    {
            for (float i = 1f; i >= 0; i -= Time.deltaTime * 1.4f)
            {
                s.color = new Color(1, 1, 1, i);
                yield return null;
            }
    }

    public IEnumerator DestroyMatches()
    {
        yield return new WaitForSeconds(.20f);
        int scoreUpdate = 0;
        for (int i = 0; i < height; i++)
        {
            for (int j = 0; j < width; j++)
            {
                if (DestroyMatchesAt(j, i))
                {
                    scoreUpdate += 100;
                    powerBar.IncreasePowerLevel(1.5f);
                }
            }
        }
        dataTracker.UpdateScore(scoreUpdate);
        currentAnimal = null;
        yield return StartCoroutine(DecreaseRowCo());
    }

    private IEnumerator DecreaseRowCo()
    {
        int nullCount = 0;
        for (int i = 0; i < width; i++)
        {
            nullCount = 0;
            for (int j = 0; j < height; j++)
            {
                if (allAnimals[i, j] == null)
                    nullCount++;
                else if(nullCount > 0)
                {
                    var tempAni = allAnimals[i, j];
                    var tempAnimal = tempAni.GetComponent<AnimalTile>();
                    tempAnimal.row -= nullCount;
                    tempAnimal.targetY -= (padding + 1) * nullCount;
                    tempAnimal.previousTargetY = tempAnimal.targetY;
                    allAnimals[i, j - nullCount] = tempAni;
                    allAnimals[i, j] = null;
                }
            }
        }
        yield return new WaitForSeconds(.2f);
        yield return StartCoroutine(FillBoardCo());

    }

    private void RefillBoard()
    {
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                if (allAnimals[i, j] == null)
                {
                    GameObject newAnimalTile = CreateNewAnimalTile(i, j);
                    var animalTileComponent = newAnimalTile.GetComponent<AnimalTile>();
                    animalTileComponent.row = j;
                    animalTileComponent.column = i;
                    allAnimals[i, j] = newAnimalTile;
                }
            }
        }
    }

    private IEnumerator FillBoardCo()
    {

        RefillBoard();
        yield return new WaitForSeconds(.15f);
        while (MatchesOnBoard())
        {
           
            yield return StartCoroutine(DestroyMatches());
        }
        findMatches.formedBombs.Clear();
        findMatches.currentMatches.Clear();

    }

    private bool MatchesOnBoard()
    {
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                if (allAnimals[i, j] != null)
                {
                    if(allAnimals[i,j].GetComponent<AnimalTile>().isMatched)
                    {
                        return true;
                    }
                }
            }
        }
        return false;
    }

    public IEnumerator StartDestroyAllCo()
    {
        yield return StartCoroutine(DestroyMatches());
        currentState = GameState.MOVE;

    }

    public void StartDestroyAllNow()
    {
        StartCoroutine(StartDestroyAllCo());
    } 

    public void StartBoard()
    {
        currentState = GameState.MOVE;
    }

    public void StopBoard()
    {
        currentState = GameState.WAIT;
    }

    public GameState GetGameState()
    {
        return currentState;
    }


}
