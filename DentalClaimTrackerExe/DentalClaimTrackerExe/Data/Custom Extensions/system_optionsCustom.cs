using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

namespace C_DentalClaimTracker
{
    static class system_options
    {
        internal static void SetImportFlag(bool value)
        {
            string sql = "UPDATE system_options SET is_importing = ";

            if (value)
                sql += "1";
            else
                sql += "0";

            user u = new user();

            u.ExecuteNonQuery(sql);

            //if (value)
            //    Properties.Settings.Default.OpenedExclusive = true;
        }

        internal static bool ImportFlag
        {
            get
            {
                bool toReturn;
                user u = new user();
                DataTable dt = u.Search("SELECT is_importing FROM system_options WHERE id = 1");

                toReturn = System.Convert.ToBoolean(dt.Rows[0][0]);

                return toReturn;
            }
        }

        internal static string ApexEDISaveFolder
        {
            get
            {
                string toReturn;
                user u = new user();

                DataTable dt = u.Search("SELECT apex_folder FROM system_options WHERE id = 1");

                toReturn = Convert.ToString(dt.Rows[0][0]);

                return toReturn;
            }
            set
            {
                user u = new user();

                u.ExecuteNonQuery("UPDATE system_options SET apex_folder = '" + value + "' WHERE id = 1");
            }
        }

        internal static string MercurySaveFolder
        {
            get
            {
                string toReturn;
                user u = new user();

                DataTable dt = u.Search("SELECT mercury_folder FROM system_options WHERE id = 1");

                toReturn = Convert.ToString(dt.Rows[0][0]);

                return toReturn;
            }
            set
            {
                user u = new user();

                u.ExecuteNonQuery("UPDATE system_options SET mercury_folder = '" + value + "' WHERE id = 1");
            }
        }

        internal static DateTime GetLastImportDate()
        {
            DateTime toReturn = new DateTime(1999, 1, 1);
            try
            {

                user u = new user();
                DataTable dt = u.Search("SELECT last_import_date FROM system_options WHERE id = 1");

                toReturn = System.Convert.ToDateTime(dt.Rows[0][0]);

            }
            catch { }

            return toReturn;

        }


        internal static void SetLastImportDate(DateTime lastDate)
        {
            try
            {
                user u = new user();
                u.ExecuteNonQuery("UPDATE system_options SET last_import_date = '" +
                    CommonFunctions.ToMySQLDateTime(lastDate) + "'");

                
            }
            catch (Exception err)
            {
                LoggingHelper.Log("Error in system_options.SetLastImportDate", LogSeverity.Error, err, true);
            }
        }


        internal static DateTime GetLastAppointmentAuditImportDate()
        {
            DateTime toReturn = new DateTime(1999, 1, 1);

            try
            {

                user u = new user();
                DataTable dt = u.Search("SELECT appointment_audit_last_update FROM system_options WHERE id = 1");

                toReturn = System.Convert.ToDateTime(dt.Rows[0][0]);

            }
            catch { }

            return toReturn;
        }



        internal static void SetLastAppointmentAuditImportDate(DateTime lastDate)
        {
            try
            {
                user u = new user();
                u.ExecuteNonQuery("UPDATE system_options SET appointment_audit_last_update = '" +
                    CommonFunctions.ToMySQLDateTime(lastDate) + "'");


            }
            catch (Exception err)
            {
                LoggingHelper.Log("Error in system_options.SetLastAppointmentAuditImportDate", LogSeverity.Error, err, true);
            }
        }

        internal static int ResendRevisit
        {
            get
            {
                return GetIntColumn("resend_revisit_date");
            }
            set
            {
                SetIntColumn("resend_revisit_date", value);
            }
        }

        private static void SetIntColumn(string colName, int value)
        {
            user u = new user();
            u.ExecuteNonQuery("UPDATE system_options SET " + colName + " = " + value);
            
        }

        private static void SetDateColumn(string colName, DateTime value)
        {
            user u = new user();
            u.ExecuteNonQuery("UPDATE system_options SET " + colName + " = '" + CommonFunctions.ToMySQLDate(value) + "'");
            
        }

        private static void SetStringColumn(string colName, string value)
        {
            user u = new user();
            u.ExecuteNonQuery(string.Format("UPDATE system_options SET {0} = '{1}'", colName, value));

        }

        

        private static int GetIntColumn(string colToGet)
        {
            int toReturn = -1;
            user u = new user();
            DataTable dt = u.Search("SELECT " + colToGet + " FROM system_options");


            try
            {
                toReturn = Convert.ToInt32(dt.Rows[0][0]);
            }
            catch
            {
                // let it slide and just return -1
            }
            return toReturn;
        }

        private static string GetStringColumn(string colToGet)
        {
            string toReturn = "";
            user u = new user();
            DataTable dt = u.Search("SELECT " + colToGet + " FROM system_options");


            try
            {
                toReturn = dt.Rows[0][0].ToString();
            }
            catch
            {
                // let it slide and return ""
            }
            return toReturn;
        }

        internal static int ResendStatus
        {
            get
            {
                return GetIntColumn("resend_status");
            }
            set
            {
                SetIntColumn("resend_status", value);
            }
        }

        internal static bool EclaimsSecondaryShowExtraInfo
        {
            get
            {
                bool toReturn = true;
                user u = new user();
                DataTable dt = u.Search("SELECT show_secondary_info_eclaims FROM system_options");

                try
                {
                    toReturn = Convert.ToBoolean(dt.Rows[0][0]); 
                }
                catch
                {
                    // let it slide and just return true
                }
                return toReturn;
            }
            set
            {
                user u = new user();
                u.ExecuteNonQuery("UPDATE system_options SET show_secondary_info_eclaims = " + Convert.ToInt32(value));
                
            }
        }

        internal static int MaxClaimsInBatch
        {
            get
            {
                return GetIntColumn("max_claims_in_batch");
            }
            set
            {
                SetIntColumn("max_claims_in_batch", value);
            }
        }

        public static bool LimitPredetermsOnSearch
        {
            get
            {
                return Convert.ToBoolean(GetIntColumn("limit_predeterm_date"));
            }
            set
            {
                SetIntColumn("limit_predeterm_date", Convert.ToInt32(value));
            }
        }

        public static DateTime PredetermSearchDateMinimum
        {
            get
            {
                DateTime toReturn = new DateTime(1999, 1, 1);
                try
                {

                    user u = new user();
                    DataTable dt = u.Search("SELECT predeterm_minimum_date FROM system_options WHERE id = 1");

                    toReturn = System.Convert.ToDateTime(dt.Rows[0][0]);

                }
                catch { }

                return toReturn;
            }
            set
            {
                SetDateColumn("predeterm_minimum_date", value);
            }
        }

        public static int ApexSendStatus 
        {
            get
            {
                return GetIntColumn("apex_send_status");
            }
            set
            {
                SetIntColumn("apex_send_status", value);
            }
        
        }
        
        public static int ApexResendStatus
        {
            get
            {
                return GetIntColumn("apex_resend_status");
            }
            set
            {
                SetIntColumn("apex_resend_status", value);
            }

        }

        internal static DateTime LastEligibilityDate
        {
            get
            {
                DateTime toReturn = new DateTime(1999, 1, 1);
                try
                {

                    user u = new user();
                    DataTable dt = u.Search("SELECT eligibility_last_update FROM system_options WHERE id = 1");

                    toReturn = System.Convert.ToDateTime(dt.Rows[0][0]);
                }
                catch { }

                return toReturn;
            }
            set
            {
                SetDateColumn("eligibility_last_update", value);
            }
        }

        public static string GetQuickNote(int index, bool fullText)
        {
            if (fullText)
                return GetStringColumn("quicktext" + index + "Long");
            else
                return GetStringColumn("quicktext" + index + "Short");
        }

        public static void SaveQuickNote(int index, string shortText, string longText)
        {
            SetStringColumn("quickText" + index + "Long", longText);
            SetStringColumn("quickText" + index + "Short", shortText);
        }

        public static bool ApexRevisitDateEnabled 
        {
            get
            {
                return Convert.ToBoolean(GetIntColumn("revisit_date_after_send_enabled"));
            }
            set
            {
                SetIntColumn("revisit_date_after_send_enabled", Convert.ToInt32(value));
            }
        }

        public static int ApexRevisitDate
        {
            get
            {
                return GetIntColumn("revisit_date_after_send");
            }
            set
            {
                SetIntColumn("revisit_date_after_send", value);
            }
        }

        internal static string OverrideStateProviderID
        {
            get
            {
                return GetStringColumn("override_state_provider_id");
            }
            set
            {
                SetStringColumn("override_state_provider_id", value);
            }
        }

        internal static string OverrideStateNewProviderID
        {
            get
            {
                return GetStringColumn("override_state_new_provider_id");
            }
            set
            {
                SetStringColumn("override_state_new_provider_id", value);
            }
        }

        public static int OverrideStateInsurance
        {
            get
            {
                return GetIntColumn("override_state_insurance");
            }
            set
            {
                SetIntColumn("override_state_insurance", value);
            }
        }

        internal static string OverrideStateState
        {
            get
            {
                return GetStringColumn("override_state_state");
            }
            set
            {
                SetStringColumn("override_state_state", value);
            }
        }

        public static bool OverrideStateEnabled
        {
            get
            {
                return Convert.ToBoolean(GetIntColumn("override_state_enabled"));
            }
            set
            {
                SetIntColumn("override_state_enabled", Convert.ToInt32(value));
            }
        }
        public static bool importerror
        {
            get
            {
                return Convert.ToBoolean(GetIntColumn("import_error"));
            }
            set
            {
                SetIntColumn("import_error", Convert.ToInt32(value));
            }
        }


    }
}
