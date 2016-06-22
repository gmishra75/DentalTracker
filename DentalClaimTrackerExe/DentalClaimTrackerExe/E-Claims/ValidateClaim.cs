using System;
using NHDG.NHDGCommon.Claims;

namespace NHDG.EClaims {
	/// <summary></summary>
	public class ValidateClaim {
		/// <summary></summary>
		/// <param name="c"></param>
		static public void Validate(Claim c) {
			// Make sure there's a carrier.
			if ((c.GeneralInformation.Carrier == null) || (c.GeneralInformation.Carrier.Name.Trim() == string.Empty)) {
				throw new Exception("Insurance Carrier is Missing.");
			}


			// Make sure there are treatments on the claim.
			if ((c.TreatmentInformation.Treatments == null) || (c.TreatmentInformation.Treatments.Count < 1)) {
				throw new Exception("Claim has no treatments listed.");
			}
		}
	}
}
