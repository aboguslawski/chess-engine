using chess_aboguslawski.statics;
using System.Collections.Generic;
using System.Linq;

namespace chess_aboguslawski.entities.pieces
{
    class Bishop : Piece
    {
        private static int[] heatmapBlack => new int[]
        {
            -50, 0, 0, 0, 0, 0, 0, -50,
            0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0,
            0, 20, 0, 0, 0, 0, 20, 0,
            0, 0, 20, 0, 0, 20, 0, 0,
            0, 20, 0, 0, 0, 0, 20, 0,
            20, 30, 0, 10, 10, 0, 30, 20,
            -50, 0, 0, 0, 0, 0, 0, -50,
        };

        private static int[] heatmapWhite => new int[]
        {
            -50, 0, 0, 0, 0, 0, 0, -50,
            20, 30, 0, 10, 10, 0, 30, 20,
            0, 20, 0, 0, 0, 0, 20, 0,
            0, 0, 20, 0, 0, 20, 0, 0,
            0, 20, 0, 0, 0, 0, 20, 0,
            0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0,
            -50, 0, 0, 0, 0, 0, 0, -50,
        };

        public Bishop(Color color, Square square)
            : base(color == Color.WHITE ? Uris.whiteBishopImage : Uris.blackBishopImage, color, square)
        {
        }

        public override char Symbol()
        {
            return Color == Color.WHITE ? 'B' : 'b';
        }

        public override IEnumerable<Square> LegalMoves()
        {
            var moves = new List<Square>();
            var upRight = true;
            var upLeft = true;
            var downRight = true;
            var downLeft = true;

            for (int i = 1; i < 8; i++)
            {
                if (upRight && Square.File + i <= 7 && Square.Rank + i <= 7)
                {
                    upRight = AddNextMoveAndReturnFlag(Square.File + i, Square.Rank + i, moves);
                }

                if (upLeft && Square.File + i <= 7 && Square.Rank - i >= 0)
                {
                    upLeft = AddNextMoveAndReturnFlag(Square.File + i, Square.Rank - i, moves);
                }

                if (downRight && Square.File - i >= 0 && Square.Rank + i <= 7)
                {
                    downRight = AddNextMoveAndReturnFlag(Square.File - i, Square.Rank + i, moves);
                }

                if (downLeft && Square.File - i >= 0 && Square.Rank - i >= 0)
                {
                    downLeft = AddNextMoveAndReturnFlag(Square.File - i, Square.Rank - i, moves);
                }
            }

            return moves;
        }

        protected override int PieceValue()
        {
            var heatmap = Color == Color.WHITE ? heatmapWhite : heatmapBlack;
            return heatmap[Square.File + Square.Rank * 8] + 300;
        }

    }
}
