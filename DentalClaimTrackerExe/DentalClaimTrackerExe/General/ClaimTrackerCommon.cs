using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Data.SqlClient;

namespace C_DentalClaimTracker
{
    class ClaimTrackerCommon
    {
        /// <summary>
        /// Prints a claim, attempts to connect to Dentrix DB in order to do this.
        /// </summary>
        /// <param name="toPrint"></param>
        public static void PrintClaim(claim toPrint)
        {
            List<claim> toPass = new List<claim>();
            toPass.Add(toPrint);
            PrintClaims(toPass);
        }

        /// <summary>
        /// Prints a set of claims, attempts to connect to Dentrix DB in order to do this.
        /// </summary>
        /// <param name="toPrint"></param>
        public static void PrintClaims(List<claim> toPrint)
        {
            // Attempt to create a connection object to look at the production db
            try
            {
                SqlConnection dbConnection = new SqlConnection(new data_mapping_schema(3).GetConnectionString(false));
                dbConnection.Open();
                //PrintClaims(toPrint, dbConnection);
                dbConnection.Close();
            }
            catch
            {
                MessageBox.Show("Could not open connection to Dentrix database.", "Could not connect");
            }
        }

        /// <summary>
        /// Prints a set of claims using a given database connection
        /// </summary>
        /// <param name="toPrint"></param>
        /// <param name="dbConnection"></param>
        public static void PrintClaims(List<claim> toPrint, SqlConnection dbConnection)
        {
            NHDG.NHDGCommon.Claims.ClaimBatch cb = new NHDG.NHDGCommon.Claims.ClaimBatch();
            System.Data.SqlClient.SqlTransaction trans;
            string _errorClaims = string.Empty;
            try
            {
                trans = dbConnection.BeginTransaction();
            }
            catch
            {
                MessageBox.Show("Could not connect to Dentrix to print claim.");
                return;
            }


            try
            {
                string _xmlPath = GetFileName(".xml", Application.StartupPath + "\\Processed XML\\Temp\\");
                string _pdfPath = GetFileName(".pdf", Application.StartupPath + "\\Claims\\");

                foreach (claim c in toPrint)
                {
                    NHDG.NHDGCommon.Claims.Claim aClaim;
                    try
                    {

                        //if (C_DentalClaimTracker.Properties.Settings.Default.ShowPaymentLineOnSecondary)//commentline
                        bool a = true;
                        if(a)
                            aClaim = new NHDG.NHDGCommon.Claims.Claim(c, trans, false, true);
                        else
                            aClaim = new NHDG.NHDGCommon.Claims.Claim(c, trans, true, true);

                        CheckForEligibilityRestrictions(c, aClaim);

                        // Special code for teeth
                        aClaim.PutToothStringInTooth();
                        aClaim.CleanAddresses();
                        cb.Claims.Add(aClaim);
                    }
                    catch (Exception err)
                    {
                        string errDisplay = err.Message;
                        Exception e = err.InnerException;
                        while (e != null)
                        {
                            errDisplay += "\n)" + e.Message;
                            e = e.InnerException;
                        }

                        _errorClaims = "\n" + c.created_on.Value.ToShortDateString() + ", " +
                             c.PatientLastNameCommaFirst + "; " + c.LinkedCompany.name + "\nSystem error:" + errDisplay;
                    }
                }

                if (cb.Claims.Count > 0)
                {
                    NHDG.NHDGCommon.Utilities.SerializeToFile(cb, typeof(NHDG.NHDGCommon.Claims.ClaimBatch), _xmlPath);

                    try
                    {
                        NHDG.ProphySplitter.ProphySplitter ps = new NHDG.ProphySplitter.ProphySplitter();
                        ps.PerformSplit(_xmlPath);
                    }
                    catch (Exception err)
                    {
                        MessageBox.Show("An error occurred splitting the procedures (for example, splitting D1110 into " +
                            "multiple procedures. Please report the following error to a system administrator: \n\n" + err.Message, "Error Splitting");
                    }

                    try
                    {
                        ProcedureCombiner pc = new ProcedureCombiner();
                        pc.Combine(_xmlPath);
                    }
                    catch (Exception err)
                    {
                        MessageBox.Show("An error occurred combining the procedures." +
                            " Please report the following error to a system administrator: \n\n" + err.Message, "Error Combining");
                    }

                    string workingDir = Application.StartupPath + "\\ada-form\\";
                    string fileToStart = "ruby";

                    string arguments = "ada_report.rb \"" +
                        _xmlPath + "\" \"" + _pdfPath + "\"";

                    Clipboard.SetText(fileToStart + " " + arguments);
                    System.Diagnostics.ProcessStartInfo pi = new System.Diagnostics.ProcessStartInfo(fileToStart, arguments);
                    pi.WorkingDirectory = workingDir;
                    pi.UseShellExecute = true;

                    System.Diagnostics.Process p = System.Diagnostics.Process.Start(pi);

                    p.WaitForExit(45000);

                    if (File.Exists(_pdfPath))
                        System.Diagnostics.Process.Start(_pdfPath);
                    else
                    {
                        while (MessageBox.Show("The .pdf document appears to be taking a while to generate. Would you like to continue waiting?\n\nThis question appears every 45 seconds.",
                            "PDF not generated", MessageBoxButtons.YesNo) == DialogResult.Yes)
                        {
                            p.WaitForExit(45000);
                            if (File.Exists(_pdfPath))
                            {
                                System.Diagnostics.Process.Start(_pdfPath);
                                break;
                            }
                        }
                    }
                }

            }
            catch (Exception err)
            {
                string innerText = "";

                innerText += err.Message;

                Exception e = err;

                while (e.InnerException != null)
                {
                    innerText += "\n\n" + e.InnerException.Message;
                    e = e.InnerException;

                    if (innerText.Length > 5000) // Just in case it's getting too long, break out
                        break;
                }

                LoggingHelper.Log("Error printing claim in ClaimTrackerCommon.PrintClaims\n" + innerText, LogSeverity.Error);
                MessageBox.Show("An unexpected error occurred trying to print the claim.\n\n" + innerText, "Unexpected Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            if (_errorClaims != string.Empty)
            {
                LoggingHelper.Log("Errors occurred printing claim in ClaimTrackerCommon.PrintClaims\n" + _errorClaims, LogSeverity.Error);
                MessageBox.Show("Some of the claims could not be printed.\n\n" + _errorClaims);
            }

        }

        public static bool ClaimExists(claim toCheck)
        {
            bool exists;
            // Attempt to create a connection object to look at the production db
            try
            {
                SqlConnection dbConnection = new SqlConnection(new data_mapping_schema(3).GetConnectionString(false));
                dbConnection.Open();

                System.Data.SqlClient.SqlTransaction trans;
                string _errorClaims = string.Empty;
                try
                {
                    trans = dbConnection.BeginTransaction();
                }
                catch
                {
                    throw new Exception("Could not connect to Dentrix to print claim.");
                }

                NHDG.NHDGCommon.Claims.Claim aClaim;
                try
                {
                    aClaim = new NHDG.NHDGCommon.Claims.Claim(toCheck, trans, false, true);
                    exists = true;
                }
                catch (Exception err)
                {
                    // Could not find claim
                    exists = false;
                    LoggingHelper.Log(string.Format("Claim with id {0} and db {1} does not exist in Dentrix.", toCheck.claimidnum, toCheck.claimdb), LogSeverity.Information, err);
                }

                dbConnection.Close();
            }
            catch
            {
                throw new Exception("Could not open connection to Dentrix database.");
            }

            return exists;
        }

        /// <summary>
        /// Looks at a local claim and if there are eligibility restrictions switches its data 
        /// as defined in the provider_eligibility table
        /// </summary>
        /// <param name="c"></param>
        /// <param name="aClaim"></param>
        public static void CheckForEligibilityRestrictions(claim c, NHDG.NHDGCommon.Claims.Claim aClaim)
        {

            if (!c.CheckForAddressOverride)
            {
                claim switchToClaim = provider_eligibility_restrictions.FindEligibilityRestrictions(c);

                if (switchToClaim != null)
                {
                    // Have to switch provider info
                    aClaim.BillingDentist.Name = switchToClaim.DoctorName;
                    aClaim.BillingDentist.Address.Street1 = switchToClaim.doctor_address;
                    aClaim.BillingDentist.Address.Street2 = switchToClaim.doctor_address2;
                    aClaim.BillingDentist.Address.City = switchToClaim.doctor_city;

                    aClaim.BillingDentist.Address.State = switchToClaim.doctor_state;
                    aClaim.BillingDentist.Address.Zip = switchToClaim.doctor_zip;

                    if (!switchToClaim.DoctorName.Contains("O'Brien")) // HACK: Remove this line once Ruby guys fix their stuff
                        aClaim.BillingDentist.TIN = switchToClaim.doctor_tax_number;
                    aClaim.BillingDentist.PhoneNumber = switchToClaim.doctor_phone_number_object.FormattedPhone;
                    aClaim.BillingDentist.License = switchToClaim.doctor_license_number;
                    aClaim.BillingDentist.ID = switchToClaim.doctor_dentrix_id;
                }
            }
        }



        /// <summary>
        /// Creates a file name for use. The file name will be saved in the specified folder. The file name will be the date and time.
        /// Extension includes leading period.
        /// </summary>
        /// <param name="extension"></param>
        /// <param name="saveRoot"></param>
        /// <returns></returns>
        public static string GetFileName(string extension, string saveRoot)
        {
            System.IO.Directory.CreateDirectory(saveRoot);

            string _savePath = saveRoot + System.DateTime.Now.ToShortDateString().Replace("/", "-") +
                ", " + System.DateTime.Now.ToLongTimeString().Replace(":", " ") + extension;

            while (System.IO.File.Exists(_savePath))
            {
                System.Threading.Thread.Sleep(300);
                _savePath = saveRoot + System.DateTime.Now.ToShortDateString().Replace("/", "-") +
                ", " + System.DateTime.Now.ToLongTimeString().Replace(":", " ") + extension;
            }
            return _savePath;
        }


        /// <summary>
        /// Converts a list of companies to a comma separated list usable in the IN section of a SQL statement
        /// </summary>
        /// <param name="companiesInAGroup"></param>
        /// <returns></returns>
        public static string CompaniesToInString(List<company> companiesInAGroup)
        {
            string toReturn = "";

            foreach (company aCompany in companiesInAGroup)
            {
                if (toReturn != "")
                    toReturn += ",";

                toReturn += aCompany.id;
            }

            return toReturn;
        }
    }


}
