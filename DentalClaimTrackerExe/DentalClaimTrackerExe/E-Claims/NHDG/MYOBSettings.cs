using System;

namespace NHDG.NHDGCommon.AppSettings {
	/// <summary>A class that groups the settings to access/execute MYOB.</summary>
	public class MYOBSettings {
		/// <summary>The path to the MYOB executable.</summary>
		public string Executable = string.Empty;

		/// <summary>The path to the MYOB data file.</summary>
		public string DataFile = string.Empty;

		/// <summary>The username to use to log into MYOB.</summary>
		public string Username = string.Empty;

		/// <summary>The password to use to log into MYOB.</summary>
		public string Password = string.Empty;

		/// <summary>The title of the MYOB window.</summary>
		public string WindowTitle = "MYOB Plus";
	}
}
