using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PieceType
{
    PAWN,
    ROOK,
    KNIGHT,
    BISHOP,
    QUEEN,
    KING,
    NONE
}

public enum PieceColor
{
    WHITE,
    BLACK
}

public enum PieceState
{
    IDLE,
    SELECTED,
    MOVING
}

public enum Direction
{
    UP,
    DOWN,
    LEFT,
    RIGHT,
}


public class GameManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
