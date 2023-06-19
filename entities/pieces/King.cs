using chess_aboguslawski.statics;
using chess_aboguslawski.utils;
using System;
using System.Collections.Generic;
using System.Linq;

namespace chess_aboguslawski.entities.pieces
{
    public class King : Piece
    {

        private static int[] heatmapBlack => new int[]
        {
            0, 0, 0, 0, 0, 0, 0, 0,
            0, 40, 40, 40, 40, 40, 40, 0,
            0, 40, 40, 40, 40, 40, 40, 0,
            0, 0, 40, 40, 40, 40, 0, 0,
            0, 0, 0, 30, 30, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0,
            0, 20, 0, 0, 0, 0, 20, 0,
            20, 30, 20, -20, -10, -10, 30, 20,
        };

        private static int[] heatmapWhite => new int[]
        {
            20, 30, 20, -20, -10, -10, 30, 20,
            0, 20, 0, 0, 0, 0, 20, 0,
            0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 30, 30, 0, 0, 0,
            0, 0, 40, 40, 40, 40, 0, 0,
            0, 40, 40, 40, 40, 40, 40, 0,
            0, 40, 40, 40, 40, 40, 40, 0,
            0, 0, 0, 0, 0, 0, 0, 0,
        };

        public bool CanCastleQueenside => Color == Color.WHITE
            ? Square.Board.CastlingRights.WhiteQueenside
            : Square.Board.CastlingRights.BlackQueenside;
        public bool CanCastleKingside => Color == Color.WHITE
            ? Square.Board.CastlingRights.WhiteKingside
            : Square.Board.CastlingRights.BlackKingside;

        public King(Color color, Square square) 
            : base(color == Color.WHITE ? Uris.whiteKingImage: Uris.blackKingImage, color, square)
        {
        }

        public override char Symbol()
        {
            return Color == Color.WHITE ? 'K' : 'k';
        }

        public override void Move(int destinationFile, int destinationRank)
        {

            MoveRookIfCastled(destinationFile, destinationRank);
            base.Move(destinationFile, destinationRank);
            DepriveFromCastlingRights();
        }

        public override IEnumerable<Square> LegalMoves()
        {
            return Square.Board.Squares
                .Where(x => 
                (Math.Abs(Square.File - x.File) <= 1 
                && Math.Abs(Square.Rank - x.Rank) <= 1
                && !this.IsSquareOccupiedByFriendlyPiece(x))
                || (CanCastleQueenside && x.File == 2 && x.Rank == Square.Rank && EmptySpaceToCastleQueenside)
                || (CanCastleKingside && x.File == 6 && x.Rank == Square.Rank && EmptySpaceToCastleKingside)
                )
                .ToList();
        }

        protected override int PieceValue()
        {
            //return 3.5;
            var heatmap = Color == Color.WHITE ? heatmapWhite : heatmapBlack;
            return heatmap[Square.File + Square.Rank * 8] + 350;
        }

        private bool EmptySpaceToCastleQueenside => Square.Board.Squares
            .Where(s => s.Rank == Square.Rank && s.File > 0 && s.File < 4)
            .All(s => s.Piece == null);
        private bool EmptySpaceToCastleKingside => Square.Board.Squares
            .Where(s => s.Rank == Square.Rank && s.File < 7 && s.File > 4)
            .All(s => s.Piece == null);

        private void MoveRookIfCastled(int destinationFile, int destinationRank)
        {
            // castled kingside
            if (Square.File == 4 && destinationFile == 6 && CanCastleKingside)
            {
                var rookSquare = Square.Board.GetSquare(7, destinationRank);
                rookSquare.Piece.Move(5, destinationRank);
            }
            // castled queenside
            if (Square.File == 4 && destinationFile == 2 && CanCastleQueenside)
            {
                var rookSquare = Square.Board.GetSquare(0, destinationRank);
                rookSquare.Piece.Move(3, destinationRank);
            }
        }

        private void DepriveFromCastlingRights()
        {
            if(Color == Color.WHITE)
            {
                Square.Board.CastlingRights.WhiteKingside = false;
                Square.Board.CastlingRights.WhiteQueenside = false;
            }
            if(Color == Color.BLACK)
            {
                Square.Board.CastlingRights.BlackKingside = false;
                Square.Board.CastlingRights.BlackQueenside = false;
            }
        }
    }
}
