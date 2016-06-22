using System;

namespace NHDG.NHDGCommon.AppSettings {
	/// <summary>A class that represents the core settings needed for an application.</summary>
	public class ApplicationSettings {
		/// <summary>The name of the application these settings are for.</summary>
		public string ApplicationName = string.Empty;

		/// <summary>The path to the application's own settings file.</summary>
		public string ConfigurationFile = string.Empty;

		/// <summary>The path to the application's log4net settings.</summary>
		public string LogConfigurationFile = string.Empty;

		/// <summary>The settings to use for connecting to the database.</summary>
		public DBSettings DatabaseConnection = new DBSettings();
	}
}
