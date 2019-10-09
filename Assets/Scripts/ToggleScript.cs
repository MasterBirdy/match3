using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToggleScript : MonoBehaviour
{
    [SerializeField] GameObject musicOn, musicOff;
    AudioSource audioSource;
    // Start is called before the first frame update

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void OnValueChange()
    {
        bool toggleValue = GetComponent<Toggle>().isOn;
        if (toggleValue)
        {
            musicOff.SetActive(false);
            musicOn.SetActive(true);
            audioSource.Play();
        }
        else
        {
            musicOn.SetActive(false);
            musicOff.SetActive(true);
            audioSource.Stop();
        }
    }
}
