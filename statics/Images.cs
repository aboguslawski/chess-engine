using System;
using System.Windows.Media.Imaging;

namespace chess_aboguslawski.statics
{
    public static class Images
    {
        public static readonly BitmapImage lightSquare = new BitmapImage(new Uri(Uris.lightSqareImage, UriKind.Relative));
        public static readonly BitmapImage darkSquare = new BitmapImage(new Uri(Uris.darkSqareImage, UriKind.Relative));
                      
        public static readonly BitmapImage whitePawn = new BitmapImage(new Uri(Uris.whitePawnImage, UriKind.Relative));
        public static readonly BitmapImage whiteKnight = new BitmapImage(new Uri(Uris.whiteKnightImage, UriKind.Relative));
        public static readonly BitmapImage whiteBishop = new BitmapImage(new Uri(Uris.whiteBishopImage, UriKind.Relative));
        public static readonly BitmapImage whiteRook = new BitmapImage(new Uri(Uris.whiteRookImage, UriKind.Relative));
        public static readonly BitmapImage whiteQueen = new BitmapImage(new Uri(Uris.whiteQueenImage, UriKind.Relative));
        public static readonly BitmapImage whiteKing = new BitmapImage(new Uri(Uris.whiteKingImage, UriKind.Relative));
                      
        public static readonly BitmapImage blackPawn = new BitmapImage(new Uri(Uris.blackPawnImage, UriKind.Relative));
        public static readonly BitmapImage blackKnight = new BitmapImage(new Uri(Uris.blackKnightImage, UriKind.Relative));
        public static readonly BitmapImage blackBishop = new BitmapImage(new Uri(Uris.blackBishopImage, UriKind.Relative));
        public static readonly BitmapImage blackRook = new BitmapImage(new Uri(Uris.blackRookImage, UriKind.Relative));
        public static readonly BitmapImage blackQueen = new BitmapImage(new Uri(Uris.blackQueenImage, UriKind.Relative));
        public static readonly BitmapImage blackKing = new BitmapImage(new Uri(Uris.blackKingImage, UriKind.Relative));

        public static readonly BitmapImage legalSquare = new BitmapImage(new Uri(Uris.legalSquare, UriKind.Relative));

    }
}
