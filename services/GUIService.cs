using chess_aboguslawski.entities;
using chess_aboguslawski.entities.pieces;
using chess_aboguslawski.statics;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace chess_aboguslawski.services
{
    public class GUIService
    {

        private Canvas canvas;
        private Board board;

        private List<Rectangle> legalSquares;
        private List<Rectangle> renderedPieces;

        public GUIService(Canvas c, Board b)
        {
            canvas = c;
            board = b;
            legalSquares = new List<Rectangle>();
            renderedPieces = new List<Rectangle>();
        }


        public void RenderSquare(Square square)
        {
            var image = square.Color == Color.WHITE ? Images.lightSquare : Images.darkSquare;
            DrawImageInSquare(image, square.File, square.Rank);
        }

        public void RemovePieces()
        {
            foreach (var piece in renderedPieces)
            {
                canvas.Children.Remove(piece);
            }
            renderedPieces = new List<Rectangle>();
        }

        public void RenderPieces()
        {
            foreach (var square in board.Squares)
            {
                if (square.Piece != null)
                {
                    var image = new BitmapImage(new System.Uri(square.Piece.ImageUri, System.UriKind.Relative));
                    var pieceRectangle = DrawImageInSquare(image, square.File, square.Rank);
                    renderedPieces.Add(pieceRectangle);
                }
            }
        }

        public void ClearLegalMoves()
        {
            foreach (var rectangle in legalSquares)
            {
                canvas.Children.Remove(rectangle);
            }
            legalSquares = new List<Rectangle>();
        }

        public void DrawLegalMoves(Piece piece)
        {
            var legalMoves = piece.LegalMoves();

            foreach (var square in legalMoves)
            {
                var img = DrawImageInSquare(Images.legalSquare, square.File, square.Rank);
                legalSquares.Add(img);
            }
        }

        private Rectangle DrawImageInSquare(BitmapImage image, int file, int rank)
        {
            var fill = new ImageBrush();
            fill.ImageSource = image;
            var rectangle = new Rectangle
            {
                Height = Sizes.SQUARE_SIDE,
                Width = Sizes.SQUARE_SIDE,
                Fill = fill,
                StrokeThickness = 1
            };
            Canvas.SetLeft(rectangle, file * Sizes.SQUARE_SIDE);
            Canvas.SetBottom(rectangle, rank * Sizes.SQUARE_SIDE);
            canvas.Children.Add(rectangle);

            return rectangle;
        }
    }
}
