using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStart : MonoBehaviour
{
    CharacterData cd;
    // Start is called before the first frame update
    void Start()
    {
        if (SaveSystem.LoadCharacterData() == null)
        {
            SaveSystem.SaveCharacterData(new CharacterData());
        }
    }

    // Update is called once per frame
}
