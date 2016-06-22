using System;
using System.Collections;
using System.Text.RegularExpressions;
using NHDG.NHDGCommon.Claims;

namespace NHDG.ProphySplitter {
	/// <summary>Represents a splitting rule.</summary>
	abstract public class Splitter {
		/// <summary>The details of this splitting rule.</summary>
		protected ArrayList ruleDetails = new ArrayList();

		/// <summary>The ADA procedure code that this rule will split.</summary>
		public string ProcedureCode = string.Empty;

		/// <summary>The specific insurance carrier that this rule is for.</summary>
		public string Carrier = string.Empty;


		/// <summary>Initializes an instance of a Splitter.</summary>
		/// <param name="procedureCode">The ADA procedure code this splitter operates on.</param>
		/// <param name="carrier">The insurance carrier this splitter is for.</param>
		public Splitter(string procedureCode, string carrier) {
			this.ProcedureCode = procedureCode;
			this.Carrier = carrier;
		}


		/// <summary>Applies the splitting rule to the claim provided.</summary>
		/// <param name="c">The claim to split.</param>
		/// <remarks>If the procedure code is not in the claim, or the claim has all
		/// of the resulting codes of this rule already, then this function will not
		/// modify the claim in any way.</remarks>
		abstract public void Split(Claim c);


		/// <summary>Check to see if all the targets of this split already exist in the claim.</summary>
		/// <param name="c">The claim to check.</param>
		/// <returns>Whether or not this claim already has all the targets of this split.</returns>
		protected bool isAlreadySplit(Claim c) {
			bool found;

			foreach (SplitRuleDetail d in ruleDetails) {
				found = false;

				foreach (Treatment t in c.TreatmentInformation.Treatments) {
					if (t.ProcedureCode == d.ProcedureCode) {
						found = true;
						break;
					}
				}

				if (!found) { return false; }
			}

			return true;
		}


		/// <summary>Determines if this splitting rule is applicable to a claim.</summary>
		/// <param name="c">The claim to check.</param>
		/// <returns>Whether or not this splitting rule applies to the claim provided.</returns>
		public bool IsApplicable(Claim c) {
			// See if the primary carrier is right.
			Regex r = new Regex(this.Carrier, RegexOptions.IgnoreCase);
			if ((c.Identity.Type == ClaimType.Primary) && (!r.IsMatch(c.GeneralInformation.Carrier.Name))) {
				return false;
			} else if ((c.Identity.Type == ClaimType.Secondary) && (!r.IsMatch(c.OtherPolicy.PlanName))) {
				return false;
			}

			// See if it has the code we're looking for.
			foreach (Treatment t in c.TreatmentInformation.Treatments) {
				if (t.ProcedureCode == this.ProcedureCode) {
					return true;
				}
			}

			return false;
		}
	}


	/// <summary>Represents a single detail of a splitting rule.</summary>
	public class SplitRuleDetail {
		/// <summary>The procedure code this detail applies to.</summary>
		public string ProcedureCode = string.Empty;

		/// <summary>The description of the Procedure Code.</summary>
		public string Description = string.Empty;

		/// <summary>The precedence of this detail over the others in the rule.</summary>
		public short Priority = 0;

		/// <summary>The value this detail uses in its calculations.</summary>
		public int Value = 0;
	}
}
