using System;
using System.Collections.Generic;
using System.Linq;
using chess_aboguslawski.statics;
using chess_aboguslawski.utils;

namespace chess_aboguslawski.entities.pieces
{
    public class Pawn : Piece
    {
        private int directionOffset;
        private static int[] heatmapBlack => new int[]
        {
            800, 800, 800, 800, 800, 800, 800, 800,
            190, 190, 190, 190, 190, 190, 190, 190,
            134, 114, 114, 124, 124, 114, 114, 134,
            100, 100, 100, 124, 124, 100, 100, 100,
            111, 100, 122, 125, 125, 100, 100, 111,
            100, 101, 111, 110, 110, 100, 101, 100,
            100, 100, 100, 100, 100, 100, 100, 100,
            100, 100, 100, 100, 100, 100, 100, 100,
        };

        private static int[] heatmapWhite => new int[]
        {
            100, 100, 100, 100, 100, 100, 100, 100,
            100, 100, 100, 100, 100, 100, 100, 100,
            100, 101, 111, 110, 110, 100, 101, 100,
            111, 100, 122, 125, 125, 100, 100, 111,
            100, 100, 100, 124, 124, 100, 100, 100,
            134, 114, 114, 124, 124, 114, 114, 134,
            190, 190, 190, 190, 190, 190, 190, 190,
            800, 800, 800, 800, 800, 800, 800, 800,
        };

        public Pawn(Color color, Square square)
            : base(color == Color.WHITE ? Uris.whitePawnImage: Uris.blackPawnImage, color, square)
        {
            directionOffset = color == Color.WHITE ? 1 : -1;
        }

        public override char Symbol()
        {
            return Color == Color.WHITE ? 'P' : 'p';
        }


        public override void Move(int destinationFile, int destinationRank)
        {
            var rankBeforeMove = Square.Rank;
            var wasCaptureEnPassant = Square.Board.GetSquare(destinationFile, destinationRank) == Square.Board.EnPassantTarget;
            base.Move(destinationFile, destinationRank);

            if (Math.Abs(destinationRank - rankBeforeMove) == 2)
            {
                Square.Board.EnPassantTarget = Square.Board.GetSquare(destinationFile, rankBeforeMove + directionOffset);
            }
            if (wasCaptureEnPassant)
            {
                Square.Board.GetSquare(destinationFile, destinationRank - directionOffset).Piece = null;
            }
            //promotion
            if (destinationRank == 7 || destinationRank == 0)
            {
                Square.Piece = new Queen(Color, Square);
            }

        }
        public override IEnumerable<Square> LegalMoves()
        {
            var boardSquares = Square.Board.Squares;
            var squareInFront = boardSquares.FirstOrDefault(x => x.File == Square.File && x.Rank == Square.Rank + directionOffset);

            return boardSquares
                .Where(destination =>
                    destination != Square
                    && !IsPieceOfTheSameColor(Square, destination)
                    && (ForwardOneSquare(Square, destination, directionOffset)
                        || (ForwardTwoSquares(Square, destination, directionOffset) && IsOnStartingRank(Square) && squareInFront != null && squareInFront.Piece == null)
                        || IsCapturing(Square, destination, directionOffset)
                        )
                    && !this.IsSquareOccupiedByFriendlyPiece(destination)
                    )
                .ToList();
        }

        public override IEnumerable<Square> AttackedSquares()
        {
            return Square.Board.Squares
                .Where(s => s.Rank == Square.Rank + directionOffset && Math.Abs(s.File - Square.File) == 1);
        }

        protected override int PieceValue()
        {
            //return 1;
            var heatmap = Color == Color.WHITE ? heatmapWhite : heatmapBlack;
            return heatmap[Square.File + Square.Rank * 8];
        }

        private Func<Square, Square, int, bool> ForwardOneSquare = (source, destination, directionOffset) =>
            destination.Piece == null
            && destination.File == source.File
            && destination.Rank == source.Rank + directionOffset;

        private Func<Square, Square, int, bool> ForwardTwoSquares = (source, destination, directionOffset) =>
            destination.Piece == null
            && destination.File == source.File
            && destination.Rank == source.Rank + directionOffset * 2;

        private Func<Square, bool> IsOnStartingRank = source =>
            source.Rank is 1 or 6;

        private Func<Square, Square, int, bool> IsCapturing = (source, destination, directionOffset) =>
            Math.Abs(destination.File - source.File) == 1
            && destination.Rank == source.Rank + directionOffset
            && (destination.Piece != null || destination.Board.EnPassantTarget == destination);

    }
}
