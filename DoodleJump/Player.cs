using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoodleJump
{
    public class Player
    {
        public string Name { get; set; }
        public int Score { get; set; }
        public Player(string name, int score)
        {
            this.Name = name;
            this.Score = score;
        }
        public override string ToString()
        {
            return $"{Name}\t{Score}";
        }
    }
}
