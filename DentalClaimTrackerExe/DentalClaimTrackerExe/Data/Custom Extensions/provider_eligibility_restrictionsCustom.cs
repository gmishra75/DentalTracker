using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

namespace C_DentalClaimTracker
{
    public partial class provider_eligibility_restrictions : DataObject
    {
        private insurance_company_group _linkedInsuranceCompanyGroup;

        public company LinkedInsurance
        {
            get
            {
                if (CommonFunctions.DBNullToZero(this["insurance_id"]) > 0)
                    return new company((int)this["insurance_id"]);
                else
                    throw new DataObjectException.NoCurrentRecordException();
            }
        }

        public insurance_company_group LinkedInsuranceCompanyGroup
        {
            get
            {
                if (_linkedInsuranceCompanyGroup == null)
                    return new insurance_company_group(insurance_group_id);
                else
                    return _linkedInsuranceCompanyGroup;
            }

            set { _linkedInsuranceCompanyGroup = value; }
        }

        private void thisRecordChanged(object sender, RecordChangedEventArgs e)
        {
            _linkedInsuranceCompanyGroup = null;
        }

        /// <summary>
        /// Searches to see whether a claim should substitute another provider's information based off of the information
        /// in the passed claim.
        /// </summary>
        /// <param name="toSearch">The claim to search for a restriction on</param>
        /// <returns></returns>
        public static claim FindEligibilityRestrictions(claim toSearch)
        {
            return FindEligibilityRestrictions(toSearch, GetAllDataAsList());
        }

        public static List<provider_eligibility_restrictions> GetAllDataAsList(bool enabledOnly = true)
        {
            List<provider_eligibility_restrictions> toReturn = new List<provider_eligibility_restrictions>();
            provider_eligibility_restrictions per = new provider_eligibility_restrictions();
            DataTable dt = per.GetAllData();

            foreach (DataRow aRow in dt.Rows)
            {
                per = new provider_eligibility_restrictions();
                per.Load(aRow);

                if (per.is_enabled || !enabledOnly)
                {
                    var initialize = per.LinkedInsuranceCompanyGroup.LinkedFilters.Count;
                    toReturn.Add(per);
                }
            }

            return toReturn;
        }

        /// <summary>
        /// Searches the given list to see whether a claim should substitute another provider's information based off of the information
        /// in the passed claim.
        /// </summary>
        /// <param name="toSearch">The claim to search for a restriction on</param>
        /// <param name="restrictionsList">The set of restrictions to search through</param>
        /// <returns></returns>
        public static claim FindEligibilityRestrictions(claim toSearch, List<provider_eligibility_restrictions> restrictionsList)
        {
            string newProv = FindEligibilityRestrictions(toSearch.doctor_provider_id, toSearch.LinkedCompany.name, toSearch.date_of_service.Value, restrictionsList, toSearch.LinkedCompanyAddress.state);

            if (newProv != "")
                return FindClaimByProviderID(newProv);
            else
                return null;

        }

        public static string FindEligibilityRestrictions(string provider, string insurance, DateTime date_of_service, string state = "")
        {
            return FindEligibilityRestrictions(provider, insurance, date_of_service, GetAllDataAsList(), state);
        }


        /// <summary>
        /// Compares criteria using regular expressions, but should match perfectly with SQL. Note that if restrictionsList has been fully initialized does not touch the database and will work in 
        /// multi-threading environments
        /// </summary>
        /// <param name="provider"></param>
        /// <param name="insurance"></param>
        /// <param name="date_of_service"></param>
        /// <param name="restrictionsList"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        public static string FindEligibilityRestrictions(string provider, string insurance, DateTime date_of_service, List<provider_eligibility_restrictions> restrictionsList, string state = "")
        {
            string newProvider = string.Empty;

            if (system_options.OverrideStateEnabled)
            {
                // Check to see if this claim should have it's state overriden
                if (system_options.OverrideStateProviderID == "" || system_options.OverrideStateProviderID == provider)
                {
                    // Provider match
                    if (system_options.OverrideStateState == state)
                    {
                        // Full match, switch provider
                        newProvider = system_options.OverrideStateNewProviderID;
                    }
                }
            }

            if (newProvider == string.Empty) // no changes to newProvider made
            {
                foreach (provider_eligibility_restrictions restriction in restrictionsList)
                {
                    bool valid = false;
                    bool checkValid = false;
                    if (restriction.provider_id.IndexOf(provider, StringComparison.CurrentCultureIgnoreCase) >= 0 || restriction.provider_id == "")
                    {
                        if (date_of_service > restriction.start_date)
                        {
                            if (restriction.end_date.HasValue)
                            {
                                if (date_of_service <= restriction.end_date)
                                {
                                    checkValid = true;
                                }
                            }
                            else
                                checkValid = true;
                        }
                    }

                    if (checkValid)
                    {
                        // Update 11-25 to do local comparisons
                        // Archived sql code included below

                        foreach (insurance_company_groups_filters filter in restriction.LinkedInsuranceCompanyGroup.LinkedFilters)
                        {
                            // Starts with %
                            //   -- Closing % - CONTAINS
                            //   -- No closing % - ENDSWITH
                            // End with % - STARTSWITH
                            // No percent - EQUALS
                            // Handle "Not" criteria at the end


                            
                            string filterText = filter.filter_text.Replace("%", "").Replace("NOT ", "");
                            bool isNot = filter.filter_text.StartsWith("NOT ");
                            bool validThisPass = false;
                            
                            if (!valid || isNot)
                            {
                                if (filter.filter_text.StartsWith("%"))
                                {
                                    if (filter.filter_text.EndsWith("%"))
                                    {
                                        if (insurance.IndexOf(filterText, StringComparison.CurrentCultureIgnoreCase) >= 0)
                                            validThisPass = true;
                                    }
                                    else
                                    {
                                        if (insurance.EndsWith(filterText, StringComparison.CurrentCultureIgnoreCase))
                                            validThisPass = true;
                                    }
                                }
                                else if (filter.filter_text.EndsWith("%"))
                                {
                                    if (insurance.StartsWith(filterText, StringComparison.CurrentCultureIgnoreCase))
                                        validThisPass = true;
                                }
                                else
                                {
                                    if (String.Compare(filterText, insurance, true) == 0)
                                        validThisPass = true;
                                }
                            }



                            if (validThisPass)
                            {
                                if (isNot)
                                {
                                    // If this was a valid match but we are actually doing a not comparison, then
                                    // mark invalid and terminate immediately
                                    valid = false;
                                    break;
                                }
                                else
                                    valid = true;
                            }
                        }
                        


                        

                        /*
                        // Check sql for applicability to rules
                        // unfortunately have to create a huge sql statement
                        // Should look roughly like this: 
                        // SELECT id FROM companies WHERE name NOT LIKE '1' AND name NOT LIKE '2' AND (name like '%a%' OR name like 'b')

                        

                        string finalSQL = insurance_company_group.GenerateCompanySQL("name", ConvertEligibilityListToStringArray(restriction.GetLinkedCompanyFilters()));
                        // Look at the criteria and make my own decision about whether it matches


                        claim c = new claim();


                        DataTable matches = c.Search(finalSQL);

                        foreach (DataRow aMatch in matches.Rows)
                            if (aMatch["name"].ToString() == insurance)
                            {
                                valid = true;
                                break;
                            }
                        */
                    }

                    if (valid)
                    {
                        newProvider = restriction.switch_to_provider_id;
                        break;
                    }
                }
            }
            return newProvider;
        }

        private static List<string> ConvertEligibilityListToStringArray(List<insurance_company_groups_filters> toConvert)
        {
            List<string> toReturn = new List<string>();
            foreach (insurance_company_groups_filters pec in toConvert)
                toReturn.Add(pec.filter_text);

            return toReturn;
        }

        private List<insurance_company_groups_filters> GetLinkedCompanyFilters()
        {
            List<insurance_company_groups_filters> toReturn = new List<insurance_company_groups_filters>();
            insurance_company_groups_filters icgf;
            claim c = new claim();
            DataTable dt = c.Search("SELECT * FROM insurance_company_groups_filters WHERE icg_id = " + insurance_group_id);
            foreach (DataRow aMatch in dt.Rows)
            {
                icgf = new insurance_company_groups_filters();
                icgf.Load(aMatch);
                toReturn.Add(icgf);
            }

            return toReturn;
        }

        public static claim FindClaimByProviderID(string ProviderID)
        {
            claim toReturn = new claim();

            DataTable matches = toReturn.Search("SELECT TOP 1 * FROM claims WHERE doctor_provider_id = '" + ProviderID +
                "' ");

            if (matches.Rows.Count > 0)
            {
                toReturn.Load(matches.Rows[0]);

                return toReturn;
            }
            else
                return null;
        }

        internal List<string> GetCompanyCriteria()
        {
            List<string> toReturn = new List<string>();
            claim workingClaim = new claim();

            DataTable matches = workingClaim.Search("SELECT * FROM provider_eligibility_companies WHERE per_id = " + id);

            foreach (DataRow aRow in matches.Rows) {
                toReturn.Add(aRow["restriction_text"].ToString());
            }

            return toReturn;
        }

        /// <summary>
        /// Returns the list of company criteria as a string
        /// </summary>
        /// <returns></returns>
        internal string GetCompanyCriteriaAsString()
        {
            return String.Join(", ", GetCompanyCriteria().ToArray());
        }

        internal void SaveFilters(System.Windows.Forms.ListBox.ObjectCollection filters)
        {
            // Clear the existing filters then add the new ones
            ExecuteNonQuery("DELETE FROM provider_eligibility_companies WHERE per_id = " + id);

            foreach (string aFilter in filters)
            {
                provider_eligibility_companies pec = new provider_eligibility_companies();
                pec.per_id = id;
                pec.restriction_text = aFilter;
                pec.Save();
            }

        }
    }
}
