using System;
using System.Collections.Generic;

namespace chess_aboguslawski.entities.pieces
{
    public abstract class Piece
    {
        public Color Color { get; set; }
        public string ImageUri { get; set; }

        public Square Square { get; set; }

        public Piece(string uri, Color color, Square square)
        {
            ImageUri = uri;
            Square = square;
            Color = color;
        }

        public abstract char Symbol();

        public virtual void Move(int destinationFile, int destinationRank)
        {
            Square.Piece = null;
            var destinationSquare = Square.Board.GetSquare(destinationFile, destinationRank);
            DepriveFromCastlingRightsIfCapturedARook(destinationSquare.File, destinationSquare.Rank);
            destinationSquare.Piece = this;
            Square = destinationSquare;
            Square.Board.EnPassantTarget = null;
        }

        private void DepriveFromCastlingRightsIfCapturedARook(int file, int rank)
        {
            if(file == 7 && rank == 7)
            {
                Square.Board.CastlingRights.BlackKingside = false;
            }
            if (file == 0 && rank == 7)
            {
                Square.Board.CastlingRights.BlackQueenside= false;
            }
            if (file == 0 && rank == 0)
            {
                Square.Board.CastlingRights.WhiteQueenside = false;
            }
            if (file == 7 && rank == 0)
            {
                Square.Board.CastlingRights.WhiteKingside = false;
            }
        }

        public abstract IEnumerable<Square> LegalMoves();

        public int GetValue()
        {
            var pieceValue = PieceValue();
            return Color == Color.WHITE
                ? pieceValue
                : -pieceValue;
        }

        protected abstract int PieceValue();

        public virtual IEnumerable<Square> AttackedSquares()
        {
            return LegalMoves();
        }

        protected bool AddNextMoveAndReturnFlag(int file, int rank, List<Square> movesAccumulator)
        {
            var move = Square.Board.GetSquare(file, rank);
            if (move.IsEmpty)
            {
                movesAccumulator.Add(move);
            }
            else
            {
                if (move.Piece.Color != Color)
                {
                    movesAccumulator.Add(move);
                }
                return false;
            }
            return true;
        }

        protected Func<Square, Square, bool> IsPieceOfTheSameColor = (source, destination) =>
            destination.Piece?.Color == source.Piece?.Color;
    }
}
