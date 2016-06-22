using System;
using System.IO;
using System.Xml.Serialization;

namespace NHDG.NHDGCommon.AppSettings {
	/// <summary>The class that manages the settings.</summary>
	public class SettingsManager {
		private static SettingsManager instance;

		/// <summary>The class containing the actual settings.</summary>
		public Settings Settings;


		/// <summary>Gets the singleton instance of the SettingsManager.</summary>
		public static SettingsManager Instance {
			get { return instance; }
		}


		/// <summary>Constructor.</summary>
		static SettingsManager() { instance = new SettingsManager(); }
		private SettingsManager() {
			if (File.Exists(Environment.GetEnvironmentVariable("NHDGCONFIG"))) {
				Settings = (Settings)Utilities.DeserializeFromFile(typeof(Settings), Environment.GetEnvironmentVariable("NHDGCONFIG"));
			} else {
				throw new FileNotFoundException("NHDG configuration file not found.");
			}
		}


		/// <summary>Reloads the configuration file into the SettingsManager.</summary>
		public void Reload() {
			if (System.IO.File.Exists(Environment.GetEnvironmentVariable("NHDGCONFIG"))) {
				Settings = (Settings)Utilities.DeserializeFromFile(typeof(Settings), Environment.GetEnvironmentVariable("NHDGCONFIG"));
			} else {
				throw new FileNotFoundException("NHDG configuration file not found.");
			}
		}
	}
}
