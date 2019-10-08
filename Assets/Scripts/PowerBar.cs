using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PowerBar : MonoBehaviour
{
    float powerLevel = .01f;
    private float lerpSpeed = 2.5f;
    Transform bar;
    [SerializeField] public Image healthBar;
    [SerializeField] public GameObject explosion;
    [SerializeField] public GameObject cloud;
    [SerializeField] public GameObject text;
    [SerializeField] public Image icon;
    [SerializeField] public GameObject[] classesOfAnimals;
    private AnimalClass classOfAnimal;


    private Camera cam;
    private DataTracker dataTracker;

    // Start is called before the first frame update
    void Start()
    {
        dataTracker = FindObjectOfType<DataTracker>();
        cam = FindObjectOfType<Camera>();
        healthBar.fillAmount = .01f;
     //   Instantiate(animalRole, transform.position, Quaternion.identity);
        classOfAnimal = classesOfAnimals[0].GetComponent<AnimalClass>();
        icon.sprite = classOfAnimal.ReturnSprite();
    }

    // Update is called once per frame
    void Update()
    {
        HandleBar();
        if (Input.GetMouseButtonDown(1) && healthBar.fillAmount == 1f)
            Activate();


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

 
}
