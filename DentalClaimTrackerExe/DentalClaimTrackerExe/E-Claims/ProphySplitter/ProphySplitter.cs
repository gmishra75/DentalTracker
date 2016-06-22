using System;
using System.IO;
using System.Collections;
using System.Data.SqlClient;
using NHDG.NHDGCommon;
using NHDG.NHDGCommon.Claims;
using NHDG.NHDGCommon.AppSettings;
using System.Data;

namespace NHDG.ProphySplitter {
	public class ProphySplitter {
        public Hashtable Splitters = new Hashtable();

		static ProphySplitter() {}

        public void PerformSplit(string inputFile)
        {
            PerformSplit(inputFile, inputFile);
        }

        public void PerformSplit(string inputFile, string outputFile)
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

            // Connect to the database.


            // Load splitting rules.
            try
            {
                LoadSplitters();
            }
            catch (Exception ex)
            {
                throw new Exception("Could not load splitting rules.", ex);
            }

            if (Splitters.Count < 1)
            {
                // No splitting rules to apply
            }
            else
            {
                // Apply splitting rules.
                Splitter splitter = null;
                int numSplits = 0;
                foreach (Claim c in batch.Claims)
                {
                    foreach (string s in Splitters.Keys)
                    {
                        splitter = GetApplicableSplitter(c, s);
                        if (splitter != null)
                        {
                            try
                            {
                                System.Diagnostics.Debug.Print("Applying splitter rule (" + splitter.ProcedureCode + "/" + splitter.Carrier + ") to claim " + c.Identity.ClaimID.ToString() + "/" + c.Identity.ClaimDB.ToString() + "...");
                                splitter.Split(c);
                                numSplits++;
                            }
                            catch (Exception ex)
                            {
                                throw new Exception("There was an error when processing claim " + c.Identity.ClaimID.ToString() + "/" + c.Identity.ClaimDB.ToString() + " with rule " + splitter.ProcedureCode + "/" + splitter.Carrier, ex);
                            }
                        }
                    }
                }
                System.Diagnostics.Debug.Print("Total of " + numSplits.ToString() + " splits performed.");
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


		// Build the splitter table.
        private void LoadSplitters()
        {
            ArrayList temp = new ArrayList();
            // Instance of DB object splitter rules here with the following SQL statement 
            C_DentalClaimTracker.splitter_rules sr = new C_DentalClaimTracker.splitter_rules();

            DataTable allSplits = sr.Search("SELECT sc.PROCEDURE_CODE, " +
                                            "       sc.CARRIER, " +
                                            "       sc.SPLIT_RULE, " +
                                            "       sr.RULE_TYPE " +
                                            "FROM SPLITTER_CODES sc " +
                                            "     INNER JOIN SPLITTER_RULES sr ON (sc.SPLIT_RULE = sr.SPLIT_RULE) " +
                                            "ORDER BY sc.PROCEDURE_CODE ASC, " +
                                            "         sc.PRIORITY DESC");

            // Get the splitter list.

            DataTableReader reader = allSplits.CreateDataReader();
            
            while (reader.Read())
            {
                try
                {
                    temp.Add(new string[4] {
											   reader.GetString(reader.GetOrdinal("PROCEDURE_CODE")),
											   reader.GetString(reader.GetOrdinal("CARRIER")),
											   reader.GetInt32(reader.GetOrdinal("SPLIT_RULE")).ToString(),
											   reader.GetString(reader.GetOrdinal("RULE_TYPE"))
					});
                }
                catch (Exception ex)
                {
                    throw new Exception("There was an error reading a splitter rule from the database.", ex);
                }
            }
            reader.Close();


            // Catalog the splitters.
            Splitter s;
            foreach (string[] t in temp)
            {
                s = SplitterFactory.GetSplitter(int.Parse(t[2]), t[3], t[0], t[1]);
                if (!Splitters.ContainsKey(s.ProcedureCode))
                {
                    Splitters.Add(s.ProcedureCode, new ArrayList());
                }

                ((ArrayList) Splitters[s.ProcedureCode]).Add(s);
            }
        }


		// For a give procedure code, find the most applicable splitter for the claim.
		private Splitter GetApplicableSplitter(Claim c, string procedureCode) {
			if (!Splitters.ContainsKey(procedureCode)) { return null; }

			foreach (Splitter s in (ArrayList)Splitters[procedureCode]) {
				if (s.IsApplicable(c)) {
					return s;
				}
			}

			return null;
		}
	}
}
