using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

namespace C_DentalClaimTracker
{
    public partial class insurance_company_group : DataObject
    {
        private List<insurance_company_groups_filters> _linkedFilters;

        /// <summary>
        /// Retrieves the filters and saves them locally so they won't be reloaded
        /// </summary>
        public List<insurance_company_groups_filters> LinkedFilters
        {
            get
            {
                if (_linkedFilters == null)
                {
                    // Load filters
                    return Filters;
                }
                else
                    return _linkedFilters;
            }
            set { _linkedFilters = value; }
        }

        private void thisRecordChanged(object sender, RecordChangedEventArgs e) {
            _linkedFilters = null;
        }

        public string GenerateCompanySQL(string selectColumns, bool oppositeResults = false)
        {
            return GenerateCompanySQL(selectColumns, FiltersTextOnly, oppositeResults);
        }

        public static string GenerateCompanySQL(string selectColumns, List<string> CompanyFilters, bool oppositeResults = false)
        {
            string notWhere = string.Empty;
            string where = string.Empty;
            string finalSQL = "SELECT " + selectColumns + " FROM companies";

            

            foreach (string aFilter in CompanyFilters)
            {
                if (aFilter.StartsWith("NOT "))
                {
                    if (oppositeResults)
                        where += String.Format(" OR name LIKE '{0}'", aFilter.Substring(4));
                    else
                        notWhere += String.Format(" AND name NOT LIKE '{0}'", aFilter.Substring(4));
                }
                else
                {
                    if (oppositeResults)
                        notWhere += String.Format(" AND name NOT LIKE '{0}'", aFilter);
                    else
                        where += String.Format(" OR name LIKE '{0}'", aFilter);
                }
            }

            if ((notWhere != string.Empty) || (where != string.Empty))
                finalSQL += " WHERE ";
            else
                finalSQL += " WHERE 0=1 "; // force 0 matches

            if (notWhere != string.Empty) // Remove initial and
                notWhere = notWhere.Substring(4);

            if (where != string.Empty) // Remove initial or
                where = where.Substring(4);

            finalSQL += notWhere;

            if ((notWhere != string.Empty) && (where != string.Empty))
                if (oppositeResults)
                    finalSQL += " OR ";
                else
                    finalSQL += " AND ";

            if (where != string.Empty)
                finalSQL += " (" + where + ")";

            finalSQL += " ORDER BY name";
            return finalSQL;
        }


        public List<insurance_company_groups_filters> Filters
        {
            get
            {
                List<insurance_company_groups_filters> toReturn = new List<insurance_company_groups_filters>();
                insurance_company_groups_filters icg = new insurance_company_groups_filters();

                DataTable matches = icg.Search("SELECT * FROM insurance_company_groups_filters WHERE icg_id = " + id);

                foreach (DataRow aRow in matches.Rows)
                {
                    icg = new insurance_company_groups_filters();
                    icg.Load(aRow);
                    toReturn.Add(icg);
                }

                return toReturn;
            }
        }

        public List<string> FiltersTextOnly
        {
            get
            {
                List<string> toReturn = new List<string>();
                insurance_company_groups_filters icg = new insurance_company_groups_filters();

                DataTable matches = icg.Search("SELECT * FROM insurance_company_groups_filters WHERE icg_id = " + id);

                foreach (DataRow aRow in matches.Rows)
                {
                    icg = new insurance_company_groups_filters(aRow);
                    toReturn.Add(icg.filter_text);
                }

                return toReturn;
            }
        }

        internal void SaveFilters(System.Windows.Forms.ListBox.ObjectCollection filters)
        {
            // Clear the existing filters then add the new ones
            ExecuteNonQuery("DELETE FROM insurance_company_groups_filters WHERE icg_id = " + id);

            foreach (string aFilter in filters)
            {
                insurance_company_groups_filters icg = new insurance_company_groups_filters();
                icg.icg_id = id;
                icg.filter_text = aFilter;
                icg.Save();
            }
        }

        internal object GetFiltersAsString()
        {
            string toReturn = "";

            foreach (insurance_company_groups_filters aFilter in Filters) {
                if (toReturn != string.Empty)
                    toReturn += ", ";

                toReturn += aFilter.filter_text;
            }
            return toReturn;
        }

        public override void Delete()
        {
            ExecuteNonQuery("DELETE FROM insurance_company_groups_filters WHERE icg_id = " + id);
            base.Delete();
        }

        public override string ToString()
        {
            return name;
        }

        internal List<company> GetMatchingCompanies()
        {
            List<company> toReturn = new List<company>();
            string finalSQL = insurance_company_group.GenerateCompanySQL("*", FiltersTextOnly);
            company workingComp = new company();

            foreach (DataRow aRow in workingComp.Search(finalSQL).Rows)
            {
                workingComp = new company(aRow);
                toReturn.Add(workingComp);
            }

            return toReturn;
        }
    }
}
