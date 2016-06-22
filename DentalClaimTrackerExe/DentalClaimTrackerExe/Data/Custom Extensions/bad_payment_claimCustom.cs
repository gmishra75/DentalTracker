using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

namespace C_DentalClaimTracker
{
    public partial class bad_payment_claims
    {
        /// <summary>
        /// Checks for an entry with the following data. If it exists, loads it in the current object.
        /// Returns true or false based on whether or not an object was discovered
        /// </summary>
        /// <param name="claimid"></param>
        /// <param name="dbnum"></param>
        /// <returns></returns>
        public bool CheckLoad(string claimid, string dbnum)
        {
            DataTable matches = Search("SELECT * FROM bad_payment_claims WHERE claimid = '" + claimid + 
                "' AND claimdb = '" + dbnum + "'");

            if (matches.Rows.Count > 0)
            {
                Load(matches.Rows[0]);
                return true;
            }
            else
                return false;
        }

        public override void Save()
        {
            last_update = DateTime.Now;
            base.Save();
        }
    }
}
