using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CharacterLoader : MonoBehaviour
{
    CharacterData characterData;
    int chosenCharacter;
    Button button;
    [SerializeField] GameObject selection;
    GameObject currentSelection;
    SceneLoader sceneLoader;
    bool isUp;
    private float lerpSpeed = 1.05f;
    private float starPadding = 40f;
    [SerializeField] GameObject star;
    [SerializeField] GameObject starHolder;
    [SerializeField] GameObject textBox;
    [SerializeField] Image whiteFilling;
    [SerializeField] TextMeshProUGUI levelText;
    [SerializeField] Image animalImage;
    [SerializeField] Sprite[] sprites;
    TextMeshProUGUI textAnimal;
    Canvas canvas;

    // Start is called before the first frame update
    void Start()
    {
        sceneLoader = FindObjectOfType<SceneLoader>();
        canvas = FindObjectOfType<Canvas>();
        characterData = SaveSystem.LoadCharacterData();
        chosenCharacter = characterData.currentCharacter;
        textAnimal = textBox.GetComponent<TextMeshProUGUI>();
        isUp = true;


        setUpButton(chosenCharacter);
        animalImage.sprite = sprites[chosenCharacter];
        levelText.text = "Lvl. " + characterData.levels[chosenCharacter];
        whiteFilling.fillAmount = GetPercentage();


        StarLoader(starHolder.transform);
        for (int i = 0; i < characterData.experience.Length; i++)
        {
            Debug.Log(characterData.experience[i]);
        }
    }

    private void Update()
    {
        if  (isUp)
        {
            float f = Mathf.Lerp(button.transform.localScale.x, 1.40f, Time.deltaTime * lerpSpeed);
            if (f > 1.3f)
                f = 1.3f;
            button.transform.localScale = new Vector3(f, f, button.transform.localScale.z);
            if (f == 1.3f)
            {
                isUp = false;
            }
        }
        else
        {
            float f = Mathf.Lerp(button.transform.localScale.x, .90f, Time.deltaTime * lerpSpeed);
            if (f < 1f)
                f = 1f;
            button.transform.localScale = new Vector3(f, f, button.transform.localScale.z);
            if (f == 1f)
            {
                isUp = true;
            }
        }
    }

    // Update is called once per frame
    public void ButtonChoose(int i)
    {
        chosenCharacter = i;
        characterData.currentCharacter = i;
        setUpButton(chosenCharacter);
        animalImage.sprite = sprites[chosenCharacter];
        levelText.text = "Lvl. " + characterData.levels[chosenCharacter];
        whiteFilling.fillAmount = GetPercentage();
    }

    private void setUpButton(int i)
    {
        if (currentSelection != null)
            Destroy(currentSelection);
        if (button != null)
            button.transform.localScale = new Vector3(1, 1, button.transform.localScale.z);
        switch (i)
        {
            case 0:
                button = GameObject.Find("Snake Button").GetComponent<Button>();
                textAnimal.text = "Snake slithers through the grid and clears rows of\nballs!";
                break;
            case 1:
                button = GameObject.Find("Penguin Button").GetComponent<Button>();
                textAnimal.text = "Penguin slides down the\nicy grid and weaves in\nand out!";
                break;
            case 2:
                button = GameObject.Find("Giraffe Button").GetComponent<Button>();
                textAnimal.text = "Giraffe stretches his tall\nneck and takes down\ncolumns of balls!";
                break;
            case 3:
                button = GameObject.Find("Panda Button").GetComponent<Button>();
                textAnimal.text = "Panda finds himself and\nhis friends and clears\nthem off the grid!";
                break;
            case 4:
                button = GameObject.Find("Monkey Button").GetComponent<Button>();
                textAnimal.text = "Monkey creates a banana\non the board and clears\nit!";
                break;
        }
        Vector3 tempVector = Camera.main.ScreenToWorldPoint(button.transform.position);
        tempVector.z = 0;
        currentSelection = Instantiate(selection, tempVector, Quaternion.identity);
    }

    public void SaveChosenButtonAndExit()
    {
        SaveSystem.SaveCharacterData(characterData);
        sceneLoader.LoadStartScene();
    }

    private void StarLoader(Transform t)
    {
        int j = 0;
        foreach (Transform child in t)
        {
            foreach (Transform childchild in child)
            {
                Vector3 tempVector = childchild.transform.position;
                childchild.gameObject.SetActive(false);
                for (int i = 0; i < characterData.levels[j]; i++)
                {
                    tempVector.x = starPadding * i + childchild.transform.position.x;
                    GameObject g = Instantiate(childchild.gameObject, tempVector, Quaternion.identity);
                    g.transform.parent = canvas.transform;
                    g.SetActive(true);
                }
                j++;
            }
        }
    }
    private float GetPercentage()
    {
        if (characterData.levels[chosenCharacter] == 1)
        {
            float t = (float) characterData.experience[chosenCharacter] / CharacterData.Level2;
            if (t == 0)
                t = .01f;
            return t;
        }
        else if (characterData.levels[chosenCharacter] == 2)
        {
            float t = (float) characterData.experience[chosenCharacter] / CharacterData.Level3;
            if (t == 0)
                t = .01f;
            return t;
        }
        else
            return 1f;
    }
}
