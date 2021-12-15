using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bank
{// Port=1433; DESKTOP-F6MVFN7 Pwd=root;
 // "Server=127.0.0.1;Port=3306;Uid=username;Pwd=password;Database=db;"
 //  "Server=127.0.0.1;Port=1433;Database=Bank;Uid=DESKTOP-F6MVFN7/SQLEXPRESS;"
 //data source=DESKTOP-F6MVFN7\SQLEXPRESS
 //Data Source=localhost;Initial Catalog=Bank;Integrated Security=True  DESKTOP-F6MVFN7\SQLEXPRESS
 //Data Source=DESKTOP-F6MVFN7\SQLEXPRESS;Initial Catalog=Bank;Integrated Security=True
    class LoginConnection
    {
        MySqlConnection connection = new MySqlConnection(@"Data Source=DESKTOP-F6MVFN7\SQLEXPRESS;Initial Catalog=Bank;Integrated Security=True");

        //public void openConnection()
        //{
        //    if(connection.State == System.Data.ConnectionState.Closed)
        //    {
        //        connection.Open();
        //    }
        //}

        //public void closeConnection()
        //{
        //    if (connection.State == System.Data.ConnectionState.Open)
        //    {
        //        connection.Close();
        //    }
        //}

        public MySqlConnection getConnection()
        {
            return connection;
        }
    }
}
