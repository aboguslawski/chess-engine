namespace chess_aboguslawski.entities
{
    public class Continuation
    {
        public string FEN { get; set; }
        public Board Board { get; set; }
        public Square PieceSquare { get; set; }
        public Square Move { get; set; }
        public double Evaluation { get; set; }
    }
}
