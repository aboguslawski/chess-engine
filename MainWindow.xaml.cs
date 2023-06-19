using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Windows;
using System.Windows.Input;
using chess_aboguslawski.statics;
using Microsoft.Extensions.Configuration;

namespace chess_aboguslawski
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Game game;

        public MainWindow()
        {
            InitializeComponent();
            SetupGame();
        }

        private void SetupGame()
        {
            //Environment.CurrentDirectory = "D:/chess-aboguslawski/chess-aboguslawski/chess-aboguslawski";
            Environment.CurrentDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            Console.WriteLine(Environment.CurrentDirectory);
            game = new Game();
            game.Init(MyCanvas);
        }

        private void MyCanvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var file = (int) Math.Floor(e.GetPosition(MyCanvas).X / Sizes.SQUARE_SIDE);
            var rank = (int) Math.Ceiling(e.GetPosition(MyCanvas).Y / Sizes.SQUARE_SIDE);
            rank = Math.Abs(rank - 8);

            if(game.PlayerToMove() == Color.WHITE)
            {
                game.ClickEvent(file, rank);
            }
        }

        private void MyCanvas_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (game.PlayerToMove() == Color.BLACK)
            {
                game.ComputerMove();
            }
        }

        private void StartGame()
        {

        }

    }
}
