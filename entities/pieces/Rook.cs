using chess_aboguslawski.statics;
using System.Collections.Generic;

namespace chess_aboguslawski.entities.pieces
{
    class Rook : Piece
    {
        private static int[] heatmapBlack => new int[]
        {
            0, 0, 0, 0, 0, 0, 0, 0,
            50, 50, 50, 50, 50, 50, 50, 50,
            0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 30, 30, 30, 30, 0, 0,
            0, 0, 20, 20, 20, 20, 0, 0,
        };

        private static int[] heatmapWhite => new int[]
        {
            0, 0, 20, 20, 20, 20, 0, 0,
            0, 0, 30, 30, 30, 30, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0,
            50, 50, 50, 50, 50, 50, 50, 50,
            0, 0, 0, 0, 0, 0, 0, 0,
        };

        public Rook(Color color, Square square) 
            : base(color == Color.WHITE ? Uris.whiteRookImage : Uris.blackRookImage, color, square)
        {
        }

        public override char Symbol()
        {
            return Color == Color.WHITE ? 'R' : 'r';
        }
        public override void Move(int destinationFile, int destinationRank) 
        {
            if (Square.File == 0)
            {
                if (Color == Color.WHITE) Square.Board.CastlingRights.WhiteQueenside = false;
                if (Color == Color.BLACK) Square.Board.CastlingRights.BlackQueenside = false;
            }
            else if (Square.File == 7)
            {
                if (Color == Color.WHITE) Square.Board.CastlingRights.WhiteKingside = false;
                if (Color == Color.BLACK) Square.Board.CastlingRights.BlackKingside = false;
            }
            base.Move(destinationFile, destinationRank);
        }

        public override IEnumerable<Square> LegalMoves()
        {
            var moves = new List<Square>();
            var up = true;
            var down = true;
            var left = true;
            var right = true;

            for (int i = 1; i < 8; i++)
            {
                if (up && Square.Rank + i <= 7)
                {
                    up = AddNextMoveAndReturnFlag(Square.File, Square.Rank + i, moves);
                }

                if (down && Square.Rank - i >= 0)
                {
                    down = AddNextMoveAndReturnFlag(Square.File, Square.Rank - i, moves);
                }

                if (left && Square.File - i >= 0)
                {
                    left = AddNextMoveAndReturnFlag(Square.File - i, Square.Rank, moves);
                }

                if (right && Square.File + i <= 7)
                {
                    right = AddNextMoveAndReturnFlag(Square.File + i, Square.Rank, moves);
                }
            }

            return moves;
        }

        protected override int PieceValue()
        {
            var heatmap = Color == Color.WHITE ? heatmapWhite : heatmapBlack;
            return heatmap[Square.File + Square.Rank * 8] + 500;
        }

    }
}
