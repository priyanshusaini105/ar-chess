using System.Collections;
using System.ComponentModel;
using UnityEngine;

public class Pawn : ChessPiece
{

    public override bool[,] PossibleMoves()
    {
        bool[,] r = new bool[8, 8];

        ChessPiece c, c2;

        int[] e = ChessBoard.Instance.EnPassantMove;

        if (pieceColor==PieceColor.WHITE)
        {
            ////// White team move //////

            // Diagonal left
            if (CurrentX != 0 && CurrentY != 7)
            {
                if (e[0] == CurrentX - 1 && e[1] == CurrentY + 1)
                    r[CurrentX - 1, CurrentY + 1] = true;

                c = ChessBoard.Instance.ChessPieces[CurrentX - 1, CurrentY + 1];
                if (c != null && !(c.pieceColor == PieceColor.WHITE))
                    r[CurrentX - 1, CurrentY + 1] = true;
            }

            // Diagonal right
            if (CurrentX != 7 && CurrentY != 7)
            {
                if (e[0] == CurrentX + 1 && e[1] == CurrentY + 1)
                    r[CurrentX + 1, CurrentY + 1] = true;

                c = ChessBoard.Instance.ChessPieces[CurrentX + 1, CurrentY + 1];
                if (c != null && !(c.pieceColor == PieceColor.WHITE))
                    r[CurrentX + 1, CurrentY + 1] = true;
            }

            // Middle
            if (CurrentY != 7)
            {
                c = ChessBoard.Instance.ChessPieces[CurrentX, CurrentY + 1];
                if (c == null)
                    r[CurrentX, CurrentY + 1] = true;
            }

            // Middle on first move
            if (CurrentY == 1)
            {
                c = ChessBoard.Instance.ChessPieces[CurrentX, CurrentY + 1];
                c2 = ChessBoard.Instance.ChessPieces[CurrentX, CurrentY + 2];
                if (c == null && c2 == null)
                    r[CurrentX, CurrentY + 2] = true;
            }
        }
        else
        {
            ////// Black team move //////

            // Diagonal left
            if (CurrentX != 0 && CurrentY != 0)
            {
                if (e[0] == CurrentX - 1 && e[1] == CurrentY - 1)
                    r[CurrentX - 1, CurrentY - 1] = true;

                c = ChessBoard.Instance.ChessPieces[CurrentX - 1, CurrentY - 1];
                if (c != null && (c.pieceColor == PieceColor.WHITE))
                    r[CurrentX - 1, CurrentY - 1] = true;
            }

            // Diagonal right
            if (CurrentX != 7 && CurrentY != 0)
            {
                if (e[0] == CurrentX + 1 && e[1] == CurrentY - 1)
                    r[CurrentX + 1, CurrentY - 1] = true;

                c = ChessBoard.Instance.ChessPieces[CurrentX + 1, CurrentY - 1];
                if (c != null && (c.pieceColor == PieceColor.WHITE))
                    r[CurrentX + 1, CurrentY - 1] = true;
            }

            // Middle
            if (CurrentY != 0)
            {
                c = ChessBoard.Instance.ChessPieces[CurrentX, CurrentY - 1];
                if (c == null)
                    r[CurrentX, CurrentY - 1] = true;
            }

            // Middle on first move
            if (CurrentY == 6)
            {
                c = ChessBoard.Instance.ChessPieces[CurrentX, CurrentY - 1];
                c2 = ChessBoard.Instance.ChessPieces[CurrentX, CurrentY - 2];
                if (c == null && c2 == null)
                    r[CurrentX, CurrentY - 2] = true;
            }
        }

        return r;
    }
}