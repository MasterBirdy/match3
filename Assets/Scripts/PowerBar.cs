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
    private byte red;
    private AnimalClass classOfAnimal;


    private Camera cam;
    private DataTracker dataTracker;
    private bool isUp;
    private bool powerReady = false;

    // Start is called before the first frame update
    void Start()
    {
        dataTracker = FindObjectOfType<DataTracker>();
        cam = FindObjectOfType<Camera>();
        powerLevel =  80f;
        healthBar.fillAmount = .80f;
        classOfAnimal = classesOfAnimals[4].GetComponent<AnimalClass>();
        icon.sprite = classOfAnimal.ReturnSprite();
    }

    // Update is called once per frame
    void Update()
    {
        //HandleBar();

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
            healthBar.color = new Color32(255, 255, 255, 255);
        }

    }

    public void IncreasePowerLevel(float i)
    {
        powerLevel += Mathf.Clamp(i, .01f, 100);
    }

    private void HandleBar()
    {
        if (powerLevel != healthBar.fillAmount)
        {
            healthBar.fillAmount = Mathf.Lerp(healthBar.fillAmount, powerLevel * .01f, Time.deltaTime * lerpSpeed);
        }
        if (healthBar.fillAmount == 1f)
        {
            Debug.Log(healthBar.color.r);
            Debug.Log(Time.deltaTime);
            if (isUp)
            {
                float tempRed = healthBar.color.r + Time.deltaTime * 200f;
                int red = Mathf.RoundToInt(Mathf.Clamp(tempRed, 0, 255));
                healthBar.color = new Color32(Convert.ToByte(red), 255, 255, 255);
                if (healthBar.color.r == 255)
                {
                    isUp = false;
                }
            }
            else
            {
                float tempRed = healthBar.color.r - Time.deltaTime * 200f;
                int red = Mathf.RoundToInt(Mathf.Clamp(tempRed, 0, 255));
                healthBar.color = new Color32(Convert.ToByte(red), 255, 255, 255);
                if (healthBar.color.r == 0)
                {
                    isUp = true;
                }
            }
        }
    }

    private void Activate()
    {
        classOfAnimal.ActivatePower();
        if (classOfAnimal.HasTimeExtension())
        {
            dataTracker.ExtendTime(5);
            Vector3 tempVector = cloud.transform.position;
            tempVector.x = cloud.transform.position.x + .5f;
            GameObject explode = Instantiate(explosion, tempVector, Quaternion.identity);
            Destroy(explode, 2f);
        }
        powerLevel = .01f;
        healthBar.fillAmount = .01f;
    }

    public string ReturnAnimalName()
    {
        return classOfAnimal.ReturnName();
    }
}
