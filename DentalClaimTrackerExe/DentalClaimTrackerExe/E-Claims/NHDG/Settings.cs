using System;
using System.Collections;
using System.Xml;
using System.Xml.Serialization;

namespace NHDG.NHDGCommon.AppSettings {
	/// <summary>The class that holds the actual settings.</summary>
	[XmlRoot("NHDG")]
	public class Settings {
		/// <summary>Paths to the Dentrix executables.</summary>
		public DentrixPathSettings DentrixPaths = new DentrixPathSettings();

		/// <summary>Information for accessing MYOB.</summary>
		public MYOBSettings MYOB = new MYOBSettings();

		/// <summary>Settings and paths for printing.</summary>
		public PrintingSettings Printing = new PrintingSettings();

		/// <summary>The settings for the individual applications.</summary>
		[XmlArrayItem(typeof(ApplicationSettings))]
		public ArrayList Applications = new ArrayList();

		/// <summary>The bucket where unidentified entries in the config file end up.</summary>
		[XmlAnyElement]
		public XmlElement[] Miscellaneous;


		/// <summary>Gets the settings for a specific application.</summary>
		public ApplicationSettings this[string application] {
			get {
				foreach (ApplicationSettings app in Applications) {
					if (app.ApplicationName == application) {
						return app;
					}
				}

				throw new ArgumentException("The settings for " + application + " are not defined.", "application");
			}
		}
	}
}
