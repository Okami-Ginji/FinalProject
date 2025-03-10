using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GoblinSpawn : MonoBehaviour
{
    public Tilemap SpawnCoordinate;
    public GameObject GoblinTNTprefab;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SpawnGoblins();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void SpawnGoblins()
    {
        List<Vector3Int> tilePositions = GetAllTilePositions(SpawnCoordinate);

        foreach (Vector3Int tilePos in tilePositions)
        {
            Vector3 worldPosition = SpawnCoordinate.CellToWorld(tilePos) + new Vector3(0.1f, 0.7f, 0); // Adjust to center the Goblin

            Instantiate(GoblinTNTprefab, worldPosition, Quaternion.identity);
        }
    }

    List<Vector3Int> GetAllTilePositions(Tilemap SpawnCoordinate)
    {
        List<Vector3Int> tilePositions = new List<Vector3Int>();

        foreach (Vector3Int pos in SpawnCoordinate.cellBounds.allPositionsWithin)
        {
            if (SpawnCoordinate.HasTile(pos))
            {
                tilePositions.Add(pos);
            }
        }
        return tilePositions;
    }

}
