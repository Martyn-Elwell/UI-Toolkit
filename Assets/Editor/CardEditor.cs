using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class CardEditor : EditorWindow
{
    public VisualTreeAsset visualTreeAsset;
    public VisualElement root;
    public GameObject cardTemplate;
    private EnumField cardTypeField;
    private SliderInt inputField;
    private EnumField suitField;
    private VisualElement traditionalElement;
    private VisualElement unoElement;

    [MenuItem("Cards/Card Editor")]
    static void CreateMenu()
    {
        var window =  GetWindow<CardEditor>("Card Editor");
    }

    private void OnEnable()
    {
        root = this.rootVisualElement;
        visualTreeAsset.CloneTree(root);

        SetupUI();
    }

    private void SetupUI()
    {

        // Find the input field and dropdown in the UXML hierarchy
        cardTypeField = root.Q<EnumField>("CardTypeField");
        inputField = root.Q<SliderInt>("InputField");
        

        traditionalElement = root.Q<VisualElement>("TraditonalElement");
        //unoElement = root.Q<VisualElement>("UnoElement");

        cardTypeField.RegisterValueChangedCallback(OnInputTypeChanged);

        Button generateButton = rootVisualElement.Q<Button>("Generate");
        if (generateButton != null)
        {
            generateButton.clicked += OnButtonPressed;
        }

        // Set the initial available fields based on the default dropdown value
        UpdateAvailableFields(CardTypeEnum.Traditional);

    }

    private void OnInputTypeChanged(ChangeEvent<System.Enum> evt)
    {

        CardTypeEnum cardType = (CardTypeEnum)evt.newValue;
        UpdateAvailableFields(cardType);
    }

    private void UpdateAvailableFields(CardTypeEnum cardType)
    {
        // Clear or modify the UI based on the selected dropdown value
        

        if (cardType == CardTypeEnum.Traditional)
        {
            Debug.Log("Traditional");
            // Show or hide UI elements based on the selected input type
            traditionalElement.style.display = DisplayStyle.Flex;
            //unoElement.style.display = DisplayStyle.None;
            

        }
        else if (cardType == CardTypeEnum.Uno)
        {
            Debug.Log("Uno");
            // Show or hide UI elements based on the selected input type
            traditionalElement.style.display = DisplayStyle.None;
            //unoElement.style.display = DisplayStyle.Flex;
            
        }
        // Add more cases for other input types as needed
    }

    private void OnButtonPressed()
    {
        Debug.Log("Click");
        // Check if the prefab is assigned
        if (cardTemplate != null)
        {
            // Instantiate the prefab
            GameObject newCard = PrefabUtility.InstantiatePrefab(cardTemplate) as GameObject;

            // Edit Prefab
            suitField = root.Q<EnumField>("SuitField");
            var cardData = newCard.GetComponent<TraditionalCard>();
            cardData.value = inputField.value;
            cardData.suit = (SuitEnum)suitField.value;
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

    private void OnGUI()
    {
        // GUI rendering
    }
}
