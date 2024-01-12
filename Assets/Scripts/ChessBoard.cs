
using UnityEngine;

public class ChessBoard : MonoBehaviour
{
    public GameObject whiteTilePrefab;
    public GameObject blackTilePrefab;
    public float tileSize = 1.0f;

    void Start()
    {
        CreateChessBoard();
    }

    void CreateChessBoard()
    {
        for (int row = 0; row < 8; row++)
        {
            for (int col = 0; col < 8; col++)
            {
                GameObject tilePrefab = (row + col) % 2 == 0 ? whiteTilePrefab : blackTilePrefab;
                GameObject tile = Instantiate(tilePrefab, new Vector3(col * tileSize, 0, row * tileSize), Quaternion.identity);
                tile.transform.parent = transform; 
            }
        }
    }
}
