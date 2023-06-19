using chess_aboguslawski.entities;
using chess_aboguslawski.entities.pieces;

namespace chess_aboguslawski.services
{
    public class FENConverter
    {
        private FENConverter()
        {
        }

        private static FENConverter _instance;
        public static FENConverter Instance => _instance ??= new FENConverter();

        public string ToFen(Board position)
        {
            var fenParts = new string[6];
            fenParts[0] = PiecesToFEN(position);
            fenParts[1] = position.PlayerToMove == Color.WHITE ? "w" : "b";
            fenParts[2] = CastlingRightsToFEN(position);
            fenParts[3] = position.EnPassantTarget?.ToString() ?? "-";
            fenParts[4] = position.HalfmoveClock.ToString() ?? "0";
            fenParts[5] = position.FullmoveCounter.ToString() ?? "0";
            return string.Join(' ', fenParts);
        }


        public Board ToPosition(string fen)
        {
            var board = new Board();
            var parts = fen.Split(' ');

            var pieces = parts[0];
            var turn = parts[1];
            var castlingRights = parts[2];
            var enPassantTarget = parts[3];
            var halfmoveClock = parts[4];
            var fullmoveNumber = parts[5];

            board.CastlingRights = CastlingRights.FromFen(castlingRights);

            PlacePiecesOnBoard(pieces, board);
            board.PlayerToMove = turn == "b"
                ? Color.BLACK
                : Color.WHITE;

            SetEnPassantTarget(board, enPassantTarget);

            return board;
        }

        private void PlacePiecesOnBoard(string fenPieces, Board board)
        {
            var rank = 7;
            var file = 0;

            foreach (var c in fenPieces)
            {
                if (char.IsDigit(c))
                {
                    var space = (int)char.GetNumericValue(c);
                    file += space;
                }
                else if (c == '/')
                {
                    rank--;
                    file = 0;
                }
                else
                {
                    var square = board.GetSquare(file, rank);
                    PutPieceOnSquare(c, square);
                    file++;
                }

            }
        }

        private void SetEnPassantTarget(Board board, string enPassantTarget)
        {
            if (enPassantTarget == "-")
            {
                board.EnPassantTarget = null;
            }
            else
            {
                int file = enPassantTarget[0] - 'a';
                var rank = int.Parse(enPassantTarget[1] + "") - 1;
                var square = board.GetSquare(file, rank);
                board.EnPassantTarget = square;
            }

        }

        private string CastlingRightsToFEN(Board position)
        {
            var result = "";
            if (position.CastlingRights.WhiteKingside) result += "K";
            if (position.CastlingRights.WhiteQueenside) result += "Q";
            if (position.CastlingRights.BlackKingside) result += "k";
            if (position.CastlingRights.BlackQueenside) result += "q";
            if (result.Length == 0) result = "-";
            return result;
        }

        private string PiecesToFEN(Board position)
        {
            var result = "";
            var emptySquaresCount = 0;

            for (var rank = 7; rank >= 0; rank--)
            {
                for (var file = 0; file <= 7; file++)
                {
                    var square = position.GetSquare(file, rank);

                    if (!square.IsEmpty)
                    {
                        if (emptySquaresCount > 0)
                        {
                            result += emptySquaresCount;
                            emptySquaresCount = 0;
                        }
                        result += square.Piece.Symbol();
                    }

                    if (square.IsEmpty)
                    {
                        emptySquaresCount++;
                    }

                    if (file == 7)
                    {
                        if (emptySquaresCount > 0)
                        {
                            result += emptySquaresCount;
                        }
                        result += '/';
                        emptySquaresCount = 0;
                    }
                }
            }

            return result;
        }

        private void PutPieceOnSquare(char c, Square square)
        {
            switch (c)
            {
                case 'p':
                    square.Piece = new Pawn(Color.BLACK, square);
                    break;
                case 'n':
                    square.Piece = new Knight(Color.BLACK, square);
                    break;
                case 'b':
                    square.Piece = new Bishop(Color.BLACK, square);
                    break;
                case 'r':
                    square.Piece = new Rook(Color.BLACK, square);
                    break;
                case 'q':
                    square.Piece = new Queen(Color.BLACK, square);
                    break;
                case 'k':
                    square.Piece = new King(Color.BLACK, square);
                    break;
                case 'P':
                    square.Piece = new Pawn(Color.WHITE, square);
                    break;
                case 'N':
                    square.Piece = new Knight(Color.WHITE, square);
                    break;
                case 'B':
                    square.Piece = new Bishop(Color.WHITE, square);
                    break;
                case 'R':
                    square.Piece = new Rook(Color.WHITE, square);
                    break;
                case 'Q':
                    square.Piece = new Queen(Color.WHITE, square);
                    break;
                case 'K':
                    square.Piece = new King(Color.WHITE, square);
                    break;
                default:
                    square.Piece = null;
                    break;
            }
        }
    }
}
