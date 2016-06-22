using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

namespace C_DentalClaimTracker
{
    partial class claim
    {
        public enum ClaimTypes
        {
            Primary = 0,
            Secondary = 1,
            Predeterm = 2,
            SecondaryPredeterm = 3
        }

        /// <summary>
        /// Returns null if status has not been set yet
        /// </summary>
        public claim_status LinkedStatus
        {
            get
            {
                int id = CommonFunctions.DBNullToZero(this["status_id"]);
                if (id > 0)
                    return new claim_status(id);
                else
                    return null;
            }
        }

        public bool HasSecondary
        {
            get
            {
                return Search("SELECT * FROM claims WHERE primary_claimid = " + claimidnum +
                    " AND primary_claimdb = " + claimdb).Rows.Count > 0;
            }
        }

        public claim LinkedSecondaryClaim
        {
            get
            {
                DataTable matches = Search("SELECT * FROM claims WHERE primary_claimid  = " + claimidnum +
                    " AND primary_claimdb = " + claimdb);

                if (matches.Rows.Count > 0)
                {
                    claim toReturn = new claim();
                    toReturn.Load(matches.Rows[0]);

                    return toReturn;
                }
                else
                    return null;
            }
        }

        public bool HasPrimary
        {
            get
            {
                return Search("SELECT * FROM claims WHERE claimidnum = " + primary_claimid +
                    " AND claimdb = " + primary_claimdb).Rows.Count > 0;
            }
        }

        public claim LinkedPrimaryClaim
        {
            get
            {
                DataTable matches = Search("SELECT * FROM claims WHERE claimidnum = " + primary_claimid +
                    " AND claimdb = " + primary_claimdb);

                if (matches.Rows.Count > 0)
                {
                    claim toReturn = new claim();
                    toReturn.Load(matches.Rows[0]);

                    return toReturn;
                }
                else
                    return null;
            }
        }

        /// <summary>
        /// A custom rule that checks to see if there is a provider_eligibility restriction that assigns a custom provider
        /// </summary>
        public bool CheckForAddressOverride
        {
            get
            {
                return override_address_provider != "";
            }
        }

        public List<claim_change_log> LinkedChanges
        {
            get
            {
                List<claim_change_log> toReturn = new List<claim_change_log>();
                DataTable matches = Search("SELECT * FROM claim_change_log WHERE claim_id = " + id + " order by change_date");
                claim_change_log workingLog;


                foreach (DataRow aMatch in matches.Rows)
                {
                    workingLog = new claim_change_log();
                    workingLog.Load(aMatch);

                    toReturn.Add(workingLog);
                }

                return toReturn;
            }
        }

        public List<procedure> LinkedProcedures
        {
            get
            {
                List<procedure> toReturn = new List<procedure>();
                DataTable matches;
                if (claim_type == ClaimTypes.Secondary)
                {
                    // Grab the id of the primary claim
                    if (HasPrimary)
                        return LinkedPrimaryClaim.LinkedProcedures;
                    else
                        return toReturn;

                }
                else
                {
                    matches = Search("SELECT * FROM procedures WHERE claim_id = " + id + " order by id");

                    procedure workingProc;

                    foreach (DataRow aMatch in matches.Rows)
                    {
                        workingProc = new procedure();
                        workingProc.Load(aMatch);

                        toReturn.Add(workingProc);
                    }

                    return toReturn;
                }
            }
        }

        /// <summary>
        /// Returns the patient name formatted in a LastName, FirstName format
        /// </summary>
        public string PatientLastNameCommaFirst
        {
            get { return patient_last_name + ", " + patient_first_name; }
        }


        /// <summary>
        /// Returns a formatted version of the subscriber name (First + MI + Last)
        /// </summary>
        public string PatientName
        {
            get
            {
                return CommonFunctions.FormatName(patient_first_name, patient_middle_initial, patient_last_name);
            }
            set
            {
                CommonFunctions.FormattedName formattedName = CommonFunctions.GetFormattedName(value);

                patient_first_name = formattedName.FirstName;
                patient_middle_initial = formattedName.MiddleInitial;
                patient_last_name = formattedName.LastName;
            }
        }

        /// <summary>
        /// Returns a formatted version of the subscriber name (First + MI + Last)
        /// </summary>
        public string DoctorName
        {
            get
            {
                return CommonFunctions.FormatName(doctor_first_name, doctor_middle_initial, doctor_last_name);
            }
            set
            {
                CommonFunctions.FormattedName formattedName = CommonFunctions.GetFormattedName(value);

                doctor_first_name = formattedName.FirstName;
                doctor_middle_initial = formattedName.MiddleInitial;
                doctor_last_name = formattedName.LastName;
            }
        }

        /// <summary>
        /// Returns a formatted version of the subscriber name (First + MI + Last)
        /// </summary>
        public string SubscriberName
        {
            get
            {
                return CommonFunctions.FormatName(subscriber_first_name, subscriber_middle_initial, subscriber_last_name);
            }
            set
            {
                CommonFunctions.FormattedName formattedName = CommonFunctions.GetFormattedName(value);

                subscriber_first_name = formattedName.FirstName;
                subscriber_middle_initial = formattedName.MiddleInitial;
                subscriber_last_name = formattedName.LastName;
            }

        }


        /// <summary>
        /// Returns a list of the base questions for a given claim. Does not return sub-questions.
        /// </summary>
        public List<question> Questions
        {
            get
            {
                if (_questions.Count == 0)
                {
                    // Load Questions                    
                    question qs = new question();
                    DataTable qdt = qs.Search(); // Get all questions
                    List<question> rawList = new List<question>();
                    foreach (DataRow row in qdt.Rows)
                    {
                        question q = new question();
                        q.Load(row);
                        rawList.Add(q);
                    }

                    foreach (question q in rawList)
                    {
                        // Add all root questions
                        if (q.parent < 1)
                        {
                            AddQuestion(q, rawList, null);
                        }
                    }
                    _questions.Sort();
                }
                return _questions;
            }
        }

        private void AddQuestion(question question, List<question> subquestions, question parentQuestion)
        {
            if (parentQuestion == null)
            {
                _questions.Add(question);
            }
            else
            {
                parentQuestion.SubQuestions.Add(question);
            }

            foreach (question q in subquestions)
            {
                if (q.parent == question.id)
                {
                    AddQuestion(q, subquestions, question);
                }
            }

            question.SubQuestions.Sort();

            if (question.type == question.QuestionTypes.MultipleChoice)
            {
                question.GetMultipleChoiceAnswers();
            }
        }

        public List<call> GetPastCalls(bool requireAnswer)
        {
            List<call> toReturn = new List<call>();
            string sql = "SELECT * FROM calls c WHERE claim_id = " + id;


            if (requireAnswer)
                sql += " AND (Select count(*) FROM choices where call_id = c.id) > 1";


            sql += " ORDER BY created_on desc";

            DataTable pastCalls = Search(sql);

            foreach (DataRow aCall in pastCalls.Rows)
            {
                call c = new call();
                c.Load(aCall);
                c.LinkedClaim = this;
                c.ReadOnly = true;
                toReturn.Add(c);
            }

            return toReturn;
        }

        public bool HasLinkedCompany
        {
            get
            {
                return (this["company_id"] is int);
            }
        }

        /// <summary>
        /// Returns the associated company_contact_info for this claim.
        /// ** Slightly different from the other linked fields - retrieves data every time.
        /// </summary>
        public company_contact_info LinkedCompanyAddress
        {
            get
            {
                try
                {
                    DataTable findContactInfo = Search("SELECT * FROM company_contact_info WHERE order_id = " + company_address_id +
                    " AND company_id = " + company_id);

                    if (findContactInfo.Rows.Count == 1)
                    {
                        company_contact_info toReturn = new company_contact_info();
                        toReturn.Load(findContactInfo.Rows[0]);

                        return toReturn;
                    }
                    else
                    {
                        LoggingHelper.Log("claim.LinkedCompanyAddress returned unexpected results. CompanyID = " + company_id + " AddressID = " + company_address_id, LogSeverity.Error);
                        throw new Exception();
                    }

                }
                catch (Exception err)
                {
                    LoggingHelper.Log("Error in claim.LinkedCompanyAddress", LogSeverity.Error, err, false);
                    throw new Exception("Relationship is not initialized (company - company_contact_info)", err);
                }
            }
        }
        // doctor_fax_number, doctor_phone_number
        public PhoneObject doctor_fax_number_object
        {
            get
            {
                if (this["doctor_fax_number"] is string)
                {
                    PhoneObject toReturn = new PhoneObject(this["doctor_fax_number"].ToString());
                    return toReturn;
                }
                else
                    return new PhoneObject();
            }
            set
            {
                this["doctor_fax_number"] = value.UnformattedPhone;
            }
        }

        public PhoneObject doctor_phone_number_object
        {
            get
            {
                if (this["doctor_phone_number"] is string)
                {
                    PhoneObject toReturn = new PhoneObject(this["doctor_phone_number"].ToString());
                    return toReturn;
                }
                else
                    return new PhoneObject();
            }
            set
            {
                this["doctor_phone_number"] = value.UnformattedPhone;
            }
        }

        /// <summary>
        /// CUSTOM: Get/Set for the formatted string patient_ssn. Returns "" if the data is null.
        /// </summary>
        public string patient_ssn
        {
            get
            {
                if (this["patient_ssn"] is string)
                {
                    return CommonFunctions.FormatSSN((string)this["patient_ssn"]);
                }
                else
                    return "";
            }
            set
            {
                this["patient_ssn"] = value.Replace("-", "");
            }
        }

        /// <summary>
        /// CUSTOM: Get/Set for the formatted string subscriber_ssn. Returns "" if the data is null.
        /// </summary>
        public string subscriber_ssn
        {
            get
            {
                if (this["subscriber_ssn"] is string)
                    return CommonFunctions.FormatSSN((string)this["subscriber_ssn"]);
                else
                    return "";
            }
            set
            {
                this["subscriber_ssn"] = value.Replace("-", "");
            }
        }

        internal void MarkAllImportsUpdated(bool updated)
        {
            ExecuteNonQuery("UPDATE claims SET import_update_flag = " + Convert.ToInt32(updated));
        }

        /// <summary>
        /// Closes all claims whose import update flag is set to false 
        /// </summary>
        internal int CloseClaimsWithoutUpdate(bool CheckForMerge = true)
        {
            int claimCount = CommonFunctions.DBNullToZero(Search("SELECT COUNT(*) FROM claims WHERE import_update_flag = 0").Rows[0][0]);
            ExecuteNonQuery("UPDATE claims SET [open] = 0 WHERE import_update_flag = 0");
            return claimCount;
        }

        internal void ClearClaimProcedures()
        {
            if (this["id"] != DBNull.Value)
            {
                ExecuteNonQuery("DELETE FROM procedures WHERE claim_id = " + id);
            }
        }

        /// <summary>
        /// CUSTOM: Get/Set for the decimal amount_of_claim. Returns decimal value (ie 1.25) although database stores as 125 (cents). Converts back to cents
        /// on set.
        /// </summary>
        public decimal amount_of_claim
        {
            get
            {
                if (this["amount_of_claim"] is decimal)
                    return ((decimal)this["amount_of_claim"] / 100);
                else
                    throw new DataObjectException("Property value is empty or invalid. (decimal - amount_of_claim)");
            }
            set
            {
                this["amount_of_claim"] = value * 100;
            }
        }

        internal List<claim_batch> LinkedBatches()
        {
            List<claim_batch> toReturn = new List<claim_batch>();
            claim_batch workingBatch;

            DataTable results = Search("SELECT id FROM claim_batch cb INNER JOIN batch_claim_list bcl ON cb.id = bcl.batch_ID " +
                "WHERE bcl.claim_id = " + id + " AND bcl.still_in_batch = 1 ORDER BY batch_date desc");

            
            foreach (DataRow aRow in results.Rows)
            {
                workingBatch = new claim_batch();
                workingBatch.Load((int) aRow["id"]);

                toReturn.Add(workingBatch);
            }

            return toReturn;
        }


        /// <summary>
        /// Get/Set for the string handling. UNIQUE: Returns "" if the data is null or unclassified.
        /// </summary>
        public string handlingEmptyString
        {
            get
            {
                if (this["handling"] is string)
                {
                    string toReturn = (string)this["handling"];

                    if (toReturn == "Unclassified")
                        return "";
                    else
                        return toReturn;
                }

                else
                    return "";
            }
            set
            {
                this["handling"] = value;
            }
        }

        /// <summary>
        /// Get/Set for the int ClaimType. CUSTOM: Uses enum. Returns Primary on invalid data. Never returns or 
        /// stores secondary predeterm, use claim_type_full for this (secondary predeterm is stored as predeterm).
        /// </summary>
        public ClaimTypes claim_type
        {
            get
            {
                if (this["claim_type"] is int)
                    return (ClaimTypes)this["claim_type"];
                else
                    return ClaimTypes.Primary;
            }
            set
            {
                this["claim_type"] = (int)value;
            }
        }

        /// <summary>
        /// Looks to see if any user other than the ActiveUser has a claim with the given ID open
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        internal List<user> UsersViewingClaim(bool excludeCurrentUser)
        {
            List<user> toReturn = new List<user>();
            string searchSQL = "SELECT * FROM users WHERE viewing_claim_id = " + id;

            if (excludeCurrentUser)
                searchSQL += " AND id != " + ActiveUser.UserObject.id;

            user u;
            DataTable dt = Search(searchSQL);
            {
                foreach (DataRow aUser in dt.Rows)
                {
                    u = new user();
                    u.Load(aUser);
                    toReturn.Add(u);
                }
            }

            return toReturn;
        }


        /// <summary>
        /// Locks or unlocks the claim for the current user
        /// </summary>
        /// <param name="toLock"></param>
        internal void LockClaim(bool toLock)
        {
            string idString;

            if (toLock)
                idString = id.ToString();
            else
                idString = "0";

            ExecuteNonQuery("UPDATE users SET viewing_claim_id = " + idString + " WHERE id = " +
                ActiveUser.UserObject.id);
        }

        internal void ReleaseOwner()
        {
            this["owner_id"] = 0;
            Save();
        }

        internal bool IsOwned
        {
            get
            {
                if (this["owner_id"] is int)
                    if ((int)this["owner_id"] > 0)
                        return true;
                    else
                        return false;
                else
                    return false;
            }
        }

        internal user Owner
        {
            get
            {
                if (this["owner_id"] is int)
                {
                    if ((int)this["owner_id"] > 0)
                    {
                        try
                        {
                            user u = new user((int)this["owner_id"]);
                            return u;
                        }
                        catch
                        {
                            return null;
                        }
                    }
                    else
                        return null;
                }
                else
                    return null;
            }
            set
            {
                this["owner_id"] = value.id;
                Save();
            }
        }

        internal List<claim_status_history> GetClaimHistory(bool showAsIS)
        {
            List<claim_status_history> toReturn = new List<claim_status_history>();

            string SQL = "SELECT * FROM claim_status_history WHERE " +
                "claim_id = " + id;

            if (!showAsIS)
            {
                SQL += " AND as_is_flag <> 1";
            }

            SQL += " ORDER BY order_id DESC";

            DataTable matches = Search(SQL);

            claim_status_history aCsh;

            foreach (DataRow aRow in matches.Rows)
            {
                aCsh = new claim_status_history();
                aCsh.Load(aRow);

                toReturn.Add(aCsh);
            }

            return toReturn;
        }

        internal List<notes> GetNotes()
        {
            // Note - at one point, notes were linked to calls and not claims
            // There might be some old architecture to reflect this
            // Aaron J December 2010
            List<notes> toReturn = new List<notes>();
            List<call> claimCalls = GetPastCalls(false);

            foreach (call call in claimCalls)
            {
                List<notes> callNotes = call.GetNotes();
                foreach (notes note in callNotes)
                {
                    if (note.claim_id == 0)
                        toReturn.Add(note);
                }
            }

            DataTable linkedNotes = Search("SELECT * FROM notes WHERE claim_id = " + id
                + " ORDER BY created_on asc");
            foreach (DataRow aNote in linkedNotes.Rows)
            {
                notes n = new notes();
                n.Load(aNote);
                toReturn.Insert(0, n);
            }

            return toReturn;
        }


        public static string BuildWherePatientName(string data, int SearchIndex)
        {
            const int STARTSWITHSEARCHTYPEINDEX = 2;
            const int EXACTSEARCHTYPEINDEX = 3;

            if (data == "")
                return "";
            else
            {
                string toReturn = " AND (";
                data = data.Replace(",", "");
                data = data.Replace("%", "");
                data = data.Trim();
                CommonFunctions.FormattedName formattedName = CommonFunctions.GetFormattedName(data);



                if (formattedName.LastName != "")
                {
                    toReturn += "((patient_first_name LIKE '%" + formattedName.FirstName.Replace("'", "''") + "%'";
                    toReturn += " AND patient_last_name LIKE '%" + formattedName.LastName.Replace("'", "''") + "%')";
                    toReturn += " OR (patient_first_name LIKE '%" + formattedName.LastName.Replace("'", "''") + "%'";
                    toReturn += " AND patient_last_name LIKE '%" + formattedName.FirstName.Replace("'", "''") + "%'))";
                    toReturn += " OR patient_last_name LIKE '%" + data.Replace("'", "''") + "%'";
                }
                else
                {
                    toReturn += "patient_first_name LIKE '%" + formattedName.FirstName.Replace("'", "''") + "%'";
                    toReturn += " OR patient_first_name LIKE '%" + data.Replace("'", "''") + "%'";
                    toReturn += " OR patient_last_name LIKE '%" + data.Replace("'", "''") + "%'";
                }

                if (SearchIndex == STARTSWITHSEARCHTYPEINDEX) // Starts With
                {
                    // Remove the last % sign
                    toReturn = toReturn.Replace("'%", "'");
                }
                else if (SearchIndex == EXACTSEARCHTYPEINDEX)
                {
                    // Remove all % signs
                    toReturn = toReturn.Replace("%", "");
                }

                toReturn += ")";

                return toReturn;
            }
        }

        /// <summary>
        /// Looks to see if any changes should be made to the claim status based on system settings when it is resent
        /// </summary>
        internal bool SetResendStatus()
        {
            bool changesMade = false;
            int revisitDate = system_options.ResendRevisit;
            int resendStatus = system_options.ResendStatus;
            

            if ((revisitDate >= 0) || (resendStatus >= 0))
            {
                // Something is changing
                changesMade = true;
                DateTime? newRevisit = revisit_date;
                int newStatus = status_id;

                
                if (revisitDate >= 0)
                    newRevisit = DateTime.Now.AddDays(system_options.ResendRevisit);

                if (resendStatus >= 0)
                    newStatus = system_options.ResendStatus;


                SetStatusAndRevisitDate(new claim_status(newStatus), newRevisit);
                
            }

            return changesMade;

            

            

        }

        /// <summary>
        /// Creates a status history for a claim. Note that if you pass 'null' for newRevisit, it will
        /// use the old value for the revisit date.
        /// </summary>
        /// <param name="newStatus"></param>
        /// <param name="newRevisit"></param>
        /// <param name="as_is_flag"></param>
        internal void CreateStatusHistory(claim_status newStatus, DateTime? newRevisit, bool as_is_flag)
        {
            string noteText = string.Empty;
            claim_status_history csh = new claim_status_history();

            csh.claim_id = id;
            csh.order_id = csh.GetNextOrderID();
            csh.user_id = ActiveUser.UserObject.id;
            csh.date_of_change = DateTime.Now;

            csh.old_status_id = status_id;

            if (newStatus == null)
                csh.new_status_id = status_id;
            else
                csh.new_status_id = newStatus.id;

            csh.old_revisit_date = revisit_date;
            if (newRevisit != null)                
                csh.new_revisit_date = newRevisit;
            
            csh.as_is_flag = as_is_flag;
            csh.Save();
        }

        internal void ResetStatus()
        {
            
            claim_status_history csh = new claim_status_history();

            csh.claim_id = id;
            csh.order_id = csh.GetNextOrderID();
            csh.user_id = ActiveUser.UserObject.id;
            csh.date_of_change = DateTime.Now;
            csh.old_status_id = status_id;
            csh.new_status_id = -1;
            csh.as_is_flag = false;
            csh.Save();

            status_id = -1;
            Save();

        }

        /// <summary>
        /// Sets a new status and revisit date for the claim, and creates a status change history if the 
        /// status changes. Pass a null object for either value to be ignored, pass an empty newDate to
        /// set the revisit date to "not set". If the revisit date and the status are unchanged, then nothing
        /// happens. 
        /// </summary>
        /// <param name="p"></param>
        /// <param name="p_2"></param>
        internal void SetStatusAndRevisitDate(claim_status newStatus, DateTime? newDate)
        {
            string noteText = "";
            claim_status cs = LinkedStatus;
            bool changesMade = false;
            DateTime? actualDateValue = newDate;


            if (newDate != null)
            {
                if (revisit_date != newDate)
                {
                    if (revisit_date.HasValue)
                        noteText = "Revisit from '" + revisit_date.Value.ToShortDateString() + "' to ";
                    else
                        noteText = "Revisit from {empty} to '";

                    if (newDate.HasValue)
                        noteText += "'" + newDate.Value.ToShortDateString() + "'";
                    else
                        noteText += "{empty}";

                    changesMade = true;
                }
            }
            else
            {
                actualDateValue = revisit_date;
                // This extra complication is because if they pass null, they're not setting it to null. We want to keep the
                // old value and when logging the change history we'll need to mark it as such
            }

            if (newStatus != null)
            {
                if (newStatus.id != status_id)
                {
                    if (cs == null)
                        noteText += " Status from {empty} to '";
                    else
                        noteText += " Status from '" + cs.name + "' to '";

                    noteText += newStatus.name + "'";
                    changesMade = true;
                }
            }

            if (changesMade)
            {
                revisit_date = actualDateValue;
                status_last_date = DateTime.Now;
                status_last_user_id = ActiveUser.UserObject.id;
                CreateStatusHistory(newStatus, actualDateValue, false);
                if (newStatus != null)
                    status_id = newStatus.id;

                Save();
                
                ActiveUser.LogAction(ActiveUser.ActionTypes.ChangeStatus, id, noteText);
            }
        }

        internal void CreateSentHistory(clsClaimEnums.SentMethods sentMethod, bool isResend)
        {
            claim_sent_history csh = new claim_sent_history();
            csh.claim_id = id;
            csh.sent_date = DateTime.Now;
            csh.send_type = sentMethod;
            csh.is_resend = isResend;
            csh.Save();
        }

        internal bool InCurrentApexResendBatch()
        {
            claim_batch cb = claim_batch.GetApexResendBatchForToday();

            DataTable dt = Search("SELECT * FROM batch_claim_list WHERE batch_id = " + cb.id +
                " AND claim_id = " + id);

            return dt.Rows.Count > 0;
        }

        internal apex_rules_rulelist GetApplicableRule()
        {
            apex_rules_rulelist toReturn = new apex_rules_rulelist();

            List<apex_rules_rulelist> allRules = toReturn.GetAllRules(false);

            toReturn = null;
            foreach (apex_rules_rulelist checkRule in allRules)
            {
                if (checkRule.Applies(this))
                {
                    // return first applicable rule
                    toReturn = checkRule;
                    break;
                }
            }
            return toReturn;

        }

        /// <summary>
        /// Returns false if no local copy was found
        /// </summary>
        /// <param name="dentrixID"></param>
        /// <param name="dentrixDB"></param>
        /// <returns></returns>
        internal bool LoadWithDentrixIDs(string dentrixID, string dentrixDB)
        {
            bool claimFound = true;
            claim c = new claim();
            c.claimidnum = dentrixID;
            c.claimdb = dentrixDB;

            DataTable matches = c.Search();

            if (matches.Rows.Count > 0)
                Load(matches.Rows[0]);
            else
                claimFound = false;

            return claimFound;
        }

        internal void SetApexStatus()
        {
            if (system_options.ApexSendStatus > 0 || system_options.ApexRevisitDateEnabled) 
            {
                try
                {
                    claim_status cs = null;
                    DateTime? newRevisit = null;
                    if (system_options.ApexSendStatus > 0)
                        cs = new claim_status(system_options.ApexSendStatus);
                    if (system_options.ApexRevisitDateEnabled)
                        newRevisit = DateTime.Today.AddDays(system_options.ApexRevisitDate);

                    SetStatusAndRevisitDate(cs, newRevisit);
                }
                catch (Exception e) { LoggingHelper.Log("Error in SetApexStatus\n" + e.Message, LogSeverity.Critical, e, false); }
            }
        }

        internal void SetApexResendStatus()
        {
            if (system_options.ApexResendStatus > 0)
            {
                try
                {
                    SetStatusAndRevisitDate(new claim_status(system_options.ApexResendStatus), null);
                }
                catch (Exception e) { LoggingHelper.Log("Error in SetApexStatus\n" + e.Message, LogSeverity.Critical, e, false); }
            }
        }

        /// <summary>
        /// After importing the revisit date is set to 1/1/1900 instead of null, fix this here
        /// </summary>
        internal void FixRevisitDateAfterImport()
        {
            ExecuteNonQuery("UPDATE claims SET revisit_date = null WHERE revisit_date='1/1/1900'");
        }

        /// <summary>
        /// Returns the last date of service 
        /// </summary>
        /// <returns></returns>
        public string DatesOfServiceString()
        {
            string toReturn = "";

            DataTable dos = Search("SELECT DISTINCT(pl_date) FROM procedures WHERE claim_id = " + id + " order by pl_date desc");

            if (dos.Rows.Count > 1)
            {
                try
                {
                    toReturn = string.Format("Dates of service: {0} - {1}", Convert.ToDateTime(dos.Rows[dos.Rows.Count - 1][0]).ToShortDateString(), Convert.ToDateTime(dos.Rows[0][0]).ToShortDateString());
                }
                catch 
                {
                    toReturn = "";
                }
            }

            return toReturn;
        }



        internal claim FindFirstClaimWithProvider(string providerID)
        {
            claim toReturn = new claim();
            string sql = "SELECT TOP 1 * FROM claims WHERE doctor_provider_id = '" + providerID +
                    "' ORDER BY ID desc";
            toReturn.Load(Search(sql).Rows[0]);
            return toReturn;
        }

        internal string claim_type_display(bool abbreviate = false)
        {
            List<string> claimDisplayValues = new List<string>();
            int index = 0;

            claimDisplayValues.Add("PRIM");
            claimDisplayValues.Add("SEC");
            claimDisplayValues.Add("PRE");
            claimDisplayValues.Add("SEC-PRE");
            claimDisplayValues.Add("Primary");
            claimDisplayValues.Add("Secondary");
            claimDisplayValues.Add("Predetermination");
            claimDisplayValues.Add("Secondary Predetermination");
            
            if (claim_type == claim.ClaimTypes.Secondary)
                index = 1;
            else if (claim_type == claim.ClaimTypes.Predeterm)
                index = 2;    
            else if (claim_type == claim.ClaimTypes.SecondaryPredeterm)
                index = 3;


            if (!abbreviate) index += 4;

            return claimDisplayValues[index];
        }

        internal bool IsSecondaryPredeterm
        {
            get
            {
                return primary_claimid != "0";
            }
        }

        internal DateTime? GetMostRecentSentDate()
        {
            string sql = string.Format("SELECT TOP 1 cb.batch_date FROM batch_claim_list bcl " +
                "INNER JOIN claim_batch cb ON bcl.batch_id = cb.id " +
                "WHERE bcl.claim_id = {0}", id);
            DataTable dt = Search(sql);

            if (dt.Rows.Count > 0)
                return new DateTime?(Convert.ToDateTime(dt.Rows[0][0]));
            else
                return null;

        }

        internal static List<claim> OverrideProviderList()
        {
            claim searchObject = new claim();
            List<claim> toReturn = new List<claim>();

            DataTable providers = searchObject.Search("SELECT DISTINCT(doctor_provider_id), doctor_first_name, doctor_last_name FROM claims " +
                "WHERE doctor_provider_id IS NOT Null AND doctor_provider_id != '' AND doctor_provider_id NOT LIKE 'H%' " +
                "AND doctor_provider_id NOT LIKE 'X%' ORDER BY doctor_provider_id");

            foreach (DataRow aRow in providers.Rows)
            {
                searchObject = new claim();
                searchObject.Load(aRow);

                toReturn.Add(searchObject);
            }

            return toReturn;
        }
    }
}
