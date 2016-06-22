using System;
using System.Collections.Generic;
using System.Text;
using System.Data.OleDb;

namespace C_DentalClaimTracker
{
    /// <summary>
    /// Provides an alias interface for SQL connection strings
    /// </summary>	
    public class ConnectionAlias
    {
        private string _databaseName;
        private string _serverName;
        private string _userName;
        private string _password;
        private string _options;
        private SourceTypes _dataSource;

        public ConnectionAlias()
        {
            InitializeConnectionAlias();
        }

        /// <summary>
        /// Customize this section to load from a file to increase portability
        /// </summary>
        private void InitializeConnectionAlias()
        {
            EnvironmentVars a = new EnvironmentVars(true);
            //ServerName = Properties.Settings.Default.ServerNameSQL;
            ServerName = "BHUPENDRA-PC";
//#if DEBUG
//            ServerName = "(local)\\SQLEXPRESS";
//#endif

            //DatabaseName = Properties.Settings.Default.DatabaseName;
            DatabaseName = "claim_production";
            //UserName = "claimtracker";
            //Password = "claim";
            UserName = "sa";
            Password = "sa123";
            _dataSource = SourceTypes.SqlServer;
        }

        public SourceTypes SourceType
        {
            get { return _dataSource; }
            set { _dataSource = value; }
        }

        public string DatabaseName
        {
            get { return _databaseName; }
            set { _databaseName = value; }
        }

        public string ServerName
        {
            get { return _serverName; }
            set { _serverName = value; }
        }

        public string UserName
        {
            get { return _userName; }
            set { _userName = value; }
        }

        public string Password
        {
            get { return _password; }
            set { _password = value; }
        }

        public string Options
        {
            get { return _options; }
            set { _options = value; }
        }

        public bool CompareToConnection(OleDbConnection conCompare)
        {
            // Look for databasename
            if (conCompare.ConnectionString.IndexOf(DatabaseName) == -1)
                return false;

            // Look for servername
            if (conCompare.ConnectionString.IndexOf(ServerName) == -1)
                return false;

            // Look for username
            if (conCompare.ConnectionString.IndexOf(UserName) == -1)
                return false;

            return true;
        }

        public bool CompareToConnection(string conCompareString)
        {
            // Look for databasename
            if (conCompareString.IndexOf(DatabaseName) == -1)
                return false;

            // Look for servername
            if (conCompareString.IndexOf(ServerName) == -1)
                return false;

            // Look for username
            if (conCompareString.IndexOf(UserName) == -1)
                return false;

            return true;
        }

        public bool CompareToConnection(ConnectionAlias conCompareAlias)
        {
            // Look for databasename
            if (conCompareAlias.DatabaseName != DatabaseName)
                return false;

            // Look for servername
            if (conCompareAlias.ServerName != ServerName)
                return false;

            // Look for username
            if (conCompareAlias.UserName != UserName)
                return false;

            return true;
        }

        // Returns a connection string based on the alias' properties.
        public string GetConnectionString()
        {
            string conString = "";
            string userLabel = "user";
            string pwLabel = "password";
            if (SourceType == SourceTypes.Access)
                conString += "Provider=Microsoft.Jet.OLEDB.4.0;";
            else if (SourceType == SourceTypes.SqlServer)
            {
                conString += "Provider=SQLOLEDB;";
                userLabel = "uid";
                pwLabel = "pwd";
            }
            else if (SourceType == SourceTypes.MySQL)
            {
                conString = "Driver={MySQL OleDb 5.1 Driver};";
            }


            if (UserName.Trim() != String.Empty)
                conString += userLabel + "=" + UserName + ";";

            if (Password.Trim() != String.Empty)
                conString += pwLabel + "=" + Password + ";";

            if ((SourceType == SourceTypes.SqlServer) && (DatabaseName.Trim() != String.Empty))
                conString += "initial catalog=" + DatabaseName + ";";
            else if ((SourceType == SourceTypes.MySQL) && (DatabaseName.Trim() != String.Empty))
                conString += "database=" + DatabaseName + ";";

            if (ServerName.Trim() != String.Empty)
                conString += "server=" + ServerName;

            return conString;
        }
    }

    public enum SourceTypes
    {
        Access,
        SqlServer,
        MySQL
    }
}
