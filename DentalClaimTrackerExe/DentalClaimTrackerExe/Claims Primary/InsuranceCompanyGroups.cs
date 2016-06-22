using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace C_DentalClaimTracker.Claims_Primary
{
    class InsuranceCompanyGroups
    {
        public string Name { get; set; }
        public List<company> Companies { get; set; }
        public insurance_company_group Group { get; set; }
        public int Count { get; set; }

        public InsuranceCompanyGroups()
        {
            Name = "";
            Companies = new List<company>();
            Count = -1;
        }

        public InsuranceCompanyGroups(string name)
        {
            Name = name;
            Companies = new List<company>();
            Count = -1;
        }

        public InsuranceCompanyGroups(string name, company cmp)
        {
            Name = name;
            Companies = new List<company>();
            Companies.Add(cmp);
            Count = -1;
        }

        public override string ToString()
        {
            string toReturn = Name;

            if (Count > -1)
            {
                toReturn += " (" + Count + ")";
            }

            return toReturn;
        }

        /// <summary>
        /// Returns all company IDs with the given separator
        /// </summary>
        /// <returns></returns>
        internal string CompanyIDAsString(string separator = ",")
        {
            string toReturn = "";

            Companies.ForEach(cmp => toReturn += "," + cmp.id);

            if (toReturn.Length > 0)
                toReturn = toReturn.Remove(0, 1);

            return toReturn;
        }

    }
}
