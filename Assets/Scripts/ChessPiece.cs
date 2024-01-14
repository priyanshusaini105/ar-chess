using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class ChessPiece : MonoBehaviour
{
    public PieceType pieceType;
    public PieceColor pieceColor;

    public int CurrentX { set; get; }
    public int CurrentY { set; get; }

    public void SetPosition(int x, int y)
    {
        CurrentX = x;
        CurrentY = y;
    }
    public bool Move(int x, int y, ref bool[,] r)
    {
        if (x >= 0 && x < 8 && y >= 0 && y < 8)
        {
            ChessPiece chessPiece = ChessBoard.Instance.ChessPieces[x, y];
            if (chessPiece == null)
                r[x, y] = true;
            else
            {
                if (pieceColor != chessPiece.pieceColor)
                    r[x, y] = true;
                return true;
            }
        }
        return false;
    }

    public virtual bool[,] PossibleMoves()
    {
        return new bool[8, 8];
    }
}
