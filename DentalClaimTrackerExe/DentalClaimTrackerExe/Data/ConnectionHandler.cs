using System;
using System.Data;
using System.Data.OleDb;
using System.Collections;
using System.Diagnostics;

namespace C_DentalClaimTracker
{
	
	/// <summary>
	/// Manages all active and non-active SQL connections.
	/// </summary>
    public class ConnectionHandler
    {

        public ConnectionHandler()
        { }

        public OleDbConnection AddConnection(ConnectionAlias alias)
        {
            OleDbConnection newConnection;

            newConnection = new OleDbConnection(alias.GetConnectionString());

            try
            {
                newConnection.Open();
            }
            catch (Exception err)
            {
                throw new Exception("The connection test for the newly added connection failed.", err);
            }

            return newConnection;
        }

        public OleDbConnection RequestConnection(ConnectionAlias alias, object obj)
        {
            return AddConnection(alias);
        }

        public void ReleaseConnection(object obj, OleDbConnection connection)
        {
            try
            {
                connection.Close();
            }
            catch (Exception err)
            {
                Debug.WriteLine("An error occurred closing a connection: " + err.Message);
            }
        }



        public enum ConnectionHolderStatus
        {
            Active,
            Closed,
            Idle
        }
    }
}
