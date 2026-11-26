using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class TileGrid : MonoBehaviour
{
    public List<ColorTile> Tiles = new();

    private void Awake()
    {
        Tiles = GetComponentsInChildren<ColorTile>().ToList();
    }
}