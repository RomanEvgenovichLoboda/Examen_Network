using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.Model
{
    public class Gamer
    {
        public int Id { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public int Coins { get; set; }
        public int Lifes { get; set; }
        public int Damage { get; set; }
        public int Kills { get; set; }
        public override string ToString()
        {
            return $"{Login}";
        }
    }
}
