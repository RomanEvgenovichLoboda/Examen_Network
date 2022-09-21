using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.Model
{

    public class ClientData
    {
        public ClientData() { }
        public ClientData(Point location, string imagePath)
        {
            Location = location;
            ImagePath = imagePath;
            //Bullets = new List<BulletData>();
        }
        public Point Location { get; set; }
        public string ImagePath { get; set; }
        //public List<BulletData> Bullets { get; set; }
        public override string ToString()
        {
            return $"Location - X = {Location.X} Y = {Location.Y} \n ImageePath - {ImagePath}";
        }
    }
}
