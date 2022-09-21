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
        public BulletData(string imagePath, string move)
        {
            ImagePath = imagePath;
            Move = move;
        }
        
        public string ImagePath { get; set; }
        public string Move { get; set; }
    }
}
