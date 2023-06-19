using chess_aboguslawski.statics;
using chess_aboguslawski.utils;
using System;
using System.Collections.Generic;
using System.Linq;

namespace chess_aboguslawski.entities.pieces
{
    class Knight : Piece
    {
        private static int[] heatmap => new int[]
        {
            -50, -20, -20, -20, -20, -20, -20, -50,
            -20, 0, 0, 10, 10, 0, 0, -20,
            -20, 10, 10, 0, 0, 10, 10, -20,
            -20, 0, 0, 20, 20, 0, 0, -20,
            -20, 0, 0, 20, 20, 0, 0, -20,
            -20, 10, 10, 0, 0, 10, 10, -20,
            -20, 0, 0, 10, 10, 0, 0, -20,
            -50, -20, -20, -20, -20, -20, -20, -50
        };

        public Knight(Color color, Square square) 
            : base(color == Color.WHITE ? Uris.whiteKnightImage : Uris.blackKnightImage, color, square)
        {
        }

        public override char Symbol()
        {
            return Color == Color.WHITE ? 'N' : 'n';
        }

        public override IEnumerable<Square> LegalMoves()
        {
            return Square.Board.Squares
                .Where(x =>
                (
                    (Math.Abs(x.File - Square.File) == 2 && Math.Abs(x.Rank - Square.Rank) == 1)
                    || (Math.Abs(x.File - Square.File) == 1 && Math.Abs(x.Rank - Square.Rank) == 2)
                )
                && !this.IsSquareOccupiedByFriendlyPiece(x));
        }

        protected override int PieceValue()
        {
            return heatmap[Square.File + Square.Rank * 8] + 300;
        }
    }
}
