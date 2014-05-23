using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using MySql.Data;
using MySql.Data.MySqlClient;

namespace backupBD
{
    public class BD
    {
        private IDbConnection _connection;
        public string CONNECTION_STRING = "";
        private string server;
        private string port;
        private string database;
        private string user;
        private string password;
        public TypeBD type;
        public enum TypeBD{MySql,SQLServer};
        public static BD BDLocal;
        public static BD BDServer;


        public static void LoadDataBases()
        {
            string server1 = Util.settings.ContainsKey("LOCAL.SERVER") && Util.settings["LOCAL.SERVER"].Trim() != "" ? "Data Source=" + Util.settings["LOCAL.SERVER"] : "";
            string port1 = Util.settings.ContainsKey("LOCAL.PORT") && Util.settings["LOCAL.PORT"].Trim() != "" ? "," + Util.settings["LOCAL.PORT"] : "";
            string database1 = Util.settings.ContainsKey("LOCAL.DATABASE") && Util.settings["LOCAL.DATABASE"].Trim() != "" ? "Database=" + Util.settings["LOCAL.DATABASE"] + ";" : "";
            string user1 = Util.settings.ContainsKey("LOCAL.USER") && Util.settings["LOCAL.USER"].Trim() != "" ? "User Id=" + Util.settings["LOCAL.USER"] + ";" : "";
            string password1 = Util.settings.ContainsKey("LOCAL.PASSWORD") && Util.settings["LOCAL.PASSWORD"].Trim() != "" ? "Password=" + Util.settings["LOCAL.PASSWORD"] + ";" : "";
            string tipo1 = Util.settings.ContainsKey("LOCAL.TYPE") && Util.settings["LOCAL.TYPE"].Trim() != "" ? Util.settings["LOCAL.TYPE"].ToLower() : "sqlserver";
            TypeBD t1 = TypeBD.MySql;
            if (tipo1 == "sqlserver")
            {
                t1 = TypeBD.SQLServer;
            }
            BDLocal = new BD(server1, port1, database1, user1, password1, t1);

            string server2 = Util.settings.ContainsKey("SERVER.SERVER") && Util.settings["SERVER.SERVER"].Trim() != "" ? "Data Source=" + Util.settings["SERVER.SERVER"] : "";
            string port2 = Util.settings.ContainsKey("SERVER.PORT") && Util.settings["SERVER.PORT"].Trim() != "" ? "," + Util.settings["SERVER.PORT"] : "";
            string database2 = Util.settings.ContainsKey("SERVER.DATABASE") && Util.settings["SERVER.DATABASE"].Trim() != "" ? "Database=" + Util.settings["SERVER.DATABASE"] + ";" : "";
            string user2 = Util.settings.ContainsKey("SERVER.USER") && Util.settings["SERVER.USER"].Trim() != "" ? "User Id=" + Util.settings["SERVER.USER"] + ";" : "";
            string password2 = Util.settings.ContainsKey("SERVER.PASSWORD") && Util.settings["SERVER.PASSWORD"].Trim() != "" ? "Password=" + Util.settings["SERVER.PASSWORD"] + ";" : "";
            string tipo2 = Util.settings.ContainsKey("SERVER.TYPE") && Util.settings["SERVER.TYPE"].Trim() != "" ? Util.settings["SERVER.TYPE"].ToLower() : "sqlserver";
            TypeBD t2 = TypeBD.MySql;
            if (tipo2 == "sqlserver")
            {
                t2 = TypeBD.SQLServer;
            }
            BDServer = new BD(server2, port2, database2, user2, password2, t2);
        }


        public BD(string server, string port, string database, string user, string password, TypeBD type)
        {
            this.server = this.server.Trim() != "" ? "Data Source=" + this.server : "";
            this.port = this.port.Trim() != "" ? "," + this.port : "";
            this.database = this.database.Trim() != "" ? "Database=" + this.database + ";" : "";
            this.user = this.user.Trim() != "" ? "User Id=" + this.user + ";" : "";
            this.password = this.password.Trim() != "" ? "Password=" + this.password + ";" : "";
            this.type = type;
            CONNECTION_STRING = database + server + port + ";" + user + password;
        }

        // Open connection
        public bool openDB()
        {
            try
            {
                if (this.type == BD.TypeBD.SQLServer)
                {
                    _connection = new SqlConnection(CONNECTION_STRING);
                }
                else
                {
                    _connection = new MySqlConnection(CONNECTION_STRING);
                }
                _connection.Open();
            }
            catch (Exception e)
            {
                return false;
            }

            return true;
        }//END

        public bool isOpen()
        {
            return _connection.State != ConnectionState.Closed;
        }

        // Close conection
        public bool closeDB()
        {
            if (_connection.State != ConnectionState.Closed)
                _connection.Close();
            return true;
        }

        public List<List<Object>> callProcedure(string name, Dictionary<String, Object> param)
        {
            List<List<Object>> ret = new List<List<object>>();
            IDbCommand cmd;
            if (this.type != BD.TypeBD.SQLServer)
            {
                cmd = new SqlCommand(name, (SqlConnection)_connection);
            }
            else { 
                cmd = new MySqlCommand(name, (MySqlConnection)_connection);
            }
            cmd.CommandType = CommandType.StoredProcedure;
            foreach (KeyValuePair<String, Object> item in param)
            {
                if (this.type != BD.TypeBD.SQLServer)
                {
                    ((SqlCommand)cmd).Parameters.AddWithValue(item.Key, item.Value);
                }
                else
                {
                    ((MySqlCommand)cmd).Parameters.AddWithValue(item.Key, item.Value);
                }
            }
            ret = executeSQL(cmd);

            return ret;
        }

        public List<List<Object>> executeSQL(IDbCommand cmd)
        {
            IDataReader dr = null;
            List<List<Object>> result = new List<List<object>>();
            try
            {
                dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    result.Add(new List<object>());
                    for (int i = 0; i < dr.FieldCount; i++)
                    {
                        result[result.Count - 1].Add(dr[i]);
                    }
                }
                dr.Close();
            }
            catch
            {
                if (dr != null)
                {
                    dr.Close();
                    dr = null;
                }
            }
            return result;
        }
    }
}