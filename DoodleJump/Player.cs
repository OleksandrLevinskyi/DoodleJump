/*
 * Player.cs
 * Player class
 * 
 * Revision History
 *          Oleksandr Levinskyi, 2020.12.06: Created & Imlemented
 *          Oleksandr Levinskyi, 2020.12.13: Revised & Completed
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoodleJump
{
    /// <summary>
    /// player class
    /// </summary>
    public class Player
    {
        public string Name { get; set; } // player name
        public int Score { get; set; } // player score

        /// <summary>
        /// player constructor
        /// </summary>
        /// <param name="name">name</param>
        /// <param name="score">score</param>
        public Player(string name, int score)
        {
            this.Name = name;
            this.Score = score;
        }

        /// <summary>
        /// makes player record a string
        /// </summary>
        /// <returns>string player record</returns>
        public override string ToString()
        {
            return $"{Name}\t{Score}";
        }
    }
}
