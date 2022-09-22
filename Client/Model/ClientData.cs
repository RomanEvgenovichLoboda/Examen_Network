using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.ConstrainedExecution;
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
            //Bullet = new BulletData();
            //Bullets = new List<BulletData>();
        }
        public bool IsBullet { get; set; }
        public Point Location { get; set; }
        public string ImagePath { get; set; }
        public string BulletMove { get; set; }
        public string Name { get; set; }
        // public BulletData Bullet { get; set; }

        //public List<BulletData> Bullets { get; set; }
        public override string ToString()
        {
            return $"Name - {Name}\n Location - X = {Location.X} Y = {Location.Y} \n ImageePath - {ImagePath}";
        }
    }
}
