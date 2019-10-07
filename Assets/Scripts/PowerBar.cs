using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PowerBar : MonoBehaviour
{
    float powerLevel = .01f;
    private float lerpSpeed = 2.3f;
    Transform bar;
    [SerializeField] public Image healthBar;

    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        HandleBar();
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
}
