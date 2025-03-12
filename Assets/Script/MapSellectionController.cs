using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MapSellectionController : MonoBehaviour
{
    public static MapSellectionController instance;

    public List<Map> mapSprites;
    public Image mapImage;
    private int selectedMap = 0;
    public TextMeshProUGUI nameText;
    public string mapName;

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

    private void changeMap(Map Map)
    {
        nameText.text = Map.name;
        mapImage.sprite = Map.mapSprites;
    }
    public void NextScreen()
    {
        mapName = mapSprites[selectedMap].name;
        SceneManager.LoadScene("ChooseCharacter");
    }

    public void BackScreen()
    {
        Destroy(gameObject);
        SceneManager.LoadScene("Menu");

    }
}
