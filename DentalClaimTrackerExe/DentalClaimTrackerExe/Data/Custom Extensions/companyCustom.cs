using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

namespace C_DentalClaimTracker
{
    partial class company
    {
        public int NumberOfAddresses
        {
            get
            {
                DataTable matches = Search("SELECT COUNT(*) FROM company_contact_info WHERE company_id = " + id);

                return (System.Convert.ToInt32(matches.Rows[0][0]));
            }
        }

        public override string ToString()
        {
            return name;
        }

        internal List<company_contact_info> GetLinkedAddresses()
        {
            List<company_contact_info> toReturn = new List<company_contact_info>();
            company_contact_info workingInfo;
            DataTable matches = Search("SELECT * FROM company_contact_info WHERE company_id = " + id);

            foreach (DataRow anInfo in matches.Rows)
            {
                workingInfo = new company_contact_info();
                workingInfo.Load(anInfo);
                toReturn.Add(workingInfo);
            }

            return toReturn;
        }

        internal company_contact_info GetFirstContactInfo()
        {
            DataTable results = Search("SELECT * FROM company_contact_info WHERE company_id = " + id + 
                " ORDER BY order_id");

            if (results.Rows.Count > 0)
            {
                company_contact_info toReturn = new company_contact_info();
                toReturn.Load(results.Rows[0]);

                return toReturn;
            }
            else
                return null;
        }

        /// <summary>
        /// Returns a list of all companies sorted by name
        /// </summary>
        /// <returns></returns>
        internal static List<company> GetSortedList()
        {
            List<company> toReturn = new List<company>();
            company workingCompany = new company();

            DataTable allData = workingCompany.GetAllData("name");

            foreach (DataRow aRow in allData.Rows)
            {
                workingCompany = new company();
                workingCompany.Load(aRow);

                toReturn.Add(workingCompany);
            }

            return toReturn;


        }

        /// <summary>
        /// Returns a list of all Insurance Company Groups and all Insurance Companies not in a group
        /// </summary>
        /// <returns></returns>
        internal List<Claims_Primary.InsuranceCompanyGroups> GetInsuranceCompaniesAsGroup()
        {
            List<Claims_Primary.InsuranceCompanyGroups> allGroups = new List<Claims_Primary.InsuranceCompanyGroups>();
            List<company> companiesInAGroup = new List<company>();
            insurance_company_group icg = new insurance_company_group();

            foreach (DataRow aRow in icg.GetAllData("name").Rows)
            {
                Claims_Primary.InsuranceCompanyGroups newGroup = new Claims_Primary.InsuranceCompanyGroups();
                icg = new insurance_company_group(aRow);
                List<company> matchingComps = icg.GetMatchingCompanies();

                newGroup.Name = icg.name;
                newGroup.Companies = matchingComps;
                newGroup.Group = icg;
                allGroups.Add(newGroup);

                companiesInAGroup.AddRange(matchingComps);
            }

            DataTable companiesNotInGroup = icg.Search(string.Format("SELECT * FROM companies WHERE ID NOT IN ({0}) ORDER BY name", ClaimTrackerCommon.CompaniesToInString(companiesInAGroup)));

            foreach (DataRow aRow in companiesNotInGroup.Rows)
            {
                company c = new company(aRow);
                Claims_Primary.InsuranceCompanyGroups newGroup = new Claims_Primary.InsuranceCompanyGroups(c.name, c);

                allGroups.Add(newGroup);
            }

            allGroups.Sort(SortInsuranceGroupsByName);

            return allGroups;
        }

        private static int SortInsuranceGroupsByName(Claims_Primary.InsuranceCompanyGroups x, Claims_Primary.InsuranceCompanyGroups y)
        {
            if (x == null)
            {
                if (y == null)
                    return 0;
                else
                    return -1;
            }
            else
            {
                if (y == null)
                    return 1;
                else
                {
                    return x.Name.CompareTo(y.Name);
                }
            }
        }
    }
}
