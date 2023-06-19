using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace chess_aboguslawski.entities
{
    public class CastlingRights
    {
        public bool WhiteKingside { get; set; }
        public bool WhiteQueenside { get; set; }
        public bool BlackKingside { get; set; }
        public bool BlackQueenside { get; set; }

        public static CastlingRights FromFen(string fen)
        {
            return new CastlingRights
            {
                WhiteKingside = fen.Contains('K'),
                WhiteQueenside = fen.Contains('Q'),
                BlackKingside = fen.Contains('k'),
                BlackQueenside = fen.Contains('q')
            };
        }
    }
}
