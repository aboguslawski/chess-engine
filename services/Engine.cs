using chess_aboguslawski.entities;
using System.Collections.Generic;
using System.Linq;

namespace chess_aboguslawski.services
{
    public class Engine
    {
        private FENConverter fenConverter;
        private EvaluationBar evalBar;

        private Engine()
        {
            fenConverter = FENConverter.Instance;
            evalBar = EvaluationBar.Instance;
        }
        private static Engine _instance;
        public static Engine Instance => _instance ??= new Engine();

        public Continuation FindAndCalculateCandidateMoves(Board position, int depth, bool firstExecution)
        {
            var continuations = AllMovesInPosition(position);

            if (!continuations.Any())
            {
                return new Continuation
                {
                    Board = position,
                    FEN = fenConverter.ToFen(position),
                    PieceSquare = null,
                    Move = null,
                    Evaluation = GameEndedEvaluation(position)
                };
            }

            if (!firstExecution)
            {
                continuations = SelectCandidateMoves(continuations, position.PlayerToMove);
            }

            if (depth != 0)
            {
                foreach (var continuation in continuations)
                {
                    continuation.Board.SwitchTurn();
                    var responses = FindAndCalculateCandidateMoves(continuation.Board, depth - 1, false);
                    continuation.Evaluation = responses.Evaluation;
                    continuation.Board = responses.Board;
                    continuation.FEN = responses.FEN;
                }
            }

            return MostFavourableContinuation(continuations, position.PlayerToMove);
        }

        private Continuation MostFavourableContinuation(List<Continuation> continuations, Color playerToMove)
        {
            if (playerToMove == Color.BLACK)
            {
                return continuations
                    .Aggregate((current, next) => current.Evaluation <= next.Evaluation ? current : next); // lowest eval
            }

            return continuations
                .Aggregate((current, next) => current.Evaluation >= next.Evaluation ? current : next); // highest eval
        }

        private List<Continuation> SelectCandidateMoves(List<Continuation> continuations, Color playerToMove)
        {
            var candidates = continuations
                .OrderBy(c => c.Evaluation)
                .ToList();

            if (playerToMove == Color.WHITE)
            {
                candidates.Reverse();
            }

            return candidates
                .Take(Game.candidatesConfig)
                .ToList();
        }

        private List<Continuation> AllMovesInPosition(Board position)
        {
            var currentFEN = fenConverter.ToFen(position);
            var continuations = new List<Continuation>();
            var squares = position.SquaresWithPiecesOfColor(position.PlayerToMove);

            foreach (var square in squares)
            {
                var moves = square.Piece.LegalMoves();
                foreach (var move in moves)
                {
                    Game.calculated_count++;
                    var workspacePosition = fenConverter.ToPosition(currentFEN);
                    workspacePosition.MovePiece(square.File, square.Rank, move.File, move.Rank);

                    if (workspacePosition.IsPositionLegal())
                    {
                        var continuation = new Continuation
                        {
                            Board = workspacePosition,
                            FEN = fenConverter.ToFen(workspacePosition),
                            PieceSquare = square,
                            Move = move,
                            Evaluation = evalBar.Evaluation(workspacePosition)
                        };
                        continuations.Add(continuation);
                    }
                }
            }

            return continuations;
        }

        private int GameEndedEvaluation(Board position)
        {
            if (position.IsKingAttacked(Color.WHITE))
            {
                return -1000000;
            }
            if (position.IsKingAttacked(Color.BLACK))
            {
                return 1000000;
            }
            return 0; // draw
        }
    }
}
