using UnityEngine;

public class ColorTile : MonoBehaviour
{
    [SerializeField] private TileColor assignedColor;
    [SerializeField] private Renderer rend;
   
    //Asigna a este tile un color y cambia su material acorde al color asignado
    public void SetupColor(TileColor color, Material colorMaterial)
    {
        assignedColor = color;
        rend.sharedMaterial = colorMaterial;
    }
}
