using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace chess_aboguslawski
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Rectangle piece;
        List<Rectangle> Squares = new List<Rectangle>();
        ImageBrush pieceImage = new ImageBrush();

        public MainWindow()
        {
            InitializeComponent();
            SetupGame();
        }

        private void MyCanvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var rank = Math.Ceiling(e.GetPosition(MyCanvas).X / 100);
            var file = Math.Ceiling(e.GetPosition(MyCanvas).Y / 100);
            Title = $"{rank} {file}";
        }

        private void SetupGame()
        {
            Environment.CurrentDirectory = "D:/chess-aboguslawski/chess-aboguslawski/chess-aboguslawski";
            pieceImage.ImageSource = new BitmapImage(new Uri("resources/pawn.png", UriKind.Relative));
            
            piece = new Rectangle
            {
                Height = 30,
                Width = 30,
                Fill = pieceImage,
                StrokeThickness = 2
            };
            MyCanvas.Children.Add(piece);

            var whiteSquareImage = new ImageBrush();
            var darkSquareImage = new ImageBrush();
            whiteSquareImage.ImageSource = new BitmapImage(new Uri("resources/white-square.png", UriKind.Relative));
            darkSquareImage.ImageSource = new BitmapImage(new Uri("resources/dark-square.png", UriKind.Relative));


            for (var i = 0; i < 8; i++)
            {
                for (var j = 0; j < 8; j++)
                {
                    var square = new Rectangle
                    {
                        Height = 100,
                        Width = 100,
                        Fill = (i + j)% 2 == 0 ? whiteSquareImage : darkSquareImage,
                        StrokeThickness = 1
                    };
                    Canvas.SetLeft(square, i * 100);
                    Canvas.SetTop(square, j * 100);
                    MyCanvas.Children.Add(square);
                }
            }
        }

        private void StartGame()
        {

        }

    }
}
