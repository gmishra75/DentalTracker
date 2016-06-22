using System;
using System.Collections.Generic;
using System.Text;
using NHDG.NHDGCommon.Claims;
using System.Collections;
using System.IO;
using NHDG.NHDGCommon;

namespace C_DentalClaimTracker
{
    class ProcedureCombiner
    {
        public void Combine(string inputFile)
        {
            Combine(inputFile, inputFile);
        }

        public void Combine(string inputFile, string outputFile)
        {
            ClaimBatch batch = null;

            if (!File.Exists(inputFile))
            {
                throw new Exception("Input file does not exist.");
            }

            // Read in the claims.
            try
            {
                batch = (ClaimBatch)Utilities.DeserializeFromFile(typeof(ClaimBatch), inputFile);
            }
            catch (Exception ex)
            {
                throw new Exception("Could not read batch file.", ex);
            }

            // Apply splitting rules.
            foreach (Claim c in batch.Claims)
            {
                try
                {
                    CombineClaim(c);
                }
                catch (Exception ex)
                {
                    throw new Exception("There was an error when processing claim " + c.Identity.ClaimID.ToString() + "/" + c.Identity.ClaimDB.ToString(), ex);
                }
            }

            // Write out the modified claims.
            try
            {
                Utilities.SerializeToFile(batch, typeof(ClaimBatch), outputFile);
            }
            catch (Exception ex)
            {
                throw new Exception("Could not write batch file.", ex);
            }
        }

        /// <summary>
        /// Combines any work done in the last 6 months with other work done in the last 6 months
        /// </summary>
        /// <param name="c"></param>
        private void CombineClaim(Claim c)
        {
            Dictionary<string, List<Treatment>> treatmentProcedures = new Dictionary<string,List<Treatment>>();
            ArrayList newTreatments = new ArrayList();

            // Test patient name is brereton

            foreach (Treatment t in c.TreatmentInformation.Treatments)
            {
                if (treatmentProcedures.ContainsKey(t.ProcedureCode))
                {
                    // Find out if I need to combine them, and if so add the second procedure to "to remove"
                    // make sure the latest date is the date used
                    bool combined = false;
                    foreach (Treatment lastTreatment in treatmentProcedures[t.ProcedureCode])
                    {
                        DateTime lastDate = Convert.ToDateTime(lastTreatment.ProcedureDate);
                        DateTime currentDate = Convert.ToDateTime(t.ProcedureDate);
                        bool shouldCombine = false;

                        
                        if (t.Surface == "")
                        {
                            if (t.Tooth != "")
                            {
                                if (t.Tooth == lastTreatment.Tooth)
                                {
                                    // only combine stuff with the same code, without surface, with a tooth number, 
                                    // and with the same tooth number
                                    shouldCombine = true;
                                }
                            }
                        }
                        
                        if (shouldCombine)
                        {
                            combined = true;
                            lastTreatment.Fee = ((decimal)Convert.ToDecimal(lastTreatment.Fee) + Convert.ToDecimal(t.Fee)).ToString("0.00");
                            if (lastDate < currentDate)
                            {
                                lastTreatment.ProcedureDate = t.ProcedureDate;
                            }
                            break;
                        }
                    }

                    if (!combined)
                    {
                        treatmentProcedures[t.ProcedureCode].Add(t);
                        newTreatments.Add(t);
                    }
                }
                else
                {
                    List<Treatment> toAdd = new List<Treatment>();
                    toAdd.Add(t);
                    treatmentProcedures.Add(t.ProcedureCode, toAdd);
                    newTreatments.Add(t);
                }
            }

            c.TreatmentInformation.Treatments = newTreatments;
            // The below line was here before - total fee shouldn't change, but verify that it works
            // c.TreatmentInformation.TotalFee = Utilities.FormatCurrencyForXML(totalFee);
        }
    }
}
