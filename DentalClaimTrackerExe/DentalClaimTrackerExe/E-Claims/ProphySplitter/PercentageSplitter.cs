using System;
using System.Collections;
using System.Data.SqlClient;
using NHDG.NHDGCommon;
using NHDG.NHDGCommon.Claims;
using System.Data;

namespace NHDG.ProphySplitter {
	/// <summary>Represents a splitting rule with percentage-based fees.</summary>
	public class PercentageSplitter : Splitter {
		/// <summary>Initializes an instance of PercentageSplitter.</summary>
		/// <param name="conn">The database connection to retreive the rule details from.</param>
		/// <param name="splitRuleID">The database ID of the rule this Splitter represents.</param>
		/// <param name="procedureCode">The ADA procedure code this splitter operates on.</param>
		/// <param name="carrier">The insurance carrier this splitter is for.</param>
        public PercentageSplitter(int splitRuleID, string procedureCode, string carrier)
            : base(procedureCode, carrier)
        {

            C_DentalClaimTracker.splitter_rule_details srd = new C_DentalClaimTracker.splitter_rule_details();

            DataTable details = srd.Search("SELECT rd.PROCEDURE_CODE, " +
                                            "       rd.Description AS DESCRIPTION, " +
                                            "       rd.RULE_VALUE " +
                                            "FROM SPLITTER_RULE_DETAILS rd " +
                                            "WHERE (SPLIT_RULE = " + splitRuleID.ToString() + ")");
            DataTableReader reader = details.CreateDataReader();

            SplitRuleDetail d;
            while (reader.Read())
            {
                d = new SplitRuleDetail();
                d.ProcedureCode = reader.GetString(reader.GetOrdinal("PROCEDURE_CODE"));
                d.Description = reader.GetString(reader.GetOrdinal("DESCRIPTION"));
                d.Value = reader.GetInt32(reader.GetOrdinal("RULE_VALUE"));
                ruleDetails.Add(d);
            }
            reader.Close();

            // Didn't get any details -- this is worthless.
            if (ruleDetails.Count < 1)
            {
                throw new Exception("No details were found for rule " + splitRuleID.ToString() + ".");
            }
        }

		/// <summary>Applies the splitting rule to the claim provided.</summary>
		/// <param name="c">The claim to split.</param>
		/// <remarks>If the procedure code is not in the claim, or the claim has all
		/// of the resulting codes of this rule already, then this function will not
		/// modify the claim in any way.</remarks>
		override public void Split(Claim c) {
			if (isAlreadySplit(c)) { return; }

			int tempFee;
			Treatment tempTreatment;
			ArrayList newTreatments = new ArrayList();
			int totalFee = 0;

			foreach (Treatment t in c.TreatmentInformation.Treatments) {
				if (t.ProcedureCode == this.ProcedureCode) {
					foreach (SplitRuleDetail d in this.ruleDetails) {
						tempTreatment = new Treatment();
						tempTreatment.Description = d.Description;
						tempTreatment.DiagnosisIndex = t.DiagnosisIndex;
						tempFee = NHDG.NHDGCommon.Utilities.UnFormatCurrency(t.Fee);
						tempFee = (int)(Math.Floor((double)tempFee * ((double)d.Value / 10000.0)));
						tempTreatment.Fee = NHDG.NHDGCommon.Utilities.FormatCurrencyForXML(tempFee);
						tempTreatment.ProcedureCode = d.ProcedureCode;
						tempTreatment.ProcedureDate = t.ProcedureDate;
						tempTreatment.Quantity = t.Quantity;
						tempTreatment.Surface = t.Surface;
						tempTreatment.Tooth = t.Tooth;
                        tempTreatment.ToothEnd = t.ToothEnd;
                        tempTreatment.SecondaryDate = t.SecondaryDate;
                        tempTreatment.PrimaryPaidAmount = t.PrimaryPaidAmount;
                        tempTreatment.PatientResponsibleAmount = t.PatientResponsibleAmount;
						newTreatments.Add(tempTreatment);
						totalFee += Utilities.UnFormatCurrency(tempTreatment.Fee);
					}
				} else {
					newTreatments.Add(t);
					totalFee += Utilities.UnFormatCurrency(t.Fee);
				}
			}

			c.TreatmentInformation.Treatments = newTreatments;
			c.TreatmentInformation.TotalFee = Utilities.FormatCurrencyForXML(totalFee);
		}
	}
}
