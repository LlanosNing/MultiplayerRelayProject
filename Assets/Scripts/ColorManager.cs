using System.Collections.Generic;
using System.Linq;
using Unity.Netcode;
using UnityEngine;

public enum TileColor
{
    RebeccaPurple, HotPink, NavajoWhite, Peru, Grey4
}

public class ColorManager : NetworkBehaviour
{
    //Variable sincronizada con el color seguro de cada ronda
    public NetworkVariable<TileColor> safeColor = new NetworkVariable<TileColor>();

    //Todos los tiles del escenario (guardados en TileGrid)
    [SerializeField] private List<ColorTile> tiles;
    //Materiales para los colores de los tiles
    [SerializeField] private Material[] colorMaterials;

    //Diccionario para guardar cuántos colores de cada tile quedan disponibles
    //Así nos aseguramos de que salga la misma cantidad de tiles de cada color
    private Dictionary<TileColor, int> availableColors = new();
    private void Start()
    {
        //Buscar el grid con todos los tiles y guardarlos
        TileGrid grid = FindFirstObjectByType<TileGrid>();
        //Guardar todos los tiles que tenga el grid
        tiles = new List<ColorTile>(grid.Tiles);

        //Añadir al diccionario todos los colores y la cantidad de tiles que habrá de cada color (7)
        for (int i = 0; i < 5; i++)
        {
            availableColors.Add((TileColor)i, 7);
        }
    }

    private void Update()
    {
        //Esto solo funcina si el jugador es el Host/Server
        if (IsServer == false) return;

        if (Input.GetKeyDown(KeyCode.C))
        {
            safeColor.Value = RandomColor();
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            RandomizeColors();
        }
    }

    void RandomizeColors()
    {
        //Crear un array con todos los colores aleatorios que se irán generando
        TileColor[] randomColors = new TileColor[tiles.Count];
        //Rellenar el array con colores aleatorios
        for (int i = 0; i < randomColors.Length; i++)
        {
            randomColors[i] = GetRandomColor();
        }
        //Sincronizar los colores con el resto de jugadores
        SyncColorsRpc(randomColors);
    }

    //Sincronizar los colores aleatorios para que todos los jugadores tengan los mismos
    //Los colores calculados se pasan como parámetro para que todos vean los mismos colores
    [Rpc(SendTo.Everyone)]
    void SyncColorsRpc(TileColor[] randomColors)
    {
        TileColor randomColor;
        int materialIndex = 0;

        //Asignar a cada tile su color correspondiente calculado por el server
        for (int i = 0; i < tiles.Count; i++)
        {
            randomColor = randomColors[i];
            //Calcular el índice del array de materiales para este color
            materialIndex = (int)randomColor;
            //Asignar el color y el material a cada tile
            tiles[i].SetupColor(randomColor, colorMaterials[materialIndex]);
        }
    }

    //Devuelve un color aleatorio de todos los posibles
    public TileColor RandomColor()
    {
        return (TileColor)Random.Range(0, 5);
    }

    //Devuelve un color aleatorio para un ColorTile
    public TileColor GetRandomColor()
    {
        TileColor chosenColor = availableColors.ElementAt(Random.Range(0, availableColors.Count)).Key;
        availableColors[chosenColor] -= 1;
        if (availableColors[chosenColor] <= 0)
        {
            availableColors.Remove(chosenColor);
        }
        return chosenColor;
    }
}
