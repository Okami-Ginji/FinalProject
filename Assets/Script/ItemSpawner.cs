using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    public GameObject healthItem;
    public Vector2 mapMin;
    public Vector2 mapMax;
    public int maxItem = 5;  
    private int currentItem = 0; 

    private void Start()
    {
        SpawnInitialItems();
    }

    void SpawnInitialItems()
    {
        for (int i = 0; i < maxItem; i++)
        {
            SpawnHealthItem();
        }
    }

    void SpawnHealthItem()
    {
        if (currentItem >= maxItem) return; 

        float x = Random.Range(mapMin.x, mapMax.x);
        float y = Random.Range(mapMin.y, mapMax.y);
        Vector2 spawnPosition = new Vector2(x, y);
        Instantiate(healthItem, spawnPosition, Quaternion.identity);

        currentItem++;
    }

    public void OnItemPickedUp()
    {
        currentItem--; 

       
        if (currentItem <= 3)
        {
            SpawnHealthItem();
        }
    }
}
