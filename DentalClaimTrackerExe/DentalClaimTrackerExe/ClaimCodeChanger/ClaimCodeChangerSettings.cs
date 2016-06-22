using System;
using System.Collections;
using System.Xml.Serialization;

namespace NHDG.ClaimCodeChanger {
	/// <summary>A class that groups the settings for the ClaimCodeChanger application.</summary>
	[XmlRoot("ClaimCodeChanger")]
	public class ClaimCodeChangerSettings {
		/// <summary>The changes to apply.</summary>
		[XmlArrayItem(ElementName="CodeChange", Type=typeof(CodeChange))]
		public ArrayList CodeChanges = new ArrayList();
	}


	/// <summary>Represents a code change.</summary>
	public class CodeChange {
		/// <summary>The regular expression that defines the Carrier this change applies to.</summary>
		public string Carrier = string.Empty;

		/// <summary>The regular expression that defines the procedure code to change.</summary>
		public string SearchFor = string.Empty;

		/// <summary>The regular expression that defines what to change the procedure code to.</summary>
		public string ReplaceWith = string.Empty;
	}
}
