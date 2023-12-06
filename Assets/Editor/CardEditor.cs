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
    private TextField inputField;
    private DropdownField cardType;
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
        inputField = root.Q<TextField>("inputField");
        cardType = root.Q<DropdownField>("CardTypeField");

        traditionalElement = root.Q<VisualElement>("TraditonalElement");
        unoElement = root.Q<VisualElement>("UnoElement");

        // Subscribe to the change event of the dropdown
        cardType.RegisterValueChangedCallback(OnInputTypeChanged);

        Button generateButton = rootVisualElement.Q<Button>("Generate");
        if (generateButton != null)
        {
            generateButton.clicked += OnButtonPressed;
        }

        // Set the initial available fields based on the default dropdown value
        UpdateAvailableFields();

    }

    private void OnInputTypeChanged(ChangeEvent<string> evt)
    {
        // Handle dropdown value change
        UpdateAvailableFields();
    }

    private void UpdateAvailableFields()
    {
        // Clear or modify the UI based on the selected dropdown value
        string selectedInputType = cardType.value;

        if (selectedInputType == "Traditional")
        {
            Debug.Log("Traditional");
            // Show or hide UI elements based on the selected input type
            traditionalElement.style.display = DisplayStyle.Flex;
            unoElement.style.display = DisplayStyle.None;
            // Additional UI elements for text input...

            // Hide or clear UI elements not needed for this input type
            //...

        }
        else if (selectedInputType == "Uno")
        {
            Debug.Log("Uno");
            // Show or hide UI elements based on the selected input type
            traditionalElement.style.display = DisplayStyle.None;
            unoElement.style.display = DisplayStyle.Flex;
            // Additional UI elements for number input...

            // Hide or clear UI elements not needed for this input type
            //...
        }
        // Add more cases for other input types as needed
        //...
    }

    private void OnButtonPressed()
    {
        Debug.Log("Click");
        // Check if the prefab is assigned
        if (cardTemplate != null)
        {
            // Instantiate the prefab
            GameObject newCard = PrefabUtility.InstantiatePrefab(cardTemplate) as GameObject;

            string folderPath = "Assets/Cards/";
            string prefabName = "NewPrefab.prefab";
            string path = folderPath + prefabName;

            // Save the prefab
            PrefabUtility.SaveAsPrefabAsset(newCard, path);

            // Log or handle the creation success
            Debug.Log("Prefab created: " + path);

            // Save the prefab
            PrefabUtility.SaveAsPrefabAsset(newCard, path);

            // Destroy the instantiated GameObject in the scene
            DestroyImmediate(newCard);

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
