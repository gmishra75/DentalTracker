using System;
using System.Data.SqlClient;
using System.Collections;
using System.Xml.Serialization;

namespace NHDG.NHDGCommon.Claims {
	/// <summary>Represents a batch group of claims.</summary>
	public class ClaimBatch {
		/// <summary>Contains general information about the batch.</summary>
		public BatchInformation BatchInformation = new BatchInformation();

		/// <summary>The list of claims.</summary>
		[XmlArrayItem(typeof(Claim))]
		public ArrayList Claims = new ArrayList();
	}


	/// <summary>Holds general batch information.</summary>
	public class BatchInformation {
		/// <summary>The unique ID for the batch.</summary>
		public int BatchID = -1;

		/// <summary>The date/time this batch was created.</summary>
		public DateTime DateCreated = DateTime.Now;
	}
}
