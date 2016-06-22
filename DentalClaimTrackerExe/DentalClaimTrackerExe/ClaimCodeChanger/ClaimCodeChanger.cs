using System;
using System.IO;
using System.Text.RegularExpressions;
using NHDG.NHDGCommon;
using NHDG.NHDGCommon.Claims;
using NHDG.NHDGCommon.AppSettings;
using log4net;
using C_DentalClaimTracker;

namespace NHDG.ClaimCodeChanger {
	/// <summary>The main class of the program.</summary>
	public class ClaimCodeChanger {
        static ClaimCodeChangerSettings changerSettings;

        public static void Process(string inputFile)
        {
            Process(inputFile, string.Empty);
        }

        public static void Process(string inputFile, string outputFile) 
        {
			ClaimBatch batch = null;
            string ConfigurationFile = System.Windows.Forms.Application.StartupPath + "\\ClaimCodeChanger.config";

            if (!File.Exists(ConfigurationFile))
            {
                LoggingHelper.Log("Error in ClaimCodeChanger.Process", LogSeverity.Critical,
                    new Exception("Configuration file (" + ConfigurationFile + ") not found."), true);
            }

			// Set things up.
            changerSettings = (ClaimCodeChangerSettings)Utilities.DeserializeFromFile(typeof(ClaimCodeChangerSettings), ConfigurationFile);

			
			if (outputFile == "") 
            {
                outputFile = inputFile;
			}

			if (!File.Exists(inputFile)) {
                LoggingHelper.Log("Error in ClaimCodeChanger.Process", LogSeverity.Critical,
                    new Exception("Specified input file (" + inputFile + ") not found."), true);
			}


			// Read in the claims.
            try
            {

                batch = (ClaimBatch)Utilities.DeserializeFromFile(typeof(ClaimBatch), inputFile);
            }
            catch (Exception ex)
            {
                LoggingHelper.Log("Error in ClaimCodeChanger.Process", LogSeverity.Critical,
                    new Exception("An unexpected error occurred processing the input file: \n\n" + ex.Message, ex), true);
            }

			// Change procedure codes.
			int totalChanges = 0;
			foreach (Claim c in batch.Claims) {
				totalChanges += Change(c);
			}

			// Write out the modified claims.
            try
            {
                Utilities.SerializeToFile(batch, typeof(ClaimBatch), outputFile);
            }
            catch (Exception ex)
            {
                LoggingHelper.Log("Error in ClaimCodeChanger.Process", LogSeverity.Critical,
                    new Exception("Could not serialize the file.\n\n" + ex.Message, ex), true);
            }

			// All done.
		}


		// Perform the search & replaces.
		static private int Change(Claim c) {
			int numChanges = 0;
			Regex r;
			string temp = string.Empty;

            

			foreach (CodeChange change in changerSettings.CodeChanges) {
				r = new Regex(change.Carrier, RegexOptions.IgnoreCase);
				if (!r.IsMatch(c.GeneralInformation.Carrier.Name)) { continue; }

				r = new Regex(change.SearchFor);
				foreach (Treatment t in c.TreatmentInformation.Treatments) {
					if (!r.IsMatch(t.ProcedureCode)) { continue; }

					temp = r.Replace(t.ProcedureCode, change.ReplaceWith);
					System.Diagnostics.Debug.WriteLine("Replacing " + t.ProcedureCode + " with " + temp + " on claim " + c.Identity.ClaimID.ToString() + "/" + c.Identity.ClaimDB.ToString() + ".");
					t.ProcedureCode = temp;
					numChanges++;
				}
			}

			return numChanges;
		}
	}
}
