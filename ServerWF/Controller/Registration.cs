using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Client.Model;
using Dapper;
using System.Windows.Forms;

namespace Client.Controller
{
    internal class Registration
    {
        public bool IsRegistered { get; set; }
        public bool sidn_in;
        string connectionString = @"Data Source=DESKTOP-54SAU6R\SQLEXPRESS;Initial Catalog=Users;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";

        public void Regestration(string _email, string _pass)
        {
            try
            {
                List<Gamer> usList = GetUsers();
                foreach (var item in usList)
                {
                    if (item.Login == _email) { IsRegistered = true; }
                }
                if (IsRegistered) { Console.WriteLine("This E-mail is Exist !"); }
                else
                {
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        Gamer regestration = new Gamer() { Login = _email, Password = _pass, Coins = 0, Lifes = 3, Damage = 1, Kills = 0 };
                        connection.Execute("INSERT INTO Gamers (Login, Password, Coins, Lifes, Damage, Kills) VALUES(@Login,@Password,@Coins,@Lifes,@Damage,@Kills)", regestration);
                        MessageBox.Show("Registration is OK !");
                    }
                }
            }
            catch { MessageBox.Show("Conection Error"); }

        }
        public void signIn(string _email, string _pass)
        {
            try
            {
                List<Gamer> usList = GetUsers();
                foreach (var item in usList)
                {
                    if (item.Login == _email && item.Password == _pass)
                    {
                        //Program.mainForm.sign = true;
                        sidn_in = true;
                    }
                }
            }
            catch {MessageBox.Show("Conection Error"); }
        }
        public List<Gamer> GetUsers() { using (IDbConnection connection = new SqlConnection(connectionString)) { return connection.Query<Gamer>($"SELECT * FROM [Gamers] ").ToList(); } }

    }
}
