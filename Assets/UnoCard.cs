using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum UnoEnum
{
    Red,
    Green,
    Blue,
    Yellow,
    Black,
    Custom
}

public enum BlackEnum
{
    Colour,
    Plus4
}

public class UnoCard : MonoBehaviour
{
    public UnoEnum colour;
    public Image panel;
    public int value;
    public Image background;
    public List<TextMeshProUGUI> texts;
    public List<Image> icons;
    public Sprite sprite;
    public string identity;
    public Color customColour;


    public Sprite backgroundSprite;

    public void Set()
    {

        // Value setting
        if (value <= 10 && colour != UnoEnum.Black)
        {
            string valueString = value.ToString();

            foreach (TextMeshProUGUI text in texts)
            {
                text.text = valueString;
            }


            foreach (Image icon in icons)
            {
                DestroyImmediate(icon);
            }
        }
        else if (value >= 11 || colour == UnoEnum.Black)
        {
            foreach (TextMeshProUGUI text in texts)
            {
                text.text = "";
            }
            foreach (Image icon in icons)
            {
                icon.sprite = sprite;
            }
        }
        
        //  Colour Setting
        switch (colour)
        {
            case UnoEnum.Red:
                panel.color = Color.red;
                break;
            case UnoEnum.Green:
                panel.color = Color.green;
                break;
            case UnoEnum.Blue:
                panel.color = Color.blue;
                break;
            case UnoEnum.Yellow:
                panel.color = Color.yellow;
                break;
            case UnoEnum.Black:
                panel.color = Color.blue;
                break;
            case UnoEnum.Custom:
                panel.color = customColour;
                break;

        }
        if (backgroundSprite != null) { background.sprite = backgroundSprite; }

        
    }

    // Update is called once per frame
    void Update()
    {

    }

}

