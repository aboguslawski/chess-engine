using System.Linq;

namespace chess_aboguslawski.services
{
    public class EvaluationBar
    {
        private FENConverter fenConverter;

        private EvaluationBar()
        {
            fenConverter = FENConverter.Instance;
        }

        private static EvaluationBar _instance;
        public static EvaluationBar Instance => _instance ??= new EvaluationBar();

        public int Evaluation(Board position)
        {
            return position.Squares
                .Where(square => !square.IsEmpty)
                .Select(square => square.Piece)
                .Aggregate(0, (result, next) => result + next.GetValue());
        }

        public int Evaluation(string fen)
        {
            return Evaluation(fenConverter.ToPosition(fen));
        }

        public int ShallowEvaluation(string fen)
        {
            int accumulator = 0;
            foreach(char c in fen)
            {
                accumulator += CharAsPieceValue(c);
            }
            return accumulator;
        }

        public bool IsBetterThan(double eval1, double eval2, Color playerToMove)
        {
            return playerToMove == Color.WHITE
                ? eval2 > eval1
                : eval2 < eval1;
        }

        private int CharAsPieceValue(char c)
        {
            switch (c)
            {
                case 'P': return 100;
                case 'N': return 300;
                case 'B': return 300;
                case 'R': return 500;
                case 'Q': return 900;
                case 'p': return -100;
                case 'n': return -300;
                case 'b': return -300;
                case 'r': return -500;
                case 'q': return -900;
                default: return 0;
            }
        }
    }
}
