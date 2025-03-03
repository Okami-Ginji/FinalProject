using NUnit.Framework;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SelectionController : MonoBehaviour
{
    public static SelectionController instance;

    public List<CharacterPlayer> selectedGameObject;
    public GameObject currentCharacter;
    private int selectedCharacter = 0;
    public TextMeshProUGUI nameText;

    public CharacterPlayer selectedPlayer;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Gi? object này khi ??i scene
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        changeCharacter(selectedGameObject[selectedCharacter]);
    }
    public void NextOption()
    {
        selectedCharacter += 1;
        if (selectedCharacter == selectedGameObject.Count)
        {
            selectedCharacter = 0;
        }
        changeCharacter(selectedGameObject[selectedCharacter]);  
    }

    public void BackOption()
    {
        selectedCharacter -= 1;
        if (selectedCharacter < 0)
        {
            selectedCharacter = selectedGameObject.Count - 1;
        }
        changeCharacter(selectedGameObject[selectedCharacter]);
    }

    private void changeCharacter(CharacterPlayer Character)
    {
        nameText.text = Character.name;
        Destroy(currentCharacter);
        GameObject character = Instantiate(Character.prefab);
        character.transform.localScale = Character.selectionScale;
        character.transform.position = Character.selectionLocation;
        currentCharacter = character;
    }
    public void PlayGame()
    {
        selectedPlayer = selectedGameObject[selectedCharacter];
        SceneManager.LoadScene("Map");
    }
}
