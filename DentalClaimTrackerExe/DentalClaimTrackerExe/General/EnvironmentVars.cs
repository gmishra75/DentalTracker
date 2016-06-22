using System;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Permissions;

namespace C_DentalClaimTracker
{
	/// <summary>
	/// Summary description for EnvironmentVars.
	/// </summary>
	public class EnvironmentVars
	{
		// Import the Kernel32 dll file.
		[DllImport("kernel32.dll",CharSet=CharSet.Auto, SetLastError=true)]

		[return:MarshalAs(UnmanagedType.Bool)]

			// The declaration is similar to the SDK function
		private static extern bool SetEnvironmentVariable(string lpName, string lpValue);

		// Set the environment vars to their default values
		public EnvironmentVars(bool setToDefault)
		{
			if (setToDefault)
			{
				ServerName = Environment.MachineName;
				FileLocation = System.Windows.Forms.Application.StartupPath; 
			}
		}

		public EnvironmentVars(string serverName, string fileLocation)
		{
			ServerName = serverName;
			FileLocation = fileLocation;
		}

		public string ServerName
		{
			get { return Environment.GetEnvironmentVariable("ServerName"); }
			set { SetEnvironmentVariableEx("ServerName", value); }
		}

		public string FileLocation
		{
			get { return Environment.GetEnvironmentVariable("FileLocation"); }
			set { SetEnvironmentVariableEx("FileLocation", value); }
		}

		private static bool SetEnvironmentVariableEx(string environmentVariable, string variableValue)
		{
			try
			{
				// Get the write permission to set the environment variable.
				EnvironmentPermission environmentPermission = new EnvironmentPermission(EnvironmentPermissionAccess.Write,environmentVariable);
				environmentPermission.Demand(); 
				return SetEnvironmentVariable(environmentVariable, variableValue);
			}
			catch( SecurityException e)
			{
                LoggingHelper.Log("Error SettingEnvironmentVariable in EnvironmentVars.SetEnvironmentVariables", LogSeverity.Error, e, false);
				Console.WriteLine("Exception:" + e.Message);
			}
			return false;
		}
	}


}
