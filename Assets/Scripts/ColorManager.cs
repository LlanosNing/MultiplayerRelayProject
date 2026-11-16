using UnityEngine;
using Unity.Netcode;

public enum TileColor
{
    RebeccaPurple, HotPink, NavajoWhite, Peru, Grey4
}

public class ColorManager : NetworkBehaviour
{
    //Variable sincronizada con el color seguro de cada ronda
    public NetworkVariable<TileColor> safeColor = new NetworkVariable<TileColor>();

    private void Start()
    {
        
    }

    private void Update()
    {
        //Esto solo funcina si el jugador es el Host/Server
        if (IsServer == false) return;

        if (Input.GetKeyDown(KeyCode.C))
        {
            safeColor.Value = RandomColor();
        }
    }

    //Devuelve un color aleatorio de todos los posibles
    public TileColor RandomColor()
    {
        return (TileColor)Random.Range(0, 5);
    }
}
