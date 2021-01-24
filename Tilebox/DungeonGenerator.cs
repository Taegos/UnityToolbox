using System.Collections;
using System.Collections.Generic;
using Toolbox.ProcGen;
using Toolbox.Tilebox.Tilemap;
using UnityEngine;

public class DungeonGenerator : MonoBehaviour
{
    [SerializeField] private GameObject floorTile;
    [SerializeField] private GameObject playerTile;

    [SerializeField] private CellularAutomataArgs cellularAutomataArgs;

    void Start()
    {
        bool[,] map = CellularAutomata.Generate(cellularAutomataArgs);
        Tilemap tilemap = FindObjectOfType<Tilemap>();
        for (int x = 0; x < map.GetLength(0); x++) {
            for (int y = 0; y < map.GetLength(1); y++) {
                if (map[x, y]) {
                    Vector2Int pos = new Vector2Int(
                        x - cellularAutomataArgs.width / 2,
                        y - cellularAutomataArgs.height / 2);
                    tilemap.Instantiate(floorTile, pos);
                }
            }
        }
    }
}