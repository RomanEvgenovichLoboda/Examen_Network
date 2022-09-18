using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.Model
{
    public class BulletData
    {
        public BulletData() { }
        public BulletData(Point location, string imagePath, string move)
        {
            Location = location;
            ImagePath = imagePath;
            Move = move;
        }
        public Point Location { get; set; }
        public string ImagePath { get; set; }
        public string Move { get; set; }
    }
}
