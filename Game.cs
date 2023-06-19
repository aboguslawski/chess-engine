using chess_aboguslawski.entities;
using chess_aboguslawski.services;
using System.Windows.Controls;
using System.Linq;
using System;
using System.Diagnostics;
using Microsoft.Extensions.Configuration;

namespace chess_aboguslawski
{
    public class Game
    {
        public static int calculated_count;
        public static int depthConfig;
        public static int candidatesConfig;
        private Board board;
        private Square storedSquare;
        private GUIService guiService;
        private Engine computer;
        private FENConverter fenConverter;
        private EvaluationBar evalBar;

        public Game()
        {
            var config = new ConfigurationBuilder()
                .AddJsonFile(Environment.CurrentDirectory + "/config.json", optional: true)
                .Build();
            depthConfig = int.Parse(config["depth"]);
            candidatesConfig = int.Parse(config["candidates"]);
            Console.WriteLine($"Engine running with depth: {depthConfig} and candidates: {candidatesConfig}");
            computer = Engine.Instance;
            fenConverter = FENConverter.Instance;
            evalBar = EvaluationBar.Instance;
        }

        public void Init(Canvas canvas)
        {
            board = fenConverter.ToPosition("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1");
            guiService = new GUIService(canvas, board);

            foreach (var square in board.Squares)
            {
                guiService.RenderSquare(square);
            }
            UpdateBoard();
        }

        public Color PlayerToMove()
        {
            return board.PlayerToMove;
        }

        public void ClickEvent(int file, int rank)
        {
            var clickedSquare = board.GetSquare(file, rank);

            if (storedSquare == null)
            {
                HandleClickEventChoosingPiece(clickedSquare);
            }
            else
            {
                HandleClickEventMovingPiece(clickedSquare);
                storedSquare = null;
            }
        }

        private void HandleClickEventChoosingPiece(Square clickedSquare)
        {
            if (!clickedSquare.IsEmpty && clickedSquare.Piece.Color == board.PlayerToMove)
            {
                guiService.DrawLegalMoves(clickedSquare.Piece);
                storedSquare = clickedSquare;
            }
        }

        private void HandleClickEventMovingPiece(Square clickedSquare)
        {
            var legalMoves = storedSquare.Piece.LegalMoves();

            if ((clickedSquare.IsEmpty
                || clickedSquare.Piece.Color != storedSquare.Piece.Color)
                && legalMoves.Contains(clickedSquare))
            {
                var boardAfterMove = fenConverter.ToPosition(fenConverter.ToFen(board));
                boardAfterMove.MovePiece(storedSquare.File, storedSquare.Rank, clickedSquare.File, clickedSquare.Rank);

                if (boardAfterMove.IsPositionLegal())
                {
                    board.MovePiece(storedSquare.File, storedSquare.Rank, clickedSquare.File, clickedSquare.Rank);
                    board.SwitchTurn();
                }
                else
                {
                    Console.WriteLine("move is not legal");
                }
            }
            UpdateBoard();
        }

        public async void ComputerMove()
        {
            calculated_count = 0;

            var watch = new Stopwatch();
            watch.Start();

            var bestMove = computer.FindAndCalculateCandidateMoves(board, depthConfig, true);

            watch.Stop();
            var src = board.GetSquare(bestMove.PieceSquare.File, bestMove.PieceSquare.Rank);
            var dst = bestMove.Move;
            board.MovePiece(src.File, src.Rank, dst.File, dst.Rank);
            board.SwitchTurn();
            UpdateBoard();

            Console.WriteLine($"calculated {calculated_count} positions");
            Console.WriteLine($"in {watch.ElapsedMilliseconds} ms");
            Console.WriteLine($"eval: {(double) evalBar.Evaluation(board)/100}");
            Console.WriteLine($"fen: {bestMove.FEN}");
        }

        
        private void UpdateBoard()
        {
            storedSquare = null;
            guiService.RemovePieces();
            guiService.ClearLegalMoves();
            guiService.RenderPieces();
        }

    }
}
