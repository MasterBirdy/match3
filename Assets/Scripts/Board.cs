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
                Vector2 tempPosition = new Vector2(x, y) 
                   + new Vector2(transform.position.x - (((width - 1) / 2) + ((width - 1) /2) * padding),
                   transform.position.y - (((height - 1) / 2) + ((height - 1) / 2) * padding))
                   + new Vector2(padding * x, padding * y);
                var newAnimalTile = Instantiate(animalChoices[Random.Range(0, animalChoices.Length)], tempPosition, Quaternion.identity) as GameObject;
                newAnimalTile.transform.parent = this.transform;
                newAnimalTile.name = (x + " " + y);
                allAnimals[(int)x, (int)y] = newAnimalTile;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
