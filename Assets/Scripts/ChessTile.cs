using UnityEngine;

public enum TileColor
{
    White,
    Black
}

public class ChessTile : MonoBehaviour
{
    public TileColor tileColor;

    // void Start()
    // {
    //     SetTileColor(tileColor);
    // }

    // public void SetTileColor(TileColor color)
    // {
    //     tileColor = color;
    //     Renderer renderer = GetComponent<Renderer>();

    //     Material material = color == TileColor.White ? GetWhiteMaterial() : GetBlackMaterial();

    //     renderer.material = material;
    // }

    // private Material GetWhiteMaterial()
    // {
    //     return new Material(Shader.Find("Standard")) { color = tileColor.White };
    // }

    // private Material GetBlackMaterial()
    // {
    //     return new Material(Shader.Find("Standard")) { color = tileColor.Black };
    // }
}
