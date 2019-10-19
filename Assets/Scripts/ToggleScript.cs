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
        if (AudioManager.instance.isMuted == true)
        {
            musicOn.SetActive(false);
            musicOff.SetActive(true);
            GetComponent<Toggle>().isOn = false;
        }
    }

    public void OnValueChange()
    {
        bool toggleValue = GetComponent<Toggle>().isOn;
        if (toggleValue)
        {
            musicOff.SetActive(false);
            musicOn.SetActive(true);
            AudioManager.instance.isMuted = false;
            AudioManager.instance.PlayMusic(0);
        }
        else
        {
            musicOn.SetActive(false);
            musicOff.SetActive(true);
            AudioManager.instance.isMuted = true;
            AudioManager.instance.StopMusic();
        }
    }
}
