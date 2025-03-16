using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Map
{
    public Sprite mapSprites;
    public string name;
    //new
    public bool isUnlocked;
    public int unlockCost;
}
