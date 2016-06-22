using System;
using System.Collections;
using System.Data.SqlClient;
using System.Xml.Serialization;

namespace NHDG.NHDGCommon.Claims {
	/// <summary>Represents a Dental Insurance Claim.</summary>
	public class Claim {
		/// <summary>Contains identification information about the claim.</summary>
		public Identity Identity = new Identity();

		/// <summary>Contains general information about the claim.</summary>
		/// <remarks>Covers fields 1-7 and 63-66 on the 1999 ADA form.</remarks>
		public ClaimInfo GeneralInformation = new ClaimInfo();

		/// <summary>Contains information about the patient.</summary>
		/// <remarks>Covers fields 8-18 on the 1999 ADA form.</remarks>
		public Patient Patient = new Patient();

		/// <summary>Contains information about the subscriber.</summary>
		/// <remarks>Covers fields 19-30, 38, and 40 on the 1999 ADA form.</remarks>
		public Subscriber Subscriber = new Subscriber();

		/// <summary>Contains information about the the patient's other policy.</summary>
		/// <remarks>Covers fields 31-37 on the 1999 ADA form.</remarks>
		public OtherPolicy OtherPolicy = null;

		/// <summary>Contains information about the billing dentist.</summary>
		/// <remarks>Covers fields 48-57 on the 1999 ADA form.</remarks>
		public BillingDentist BillingDentist = new BillingDentist();

		/// <summary>Contains information about treatments being claimed.</summary>
		/// <remarks>Covers fields 58-61 on the 1999 ADA form.</remarks>
		public TreatmentInformation TreatmentInformation = new TreatmentInformation();

        /// <summary>Contains ancillary information about the claim.</summary>
        /// <remarks>Covers fields 42-47 on the 1999 ADA form.</remarks>
        public AncillaryData AncillaryData = new AncillaryData();

        /// <summary>Contains information about the claim if it is a secondary.</summary>
        public SecondaryInformation SecondaryInfo = new SecondaryInformation();


		/// <summary>Initializes an instance of a Claim.</summary>
		public Claim() {}

        public Claim(C_DentalClaimTracker.claim aClaim, SqlTransaction trans, bool hideTransactionPayments, bool isApex)
        {
            InitializeClaim(aClaim.claimidnum, aClaim.claimdb, trans, hideTransactionPayments, aClaim, isApex);
        }

		/// <summary>Initializes an instance of a Claim.</summary>
		/// <param name="claimID">The ID of the claim in Dentrix.</param>
		/// <param name="claimDB">The Database ID of the claim in Dentrix.</param>
		/// <param name="trans">The SQL Transaction to use to retrieve the claim information.</param>
        public Claim(int claimID, int claimDB, SqlTransaction trans, bool hideTransactionPayments, C_DentalClaimTracker.claim claimObject, bool isApex)
        {
            InitializeClaim(claimID.ToString(), claimDB.ToString(), trans, hideTransactionPayments, claimObject, isApex);
        }

        private void InitializeClaim(string claimID, string claimDB, SqlTransaction trans, bool hideTransactionPayments, C_DentalClaimTracker.claim claimObject, bool isApex)
        {
            SqlCommand cmd;
            string sqlClaim = @"SELECT     claim.CLAIMID AS CLAIM_ID, claim.CLAIMDB AS CLAIM_DB,
                      (CASE WHEN claim.type = 1 THEN 'PREDETERM' WHEN claim.PRIMCLAIMID = 0 THEN 'PRIMARY' WHEN claim.PRIMCLAIMID > claim.CLAIMID THEN 'PRIMARY' WHEN claim.PRIMCLAIMID IS NULL
                       THEN 'PRIMARY' ELSE 'SECONDARY' END) AS CLAIM_ORD, claim.PRIMCLAIMID AS CLAIM_OTHER_ID, claim.PRIMCLAIMDB AS CLAIM_OTHER_DB, 
                      claim.DATEOFCLAIM AS CLAIM_DATE, claim.TOTALBILLED AS CLAIM_TOTAL_BILLED, claim.TOTALRECEIVED AS CLAIM_TOTAL_RECEIVED, 
                      claim2.DATERECEIVED AS PRIMARYCLAIM_DATE_RECEIVED, claim2.TOTALBILLED AS PRIMARYCLAIM_TOTAL_BILLED, claim2.TOTALRECEIVED AS PRIMARYCLAIM_TOTAL_RECEIVED,
                      (CASE WHEN claim2.TOTALBILLED IS NULL THEN 0 ELSE claim2.TOTALBILLED END) AS CLAIM_OTHER_TOTAL_BILLED, 
                      (CASE WHEN claim2.TOTALRECEIVED IS NULL THEN 0 ELSE claim2.TOTALRECEIVED END) AS CLAIM_OTHER_TOTAL_RECEIVED, 
                      cinfo.AUTHNUM AS CLAIM_PRIOR_AUTH, cinfo.FIRSTVISITDATE AS CLAIM_FIRST_VISIT, cinfo.PLACEOFTREAT AS CLAIM_PLACE, 
                      cinfo.XENCLOSED AS CLAIM_RADIOGRAPHS_ENCLOSED, cinfo.XCOUNT AS CLAIM_NUM_RADIOGRAPHS, cinfo.ORTHO AS CLAIM_FOR_ORTHO, 
                      cinfo.ORTHODATE AS CLAIM_ORTHO_APPL_PLACED, cinfo.ORTHOMREMAIN AS CLAIM_ORTHO_REMAINING, 
                      cinfo.PROTHINIT AS CLAIM_PROTH_INIT_REPL, cinfo.SCHOOL AS CLAIM_SCHOOL, cinfo.STUDENTSTATUS AS CLAIM_STUDENTSTATUS, 
                      cinfo.PROTHREASON AS CLAIM_PROTH_REASON, cinfo.PROTHDATE AS CLAIM_PROTH_PRIOR_DATE, cinfo.OCCUPILL AS CLAIM_OCCUP_INJ, 
                      cinfo.OCCUPILLDATE AS CLAIM_OCCUP_DATE, cinfo.OCCUPILLDESC AS CLAIM_OCCUP_DESC, cinfo.ACCIDENT AS CLAIM_ACCIDENT, 
                      cinfo.ACCIDENTDATE AS CLAIM_ACCIDENT_DATE, cinfo.ACCIDENTDESC AS CLAIM_ACCIDENT_DESC, ins1.INSCONAME AS INS_NAME, 
                      ins1.STREET AS INS_STREET1, ins1.STREET2 AS INS_STREET2, ins1.CITY AS INS_CITY, ins1.STATE AS INS_STATE, ins1.ZIP AS INS_ZIP, 
                      insd1.IDNUM AS INS_SUBSCRIBER_ID, ins1.GROUPNUM AS INS_GROUP_NUM, pat.LASTNAME AS PAT_LASTNAME, 
                      pat.FIRSTNAME AS PAT_FIRSTNAME, pat.MI AS PAT_MIDDLENAME, pat.BIRTHDATE AS PAT_BIRTHDATE, pat.UPCHART AS PAT_CHART, 
                      pat.GENDER AS PAT_GENDER, pat.PRINSREL AS PAT_PRIMARY_INS_REL, pat.SCINSREL AS PAT_SECONDARY_INS_REL, 
                      pataddr.STREET AS PAT_STREET1, pataddr.STREET2 AS PAT_STREET2, pataddr.CITY AS PAT_CITY, pataddr.STATE AS PAT_STATE, 
                      pataddr.ZIP AS PAT_ZIP, pataddr.PHONE AS PAT_PHONE, patemp.NAME AS PAT_EMP_NAME, patemp.CITY AS PAT_EMP_CITY, 
                      patemp.STATE AS PAT_EMP_STATE, patemp.ZIP AS PAT_EMP_ZIP, prov.NAME_LAST AS PROV_LASTNAME, prov.NAME_FIRST AS PROV_FIRSTNAME, 
                      prov.NAME_INITIAL AS PROV_MIDDLENAME, prov.NAME_TITLE AS PROV_TITLE, prov.URSCID AS PROV_ID, prov.[ADA] AS PROV_TIN, 
                      prov.STATEID AS PROV_LICENSE, clinic.STREET1 AS CLINIC_STREET1, clinic.STREET2 AS CLINIC_STREET2, clinic.CITY AS CLINIC_CITY, 
                      clinic.STATE AS CLINIC_STATE, clinic.ZIP AS CLINIC_ZIP, clinic.PHONE AS CLINIC_PHONE, sub.LASTNAME AS SUB_LASTNAME, 
                      sub.FIRSTNAME AS SUB_FIRSTNAME, sub.MI AS SUB_MIDDLENAME, sub.BIRTHDATE AS SUB_BIRTHDATE, sub.GENDER AS SUB_GENDER, 
                      sub.FAMPOS AS SUB_MARITAL_STATUS, subemp.NAME AS SUB_EMP_NAME, subemp.CITY AS SUB_EMP_CITY, 
                      subemp.STATE AS SUB_EMP_STATE, subemp.ZIP AS SUB_EMP_ZIP, subaddr.STREET AS SUB_STREET1, subaddr.STREET2 AS SUB_STREET2, 
                      subaddr.CITY AS SUB_CITY, subaddr.STATE AS SUB_STATE, subaddr.ZIP AS SUB_ZIP, subaddr.PHONE AS SUB_PHONE, 
                      ins2.INSCONAME AS INS2_NAME, ins2.GROUPNUM AS INS2_GROUP_NUM, sub2.LASTNAME AS SUB2_LASTNAME, 
                      sub2.FIRSTNAME AS SUB2_FIRSTNAME, sub2.MI AS SUB2_MIDDLENAME, sub2.BIRTHDATE AS SUB2_BIRTHDATE, 
                      sub2.GENDER AS SUB2_GENDER, sub2.FAMPOS AS SUB2_MARITAL_STATUS, sub2emp.NAME AS SUB2_EMP_NAME, 
                      sub2addr.STREET AS sub2_street, sub2addr.CITY AS sub2_city, sub2addr.STATE AS sub2_state, sub2addr.ZIP AS sub2_zip, 
                      sub2emp.CITY AS SUB2_EMP_CITY, sub2emp.STATE AS SUB2_EMP_STATE, sub2emp.ZIP AS SUB2_EMP_ZIP, 
                      claim.DATE3 AS CLAIM_RESENT_DATE, claim.TYPE, claim.OINSID, claim.PATID, claim.PATDB,
                      claim.ClinicAppliedTo, sub2.scinsrel as SUB2_SCINSREL, insd2.idnum as INS2_IDNUM, 
                      INS2.Street AS INS2_Street, INS2.City as INs2_City, INS2.State AS INS2_State, INS2.ZIP as INs2_Zip, 
                      prov.ID1 as NPI
                      FROM         dbo.DDB_CLAIM_BASE claim LEFT OUTER JOIN
                      dbo.DDB_CLAIM_INFO_BASE cinfo ON claim.CLAIMINFID = cinfo.CLAIMINFID AND claim.CLAIMINFDB = cinfo.CLAIMINFDB LEFT OUTER JOIN
                      dbo.DDB_INSURANCE_BASE ins1 ON claim.INSID = ins1.INSID AND claim.INSDB = ins1.INSDB LEFT OUTER JOIN
                      dbo.DDB_PAT_BASE pat ON claim.PATID = pat.PATID AND claim.PATDB = pat.PATDB LEFT OUTER JOIN
                      dbo.DDB_ADDRESS_BASE pataddr ON pat.ADDRESSID = pataddr.ADDRESSID AND pat.ADDRESSDB = pataddr.ADDRESSDB LEFT OUTER JOIN
                      dbo.DDB_EMP_BASE patemp ON pat.EMPID = patemp.EMPID AND pat.EMPDB = patemp.EMPDB LEFT OUTER JOIN
                      dbo.DDB_RSC_BASE prov ON claim.PROVID = prov.URSCID AND claim.PROVDB = prov.RSCDB LEFT OUTER JOIN
                      dbo.NHDG_INSURANCE_CLAIM_LOCATION clinic ON claim.CLAIMID = clinic.CLAIM_ID AND claim.CLAIMDB = clinic.CLAIM_DB LEFT OUTER JOIN
                      dbo.DDB_INSURED_BASE insd1 ON claim.INSUREDID = insd1.INSUREDID AND claim.INSUREDDB = insd1.INSUREDDB LEFT OUTER JOIN
                      dbo.DDB_PAT_BASE sub ON claim.INSUREDPARTYID = sub.PATID AND claim.INSUREDPARTYDB = sub.PATDB LEFT OUTER JOIN
                      dbo.DDB_EMP_BASE subemp ON sub.EMPID = subemp.EMPID AND sub.EMPDB = subemp.EMPDB LEFT OUTER JOIN
                      dbo.DDB_ADDRESS_BASE subaddr ON sub.ADDRESSID = subaddr.ADDRESSID AND sub.ADDRESSDB = subaddr.ADDRESSDB LEFT OUTER JOIN
                      dbo.DDB_INSURANCE_BASE ins2 ON claim.OINSID = ins2.INSID AND claim.OINSDB = ins2.INSDB LEFT OUTER JOIN
                      dbo.DDB_PAT_BASE sub2 ON claim.OINSUREDPARTYID = sub2.PATID AND claim.OINSUREDPARTYDB = sub2.PATDB LEFT OUTER JOIN
                      dbo.DDB_ADDRESS_BASE sub2addr ON sub2.AddressID = sub2addr.AddressID AND sub2.AddressDB = sub2addr.AddressDB LEFT OUTER JOIN
                      dbo.DDB_EMP_BASE sub2emp ON sub2.EMPID = sub2emp.EMPID AND sub2.EMPDB = sub2emp.EMPDB LEFT OUTER JOIN
                      dbo.DDB_CLAIM_BASE claim2 ON (claim.PRIMCLAIMID = claim2.CLAIMID AND claim.PRIMCLAIMDB = claim2.CLAIMDB AND claim.PRIMCLAIMID != 0)
                      LEFT OUTER JOIN dbo.DDB_INSURED insd2 ON claim.OInsuredPartyID = insd2.InsuredID 
                      WHERE (claim.claimid = " + claimID.ToString() + ") " +
                      "  AND (claim.claimdb = " + claimDB.ToString() + ")";
            cmd = new SqlCommand(sqlClaim, trans.Connection, trans);

            try
            {
                GetClaimInfo(cmd, claimObject);
            }
            catch (Exception ex)
            {
                throw new Exception("Could not retrieve claim " + claimID.ToString() + "/" + claimDB.ToString() + ".", ex);
            }

            string queryClaimID;
            string queryClaimDB;

            if (Identity.Type == ClaimType.Primary)
            {
                queryClaimID = "WHERE (CLAIM_ID = " + Identity.ClaimID.ToString() + ")";
                queryClaimDB = " AND (CLAIM_DB = " + Identity.ClaimDB.ToString() + ")";
            }
            else if (Identity.Type == ClaimType.Secondary)
            {
                queryClaimID = "WHERE (CLAIM_ID = " + Identity.OtherClaimID.ToString() + ")";
                queryClaimDB = " AND (CLAIM_DB = " + Identity.OtherClaimDB.ToString() + ")";
            }
            else if (claimObject.claim_type == C_DentalClaimTracker.claim.ClaimTypes.Predeterm)
            {
                queryClaimID = "WHERE (PRE_AUTHID = " + Identity.ClaimID.ToString() + ")";
                queryClaimDB = " AND (PRE_AUTHDB = " + Identity.ClaimDB.ToString() + ")";
            }
            else // secondary predeterm
            {
                queryClaimID = "WHERE (PRE_AUTHID = " + claimObject.primary_claimid.ToString() + ")";
                queryClaimDB = " AND (PRE_AUTHDB = " + claimObject.primary_claimdb.ToString() + ")";
            }

            string sql = "SELECT [DATE], " +
                "       TOOTH, TOOTH_END, " +
                "       SURFACE, " +
                "       DIAGNOSIS_INDEX, " +
                "       PROCEDURE_CODE, " +
                "       QUANTITY, " +
                "       DESCRIPTION, " +
                "       FEE, AMTPINSPAID " +
                "FROM NHDG_INSURANCE_CLAIM_TREATMENTS " +
                queryClaimID + queryClaimDB;

            if (hideTransactionPayments)
            {
                sql += " AND PROCEDURE_CODE IS NOT NULL";
            }

            cmd = new SqlCommand(
                
                sql, trans.Connection, trans);
             
            try
            {
                GetClaimTransactions(cmd);
            }
            catch (Exception ex)
            {
                throw new Exception("Could not retrieve claim procedures " + claimID.ToString() + "/" + claimDB.ToString() + ".", ex);
            }

            try
            {
                cmd = new SqlCommand("SELECT NOTETEXT FROM DDB_NOTE_BASE WHERE NOTEID = " + Identity.ClaimID +
                " AND NoteType = 11", trans.Connection, trans);
                GetNoteInfo(cmd, claimObject, isApex);
            }
            catch (Exception ex)
            {
                throw new Exception("Could not retrieve notes for claim " + claimID.ToString() + "/" + claimDB.ToString() + ".", ex);
            }

            try
            {
                cmd = new SqlCommand("SELECT Street, Street2, City, State, Zip FROM DDB_RSC where uRSCID = "
                    + GeneralInformation.ClinicAppliedTo, trans.Connection, trans);
                GetTreatmentInfo(cmd, claimObject);
            }
            catch (Exception ex)
            {
                throw new Exception("Could not retrieve treatment info for claim " + claimID.ToString() + "/" + claimDB.ToString() + ".", ex);
            }
        }


        private void GetTreatmentInfo(SqlCommand cmd, C_DentalClaimTracker.claim claimObject)
        {
            SqlDataReader reader = cmd.ExecuteReader();
            if (!reader.Read())
            {
                try { reader.Close(); }
                catch { }
                BillingDentist.Address = new Address();
            }
            else
            {
                if (claimObject.CheckForAddressOverride)
                {
                    C_DentalClaimTracker.dentrix_providers dp = new C_DentalClaimTracker.dentrix_providers();
                    dp = dp.GetProvider(claimObject.override_address_provider);

                    BillingDentist.Address.Street1 = dp.STREET;
                    BillingDentist.Address.Street2 = dp.STREET2;
                    BillingDentist.Address.City = dp.CITY;
                    BillingDentist.Address.State = dp.STATE;
                    BillingDentist.Address.Zip = dp.ZIP;
                }
                else
                {
                    try { BillingDentist.Address.Street1 = reader.GetString(reader.GetOrdinal("Street")); }
                    catch (System.Data.SqlTypes.SqlNullValueException) { }
                    try { BillingDentist.Address.Street2 = reader.GetString(reader.GetOrdinal("Street2")); }
                    catch (System.Data.SqlTypes.SqlNullValueException) { }
                    try { BillingDentist.Address.City = reader.GetString(reader.GetOrdinal("City")); }
                    catch (System.Data.SqlTypes.SqlNullValueException) { }
                    try { BillingDentist.Address.State = reader.GetString(reader.GetOrdinal("State")); }
                    catch (System.Data.SqlTypes.SqlNullValueException) { }
                    try { BillingDentist.Address.Zip = reader.GetString(reader.GetOrdinal("Zip")); }
                    catch (System.Data.SqlTypes.SqlNullValueException) { }
                }
                
                reader.Close();
            }

         
        }

        private void GetNoteInfo(SqlCommand cmd, C_DentalClaimTracker.claim claimObject, bool isApex)
        {
            // First, we look to see if there is any default text
            // Second, if it is an APEX claim we add any Apex specific notes/flags
            // Third, we append the "nea_number" field from the claim

            SqlDataReader reader = cmd.ExecuteReader();


            if (!reader.Read())
            {
                try { reader.Close(); }
                catch { }
                TreatmentInformation.RemarksForUnusualServices = "";
            }
            else
            {
                try { TreatmentInformation.RemarksForUnusualServices = reader.GetString(reader.GetOrdinal("NOTETEXT")); }
                catch (System.Data.SqlTypes.SqlNullValueException) { }

                reader.Close();
            }


            // AJ 8-11-2011
            // Append the apex rules or the override specified in the claim object
            if (isApex)
            {
                
                
                string treatmentInfo = "";
                if (claimObject.apex_override_default_message)
                {
                    treatmentInfo = claimObject.apex_message_text;
                }
                else
                {
                    // Find and use apex defaults
                    C_DentalClaimTracker.apex_rules_rulelist arr = claimObject.GetApplicableRule();

                    if (arr != null)
                    {
                        treatmentInfo = arr.rule_text;
                    }

                }

                TreatmentInformation.RemarksForUnusualServices = Convert.ToString(treatmentInfo + " " + TreatmentInformation.RemarksForUnusualServices).Trim();
            }

            
            // Add in NEA Number
            TreatmentInformation.RemarksForUnusualServices += " " + claimObject.nea_number;
            TreatmentInformation.RemarksForUnusualServices = TreatmentInformation.RemarksForUnusualServices.Trim();
        }


		private void GetClaimInfo(SqlCommand cmd, C_DentalClaimTracker.claim claimObject) {
			SqlDataReader reader = cmd.ExecuteReader();

            try
            {
                if (!reader.Read())
                {
                    try { reader.Close(); }
                    catch { }
                    throw new Exception("Claim not found.");
                }

                // Identity Information.
                Identity.ClaimID = reader.GetInt32(reader.GetOrdinal("CLAIM_ID"));
                Identity.ClaimDB = reader.GetInt32(reader.GetOrdinal("CLAIM_DB"));
                string temp = "PRIMARY";
                try { temp = reader.GetString(reader.GetOrdinal("CLAIM_ORD")); }
                catch (System.Data.SqlTypes.SqlNullValueException) { }
                if (temp == "PRIMARY")
                {
                    Identity.Type = ClaimType.Primary;
                }
                else if (temp == "SECONDARY")
                {
                    Identity.Type = ClaimType.Secondary;
                }
                else
                {
                    Identity.Type = ClaimType.Predeterm;
                    try { GeneralInformation.Purpose = "Request"; }
                    catch (System.Data.SqlTypes.SqlNullValueException) { }
                }
                try { Identity.OtherClaimID = reader.GetInt32(reader.GetOrdinal("CLAIM_OTHER_ID")); }
                catch (System.Data.SqlTypes.SqlNullValueException) { }
                try { Identity.OtherClaimDB = reader.GetInt32(reader.GetOrdinal("CLAIM_OTHER_DB")); }
                catch (System.Data.SqlTypes.SqlNullValueException) { }

                // General Information.
                try { GeneralInformation.Date = reader.GetDateTime(reader.GetOrdinal("CLAIM_DATE")).ToString("MM/dd/yyyy"); }
                catch (System.Data.SqlTypes.SqlNullValueException) { }
                try { GeneralInformation.PriorAuthorization = reader.GetString(reader.GetOrdinal("CLAIM_PRIOR_AUTH")); }
                catch (System.Data.SqlTypes.SqlNullValueException) { }
                try { GeneralInformation.Carrier.Name = reader.GetString(reader.GetOrdinal("INS_NAME")); }
                catch (System.Data.SqlTypes.SqlNullValueException) { }
                try { GeneralInformation.Carrier.Address.Street1 = reader.GetString(reader.GetOrdinal("INS_STREET1")); }
                catch (System.Data.SqlTypes.SqlNullValueException) { }
                try { GeneralInformation.Carrier.Address.Street2 = reader.GetString(reader.GetOrdinal("INS_STREET2")); }
                catch (System.Data.SqlTypes.SqlNullValueException) { }
                try { GeneralInformation.Carrier.Address.City = reader.GetString(reader.GetOrdinal("INS_CITY")); }
                catch (System.Data.SqlTypes.SqlNullValueException) { }
                try { GeneralInformation.Carrier.Address.State = reader.GetString(reader.GetOrdinal("INS_STATE")); }
                catch (System.Data.SqlTypes.SqlNullValueException) { }
                try { GeneralInformation.Carrier.Address.Zip = Utilities.FormatZipCode(reader.GetString(reader.GetOrdinal("INS_ZIP"))); }
                catch (System.Data.SqlTypes.SqlNullValueException) { }
                

                if (claimObject.CheckForAddressOverride)
                {
                    C_DentalClaimTracker.dentrix_providers dp = new C_DentalClaimTracker.dentrix_providers();
                    dp = dp.GetProvider(claimObject.override_address_provider);
                    GeneralInformation.TreatmentLocation.Street1 = dp.STREET;
                    GeneralInformation.TreatmentLocation.Street2 = dp.STREET2;
                    GeneralInformation.TreatmentLocation.City = dp.CITY;
                    GeneralInformation.TreatmentLocation.State = dp.STATE;
                    GeneralInformation.TreatmentLocation.Zip = dp.ZIP;
                    GeneralInformation.BillingNPI = dp.ID1;
                }
                else
                {
                    try { GeneralInformation.TreatmentLocation.Street1 = reader.GetString(reader.GetOrdinal("CLINIC_STREET1")); }
                    catch (System.Data.SqlTypes.SqlNullValueException) { }
                    try { GeneralInformation.TreatmentLocation.Street2 = reader.GetString(reader.GetOrdinal("CLINIC_STREET2")); }
                    catch (System.Data.SqlTypes.SqlNullValueException) { }
                    try { GeneralInformation.TreatmentLocation.City = reader.GetString(reader.GetOrdinal("CLINIC_CITY")); }
                    catch (System.Data.SqlTypes.SqlNullValueException) { }
                    try { GeneralInformation.TreatmentLocation.State = reader.GetString(reader.GetOrdinal("CLINIC_STATE")); }
                    catch (System.Data.SqlTypes.SqlNullValueException) { }
                    try { GeneralInformation.TreatmentLocation.Zip = Utilities.FormatZipCode(reader.GetString(reader.GetOrdinal("CLINIC_ZIP"))); }
                    catch (System.Data.SqlTypes.SqlNullValueException) { }
                    
                    // Billing NPI is currently hard-coded, could pull this from the DB or make it an application setting at some point
                    GeneralInformation.BillingNPI = "1609916089";

                }

                try { GeneralInformation.ClinicAppliedTo = reader.GetInt32(reader.GetOrdinal("CLINICAPPLIEDTO")); }
                catch (System.Data.SqlTypes.SqlNullValueException) { }

                // Patient Information.
                try { Patient.Name = reader.GetString(reader.GetOrdinal("PAT_LASTNAME")) + ", "; }
                catch (System.Data.SqlTypes.SqlNullValueException) { }
                try { Patient.Name += reader.GetString(reader.GetOrdinal("PAT_FIRSTNAME")) + " "; }
                catch (System.Data.SqlTypes.SqlNullValueException) { }
                try { Patient.Name += reader.GetString(reader.GetOrdinal("PAT_MIDDLENAME")); }
                catch (System.Data.SqlTypes.SqlNullValueException) { }
                Patient.Name = Patient.Name.Trim();
                try { Patient.Address.Street1 = reader.GetString(reader.GetOrdinal("PAT_STREET1")); }
                catch (System.Data.SqlTypes.SqlNullValueException) { }
                try { Patient.Address.Street2 = reader.GetString(reader.GetOrdinal("PAT_STREET2")); }
                catch (System.Data.SqlTypes.SqlNullValueException) { }
                try { Patient.Address.City = reader.GetString(reader.GetOrdinal("PAT_CITY")); }
                catch (System.Data.SqlTypes.SqlNullValueException) { }
                try { Patient.Address.State = reader.GetString(reader.GetOrdinal("PAT_STATE")); }
                catch (System.Data.SqlTypes.SqlNullValueException) { }
                try { Patient.Address.Zip = Utilities.FormatZipCode(reader.GetString(reader.GetOrdinal("PAT_ZIP"))); }
                catch (System.Data.SqlTypes.SqlNullValueException) { }
                try { Patient.BirthDate = reader.GetDateTime(reader.GetOrdinal("PAT_BIRTHDATE")).ToString("MM/dd/yyyy"); }
                catch (System.Data.SqlTypes.SqlNullValueException) { }
                try { Patient.ID = reader.GetString(reader.GetOrdinal("PAT_CHART")); }
                catch (System.Data.SqlTypes.SqlNullValueException) { }
                try { Patient.Sex = (Gender)reader.GetInt16(reader.GetOrdinal("PAT_GENDER")); }
                catch (System.Data.SqlTypes.SqlNullValueException) { }
                try { Patient.PhoneNumber = Utilities.FormatPhoneNumber(reader.GetString(reader.GetOrdinal("PAT_PHONE"))); }
                catch (System.Data.SqlTypes.SqlNullValueException) { }
                if (Identity.Type == ClaimType.Secondary)
                {
                    try
                    {
                        Patient.SubscriberRelationship = (Relationship)reader.GetInt16(reader.GetOrdinal("PAT_SECONDARY_INS_REL"));
                    }
                    catch { Patient.SubscriberRelationship = Relationship.None; }
                }
                else
                {
                    try
                    {
                        Patient.SubscriberRelationship = (Relationship)reader.GetInt16(reader.GetOrdinal("PAT_PRIMARY_INS_REL"));
                    }
                    catch { Patient.SubscriberRelationship = Relationship.None; }
                }

                // Hack/fix for some unusual codes coming through for subscriber relationships
                if ((int) Patient.SubscriberRelationship >= 5)
                {
                    Patient.SubscriberRelationship = Relationship.Other;
                }
                  



                temp = string.Empty;
                try { temp = reader.GetString(reader.GetOrdinal("CLAIM_SCHOOL")); }
                catch (System.Data.SqlTypes.SqlNullValueException) { }
                if (temp.Trim() == string.Empty)
                {
                    try { Patient.Employer.Name = reader.GetString(reader.GetOrdinal("PAT_EMP_NAME")); }
                    catch (System.Data.SqlTypes.SqlNullValueException) { }
                    try { Patient.Employer.Address = reader.GetString(reader.GetOrdinal("PAT_EMP_CITY")) + ", "; }
                    catch (System.Data.SqlTypes.SqlNullValueException) { }
                    try { Patient.Employer.Address += reader.GetString(reader.GetOrdinal("PAT_EMP_STATE")) + " "; }
                    catch (System.Data.SqlTypes.SqlNullValueException) { }
                    try { Patient.Employer.Address += Utilities.FormatZipCode(reader.GetString(reader.GetOrdinal("PAT_EMP_ZIP"))); }
                    catch (System.Data.SqlTypes.SqlNullValueException) { }
                    Patient.Employer.Address = Patient.Employer.Address.Trim();
                    if (Patient.Employer.Address == ",") { Patient.Employer.Address = string.Empty; }
                }
                else
                {
                    Patient.Employer.Name = temp;
                }

                // Subscriber Information.
                try { Subscriber.ID = reader.GetString(reader.GetOrdinal("INS_SUBSCRIBER_ID")); }
                catch (System.Data.SqlTypes.SqlNullValueException) { }
                try { Subscriber.Employer.Name = reader.GetString(reader.GetOrdinal("SUB_EMP_NAME")); }
                catch (System.Data.SqlTypes.SqlNullValueException) { }
                try { Subscriber.Employer.Address = reader.GetString(reader.GetOrdinal("SUB_EMP_CITY")) + ", "; }
                catch (System.Data.SqlTypes.SqlNullValueException) { }
                try { Subscriber.Employer.Address += reader.GetString(reader.GetOrdinal("SUB_EMP_STATE")) + " "; }
                catch (System.Data.SqlTypes.SqlNullValueException) { }
                try { Subscriber.Employer.Address += Utilities.FormatZipCode(reader.GetString(reader.GetOrdinal("SUB_EMP_ZIP"))); }
                catch (System.Data.SqlTypes.SqlNullValueException) { }
                Subscriber.Employer.Address = Subscriber.Employer.Address.Trim();
                if (Subscriber.Employer.Address == ",") { Subscriber.Employer.Address = string.Empty; }
                try { Subscriber.GroupNumber = reader.GetString(reader.GetOrdinal("INS_GROUP_NUM")); }
                catch (System.Data.SqlTypes.SqlNullValueException) { }
                try { Subscriber.Name = reader.GetString(reader.GetOrdinal("SUB_LASTNAME")) + ", "; }
                catch (System.Data.SqlTypes.SqlNullValueException) { }
                try { Subscriber.Name += reader.GetString(reader.GetOrdinal("SUB_FIRSTNAME")) + " "; }
                catch (System.Data.SqlTypes.SqlNullValueException) { }
                try { Subscriber.Name += reader.GetString(reader.GetOrdinal("SUB_MIDDLENAME")); }
                catch (System.Data.SqlTypes.SqlNullValueException) { }
                Subscriber.Name = Subscriber.Name.Trim();
                try { Subscriber.Address.Street1 = reader.GetString(reader.GetOrdinal("SUB_STREET1")); }
                catch (System.Data.SqlTypes.SqlNullValueException) { }
                try { Subscriber.Address.Street2 = reader.GetString(reader.GetOrdinal("SUB_STREET2")); }
                catch (System.Data.SqlTypes.SqlNullValueException) { }
                try { Subscriber.Address.City = reader.GetString(reader.GetOrdinal("SUB_CITY")); }
                catch (System.Data.SqlTypes.SqlNullValueException) { }
                try { Subscriber.Address.State = reader.GetString(reader.GetOrdinal("SUB_STATE")); }
                catch (System.Data.SqlTypes.SqlNullValueException) { }
                try { Subscriber.Address.Zip = Utilities.FormatZipCode(reader.GetString(reader.GetOrdinal("SUB_ZIP"))); }
                catch (System.Data.SqlTypes.SqlNullValueException) { }
                try { Subscriber.PhoneNumber = Utilities.FormatPhoneNumber(reader.GetString(reader.GetOrdinal("SUB_PHONE"))); }
                catch (System.Data.SqlTypes.SqlNullValueException) { }
                try { Subscriber.BirthDate = reader.GetDateTime(reader.GetOrdinal("SUB_BIRTHDATE")).ToString("MM/dd/yyyy"); }
                catch (System.Data.SqlTypes.SqlNullValueException) { }
                try
                {
                    short ms = reader.GetInt16(reader.GetOrdinal("SUB_MARITAL_STATUS"));
                    if ((ms > 4) || (ms < 0))
                    {
                        Subscriber.MaritalStatus = MaritalStatus.Other;
                    }
                    else
                    {
                        Subscriber.MaritalStatus = (MaritalStatus)ms;
                    }
                }
                catch
                {
                    Subscriber.MaritalStatus = MaritalStatus.Other;
                }
                try { Subscriber.Sex = (Gender)reader.GetInt16(reader.GetOrdinal("SUB_GENDER")); }
                catch (System.Data.SqlTypes.SqlNullValueException) { }
                // TODO: Subscriber.EmploymentStatus

                // Other Policy Information.
                temp = string.Empty;
                try { temp = reader.GetString(reader.GetOrdinal("INS2_NAME")); }
                catch (System.Data.SqlTypes.SqlNullValueException) { }

                OtherPolicy = new OtherPolicy();
                OtherPolicy.PlanName = temp;
                if (temp.Trim() != string.Empty)
                {
                    try { OtherPolicy.PolicyNumber = reader.GetString(reader.GetOrdinal("INS2_GROUP_NUM")); }
                    catch (System.Data.SqlTypes.SqlNullValueException) { }
                    try { OtherPolicy.SubscriberName = reader.GetString(reader.GetOrdinal("SUB2_LASTNAME")) + ", "; }
                    catch (System.Data.SqlTypes.SqlNullValueException) { }
                    try { OtherPolicy.SubscriberName += reader.GetString(reader.GetOrdinal("SUB2_FIRSTNAME")) + " "; }
                    catch (System.Data.SqlTypes.SqlNullValueException) { }
                    try { OtherPolicy.SubscriberName += reader.GetString(reader.GetOrdinal("SUB2_MIDDLENAME")); }
                    catch (System.Data.SqlTypes.SqlNullValueException) { }
                    OtherPolicy.SubscriberName = OtherPolicy.SubscriberName.Trim();
                    try { OtherPolicy.SubscriberBirthDate = reader.GetDateTime(reader.GetOrdinal("SUB2_BIRTHDATE")).ToString("MM/dd/yyyy"); }
                    catch (System.Data.SqlTypes.SqlNullValueException) { }
                    try { OtherPolicy.SubscriberSex = (Gender)reader.GetInt16(reader.GetOrdinal("SUB2_GENDER")); }
                    catch (System.Data.SqlTypes.SqlNullValueException) { }
                    try { OtherPolicy.SubscriberEmployer.Name = reader.GetString(reader.GetOrdinal("SUB2_EMP_NAME")); }
                    catch (System.Data.SqlTypes.SqlNullValueException) { }
                    try { OtherPolicy.SubscriberEmployer.Address = reader.GetString(reader.GetOrdinal("SUB2_EMP_CITY")) + ", "; }
                    catch (System.Data.SqlTypes.SqlNullValueException) { }
                    try { OtherPolicy.SubscriberEmployer.Address += reader.GetString(reader.GetOrdinal("SUB2_EMP_STATE")) + " "; }
                    catch (System.Data.SqlTypes.SqlNullValueException) { }
                    try { OtherPolicy.SubscriberEmployer.Address += Utilities.FormatZipCode(reader.GetString(reader.GetOrdinal("SUB2_EMP_ZIP"))); }
                    catch (System.Data.SqlTypes.SqlNullValueException) { }
                    OtherPolicy.SubscriberEmployer.Address = OtherPolicy.SubscriberEmployer.Address.Trim();

                    // *** NEW AS OF 3/26/2013
                    // Added for Mercury support - secondary subscriber address information
                    try { OtherPolicy.SubscriberAddress.RecipientName = OtherPolicy.SubscriberName; }
                    catch (System.Data.SqlTypes.SqlNullValueException) { }
                    try { OtherPolicy.SubscriberAddress.Street1 = reader.GetString(reader.GetOrdinal("SUB2_Street")); }
                    catch (System.Data.SqlTypes.SqlNullValueException) { }
                    try { OtherPolicy.SubscriberAddress.City = reader.GetString(reader.GetOrdinal("SUB2_City")); }
                    catch (System.Data.SqlTypes.SqlNullValueException) { }
                    try { OtherPolicy.SubscriberAddress.State = reader.GetString(reader.GetOrdinal("SUB2_State")); }
                    catch (System.Data.SqlTypes.SqlNullValueException) { }
                    try { OtherPolicy.SubscriberAddress.Zip = reader.GetString(reader.GetOrdinal("SUB2_Zip")); }
                    catch (System.Data.SqlTypes.SqlNullValueException) { }
                    // ****

                    try { OtherPolicy.SubscriberID = reader.GetString(reader.GetOrdinal("INS2_IDNUM")); }
                    catch (System.Data.SqlTypes.SqlNullValueException) { }
                    try { OtherPolicy.PatientRelationship = reader.GetInt16(reader.GetOrdinal("PAT_SECONDARY_INS_REL")).ToString(); }
                    catch (System.Data.SqlTypes.SqlNullValueException) { }
                    try { OtherPolicy.InsuranceAddress.RecipientName = reader.GetString(reader.GetOrdinal("INS2_NAME")); }
                    catch (System.Data.SqlTypes.SqlNullValueException) { }
                    try { OtherPolicy.InsuranceAddress.Street1 = reader.GetString(reader.GetOrdinal("INS2_Street")); }
                    catch (System.Data.SqlTypes.SqlNullValueException) { }
                    try { OtherPolicy.InsuranceAddress.City = reader.GetString(reader.GetOrdinal("INS2_City")); }
                    catch (System.Data.SqlTypes.SqlNullValueException) { }
                    try { OtherPolicy.InsuranceAddress.State = reader.GetString(reader.GetOrdinal("INS2_State")); }
                    catch (System.Data.SqlTypes.SqlNullValueException) { }
                    try { OtherPolicy.InsuranceAddress.Zip = reader.GetString(reader.GetOrdinal("INS2_Zip")); }
                    catch (System.Data.SqlTypes.SqlNullValueException) { }

                    if (OtherPolicy.SubscriberEmployer.Address == ",") { OtherPolicy.SubscriberEmployer.Address = string.Empty; }
                }
                else
                    OtherPolicy = null;

                
                

                // Special override of some billing dentist information
                if (claimObject.CheckForAddressOverride)
                {
                    C_DentalClaimTracker.dentrix_providers dp = new C_DentalClaimTracker.dentrix_providers();
                    dp = dp.GetProvider(claimObject.override_address_provider);

                    try { BillingDentist.ID = C_DentalClaimTracker.CommonFunctions.DBNullToString(dp["URSCID"]); }
                    catch { }
                    // Billing Dentist Information
                    BillingDentist.Name = dp.NAME_FIRST + " " + dp.NAME_INITIAL + " " + dp.NAME_LAST + " " + dp.NAME_TITLE;
                    BillingDentist.NPINumber = dp.ADA;

                    BillingDentist.PhoneNumber = dp.PHONE;
                    BillingDentist.TIN = dp.ADA;
                    BillingDentist.Address.Street1 = dp.STREET;
                    BillingDentist.Address.Street2 = dp.STREET2;
                    BillingDentist.Address.City = dp.CITY;
                    BillingDentist.Address.State = dp.STATE;
                    BillingDentist.Address.Zip = dp.ZIP;
                    BillingDentist.License = dp.STATEID;
                }
                else
                {
                    // Billing Dentist Information
                    try { BillingDentist.Name = reader.GetString(reader.GetOrdinal("PROV_FIRSTNAME")) + " "; }
                    catch (System.Data.SqlTypes.SqlNullValueException) { }
                    try { BillingDentist.Name += reader.GetString(reader.GetOrdinal("PROV_MIDDLENAME")) + " "; }
                    catch (System.Data.SqlTypes.SqlNullValueException) { }
                    try { BillingDentist.Name += reader.GetString(reader.GetOrdinal("PROV_LASTNAME")) + " "; }
                    catch (System.Data.SqlTypes.SqlNullValueException) { }
                    try { BillingDentist.Name += reader.GetString(reader.GetOrdinal("PROV_TITLE")).Trim(); }
                    catch (System.Data.SqlTypes.SqlNullValueException) { }
                    try { BillingDentist.NPINumber = reader.GetString(reader.GetOrdinal("NPI")); }
                    catch (System.Data.SqlTypes.SqlNullValueException) { }


                    try { BillingDentist.PhoneNumber = Utilities.FormatPhoneNumber(reader.GetString(reader.GetOrdinal("CLINIC_PHONE"))); }
                    catch (System.Data.SqlTypes.SqlNullValueException) { }
                    try { BillingDentist.ID = reader.GetInt32(reader.GetOrdinal("PROV_ID")).ToString(); }
                    catch (System.Data.SqlTypes.SqlNullValueException) { }


                    try { BillingDentist.TIN = reader.GetString(reader.GetOrdinal("PROV_TIN")); }
                    catch (System.Data.SqlTypes.SqlNullValueException) { }
                    try { BillingDentist.Address.Street1 = reader.GetString(reader.GetOrdinal("CLINIC_STREET1")); }
                    catch (System.Data.SqlTypes.SqlNullValueException) { }
                    try { BillingDentist.Address.Street2 = reader.GetString(reader.GetOrdinal("CLINIC_STREET2")); }
                    catch (System.Data.SqlTypes.SqlNullValueException) { }
                    try { BillingDentist.Address.City = reader.GetString(reader.GetOrdinal("CLINIC_CITY")); }
                    catch (System.Data.SqlTypes.SqlNullValueException) { }
                    try { BillingDentist.Address.State = reader.GetString(reader.GetOrdinal("CLINIC_STATE")); }
                    catch (System.Data.SqlTypes.SqlNullValueException) { }
                    try { BillingDentist.Address.Zip = reader.GetString(reader.GetOrdinal("CLINIC_ZIP")); }
                    catch (System.Data.SqlTypes.SqlNullValueException) { }
                    try { BillingDentist.License = reader.GetString(reader.GetOrdinal("PROV_LICENSE")); }
                    catch (System.Data.SqlTypes.SqlNullValueException) { }
                }


                
                try { AncillaryData.PlaceOfTreatment = (Place)reader.GetInt16(reader.GetOrdinal("CLAIM_PLACE")); }
                catch (System.Data.SqlTypes.SqlNullValueException) { }
                try { AncillaryData.NumRadiographsEnclosed = reader.GetInt32(reader.GetOrdinal("CLAIM_NUM_RADIOGRAPHS")); }
                catch (System.Data.SqlTypes.SqlNullValueException) { }
                try { AncillaryData.Orthodontics.TreatmentForOrthodontics = (reader.GetInt16(reader.GetOrdinal("CLAIM_FOR_ORTHO")) == 1) ? true : false; }
                catch (System.Data.SqlTypes.SqlNullValueException) { }
                try { AncillaryData.Orthodontics.DateAppliancePlaced = reader.GetDateTime(reader.GetOrdinal("CLAIM_ORTHO_APPL_PLACED")).ToString("MM/dd/yyyy").Replace("03/02/1753", ""); }
                catch (System.Data.SqlTypes.SqlNullValueException) { }
                try { AncillaryData.Orthodontics.RemainingMonths = reader.GetInt16(reader.GetOrdinal("CLAIM_ORTHO_REMAINING")); }
                catch (System.Data.SqlTypes.SqlNullValueException) { }
                try { AncillaryData.Prosthesis.InitialReplacement = reader.GetInt16(reader.GetOrdinal("CLAIM_PROTH_INIT_REPL")).ToString(); }
                catch (System.Data.SqlTypes.SqlNullValueException) { }
                switch (AncillaryData.Prosthesis.InitialReplacement)
                {
                    case "1": AncillaryData.Prosthesis.InitialReplacement = "Yes"; break;
                    case "2": AncillaryData.Prosthesis.InitialReplacement = "No"; break;
                    default: AncillaryData.Prosthesis.InitialReplacement = string.Empty; break;
                }
                try { AncillaryData.Prosthesis.ReasonForReplacement = reader.GetString(reader.GetOrdinal("CLAIM_PROTH_REASON")); }
                catch (System.Data.SqlTypes.SqlNullValueException) { }
                try { AncillaryData.Prosthesis.PriorPlacement = reader.GetDateTime(reader.GetOrdinal("CLAIM_PROTH_PRIOR_DATE")).ToString("MM/dd/yyyy").Replace("03/02/1753", ""); }
                catch (System.Data.SqlTypes.SqlNullValueException) { }
                
                try { AncillaryData.TreatmentResultingFrom = (reader.GetInt16(reader.GetOrdinal("CLAIM_OCCUP_INJ")) == 1) ? true : false; } catch (System.Data.SqlTypes.SqlNullValueException) {}
                
                try { AncillaryData.Description = reader.GetDateTime(reader.GetOrdinal("CLAIM_OCCUP_DATE")).ToString("MM/dd/yyyy").Replace("03/02/1753", "") + " "; } catch (System.Data.SqlTypes.SqlNullValueException) {}
                try { AncillaryData.Description += reader.GetString(reader.GetOrdinal("CLAIM_OCCUP_DESC")); }
                catch (System.Data.SqlTypes.SqlNullValueException) { }

                AncillaryData.Description = AncillaryData.Description.Trim();
                
                try { BillingDentist.Accident.Type = (AccidentType)reader.GetInt16(reader.GetOrdinal("CLAIM_ACCIDENT")); }
                catch (System.Data.SqlTypes.SqlNullValueException) { }
                try { BillingDentist.Accident.Description = reader.GetDateTime(reader.GetOrdinal("CLAIM_ACCIDENT_DATE")).ToString("MM/dd/yyyy").Replace("03/02/1753", "") + " "; }
                catch (System.Data.SqlTypes.SqlNullValueException) { }
                try { BillingDentist.Accident.Description += reader.GetString(reader.GetOrdinal("CLAIM_ACCIDENT_DESC")); }
                catch (System.Data.SqlTypes.SqlNullValueException) { }
                BillingDentist.Accident.Description = BillingDentist.Accident.Description.Trim();

                // Treatment Information.
                try { TreatmentInformation.TotalFee = Utilities.FormatCurrencyForXML(reader.GetInt32(reader.GetOrdinal("CLAIM_TOTAL_BILLED"))); }
                catch (System.Data.SqlTypes.SqlNullValueException) { }
                try { TreatmentInformation.PaymentByOtherPlan = Utilities.FormatCurrencyForXML(reader.GetInt32(reader.GetOrdinal("CLAIM_OTHER_TOTAL_RECEIVED"))); }
                catch (System.Data.SqlTypes.SqlNullValueException) { }

                // Add the node with secondary information (only valid on secondaries)
                if ((Identity.Type == ClaimType.Secondary) && (C_DentalClaimTracker.system_options.EclaimsSecondaryShowExtraInfo))
                {

                    int primaryPayAmount = 0;

                    // Seondary Information
                    try { SecondaryInfo.AdjudicationDate = reader.GetDateTime(reader.GetOrdinal("PRIMARYCLAIM_DATE_RECEIVED")).ToString("MM/dd/yyyy").Replace("03/02/1753", "") + " "; } 
                    catch (System.Data.SqlTypes.SqlNullValueException) { }

                    try
                    {
                        primaryPayAmount = reader.GetInt32(reader.GetOrdinal("PRIMARYCLAIM_TOTAL_RECEIVED"));

                    }
                    catch (System.Data.SqlTypes.SqlNullValueException)
                    {
                        primaryPayAmount = 0;
                    }

                    SecondaryInfo.PrimaryPaidAmount = Utilities.FormatCurrencyForXML(primaryPayAmount);

                    int totalBilled;
                    try { totalBilled = reader.GetInt32(reader.GetOrdinal("PRIMARYCLAIM_TOTAL_BILLED")); }
                    catch (System.Data.SqlTypes.SqlNullValueException) { totalBilled = 0; }

                    SecondaryInfo.PatientResponsibleAmount = Utilities.FormatCurrencyForXML(totalBilled - primaryPayAmount);
                    
                }
            }
            catch (Exception ex)
            {
                reader.Close();
                throw ex;
            }

			reader.Close();

		}


		// Get individual treatments.
		private void GetClaimTransactions(SqlCommand cmd) {
			SqlDataReader reader = cmd.ExecuteReader();
			Treatment t;
			while (reader.Read()) {
				t = new Treatment();
                try
                {
                    if (Identity.Type == ClaimType.Predeterm)
                    {
                        t.ProcedureDate = "";
                    }
                    else
                    {
                        t.ProcedureDate = reader.GetDateTime(reader.GetOrdinal("DATE")).ToString("MM/dd/yyyy");
                    }
                }
                catch (System.Data.SqlTypes.SqlNullValueException) { }
				try { t.Tooth = C_DentalClaimTracker.procedure.ToothToLetter(reader.GetInt32(reader.GetOrdinal("TOOTH"))).ToString(); } catch (System.Data.SqlTypes.SqlNullValueException) {}
                try { t.ToothEnd = C_DentalClaimTracker.procedure.ToothToLetter(reader.GetInt32(reader.GetOrdinal("TOOTH_END"))).ToString(); } catch (System.Data.SqlTypes.SqlNullValueException) {}
                
                try { t.Surface = C_DentalClaimTracker.CommonFunctions.RemoveNonPrintableCharacters(reader.GetString(reader.GetOrdinal("SURFACE"))); }
                catch (System.Data.SqlTypes.SqlNullValueException) { }
                catch
                {
                    // Let this slide
                    t.Surface = "";
                }
				try { t.DiagnosisIndex = reader.GetString(reader.GetOrdinal("DIAGNOSIS_INDEX")); } catch (System.Data.SqlTypes.SqlNullValueException) {} catch (InvalidCastException) {}
                try
                {
                    string procCode = reader.GetString(reader.GetOrdinal("PROCEDURE_CODE"));

                    if (procCode.Length > 5)
                        procCode = procCode.Substring(0, 5);

                    t.ProcedureCode = procCode;
                }
                catch (System.Data.SqlTypes.SqlNullValueException) {
                    t.ProcedureCode = "PAYMENT";
                }
				try { t.Quantity = reader.GetInt32(reader.GetOrdinal("QUANTITY")).ToString(); } catch (System.Data.SqlTypes.SqlNullValueException) {} catch (InvalidCastException) {}
				try { t.Description = reader.GetString(reader.GetOrdinal("DESCRIPTION")); } catch (System.Data.SqlTypes.SqlNullValueException) {}
				try { t.Fee = Utilities.FormatCurrencyForXML(reader.GetInt32(reader.GetOrdinal("FEE"))); } catch (System.Data.SqlTypes.SqlNullValueException) {}

                // Add the node with secondary information (only valid on secondaries)
                if (Identity.Type == ClaimType.Secondary)
                {

                    int primaryPayAmount = 0;

                    // Seondary Information
                    try { t.SecondaryDate = SecondaryInfo.AdjudicationDate; } 
                    catch { }

                    try
                    {
                        try { primaryPayAmount = reader.GetInt32(reader.GetOrdinal("AMTPINSPAID")); }
                        catch { primaryPayAmount = 0; }
                        t.PrimaryPaidAmount = Utilities.FormatCurrencyForXML(primaryPayAmount);
                    }
                    catch (System.Data.SqlTypes.SqlNullValueException) { }

                    
                        try
                        {
                            int totalBilled = reader.GetInt32(reader.GetOrdinal("FEE"));
                            t.PatientResponsibleAmount = Utilities.FormatCurrencyForXML(totalBilled - primaryPayAmount);    
                        }
                        catch (System.Data.SqlTypes.SqlNullValueException) { }
                }

				TreatmentInformation.Treatments.Add(t);
			}
			reader.Close();
			if (TreatmentInformation.Treatments.Count < 1) {
				throw new Exception("No treatments found.");
			}
		}

        /// <summary>
        /// A bit of a hack, but this puts both tooth and toothend into the "tooth" field 
        /// since the PDF reader only reads from the tooth field
        /// </summary>
        internal void PutToothStringInTooth()
        {
            foreach(Treatment t in this.TreatmentInformation.Treatments)
            {
                string toothString = t.Tooth;

                if (t.Tooth != t.ToothEnd)
                    toothString += "-" + t.ToothEnd;

                t.Tooth = toothString;
            }
        }

        /// <summary>
        /// Removes some specific text from the address fields (any address starting with "BILLING ENTITY NPI" blah blah
        /// </summary>
        internal void CleanAddresses()
        {
            if (OtherPolicy != null)
                CleanAddress(this.OtherPolicy.InsuranceAddress);

            if (BillingDentist != null)
            {
                CleanAddress(BillingDentist.Address);
                CleanAddress(BillingDentist.TreatmentLocation);
            }

            if (GeneralInformation != null)
            {
                CleanAddress(this.GeneralInformation.TreatmentLocation);
                CleanAddress(this.GeneralInformation.Carrier.Address);
            }

            if (Patient != null)
                CleanAddress(Patient.Address);

            if (Subscriber != null)
                CleanAddress(Subscriber.Address);
        }

        /// <summary>
        /// Removes the string "billing entity NPI " and any other information from a specific address
        /// </summary>
        /// <param name="address"></param>
        private void CleanAddress(Address address)
        {
            if (address != null)
            {
                try { address.Street1 = CleanStreet(address.Street1); }
                catch { }
                try { address.Street2 = CleanStreet(address.Street2); }
                catch { }
            }
        }

        private string CleanStreet(string p)
        {
            string toReturn = string.Empty;
            foreach (string aLine in p.Split("\r\n".ToCharArray()))
            {
                if (!aLine.Contains("Billing Entity"))
                {
                    if (toReturn != string.Empty)
                        toReturn += "\r\n";
                    
                    toReturn += aLine;
                }
            }

            return toReturn;
        }

    }


	/// <summary>Represents a address.</summary>
	public class Address {
		/// <summary>Top line of the address - name of recipient.</summary>
        public string RecipientName = string.Empty;
        
        /// <summary>The first line of the Address.</summary>
		public string Street1 = string.Empty;

		/// <summary>The second line of the Address.</summary>
		public string Street2 = string.Empty;

		/// <summary>The city.</summary>
		public string City = string.Empty;

		/// <summary>The state.</summary>
		public string State = string.Empty;

		/// <summary>The zip code.</summary>
		public string Zip = string.Empty;
	}


	/// <summary>Represents the Employer information on a claim.</summary>
	public class Employer {
		/// <summary>The name of the employer.</summary>
		public string Name = string.Empty;

		/// <summary>The short address of the employer.</summary>
		public string Address = string.Empty;
	}


	/// <summary>Represents a carrier.</summary>
	public class Carrier {
		/// <summary>The carrier's name.</summary>
		public string Name = string.Empty;

		/// <summary>The carrier's address.</summary>
		public Address Address = new Address();
	}


	/// <summary>Contains identity information about the claim.</summary>
	public class Identity {
		/// <summary>The ID of the claim in Dentrix.</summary>
		public int ClaimID = -1;

		/// <summary>The Database ID of the claim in Dentrix.</summary>
		public int ClaimDB = -1;

		/// <summary>The type of the claim.</summary>
		public ClaimType Type = ClaimType.Primary;

		/// <summary>The ID of the other claim in Dentrix.</summary>
		public int OtherClaimID = -1;

		/// <summary>The Database ID of the other claim in Dentrix.</summary>
		public int OtherClaimDB = -1;

	}


	/// <summary>Holds general information about the claim.</summary>
	public class ClaimInfo {
		/// <summary>The date the claim was generated.</summary>
		public string Date = string.Empty;

		/// <summary>The intent of the claim.</summary>
		public string Purpose = ClaimPurpose.Statement.ToString();

		/// <summary>The type of claim.</summary>
		public SpecialClaimType Type = SpecialClaimType.Normal;

		/// <summary>Specialty.</summary>
		public string Specialty = string.Empty;

		/// <summary>Prior authorization number.</summary>
		public string PriorAuthorization = string.Empty;

		/// <summary>The carrier the claim is going to.</summary>
		public Carrier Carrier = new Carrier();

		/// <summary>Address where the treatment was performed.</summary>
		public Address TreatmentLocation = new Address();

        /// <summary>ID of the treatment clinic</summary>
        public int ClinicAppliedTo = 0;

        /// <summary>ID of the the other insured party</summary>
        public int OtherInsuredPartyID = 0;

        /// <summary>Billing NPI, hard coded for now </summary>
        public string BillingNPI = "";
	}


	/// <summary>Represents the patient the claim is for.</summary>
	public class Patient {
		/// <summary>The ID of the patient (chart number).</summary>
		public string ID = string.Empty;

		/// <summary>The patient's name.</summary>
		public string Name = string.Empty;

        /// <summary>19. Student status</summary>
        public string StudentStatus = string.Empty;

		/// <summary>The patient's address.</summary>
		public Address Address = new Address();

		/// <summary>The patient's birth day.</summary>
		public string BirthDate = string.Empty;

		/// <summary>The sex of the patient.</summary>
		public Gender Sex = Gender.Unknown;

		/// <summary>The patient's phone number.</summary>
		public string PhoneNumber = string.Empty;

		/// <summary>The patient's employer.</summary>
		public Employer Employer = new Employer();

		/// <summary>The relationship this patient has with the subscriber.</summary>
		public Relationship SubscriberRelationship = Relationship.Self;
	}


	/// <summary>Represents the subscriber of the insurance that the claim is going to.</summary>
	public class Subscriber {
		/// <summary>The subscriber's ID number.</summary>
		public string ID = string.Empty;

		/// <summary>The subscriber's employer.</summary>
		public Employer Employer = new Employer();

		/// <summary>The subscriber's group policy number.</summary>
		public string GroupNumber = string.Empty;

		/// <summary>The subscriber's name.</summary>
		public string Name = string.Empty;

		/// <summary>The subscriber's address.</summary>
		public Address Address = new Address();

		/// <summary>The subscriber's phone number.</summary>
		public string PhoneNumber = string.Empty;

		/// <summary>The subscriber's birth day.</summary>
		public string BirthDate = string.Empty;

		/// <summary>The marital status of the subscriber.</summary>
		public MaritalStatus MaritalStatus = MaritalStatus.Single;

		/// <summary>The gender of the subscriber.</summary>
		public Gender Sex = Gender.Unknown;

		/// <summary>The employment status of the subscriber.</summary>
		public EmploymentStatus EmploymentStatus = EmploymentStatus.EmployedFullTime;
	}


	/// <summary>Represents the patient's other policy.</summary>
	public class OtherPolicy {
		/// <summary>The type of insurance this is.</summary>
		public InsuranceType Type = InsuranceType.Dental;

		/// <summary>The policy number.</summary>
		public string PolicyNumber = string.Empty;

		/// <summary>The name of the plan/program/carrier.</summary>
		public string PlanName = string.Empty;

		/// <summary>The name of the subscriber.</summary>
		public string SubscriberName = string.Empty;

		/// <summary>The subscriber's birth day.</summary>
		public string SubscriberBirthDate = string.Empty;

		/// <summary>The subscriber's gender.</summary>
		public Gender SubscriberSex = Gender.Unknown;

		/// <summary>The subscriber's employer.</summary>
		public Employer SubscriberEmployer = new Employer();

        /// <summary>8. Policyholder/subscriberID</summary>
        public string SubscriberID = string.Empty;

        public Address SubscriberAddress = new Address();
        
        private string _patientRelationship = string.Empty;
        
        /// <summary>10. Patient's Relationship to Person Named in #5</summary>
        public string PatientRelationship
        {
            get { return _patientRelationship; }
            set
            {
                switch (value)
                {
                    case "0":
                        _patientRelationship = "";
                        break;
                    case "1":
                        _patientRelationship = "Self";
                        break;
                    case "2":
                        _patientRelationship = "Spouse";
                        break;
                    case "3":
                        _patientRelationship = "Child";
                        break;
                    case "4":
                        _patientRelationship = "Other";
                        break;
                    default:
                        _patientRelationship = value;
                        break;
                }

            }
        }

        /// <summary>11. Other Insurance Company/Dental Benefit plan name, address, city, state, zip code</summary>
        public Address InsuranceAddress = new Address();
	}


	/// <summary>Represents the dentist that is billing the carrier.</summary>
	public class BillingDentist {
		/// <summary>The dentist's name.</summary>
		public string Name = string.Empty;

		/// <summary>The dentist's phone number.</summary>
		public string PhoneNumber = string.Empty;

		/// <summary>The dentist's ID.</summary>
		public string ID = string.Empty;

		/// <summary>The dentist's TIN.</summary>
		public string TIN = string.Empty;

		/// <summary>The dentist's address.</summary>
		public Address Address = new Address();

		/// <summary>The dentist's license number.</summary>
		public string License = string.Empty;

		/// <summary>The first date of this series of treatment.</summary>
		public string FirstDateOfSeries = string.Empty;

		/// <summary>Occupational cause information about the treatment.</summary>
		public Occupational Occupational = new Occupational();

		/// <summary>Accidental cause information about the treatment.</summary>
		public Accident Accident = new Accident();

        /// <summary>///53. Signature Date /// </summary>
        public string SignatureDate = string.Empty;
        
        /// <summary>///56. Address City State Zip  /// </summary>
        public Address TreatmentLocation = new Address();

        /// <summary>///57. Phone Number/// </summary>
        public string TreatmentPhoneNumber = string.Empty;

        /// <summary>///57. NPI Number/// </summary>
        public string NPINumber = string.Empty;

        

        public BillingDentist()
        {
            // Set address to fixed address
            Address.RecipientName = "New Haven Dental Group";
            Address.Street1 = "123 York St.";
            Address.Street2 = "St. 4L";
            Address.State = "CN";
            Address.City = "New Haven";
            Address.Zip = "06511";
        }
	}


	/// <summary>Orthodontic information.</summary>
	public class Orthodontics {
		/// <summary>Is this claim part of a treatment for orthodontics?</summary>
		public bool TreatmentForOrthodontics = false;

		/// <summary>The date the appliance was placed.</summary>
		public string DateAppliancePlaced = string.Empty;

		/// <summary>Number of remaining months in treatment.</summary>
		public int RemainingMonths = 0;
	}

    /// <summary>
    /// Ancillary Data, items 42-47 on the form
    /// </summary>
    public class AncillaryData
    {
        /// <summary>38. The type of place this treatment occurred in.</summary>
        public Place PlaceOfTreatment = Place.Office;

        /// <summary>39. The number of radiographs enclosed with the claim.</summary>
        public int NumRadiographsEnclosed = 0;


        /// <summary>45. Treatment resulting from</summary>
        public bool TreatmentResultingFrom = false;

        /// <summary>46. Date of Accident</summary>
        public string DateOfAccident = string.Empty;

        /// <summary>47. Accident State</summary>
        public string AccidentState = string.Empty;

        /// <summary>Orthodontic information about the treatment.</summary>
        public Orthodontics Orthodontics = new Orthodontics();

        /// <summary>Prosthesis related information </summary>
        public Prosthesis Prosthesis = new Prosthesis();

        /// <summary>Description of treatment</summary>
        public string Description = string.Empty;
    }


	/// <summary>Prosthesis information.</summary>
	public class Prosthesis {
		/// <summary>Is this the initial replacement?</summary>
		public string InitialReplacement = string.Empty;

		/// <summary>Reason for replacement.</summary>
		public string ReasonForReplacement = string.Empty;

		/// <summary>Date of the prior placement.</summary>
		public string PriorPlacement = string.Empty;

	}


	/// <summary>Occupational cause information.</summary>
	public class Occupational {
		
	}


	/// <summary>Accidental cause information.</summary>
	public class Accident {
		/// <summary>What type of accident caused this treatment.</summary>
		public AccidentType Type = AccidentType.Neither;

		/// <summary>A description of the accident.</summary>
		public string Description = string.Empty;
	}


	/// <summary>Information about the treatments being claimed.</summary>
	public class TreatmentInformation {
		/// <summary>Total charge for the treatment(s).</summary>
		public string TotalFee = string.Empty;

		/// <summary>Amount paid by patient's other plan.</summary>
		public string PaymentByOtherPlan = string.Empty;

		/// <summary>The list of treatments.</summary>
		[XmlArrayItem(typeof(Treatment))]
		public ArrayList Treatments = new ArrayList();

		/// <summary>Remarks describing any unusual services.</summary>
		public string RemarksForUnusualServices = string.Empty;
	}

    /// <summary>
    /// Information pertinent only to secondary claims
    /// </summary>
    public class SecondaryInformation
    {
        /// <summary>The date the primary was paid.</summary>
        public string AdjudicationDate = string.Empty;

        /// <summary>The amount paid on the primary.</summary>
        public string PrimaryPaidAmount = string.Empty;

        /// <summary>The amount remaining for the patient to pay.</summary>
        public string PatientResponsibleAmount = string.Empty;

        /// <summary>Subtract primary paid and patient responsible amount from total line amount (?)/summary>
        public string AdjustedAmount = string.Empty;
    }


	/// <summary>Represents an treatment/payment entry on the claim.</summary>
	public class Treatment {
		/// <summary>The date the treatment/payment occurred.</summary>
		public string ProcedureDate = string.Empty;

		/// <summary>The tooth involved with the procedure</summary>
		public string Tooth = string.Empty;

        /// <summary>The last tooth involved with the procedure</summary>
        public string ToothEnd = string.Empty;

		/// <summary>The surfaces affected on the tooth.</summary>
		public string Surface = string.Empty;

		/// <summary>The diagnosis index number.</summary>
		public string DiagnosisIndex = string.Empty;

		/// <summary>The ADA code for the procedure.</summary>
		public string ProcedureCode = string.Empty;

		/// <summary>The quantity.</summary>
		public string Quantity = string.Empty;

		/// <summary>A description of the treatment/payment.</summary>
		public string Description = string.Empty;

		/// <summary>The amount charged or received for the treatment/payment.</summary>
		public string Fee = string.Empty;

        /// <summary>The date the secondary payment was received</summary>
        public string SecondaryDate = string.Empty;

        /// <summary>The amount paid on the primary claim</summary>
        public string PrimaryPaidAmount = string.Empty;

        /// <summary>The amount the patient is responsible for</summary>
        public string PatientResponsibleAmount = string.Empty;
	}


	/// <summary>The gender of a person.</summary>
	public enum Gender {
		/// <summary>Unknown</summary>
		Unknown,

		/// <summary>Male</summary>
		Male,

		/// <summary>Female</summary>
		Female
	}


	/// <summary>The relationship a person has with another.</summary>
	public enum Relationship {
		/// <summary>No relationship.</summary>
		None,

		/// <summary>The same person.</summary>
		Self,

		/// <summary>The other's spouse.</summary>
		Spouse,

		/// <summary>The other's child.</summary>
		Child,

		/// <summary>Other type of relationship.</summary>
		Other
	}


	/// <summary>A person's marital status.</summary>
	public enum MaritalStatus {
		/// <summary>No status.</summary>
		None,

		/// <summary>The person is single.</summary>
		Single,

		/// <summary>The person is married.</summary>
		Married,

		/// <summary>The person is a child.</summary>
		Child,

		/// <summary>The person is in some other status.</summary>
		Other
	}


	/// <summary>A person's employment status</summary>
	public enum EmploymentStatus {
		/// <summary>Employed full time.</summary>
		EmployedFullTime,

		/// <summary>Employed part time.</summary>
		EmployedPartTime,

		/// <summary>In school full time.</summary>
		SchoolFullTime,

		/// <summary>In school part time.</summary>
		SchoolPartTime
	}


	/// <summary>The place a treatment took place.</summary>
	public enum Place {
		/// <summary>A dental office.</summary>
		Office,

		/// <summary>A hospital.</summary>
		Hospital,

		/// <summary>ECF.</summary>
		ECF,

		/// <summary>Some other location.</summary>
		Other
	}


	/// <summary>The type of accident.</summary>
	public enum AccidentType {
		/// <summary>No accident.</summary>
		Neither,

		/// <summary>An automobile accident.</summary>
		Automobile,

		/// <summary>Some other type of accident.</summary>
		Other
	}


	/// <summary>The purpose of a claim.</summary>
	public enum ClaimPurpose {
		/// <summary>Sends statement to carrier with hopes of payment.</summary>
		Statement,

		/// <summary>Sends treatment plan to carrier to determine payout.</summary>
		Estimate
	}


	/// <summary>The type of claim.</summary>
	public enum SpecialClaimType {
		/// <summary>Normal dental insurance claim.</summary>
		Normal,

		/// <summary>A Medicaid Claim.</summary>
		Medicaid,

		/// <summary>EPSDT.</summary>
		EPSDT
	}


	/// <summary>The type of relationship a person has with a carrier that the claim is going to.</summary>
	public enum ClaimType {
		/// <summary>Claim to Primary insurance carrier.</summary>
		Primary,

		/// <summary>Claim to Secondary insurance carrier.</summary>
		Secondary,

        Predeterm
	}


	/// <summary>The type of insurance a carrier handles.</summary>
	public enum InsuranceType {
		/// <summary>Carrier handles dental insurance.</summary>
		Dental,

		/// <summary>Carrier handles medical insurance.</summary>
		Medical
	}
}
