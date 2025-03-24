using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class MapSellectionController : MonoBehaviour
{
    public static MapSellectionController instance;

    public List<Map> mapSprites;
    public Image mapImage;
    public Image lockImage;
    public Button chooseCharacterButton;
    public int selectedMap = 0;
    public TextMeshProUGUI nameText;
    public string mapName;

    //public Button unlockButton;

    public CharacterPlayer selectedPlayer;
     
    public TMP_Text warningText;
    public TMP_Text coinText;

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
        LoadUnlockedMaps();
        //CoinDisplay();
        changeMap(mapSprites[selectedMap]);
    }

    public void NextOption()
    {
        selectedMap += 1;
        if (selectedMap == mapSprites.Count)
        {
            selectedMap = 0;
        }
        changeMap(mapSprites[selectedMap]);
    }

    public void BackOption()
    {
        selectedMap -= 1;
        if (selectedMap < 0)
        {
            selectedMap = mapSprites.Count - 1;
        }
        changeMap(mapSprites[selectedMap]);
    }

    private void changeMap(Map map)
    {
        nameText.text = map.name;
        mapImage.sprite = map.mapSprites;

        if (map.isUnlocked)
        {
            lockImage.gameObject.SetActive(false);
            chooseCharacterButton.interactable = true;
        }
        else
        {
            lockImage.gameObject.SetActive(true);
            chooseCharacterButton.interactable = false;
        }
    }

    public void UnlockMap()
    {      
                  
            PlayerPrefs.SetInt("Map_" + selectedMap + "_Unlocked", 1);
            PlayerPrefs.Save();

            mapSprites[selectedMap].isUnlocked = true;
            lockImage.gameObject.SetActive(false);
            chooseCharacterButton.interactable = true;
            //changeMap(mapSprites[selectedMap]); 
         
       
    }

    //public void UnlockMap()
    //{
    //    int playerCoins = PlayerPrefs.GetInt("PlayerCoins", 0);
    //    int cost = mapSprites[selectedMap].unlockCost;

    //    if (playerCoins >= cost)
    //    {
    //        playerCoins -= cost;
    //        PlayerPrefs.SetInt("PlayerCoins", playerCoins);
    //        PlayerPrefs.Save();

    //        PlayerPrefs.SetInt("Map_" + selectedMap + "_Unlocked", 1);
    //        PlayerPrefs.Save();

    //        mapSprites[selectedMap].isUnlocked = true;
    //        //unlockButton.gameObject.SetActive(false); 

    //        //changeMap(mapSprites[selectedMap]); 

    //        CoinDisplay();
    //    }
    //    else
    //    {
    //        StartCoroutine(ShowMessage("Bơm tiền vào mà mua =))", 5f));
    //        Debug.Log("Không đủ coins để mở khóa map này! Cần ít nhất 1000 coins.");
    //    }
    //}

    private void LoadUnlockedMaps()
    {
        for (int i = 1; i < mapSprites.Count; i++)
        {
            bool isUnlocked = PlayerPrefs.GetInt("Map_" + i + "_Unlocked", 0) == 1;
            mapSprites[i].isUnlocked = isUnlocked;

            if (isUnlocked)
            {
                lockImage.gameObject.SetActive(false);
                chooseCharacterButton.interactable = true;
            }                    
        }
    }


    void CoinDisplay()
    {
        if (coinText != null)
        {
            int playerCoins = PlayerPrefs.GetInt("PlayerCoins", 0);
            coinText.text = "" + playerCoins;
        }
    }

    public void NextScreen()
    {
        if (mapSprites[selectedMap].isUnlocked)
        {
            mapName = mapSprites[selectedMap].name;
            SceneManager.LoadScene("ChooseCharacter");
        }      
    }

    IEnumerator ShowMessage(string message, float duration)
    {
        warningText.text = message;
        warningText.enabled = true;
        yield return new WaitForSeconds(duration);
        warningText.enabled = false;
    }

    public void BackScreen()
    {
        Destroy(gameObject);
        SceneManager.LoadScene("Menu");
    }

    public void clearData()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();
    }
}


//public class MapSellectionController : MonoBehaviour
//{
//    public static MapSellectionController instance;

//    public List<Map> mapSprites;
//    public Image mapImage;
//    private int selectedMap = 0;
//    public TextMeshProUGUI nameText;
//    public string mapName;

//    public CharacterPlayer selectedPlayer;
//    // Start is called once before the first execution of Update after the MonoBehaviour is created

//    private void Awake()
//    {
//        if (instance == null)
//        {
//            instance = this;
//            DontDestroyOnLoad(gameObject); // Gi? object này khi ??i scene
//        }
//        else
//        {
//            Destroy(gameObject);
//        }
//    }

//    private void Start()
//    {
//        changeMap(mapSprites[selectedMap]);
//    }
//    public void NextOption()
//    {
//        selectedMap += 1;
//        if (selectedMap == mapSprites.Count)
//        {
//            selectedMap = 0;
//        }
//        changeMap(mapSprites[selectedMap]);
//    }

//    public void BackOption()
//    {
//        selectedMap -= 1;
//        if (selectedMap < 0)
//        {
//            selectedMap = mapSprites.Count - 1;
//        }
//        changeMap(mapSprites[selectedMap]);
//    }

//    private void changeMap(Map Map)
//    {
//        nameText.text = Map.name;
//        mapImage.sprite = Map.mapSprites;
//    }
//    public void NextScreen()
//    {
//        mapName = mapSprites[selectedMap].name;
//        SceneManager.LoadScene("ChooseCharacter");
//    }

//    public void BackScreen()
//    {
//        Destroy(gameObject);
//        SceneManager.LoadScene("Menu");

//    }
//}
