using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{

    public int height = 7;
    public int width = 7;
    public float padding = .1f;
    [SerializeField] private GameObject[] animalChoices;
    public GameObject[,] allAnimals;
    //private BackgroundTile[,];
    [SerializeField] GameObject tile;
    // Start is called before the first frame update
    void Start()
    {
        allAnimals = new GameObject[width, height];
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



    private void DestroyMatchesAt(int column, int row)
    {
        if (allAnimals[column, row].GetComponent<AnimalTile>().isMatched)
        {
            Destroy(allAnimals[column, row]);
            allAnimals[column, row] = null;
        }
    }

    public void DestroyMatches()
    {
        for (int i = 0; i < height; i++)
        {
            for (int j = 0; j < width; j++)
            {
                DestroyMatchesAt(j, i);
            }
        }
        StartCoroutine(RefillBoard());
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
                    var tempAnimal = allAnimals[i, j].GetComponent<AnimalTile>();
                    tempAnimal.row -= nullCount;
                    tempAnimal.targetY -= (padding + 1) * nullCount;
                    tempAnimal.previousTargetY = tempAnimal.targetY;
                    allAnimals[i, j - nullCount] = allAnimals[i, j];
                    allAnimals[i, j] = null;
                }
            }
        }
        yield return new WaitForSeconds(.4f);

    }

    private IEnumerator RefillBoard()
    {
        yield return StartCoroutine(DecreaseRowCo());
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
 


    // Update is called once per frame
    void Update()
    {
        
    }
}
