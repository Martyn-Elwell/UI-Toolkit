using Codice.CM.Client.Differences;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Search;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;
using ObjectField = UnityEditor.UIElements.ObjectField;

public class CardEditor : EditorWindow
{
    public VisualTreeAsset visualTreeAsset;
    public VisualElement root;
    public List<GameObject> cardTemplate = new List<GameObject>();
    public CardTypeEnum cardType;

    private VisualElement traditionalElement;
    private VisualElement unoElement;
    private EnumField cardTypeField;

    // Traditional Cards
    private SliderInt inputField;
    private EnumField suitField;
    private ObjectField objectField;
    private Sprite objectSprite;
    private ObjectField backgroundField;

    // Basic Uno Cards
    private EnumField colourEnumField;
    private EnumField blackCardEnum;
    private SliderInt inputFieldUno;
    private ObjectField objectFieldUno;

    // Custom Uno Cards
    
    private TextField customColourTextUno;
    private ColorField customUnoColourField;


    [MenuItem("Cards/Card Editor")]
    static void CreateMenu()
    {
        var window =  GetWindow<CardEditor>("Card Editor");
    }

    private void OnEnable()
    {
        root = this.rootVisualElement;
        visualTreeAsset.CloneTree(root);

        cardTemplate.Add(AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Cards/Template/CardTemplate3D.prefab"));
        cardTemplate.Add(AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Cards/Template/UnoTemplate3D.prefab"));

        SetupUI();
    }

    private void SetupUI()
    {

        // Parent Element Setup
        traditionalElement = root.Q<VisualElement>("TraditonalElement");
        unoElement = root.Q<VisualElement>("UnoElement");
        cardTypeField = root.Q<EnumField>("CardTypeField");

        // Traditional Card Setup
        inputField = root.Q<SliderInt>("InputField");
        inputField.RegisterValueChangedCallback(OnCardValueChanged);
        objectField = root.Q<ObjectField>("ObjectField");
        backgroundField = root.Q<ObjectField>("BackgroundField");

        // Basic Uno Card Setup
        colourEnumField = root.Q<EnumField>("ColourEnumField"); // Card Colour
        colourEnumField.RegisterValueChangedCallback(OnCardColourChanged);
        blackCardEnum = root.Q<EnumField>("BlackCardOptions"); // Black Card Options
        inputFieldUno = root.Q<SliderInt>("InputFieldUno"); // Card Number
        inputFieldUno.RegisterValueChangedCallback(OnCardValueChanged);
        objectFieldUno = root.Q<ObjectField>("ObjectFieldUno"); // Card Icons

        // Custom Uno Card Setup
        customColourTextUno = root.Q<TextField>("CustomUnoTextField");
        customUnoColourField = root.Q<ColorField>("CustomUnoColourField");



        // Card Type Switcher
        cardTypeField.RegisterValueChangedCallback(OnInputTypeChanged);

        // Button Setup
        Button generateButton = rootVisualElement.Q<Button>("Generate");
        if (generateButton != null)
        {
            generateButton.clicked += OnButtonPressed;
        }
        cardType = CardTypeEnum.Traditional;
        UpdateAvailableFields();
        UpdateTradiotionalOptions(1);
        UpdateUnoOptions(1);
        UpdateBlackCardOptions(UnoEnum.Red);

    }

    private void OnInputTypeChanged(ChangeEvent<System.Enum> evt)
    {

        cardType = (CardTypeEnum)evt.newValue;
        UpdateAvailableFields();
    }

    private void UpdateAvailableFields()
    {
        // Clear or modify the UI based on the selected dropdown value
        

        if (cardType == CardTypeEnum.Traditional)
        {
            Debug.Log("Traditional");
            // Show or hide UI elements based on the selected input type
            traditionalElement.style.display = DisplayStyle.Flex;
            unoElement.style.display = DisplayStyle.None;
            

        }
        else if (cardType == CardTypeEnum.Uno)
        {
            Debug.Log("Uno");
            // Show or hide UI elements based on the selected input type
            traditionalElement.style.display = DisplayStyle.None;
            unoElement.style.display = DisplayStyle.Flex;
            
        }
        // Add more cases for other input types as needed
    }

    private void OnButtonPressed()
    {
        Debug.Log("Click");
        // Check if the prefab is assigned
        switch (cardType)
        {
            case CardTypeEnum.Traditional:
                CreateTraditionalCard();
                break;
            case CardTypeEnum.Uno:
                CreateUnoCard();
                break;

        }
    }


    private void OnCardValueChanged(ChangeEvent<int> evt)
    {

        int cardValue = (int)evt.newValue;

        switch (cardType)
        {
            case CardTypeEnum.Traditional:
                UpdateTradiotionalOptions(cardValue);
                break;
            case CardTypeEnum.Uno:
                UpdateUnoOptions(cardValue);
                break;

        }
        
    }

    private void CreateTraditionalCard()
    {
        if (cardTemplate != null)
        {
            // Instantiate the prefab
            GameObject newCard = PrefabUtility.InstantiatePrefab(cardTemplate[0]) as GameObject;

            // Edit Prefab
            suitField = root.Q<EnumField>("SuitField");
            var cardData = newCard.GetComponent<TraditionalCard>();
            cardData.value = inputField.value;
            cardData.suit = (SuitEnum)suitField.value;
            objectSprite = objectField.value as Sprite;
            if (objectSprite != null) { cardData.faceSprite = objectSprite; Debug.Log("passed"); }
            objectSprite = backgroundField.value as Sprite;
            if (objectSprite != null) { cardData.backgroundSprite = objectSprite; Debug.Log("passed"); }
            cardData.Set();


            // Save the prefab
            string folderPath = "Assets/Cards/";
            string prefabName = inputField.value + "_of_" + (SuitEnum)suitField.value + ".prefab";
            string path = folderPath + prefabName;


            PrefabUtility.SaveAsPrefabAsset(newCard, path);
            //DestroyImmediate(newCard);

            // Log or handle the creation success
            Debug.Log("Prefab created: " + path);
        }
        else
        {
            Debug.LogError("Prefab is not assigned. Please assign a prefab to the prefabToInstantiate variable.");
        }
    }

    private void CreateUnoCard()
    {
        if (cardTemplate != null)
        {
            // Instantiate the prefab
            GameObject newCard = PrefabUtility.InstantiatePrefab(cardTemplate[1]) as GameObject;

            // Edit Prefab
            var cardData = newCard.GetComponent<UnoCard>();
            cardData.value = inputFieldUno.value;
            cardData.colour = (UnoEnum)colourEnumField.value;
            objectSprite = objectField.value as Sprite;
            if (objectSprite != null){ cardData.sprite = objectSprite; Debug.Log("passed"); }
            objectSprite = backgroundField.value as Sprite;
            if (objectSprite != null) { cardData.backgroundSprite = objectSprite; Debug.Log("passed"); }
            if (customUnoColourField.value != null) { cardData.customColour = customUnoColourField.value; }
            cardData.Set();


            // Save the prefab
            string folderPath = "Assets/Cards/";
            string prefabName = "uno_" + (UnoEnum)colourEnumField.value + "_" + inputField.value + ".prefab";
            string path = folderPath + prefabName;


            PrefabUtility.SaveAsPrefabAsset(newCard, path);
            //DestroyImmediate(newCard);

            // Log or handle the creation success
            Debug.Log("Prefab created: " + path);
        }
        else
        {
            Debug.LogError("Prefab is not assigned. Please assign a prefab to the prefabToInstantiate variable.");
        }
    }

    private void OnCardColourChanged(ChangeEvent<System.Enum> evt)
    {

        UnoEnum cardColour = (UnoEnum)evt.newValue;
        UpdateBlackCardOptions(cardColour);
    }

    private void UpdateBlackCardOptions(UnoEnum cardColour)
    {
        if (cardColour == UnoEnum.Black)
        {
            objectField.style.display = DisplayStyle.None;
            inputFieldUno.style.display = DisplayStyle.None;
            blackCardEnum.style.display = DisplayStyle.Flex;

            customColourTextUno.style.display = DisplayStyle.None;
            customUnoColourField.style.display = DisplayStyle.None;
        }
        else if (cardColour == UnoEnum.Custom)
        {
            objectField.style.display = DisplayStyle.Flex;
            inputFieldUno.style.display = DisplayStyle.Flex;
            blackCardEnum.style.display = DisplayStyle.None;

            customColourTextUno.style.display = DisplayStyle.Flex;
            customUnoColourField.style.display = DisplayStyle.Flex;
        }
        else
        {
            objectField.style.display = DisplayStyle.Flex;
            inputFieldUno.style.display = DisplayStyle.Flex;
            blackCardEnum.style.display = DisplayStyle.None;

            customColourTextUno.style.display = DisplayStyle.None;
            customUnoColourField.style.display = DisplayStyle.None;
        }
    }

    private void UpdateTradiotionalOptions(int cardValue)
    {
        objectField.style.display = DisplayStyle.None;
        if ((cardValue >= 11 && cardValue <= 13) || cardValue == 0)
        {
            objectField.style.display = DisplayStyle.Flex;
        }
        else
        {
            objectField.style.display = DisplayStyle.None;
        }

    }

    private void UpdateUnoOptions(int cardValue)
    {
        objectField.style.display = DisplayStyle.None;
        if (cardValue >= 11 && cardValue <= 13)
        {
            objectFieldUno.style.display = DisplayStyle.Flex;
        }
        else
        {
            objectFieldUno.style.display = DisplayStyle.None;
        }

    }

    private void OnGUI()
    {
        // GUI rendering
    }
}
