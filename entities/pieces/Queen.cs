using chess_aboguslawski.statics;
using System.Collections.Generic;

namespace chess_aboguslawski.entities.pieces
{
    class Queen : Piece
    {
        public Queen(Color color, Square square) 
            : base(color == Color.WHITE ? Uris.whiteQueenImage : Uris.blackQueenImage, color, square)
        {
        }

        public override char Symbol()
        {
            return Color == Color.WHITE ? 'Q' : 'q';
        }

        public override IEnumerable<Square> LegalMoves()
        {
            var moves = new List<Square>();
            var up = true;
            var down = true;
            var left = true;
            var right = true;
            var upRight = true;
            var upLeft = true;
            var downRight = true;
            var downLeft = true;

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
            return 900;
        }

    }
}
