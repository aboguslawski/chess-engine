using chess_aboguslawski.entities.pieces;

namespace chess_aboguslawski.entities
{
    public class Square
    {
        public int File { get; set; }
        public int Rank { get; set; }
        public Color Color { get; set; }
        public Piece? Piece { get; set; }
        public Board Board { get; set; } 

        public Square() { }

        public Square(int file, int rank, Board board)
        {
            Board = board;
            File = file;
            Rank = rank;
            Color = (file + rank) % 2 == 0 ? Color.BLACK: Color.WHITE;
        }

        public bool IsEmpty => Piece == null;

        public override string ToString()
        {
            var result = string.Empty;

            switch (File)
            {
                case 0:
                    result += 'a';
                    break;
                case 1:
                    result += 'b';
                    break;
                case 2:
                    result += 'c';
                    break;
                case 3:
                    result += 'd';
                    break;
                case 4:
                    result += 'e';
                    break;
                case 5:
                    result += 'f';
                    break;
                case 6:
                    result += 'g';
                    break;
                case 7:
                    result += 'h';
                    break;
            }

            result += Rank + 1;
            return result;
        }
    }
}
