using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PowerBar : MonoBehaviour
{
    float powerLevel;
    private float lerpSpeed = 2.5f;
    Transform bar;
    [SerializeField] public Image healthBar;
    [SerializeField] public GameObject explosion;
    [SerializeField] public GameObject cloud;
    [SerializeField] public GameObject text;
    [SerializeField] public Image icon;
    [SerializeField] public GameObject[] classesOfAnimals;
    [SerializeField] public Sprite[] classesOfAnimalImages;
    [SerializeField] public GameObject alertIcon;
    public Image animalImage;
    private GameObject currentAlert;
    private byte red;
    private AnimalClass classOfAnimal;


    private Camera cam;
    private DataTracker dataTracker;
    private Board board;
    private bool isUp;
    private bool isLeft;
    private bool powerReady = false;
    private CharacterData characterData;
    private int characterLevel;

    // Start is called before the first frame update
    void Start()
    {
        dataTracker = FindObjectOfType<DataTracker>();
        cam = FindObjectOfType<Camera>();
        board = FindObjectOfType<Board>();
        powerLevel =  .01f;
        healthBar.fillAmount = .01f;
        characterData = SaveSystem.LoadCharacterData();
        classOfAnimal = classesOfAnimals[characterData.currentCharacter].GetComponent<AnimalClass>();
        characterLevel = characterData.levels[characterData.currentCharacter];
        icon.sprite = classOfAnimal.ReturnSprite();
        animalImage.sprite = classesOfAnimalImages[characterData.currentCharacter];
        isLeft = true;
        Vector3 tempVector = Camera.main.ScreenToWorldPoint(icon.transform.position);
        tempVector = new Vector3(tempVector.x, tempVector.y + 1.03f, 0);
        currentAlert = Instantiate(alertIcon, tempVector, Quaternion.identity);
        currentAlert.GetComponent<ParticleSystem>().Pause();
        currentAlert.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

        if (powerLevel != healthBar.fillAmount)
        {
            healthBar.fillAmount = Mathf.Lerp(healthBar.fillAmount, powerLevel * .01f, Time.deltaTime * lerpSpeed);
        }
        if (healthBar.fillAmount == 1f)
        {
            if (!powerReady)
            {
                red = 255;
                isUp = false;
                powerReady = true;
                currentAlert.SetActive(true);
            } 
            if (isUp)
            {
                float tempRed = red + Time.deltaTime * 300f;
                red = Convert.ToByte(Mathf.RoundToInt(Mathf.Min(tempRed, 255)));
                healthBar.color = new Color32(255, red, red, 255);
                if (red == 255)
                {
                    isUp = false;
                }
            }
            else
            {
                float tempRed = red - Time.deltaTime * 300f;
                red = Convert.ToByte(Mathf.RoundToInt(Mathf.Max(tempRed, 0)));
                healthBar.color = new Color32(255, red, red, 255);
                if (red == 0)
                {
                    isUp = true;
                }
            }
        }

        if (Input.GetMouseButtonDown(1) && healthBar.fillAmount == 1f)
        {
            Activate();
        }

        if(dataTracker.GetSessionState() != SessionState.NOTSTARTED)
        {
            if (isLeft)
             {
                Quaternion quar = Quaternion.Euler(0, 0, 6.2f);
                icon.transform.localRotation = Quaternion.Lerp(icon.transform.localRotation, quar, Time.deltaTime * 1.2f);
                 if (icon.transform.localRotation.z > .05f)
                     isLeft = false;
             }
             else
             {
                Quaternion quar = Quaternion.Euler(0, 0, -6.2f);
                icon.transform.localRotation = Quaternion.Lerp(icon.gameObject.transform.localRotation, quar, Time.deltaTime * 1.2f);
                if (icon.transform.localRotation.z < -.05f)
                    isLeft = true;
            }
        }

    }

    public void IncreasePowerLevel(float i)
    {
        powerLevel += Mathf.Clamp(i, .01f, 100);
    }

    private void Activate()
    {
        healthBar.color = new Color32(255, 255, 255, 255);
        currentAlert.SetActive(false);
        powerLevel = .01f;
        healthBar.fillAmount = .01f;
        StartCoroutine(PowerAnimation());
        board.activatedPower = true;
        classOfAnimal.ActivatePower(characterLevel);
        if (classOfAnimal.HasTimeExtension())
        {
            dataTracker.ExtendTime(5);
            Vector3 tempVector = cloud.transform.position;
            tempVector.x = cloud.transform.position.x + .5f;
            GameObject explode = Instantiate(explosion, tempVector, Quaternion.identity);
            Destroy(explode, 2f);
        }
        powerReady = false;
    }

    public string ReturnAnimalName()
    {
        return classOfAnimal.ReturnName();
    }

    private IEnumerator PowerAnimation()
    {
        for (int i = (int) animalImage.transform.position.x; i > 713 ;i -= 3)
        {
            animalImage.transform.position = new Vector3(i, animalImage.transform.position.y, animalImage.transform.position.z);
            yield return new WaitForSeconds(.008f);
        }
        yield return new WaitForSeconds(1.3f);
        for (int i = (int)animalImage.transform.position.x; i <900; i++)
        {
            animalImage.transform.position = new Vector3(i, animalImage.transform.position.y, animalImage.transform.position.z);
            yield return new WaitForSeconds(.0015f);
        }

    }
}
