using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class CharacterPlayer
{
    public GameObject prefab;
    public string name;
    public Vector3 selectionScale;
    public Vector3 selectionLocation;
    public Vector3 gameScale;       
}

public static class CharacterSelectionData
{
    public static CharacterPlayer selectedCharacter;
}
