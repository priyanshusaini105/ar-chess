using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChessBoard : MonoBehaviour
{
    public static ChessBoard Instance { get; set; }
    private bool[,] allowedMoves { get; set; }

    private const float TILE_SIZE = 1.0f;
    private const float TILE_OFFSET = 0.5f;

    private int selectionX = -1;
    private int selectionY = -1;

    public List<GameObject> chessPiecePrefabs;
    private List<GameObject> activeChessPieces;

    private Quaternion whiteOrientation = Quaternion.Euler(0, 270, 0);
    private Quaternion blackOrientation = Quaternion.Euler(0, 90, 0);

    public ChessPiece[,] ChessPieces { get; set; }
    private ChessPiece selectedChessPiece;

    public bool isWhiteTurn = true;

    private Material previousMaterial;
    public Material selectedMaterial;

    public int[] EnPassantMove { set; get; }

    // Use this for initialization
    void Start()
    {
        Instance = this;
        SpawnAllChessPieces();
        EnPassantMove = new int[2] { -1, -1 };
    }

    // Update is called once per frame
    void Update()
    {
        UpdateSelection();

        if (Input.GetMouseButtonDown(0))
        {
            if (selectionX >= 0 && selectionY >= 0)
            {
                if (selectedChessPiece == null)
                {
                    // Select the chess piece
                    SelectChessPiece(selectionX, selectionY);
                }
                else
                {
                    // Move the chess piece
                    MoveChessPiece(selectionX, selectionY);
                }
            }
        }

        if (Input.GetKey("escape"))
            Application.Quit();
    }

    private void SelectChessPiece(int x, int y)
    {
        if (ChessPieces[x, y] == null) return;

        if ((ChessPieces[x, y].pieceColor==PieceColor.WHITE)!= isWhiteTurn) return;

        bool hasAtLeastOneMove = false;

        allowedMoves = ChessPieces[x, y].PossibleMoves();
        for (int xCoordinate = 0; xCoordinate < 8; xCoordinate++)
        {
            for (int yCoordinate = 0; yCoordinate < 8; yCoordinate++)
            {
                if (allowedMoves[xCoordinate, yCoordinate])
                {
                    hasAtLeastOneMove = true;
                    xCoordinate = 8;
                    break;
                }
            }
        }

        if (!hasAtLeastOneMove)
            return;

        selectedChessPiece = ChessPieces[x, y];
        previousMaterial = selectedChessPiece.GetComponent<MeshRenderer>().material;
        selectedMaterial.mainTexture = previousMaterial.mainTexture;
        selectedChessPiece.GetComponent<MeshRenderer>().material = selectedMaterial;

        BoardHighlights.Instance.HighLightAllowedMoves(allowedMoves);
    }

    private void MoveChessPiece(int x, int y)
    {
        if (allowedMoves[x, y])
        {
            ChessPiece targetChessPiece = ChessPieces[x, y];

            if (targetChessPiece != null && (targetChessPiece.pieceColor == PieceColor.WHITE) != isWhiteTurn)
            {
                // Capture a piece

                if (targetChessPiece.GetType() == typeof(King))
                {
                    // End the game
                    EndGame();
                    return;
                }

                activeChessPieces.Remove(targetChessPiece.gameObject);
                Destroy(targetChessPiece.gameObject);
            }
            if (x == EnPassantMove[0] && y == EnPassantMove[1])
            {
                if (isWhiteTurn)
                    targetChessPiece = ChessPieces[x, y - 1];
                else
                    targetChessPiece = ChessPieces[x, y + 1];

                activeChessPieces.Remove(targetChessPiece.gameObject);
                Destroy(targetChessPiece.gameObject);
            }
            EnPassantMove[0] = -1;
            EnPassantMove[1] = -1;
            if (selectedChessPiece.GetType() == typeof(Pawn))
            {
                if (y == 7) // White Promotion
                {
                    activeChessPieces.Remove(selectedChessPiece.gameObject);
                    Destroy(selectedChessPiece.gameObject);
                    SpawnChessPiece(1, x, y, true);
                    selectedChessPiece = ChessPieces[x, y];
                }
                else if (y == 0) // Black Promotion
                {
                    activeChessPieces.Remove(selectedChessPiece.gameObject);
                    Destroy(selectedChessPiece.gameObject);
                    SpawnChessPiece(7, x, y, false);
                    selectedChessPiece = ChessPieces[x, y];
                }
                EnPassantMove[0] = x;
                if (selectedChessPiece.CurrentY == 1 && y == 3)
                    EnPassantMove[1] = y - 1;
                else if (selectedChessPiece.CurrentY == 6 && y == 4)
                    EnPassantMove[1] = y + 1;
            }

            ChessPieces[selectedChessPiece.CurrentX, selectedChessPiece.CurrentY] = null;
            selectedChessPiece.transform.position = GetTileCenter(x, y);
            selectedChessPiece.SetPosition(x, y);
            ChessPieces[x, y] = selectedChessPiece;
            isWhiteTurn = !isWhiteTurn;
        }

        selectedChessPiece.GetComponent<MeshRenderer>().material = previousMaterial;

        BoardHighlights.Instance.HideHighlights();
        selectedChessPiece = null;
    }

    private void UpdateSelection()
    {
        if (!Camera.main) return;

        RaycastHit hitInfo;
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hitInfo, 50.0f, LayerMask.GetMask("ChessPlane")))
        {
            selectionX = (int)hitInfo.point.x;
            selectionY = (int)hitInfo.point.z;
        }
        else
        {
            selectionX = -1;
            selectionY = -1;
        }
    }

    private void SpawnChessPiece(int index, int x, int y, bool isWhite)
    {
        Vector3 position = GetTileCenter(x, y);
        GameObject chessPieceObject;

        if (isWhite)
        {
            chessPieceObject = Instantiate(chessPiecePrefabs[index], position, whiteOrientation) as GameObject;
        }
        else
        {
            chessPieceObject = Instantiate(chessPiecePrefabs[index], position, blackOrientation) as GameObject;
        }

        chessPieceObject.transform.SetParent(transform);
        ChessPieces[x, y] = chessPieceObject.GetComponent<ChessPiece>();
        ChessPieces[x, y].SetPosition(x, y);
        activeChessPieces.Add(chessPieceObject);
    }

    private Vector3 GetTileCenter(int x, int y)
    {
        Vector3 origin = Vector3.zero;
        origin.x += (TILE_SIZE * x) + TILE_OFFSET;
        origin.z += (TILE_SIZE * y) + TILE_OFFSET;

        return origin;
    }

    private void SpawnAllChessPieces()
    {
        activeChessPieces = new List<GameObject>();

        ChessPieces = new ChessPiece[8, 8];

        /////// White ///////

        // King
        SpawnChessPiece(0, 3, 0, true);

        // Queen
        SpawnChessPiece(1, 4, 0, true);

        // Rooks
        SpawnChessPiece(2, 0, 0, true);
        SpawnChessPiece(2, 7, 0, true);

        // Bishops
        SpawnChessPiece(3, 2, 0, true);
        SpawnChessPiece(3, 5, 0, true);

        // Knights
        SpawnChessPiece(4, 1, 0, true);
        SpawnChessPiece(4, 6, 0, true);

        // Pawns
        for (int i = 0; i < 8; i++)
        {
            SpawnChessPiece(5, i, 1, true);
        }

        /////// Black ///////

        // King
        SpawnChessPiece(6, 4, 7, false);

        // Queen
        SpawnChessPiece(7, 3, 7, false);

        // Rooks
        SpawnChessPiece(8, 0, 7, false);
        SpawnChessPiece(8, 7, 7, false);

        // Bishops
        SpawnChessPiece(9, 2, 7, false);
        SpawnChessPiece(9, 5, 7, false);

        // Knights
        SpawnChessPiece(10, 1, 7, false);
        SpawnChessPiece(10, 6, 7, false);

        // Pawns
        for (int i = 0; i < 8; i++)
        {
            SpawnChessPiece(11, i, 6, false);
        }
    }

    private void EndGame()
    {
        if (isWhiteTurn)
            Debug.Log("White wins");
        else
            Debug.Log("Black wins");

        foreach (GameObject chessPieceObject in activeChessPieces)
        {
            Destroy(chessPieceObject);
        }

        isWhiteTurn = true;
        BoardHighlights.Instance.HideHighlights();
        SpawnAllChessPieces();
    }
}
