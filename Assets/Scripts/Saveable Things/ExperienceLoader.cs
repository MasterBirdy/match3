using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ExperienceLoader : MonoBehaviour
{

    private HighScoreData highScoreData;
    private CharacterData characterData;
    private int currentCharacter;
    private int characterLevel;
    private int currentExp;
    private int currentScore;
    private int newExpTotal = 0;
    private bool levelGained = false;

    [SerializeField] public Sprite[] animalIconChoices;
    [SerializeField] public Image healthBar;
    [SerializeField] public Image profileIcon;
    [SerializeField] public TextMeshProUGUI scoreTextObject;
    [SerializeField] public TextMeshProUGUI levelTextObject;
    [SerializeField] public GameObject explosion;


    private void Awake()
    {
        highScoreData = SaveSystem.LoadHighScore();
        characterData = SaveSystem.LoadCharacterData();
        currentCharacter = characterData.currentCharacter;
        characterLevel = characterData.levels[currentCharacter];
        currentExp = characterData.experience[currentCharacter];
        currentScore = highScoreData.currentScore;
    }

    // Start is called before the first frame update
    void Start()
    {
        profileIcon.sprite = animalIconChoices[currentCharacter];
        healthBar.fillAmount = SetPercentage(currentExp);
        scoreTextObject.text = "Score: " + currentScore;
        levelTextObject.text = "Level: " + characterLevel;
        newExpTotal = currentExp + currentScore;
        levelGained = characterData.AddExperience(currentScore);
        SaveSystem.SaveCharacterData(characterData);
        StartCoroutine(AddExperience());
    }

    // Update is called once per frame

    private IEnumerator AddExperience()
    {
        yield return new WaitForSeconds(1f);
        int i = currentScore;
        int j = currentExp;
        int difference = 100;
        AudioManager.instance.PlayExperienceGainedSound();
         while (i > 0)
         {
             i -= difference;
             if (i < 0)
                 i = 0;
             scoreTextObject.text = "Score: " + Mathf.RoundToInt(i);
             j += difference;
             float testPercentage = SetPercentage(j);
             if (testPercentage > 1f)
                 testPercentage = 1f;
             healthBar.fillAmount = testPercentage;
            if (!AudioManager.instance.isPlayingSound)
                StartCoroutine(AudioManager.instance.PlayExperienceGainedSound());
            yield return new WaitForSeconds(.005f);
        }
        yield return new WaitForSeconds(1f);
        if (levelGained)
        {
            explosion.SetActive(true);
            AudioManager.instance.PlayLevelUpSound();
            levelTextObject.text = "Level: " + (characterLevel + 1);
        }

    }

    private float SetPercentage(float i)
    {
        if (characterLevel == 1)
        {
            return i / CharacterData.Level2;
        }
        else if (characterLevel == 2)
        {
            return i / CharacterData.Level3;
        }
        else
            return 1f;
    }
}
