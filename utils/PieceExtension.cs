using chess_aboguslawski.entities;
using chess_aboguslawski.entities.pieces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace chess_aboguslawski.utils
{
    static class PieceExtension
    {
        public static bool IsSquareOccupiedByFriendlyPiece(this Piece piece, Square square)
        {
            return square.Piece == null
                ? false
                : piece.Color == square.Piece.Color;
        }

        public static bool IsKing(this Piece piece)
        {
            return piece == null
                ? false
                : piece.GetType() == typeof(King);
        }

        public static bool IsPawn(this Piece piece)
        {
            return piece == null
                ? false
                : piece.GetType() == typeof(Pawn);
        }

        public static bool IsRookOfColor(this Piece piece, Color color)
        {
            return piece == null
                ? false
                : piece.GetType() == typeof(Rook) && piece.Color == color;
        }

        public static Color OpposingColor(this Piece piece)
        {
            return piece.Color == Color.WHITE
                ? Color.BLACK
                : Color.WHITE;
        }
    }
}
