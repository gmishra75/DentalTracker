using System;

namespace NHDG.NHDGCommon.AppSettings {
	/// <summary>A class that groups the settings to use when printing.</summary>
	public class PrintingSettings {
		/// <summary>The program/command to use when printing HTML.</summary>
		public string HTMLCommand = string.Empty;

		/// <summary>The arguments to pass to the PrintCommand when printing HTML.</summary>
		public string HTMLArguments = string.Empty;
	}
}
