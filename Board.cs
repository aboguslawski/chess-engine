using chess_aboguslawski.entities;
using chess_aboguslawski.utils;
using System.Collections.Generic;
using System.Linq;

namespace chess_aboguslawski
{
    public class Board
    {
        public Square[] Squares { get; set; } = new Square[64];
        public Color PlayerToMove { get; set; }
        public CastlingRights CastlingRights { get; set; }
        public Square EnPassantTarget { get; set; }
        public int HalfmoveClock { get; set; }
        public int FullmoveCounter { get; set; }

        public Board()
        {
            for (var rank = 7; rank >= 0; rank--)
            {
                for (var file = 7; file >= 0; file--)
                {
                    var s = new Square(file, rank, this);
                    Squares[file + rank * 8] = s;
                }
            }
        }

        public Square GetSquare(int file, int rank)
        {
            return Squares[file + rank * 8];
        }

        private bool IsSquareAttackedByColor(Square checkedSquare, Color color)
        {
            var pieces = Squares
                .Where(s => s.Piece != null && s.Piece.Color == color)
                .Select(s => s.Piece);
            return pieces.Any(p => p.AttackedSquares().Contains(checkedSquare));
        }

        public void MovePiece(int fromFile, int fromRank, int toFile, int toRank)
        {
            var from = GetSquare(fromFile, fromRank);

            from.Piece.Move(toFile, toRank);
        }

        public bool IsPositionLegal()
        {
            var whiteKingExist = Squares.Any(s => s.Piece.IsKing() && s.Piece.Color == Color.WHITE);
            var blackKingExist = Squares.Any(s => s.Piece.IsKing() && s.Piece.Color == Color.BLACK);

            if (!whiteKingExist || !blackKingExist)
            {
                return false;
            }
            if (PlayerToMove == Color.BLACK)
            {
                return !IsKingAttacked(Color.BLACK);
            }
            if (PlayerToMove == Color.WHITE)
            {
                return !IsKingAttacked(Color.WHITE);
            }
            return true;
        }

        public bool IsKingAttacked(Color kingColor)
        {
            var kingSquare = Squares
                .Where(s => s.Piece.IsKing() && s.Piece.Color == kingColor)
                .First();
            return IsSquareAttackedByColor(kingSquare, kingSquare.Piece.OpposingColor());
        }
        
        public IList<Square> SquaresWithPiecesOfColor(Color color)
        {
            return Squares
                .Where(s => !s.IsEmpty
                && s.Piece.Color == color)
                .ToList();
        }
        public void SwitchTurn()
        {
            PlayerToMove = PlayerToMove == Color.WHITE
                ? Color.BLACK
                : Color.WHITE;
        }

    }

}
