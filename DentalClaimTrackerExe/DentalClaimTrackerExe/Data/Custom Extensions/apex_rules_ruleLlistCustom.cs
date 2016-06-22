using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

namespace C_DentalClaimTracker
{
    partial class apex_rules_rulelist : DataObject
    {
        List<apex_rules_rulelist> allRules = null;

        internal void AddProcedureCodes(System.Windows.Forms.ListBox.ObjectCollection objectCollection)
        {
            apex_rules_procedure_codes arpc;
            foreach (string toAdd in objectCollection)
            {
                arpc = new apex_rules_procedure_codes();
                arpc.rule_id = id;
                arpc.procedure_code = toAdd;
                arpc.Save();
            }
        }

        public void AddInsuranceCompanyFilter(System.Windows.Forms.ListBox.ObjectCollection companyFilters)
        {
            apex_rules_companies arc;
            foreach (string toAdd in companyFilters)
            {
                arc = new apex_rules_companies();
                arc.rule_id = id;
                arc.company_info = toAdd;
                arc.Save();
            }
        }

        public void DeleteAllLinkedProcedureCodes()
        {
            apex_rules_procedure_codes arpc = new apex_rules_procedure_codes();
            ExecuteNonQuery("DELETE FROM apex_rules_procedure_codes WHERE rule_id = " + id);
        }

        public void DeleteAllLinkedInsuranceCompanies()
        {
            apex_rules_companies arc = new apex_rules_companies();
            ExecuteNonQuery("DELETE FROM apex_rules_companies WHERE rule_id = " + id);
        }

        /// <summary>
        /// Deletes linked insurance companies and procedures and conveniently checks for a null on id so
        /// this can be run on newly instantiated objects
        /// </summary>
        public void DeleteAllLinkedData()
        {
            if (CommonFunctions.DBNullToZero(this["id"]) > 0)
            {
                DeleteAllLinkedInsuranceCompanies();
                DeleteAllLinkedProcedureCodes();
            }
        }



        internal List<apex_rules_rulelist> GetAllRules(bool forceRefresh)
        {
            if ((allRules == null) || (forceRefresh))
            {
                apex_rules_rulelist arr = new apex_rules_rulelist();
                allRules = new List<apex_rules_rulelist>();
                DataTable allData = arr.GetAllData("priority");

                foreach (DataRow aRow in allData.Rows)
                {
                    arr = new apex_rules_rulelist();
                    arr.Load(aRow);

                    allRules.Add(arr);
                }
            }
                
            return allRules;
        }

        public List<apex_rules_companies> LinkedInsurance
        {
            get
            {
                List<apex_rules_companies> toReturn = new List<apex_rules_companies>();
                apex_rules_companies arc;
                DataTable matches = Search("SELECT arc.* FROM apex_rules_companies arc " +
                "WHERE rule_id = " + id + " ORDER BY company_info");

                foreach (DataRow aMatch in matches.Rows)
                {
                    arc = new apex_rules_companies();
                    arc.Load(aMatch);
                    toReturn.Add(arc);
                }

                return toReturn;
            }
        }

        public string LinkedInsuranceAsString
        {
            get
            {
                string toReturn = "";

                foreach (apex_rules_companies aCompany in LinkedInsurance)
                {
                    toReturn += aCompany.company_info + ", ";
                }

                if (toReturn != "")
                {
                    toReturn = toReturn.Trim();
                    toReturn = toReturn.Substring(0, toReturn.Length - 1);
                }

                return toReturn;
            }
        }

        public List<apex_rules_procedure_codes> LinkedProcedureCodes
        {
            get
            {
                List<apex_rules_procedure_codes> toReturn = new List<apex_rules_procedure_codes>();
                apex_rules_procedure_codes arpc;
                DataTable matches = Search("SELECT arpc.* FROM apex_rules_procedure_codes arpc " +
                "WHERE rule_id = " + id + " ORDER BY procedure_code"); 

                foreach (DataRow aMatch in matches.Rows)
                {
                    arpc = new apex_rules_procedure_codes();
                    arpc.Load(aMatch);
                    toReturn.Add(arpc);
                }

                return toReturn;
            }
        }

        public string LinkedProcedureCodesAsString
        {
            get
            {
                string toReturn = "";

                foreach (apex_rules_procedure_codes aProc in LinkedProcedureCodes)
                {
                    toReturn += aProc.procedure_code + ", ";
                }

                if (toReturn != "")
                {
                    toReturn = toReturn.Trim();
                    toReturn = toReturn.Substring(0, toReturn.Length - 1);
                }

                return toReturn;
            }
        }


        internal bool Applies(claim toCheck)
        {
            bool isValid = true;


            if (toCheck.claim_type == claim.ClaimTypes.Primary)
                isValid = apply_primary;
            else if (toCheck.claim_type == claim.ClaimTypes.Secondary)
                isValid = apply_secondary;
            else if (toCheck.claim_type == claim.ClaimTypes.Predeterm)
                isValid = apply_predeterm;
            else
                isValid = false;

            if (isValid)
            {
                string procedureWHERE = "";
                string insuranceWHERE = "";

                List<apex_rules_procedure_codes> allProcs = LinkedProcedureCodes;
                if (allProcs.Count > 0)
                {
                    procedureWHERE = "AND (";
                    foreach(apex_rules_procedure_codes arpc in allProcs)
                    {
                        procedureWHERE += " p.ada_code LIKE '" + arpc.procedure_code + "' OR";
                    }

                    procedureWHERE = procedureWHERE.Substring(0, procedureWHERE.Length - 2); // Remove final "OR"
                        procedureWHERE+= ")";
                }

                List<apex_rules_companies> allCompanies = LinkedInsurance;
                if (allCompanies.Count > 0)
                {
                    insuranceWHERE = "AND (";
                    foreach (apex_rules_companies arc in allCompanies)
                    {
                        insuranceWHERE += " co.name LIKE '" + arc.company_info + "' OR";
                    }

                    insuranceWHERE = insuranceWHERE.Substring(0, insuranceWHERE.Length - 2); // Remove final "OR"
                    insuranceWHERE += ")";
                }

                string sql = "SELECT * FROM claims c INNER JOIN companies co ON c.company_id = co.id " +
                    "INNER JOIN procedures p ON c.id = p.claim_id " +
                    "WHERE c.id = " + toCheck.id + " " + procedureWHERE + " " + insuranceWHERE;

                DataTable check = Search(sql);

                if (check.Rows.Count == 0)
                {
                    isValid = false;
                }

            }


            return isValid;
        }

        internal int GetLastPriority()
        {
            DataTable dt = Search("SELECT MAX(priority) FROM apex_rules_ruleList");

            int index = 10000;
            try
            {
                index = CommonFunctions.DBNullToZero(dt.Rows[0][0]);
            }
            catch
            {
                index = 10000;
            }

            return index;
        }
    }
}
