using NUnit.Framework;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;

public class SelectionController : MonoBehaviour
{
    public static SelectionController instance;

    public List<CharacterPlayer> selectedGameObject;
    public Image lockImage;
    public GameObject currentCharacter;
    private int selectedCharacter = 0;
    public TextMeshProUGUI nameText;
    public TMP_Text coinText;
    public TextMeshProUGUI buyButtonText;
    public Button buyButton;
    public Button playButton;
    public CharacterPlayer selectedPlayer;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        LoadUnlockedCharacter();
        CoinDisplay();
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
        int playerCoins = PlayerPrefs.GetInt("PlayerCoins", 0);
        nameText.text = Character.name;
        Destroy(currentCharacter);
        GameObject character = Instantiate(Character.prefab);
        character.transform.localScale = Character.selectionScale;
        character.transform.position = Character.selectionLocation;
        currentCharacter = character;
        if (Character.isUnlocked)
        {          
            lockImage.gameObject.SetActive(false);           
            playButton.gameObject.SetActive(true);
        }
        else
        {
            lockImage.gameObject.SetActive(true);          
            buyButtonText.text = Character.unlockCost.ToString();
            if(playerCoins < Character.unlockCost)
            {
                buyButton.interactable = false;
            }
            else
            {
                buyButton.interactable = true;
            }
            playButton.gameObject.SetActive(false);
        }
    }

    public void UnlockCharacter()
    {
        int playerCoins = PlayerPrefs.GetInt("PlayerCoins", 0);
        int cost = selectedGameObject[selectedCharacter].unlockCost;

        if (playerCoins >= cost)
        {
            playerCoins -= cost;
            PlayerPrefs.SetInt("PlayerCoins", playerCoins);
            PlayerPrefs.Save();

            PlayerPrefs.SetInt("Character_" + selectedCharacter + "_Unlocked", 1);
            PlayerPrefs.Save();

            lockImage.gameObject.SetActive(false);           
            playButton.gameObject.SetActive(true);

            CoinDisplay();
        }
        //else
        //{
        //    StartCoroutine(ShowMessage("Bơm tiền vào mà mua =))", 5f));
        //    Debug.Log("Không đủ coins để mở khóa map này! Cần ít nhất 1000 coins.");
        //}
    }

    private void CoinDisplay()
    {
        if (coinText != null)
        {
            int playerCoins = PlayerPrefs.GetInt("PlayerCoins", 0);
            coinText.text = "" + playerCoins;
        }
    }

    public void PlayGame()
    {
        string mapName = MapSellectionController.instance.mapName;
        selectedPlayer = selectedGameObject[selectedCharacter];
        SceneManager.LoadScene(mapName);
    }

   



    public void BackScreen()
    {
        Destroy(MapSellectionController.instance.gameObject);
        Destroy(gameObject);
        SceneManager.LoadScene("ChooseMap");
    }

    private void LoadUnlockedCharacter()
    {
        for (int i = 1; i < selectedGameObject.Count; i++)
        {
            bool isUnlocked = PlayerPrefs.GetInt("Character_" + i + "_Unlocked", 0) == 1;
            selectedGameObject[i].isUnlocked = isUnlocked;          
        }
    }
}
