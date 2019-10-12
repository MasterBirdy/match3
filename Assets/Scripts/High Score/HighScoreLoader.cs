using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HighScoreLoader : MonoBehaviour
{
    Vector3 tempPosition;
    HighScoreData highScore;
    // Start is called before the first frame update
    void Start()
    {

        Transform template = gameObject.transform.Find("High Score Entry");
        template.gameObject.SetActive(false);

        highScore = SaveSystem.LoadHighScore();
        for (int i = 0; i < highScore.animalClass.Length; i++)
        {
            int padding = -70;
            if (highScore.highScores[i] != 0)
            {
                Transform tempVar = Instantiate(template, transform);
                tempVar.position = new Vector3(tempVar.position.x, tempVar.position.y + (padding * i), tempVar.position.z);
                tempVar.Find("Position Text").GetComponent<TextMeshProUGUI>().text = i + 1 + ".";
                tempVar.Find("Score Text").GetComponent<TextMeshProUGUI>().text = highScore.highScores[i] + "";
                TextMeshProUGUI roleText = tempVar.Find("Role Text").GetComponent<TextMeshProUGUI>();
                ColorPicker(roleText, highScore.animalClass[i]);
                roleText.text = highScore.animalClass[i] + "";
                tempVar.gameObject.SetActive(true);
            }
        }
    }

    private void ColorPicker(TextMeshProUGUI g, string s)
    {
        if (s.Equals("Snake"))
        {
            g.color = new Color32(135, 255, 28, 255);
        }
        else if (s.Equals("Penguin"))
        {
            g.color = new Color32(200, 200, 200, 255);
        }
    }
}
