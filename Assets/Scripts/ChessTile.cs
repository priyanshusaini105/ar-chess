using UnityEngine;

public enum TileColor
{
    White,
    Black
}

public class ChessTile : MonoBehaviour
{
    public TileColor tileColor;
    public Color myTileColor;
    public Color highLightColor;
    private Material tileMaterial;

    void Start()
    {
        tileMaterial = GetComponent<Renderer>().material;
        myTileColor = tileColor == TileColor.White ? Color.white : Color.black;
        SetTileColor(myTileColor);
        highLightColor = Color.yellow;
    }

    private void Update()
    {
        HighlightTileOnHover();
    }

    public void SetTileColor(Color color)
    {
        tileMaterial.color = color;
    }

  
    public void HighlightTileOnHover()
    {
       
        if (IsMouseOverTile())
        {
            
            SetTileColor(highLightColor);
        }
        else
        {
            SetTileColor(myTileColor);
        }
    }

    private bool IsMouseOverTile()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        // Cast a ray from the camera to the mouse position
        if (Physics.Raycast(ray, out hit))
        {
            // Check if the hit object is this tile
            if (hit.collider.gameObject == gameObject)
            {
                return true;
            }
        }

        return false;
    }

    public PieceType GetPieceName()
    {
        var piece = GetComponentInChildren<ChessPiece>();
        if (piece != null)
        {
            return piece.pieceType;
        }
        return PieceType.NONE;
    }
}
