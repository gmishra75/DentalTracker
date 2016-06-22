using System;

namespace NHDG.NHDGCommon.AppSettings {
	/// <summary>A class that groups the settings to access a database.</summary>
	public class DBSettings {
		/// <summary>The name of the server that hosts the database.</summary>
		public string Server = string.Empty;

		/// <summary>The name of the database.</summary>
		public string Database = string.Empty;

		/// <summary>The username to use to log into the database.</summary>
		public string Username = string.Empty;

		/// <summary>The password to use to log into the database.</summary>
		public string Password = string.Empty;


		/// <summary>Return the string to use in a SqlConnction object.</summary>
		/// <returns>The SqlConnection string.</returns>
		public string GetSqlConnectionString() { return GetSqlConnectionString(string.Empty); }
		/// <summary>Return the string to use in a SqlConnction object.</summary>
		/// <param name="extraParameters">Extra parameters to include in the string.</param>
		/// <returns>The SqlConnection string.</returns>
		public string GetSqlConnectionString(string extraParameters) {
			string tmp;

			// Start off with the server.
			tmp = "Server=";
			if (Server != string.Empty) {
				tmp += Server;
			} else {
				tmp += "localhost";
			}
			tmp += "; ";

			// Add the database.
			if (Database != string.Empty) {
				tmp += "Database=" + Database + "; ";
			}

			// Add the username.
			if (Username != string.Empty) {
				tmp += "User ID=" + Username + "; ";
			}

			// Add the password.
			if (Password != string.Empty) {
				tmp += "Password=" + Password + "; ";
			}

			// Add any extra parameters.
			if (extraParameters != string.Empty) {
				tmp += extraParameters;
			}

			// Tidy up and return.
			tmp = tmp.Trim();
			if (tmp.EndsWith(";")) {
				return tmp.Substring(0, (tmp.Length -1));
			} else {
				return tmp;
			}
		}
	}
}
