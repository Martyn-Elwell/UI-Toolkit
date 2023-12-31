using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum SuitEnum
{
    Diamonds,
    Clubs,
    Hearts,
    Spades
}

public class TraditionalCard : MonoBehaviour
{
    public SuitEnum suit;
    public List<Image> icons;
    public Image background;
    public List<TextMeshProUGUI> texts;
    
    public int value;
    public List<Sprite> suitSprites;
    public Sprite faceSprite;
    public Sprite backgroundSprite;

    public void Set()
    {

        // Value setting
        string valueString = "";
        if (value == 1)
        {
            valueString = "A";
        }
        else if (value >= 2 && value <= 10)
        {
            valueString = value.ToString();
        }
        else if (value == 11) { valueString = "J"; }
        else if (value == 12) { valueString = "Q"; }
        else if (value == 13) { valueString = "K"; }

        foreach (TextMeshProUGUI text in texts)
        {
            text.text = valueString;
        }
        

        switch (suit)
        {
            case SuitEnum.Diamonds:
                foreach (Image icon in icons)
                {
                    icon.sprite = suitSprites[0];
                    foreach (TextMeshProUGUI text in texts)
                    {
                        text.color = Color.red;
                    }
                }
                break;
            case SuitEnum.Clubs:
                foreach (Image icon in icons)
                {
                    icon.sprite = suitSprites[1];
                }
                break;
            case SuitEnum.Hearts:
                foreach (Image icon in icons)
                {
                    icon.sprite = suitSprites[2];
                    foreach (TextMeshProUGUI text in texts)
                    {
                        text.color = Color.red;
                    }
                }
                break;
            case SuitEnum.Spades:
                foreach (Image icon in icons)
                {
                    icon.sprite = suitSprites[3];
                }
                break;
        }
        if (value == 1)
        {
            icons[1].transform.localScale = Vector3.one * 1.5f;
            texts[1].text = "";
        }
        else if (value <= 10)
        {
            CreateCircleFormation();
            texts[1].text = "";
            DestroyImmediate(icons[1]);
        }
        else if (value >= 11)
        {
            if (faceSprite != null)
            {
                texts[1].text = "";
                icons[1].sprite = faceSprite;
                icons[1].transform.localScale = Vector3.one * 8;
            }
            else
            {
            }
        }
        if (backgroundSprite != null) { background.sprite = backgroundSprite; }

        
    }

    // Update is called once per frame
    void Update()
    {

    }


    void CreateCircleFormation()
    {
        for (int i = 0; i < value; i++)
        {
            float angle = i * (360f / value);
            Vector3 spawnPosition = GetCirclePosition(angle);
            Quaternion spawnRotation = Quaternion.Euler(90, angle, 0);

            Image image = Instantiate(icons[1], spawnPosition, icons[1].transform.rotation);
            image.transform.SetParent(icons[1].transform.parent);  // Set the instantiated object as a child of the CircleFormation object
            image.transform.localScale = Vector3.one * 1.5f;
        }
    }

    Vector3 GetCirclePosition(float angle)
    {
        float radians = angle * Mathf.Deg2Rad;
        float x = Mathf.Sin(radians);
        float z = Mathf.Cos(radians);

        return new Vector3(x * 0.5f, 0, z * 0.5f) + icons[1].transform.position;
    }

}

