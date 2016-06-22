using System;
using System.Collections;
using System.Data.SqlClient;
using NHDG.NHDGCommon;
using NHDG.NHDGCommon.Claims;
using System.Data;
using System.Collections.Generic;

namespace NHDG.ProphySplitter {
	/// <summary>Represents a splitting rule with remainder fees.</summary>
	public class RemainderSplitter : Splitter {
		/// <summary>Initializes an instance of RemainderSplitter.</summary>
		/// <param name="conn">The database connection to retreive the rule details from.</param>
		/// <param name="splitRuleID">The database ID of the rule this Splitter represents.</param>
		/// <param name="procedureCode">The ADA procedure code this splitter operates on.</param>
		/// <param name="carrier">The insurance carrier this splitter is for.</param>
        public RemainderSplitter(int splitRuleID, string procedureCode, string carrier)
            : base(procedureCode, carrier)
        {
            C_DentalClaimTracker.splitter_rule_details srd = new C_DentalClaimTracker.splitter_rule_details();

            DataTable details = srd.Search("SELECT rd.PROCEDURE_CODE, " +
                                            "       rd.PRIORITY, " +
                                            "       rd.DESCRIPTION, " +
                                            "       rd.RULE_VALUE " +
                                            "FROM SPLITTER_RULE_DETAILS rd " +                             
                                            "WHERE (SPLIT_RULE = " + splitRuleID.ToString() + ") " +
                                            "ORDER BY rd.PRIORITY DESC");

            // "       ISNULL(code.DESCRIPTION, ' ') AS DESCRIPTION, " +
            // "     LEFT OUTER JOIN DDB_PROC_CODE_BASE code ON (rd.PROCEDURE_CODE = code.ADACODE) " +
            DataTableReader reader = details.CreateDataReader();

            SplitRuleDetail d;
            while (reader.Read())
            {
                d = new SplitRuleDetail();
                d.ProcedureCode = reader.GetString(reader.GetOrdinal("PROCEDURE_CODE"));
                d.Description = reader.GetString(reader.GetOrdinal("DESCRIPTION"));
                d.Value = reader.GetInt32(reader.GetOrdinal("RULE_VALUE"));
                d.Priority = (Int16) reader.GetInt32(reader.GetOrdinal("PRIORITY"));
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
            int tempPayments;
			Treatment tempTreatment;
			ArrayList newTreatments = new ArrayList();
			int totalFee = 0;
            bool isSecondary = c.Identity.Type == ClaimType.Secondary;

            foreach (Treatment t in c.TreatmentInformation.Treatments)
            {
                if (t.ProcedureCode == this.ProcedureCode)
                {
                    bool dontSplit = false;


                    if ((t.ProcedureCode == "D1110") || (t.ProcedureCode == "D1120"))
                    {
                        List<string> dontSplitProcedures = new List<string>(new string[] { "D0120", "D0130", "D0140",
                            "D0150","D0160","D0180" });

                        // Have to check and see if a certain list of procedures exist on the claim - if so, don't merge
                        foreach (Treatment trt in c.TreatmentInformation.Treatments)
                        {
                            if (dontSplitProcedures.Contains(trt.ProcedureCode))
                            {
                                dontSplit = true;
                                break;
                            }
                        }
                    }

                    if (!dontSplit)
                    {
                        // Procedure is ok to split
                        tempFee = NHDG.NHDGCommon.Utilities.UnFormatCurrency(t.Fee);

                        if (isSecondary)
                            tempPayments = NHDG.NHDGCommon.Utilities.UnFormatCurrency(t.PrimaryPaidAmount);
                        else
                            tempPayments = 0;
                        foreach (SplitRuleDetail d in this.ruleDetails)
                        {
                            tempTreatment = new Treatment();
                            tempTreatment.Description = d.Description;
                            tempTreatment.DiagnosisIndex = t.DiagnosisIndex;
                            if (d.Priority == 0)
                            {
                                tempTreatment.Fee = NHDG.NHDGCommon.Utilities.FormatCurrencyForXML(tempFee);
                                if (isSecondary)
                                    tempTreatment.PrimaryPaidAmount = NHDG.NHDGCommon.Utilities.FormatCurrencyForXML(tempPayments);
                            }
                            else if (tempFee >= d.Value)
                            {
                                tempFee -= d.Value;
                                tempTreatment.Fee = NHDG.NHDGCommon.Utilities.FormatCurrencyForXML(d.Value);

                                if (isSecondary)
                                {
                                    if (tempPayments > NHDG.NHDGCommon.Utilities.UnFormatCurrency(tempTreatment.Fee))
                                    {
                                        // There is more than enough money to cover this first payment
                                        tempTreatment.PrimaryPaidAmount = tempTreatment.Fee;
                                    }
                                    else
                                    {
                                        // All the available money goes toward this treatment
                                        tempTreatment.PrimaryPaidAmount = t.PrimaryPaidAmount;
                                    }

                                    tempPayments -= NHDG.NHDGCommon.Utilities.UnFormatCurrency(tempTreatment.PrimaryPaidAmount);
                                }
                            }
                            else
                            {
                                tempTreatment.Fee = NHDG.NHDGCommon.Utilities.FormatCurrencyForXML(tempFee);
                                if (tempFee > 0) { tempFee = 0; }

                                if (isSecondary)
                                {
                                    if (tempPayments > NHDG.NHDGCommon.Utilities.UnFormatCurrency(tempTreatment.Fee))
                                    {
                                        // There is more than enough money to cover this first payment
                                        tempTreatment.PrimaryPaidAmount = tempTreatment.Fee;
                                    }
                                    else
                                    {
                                        // All the available money goes toward this treatment
                                        tempTreatment.PrimaryPaidAmount = NHDG.NHDGCommon.Utilities.FormatCurrencyForXML(tempPayments);
                                    }

                                    tempPayments -= NHDG.NHDGCommon.Utilities.UnFormatCurrency(tempTreatment.PrimaryPaidAmount);
                                }
                            }
                            tempTreatment.ProcedureCode = d.ProcedureCode;
                            tempTreatment.ProcedureDate = t.ProcedureDate;
                            tempTreatment.Quantity = t.Quantity;
                            tempTreatment.Surface = t.Surface;
                            tempTreatment.Tooth = t.Tooth;
                            tempTreatment.ToothEnd = t.ToothEnd;
                            if (isSecondary)
                            {
                                tempTreatment.SecondaryDate = t.SecondaryDate;
                                tempTreatment.PatientResponsibleAmount =
                                    NHDG.NHDGCommon.Utilities.FormatCurrencyForXML(NHDG.NHDGCommon.Utilities.UnFormatCurrency(tempTreatment.Fee) -
                                        NHDG.NHDGCommon.Utilities.UnFormatCurrency(tempTreatment.PrimaryPaidAmount));
                            }
                            newTreatments.Add(tempTreatment);
                            totalFee += Utilities.UnFormatCurrency(tempTreatment.Fee);
                        }
                    }
                    else
                        dontSplit = true;


                    if (dontSplit)
                    {
                        newTreatments.Add(t);
                        totalFee += Utilities.UnFormatCurrency(t.Fee);
                    }
                }
                else
                {
                    newTreatments.Add(t);
                    totalFee += Utilities.UnFormatCurrency(t.Fee);
                }
            }

			c.TreatmentInformation.Treatments = newTreatments;
			c.TreatmentInformation.TotalFee = Utilities.FormatCurrencyForXML(totalFee);
		}
	}
}
