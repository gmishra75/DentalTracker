using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace C_DentalClaimTracker
{
    partial class mercury_payer_list : DataObject
    {
        /// <summary>
        /// Looks to see if a set of criteria has a match. Returns "" if no match found.
        /// </summary>
        /// <param name="searchCriteria"></param>
        /// <returns></returns>
        internal Dictionary<string, string> SearchIncludeAlias(string searchCriteria)
        {
            Dictionary<string, string> toReturn = new Dictionary<string, string>();
            DataTable easyMatches = Search(string.Format("SELECT mpl.payer_id, mpl.Name FROM mercury_payer_list mpl " +
                "LEFT JOIN mercury_payer_alias mpa ON mpl.id = mpa.id WHERE mpl.Name = '{0}' " +
                "OR mpa.alias = '{0}'", searchCriteria));

            if (easyMatches.Rows.Count > 0)
            {
                // We have a match on the payer name
                toReturn.Add("id", easyMatches.Rows[0][0].ToString());
                toReturn.Add("name", easyMatches.Rows[0][1].ToString());
                return toReturn;
            }
            else
                return null;
            
        }
    }
}
