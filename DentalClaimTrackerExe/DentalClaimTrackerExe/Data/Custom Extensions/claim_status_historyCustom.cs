using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

namespace C_DentalClaimTracker
{
    partial class claim_status_history
    {
        /// <summary>
        /// Returns the next open order id for the current claim number
        /// </summary>
        /// <returns></returns>
        public int GetNextOrderID()
        {
            return GetNextOrderID(claim_id);   
        }

        /// <summary>
        /// Returns the next open order id for the next claim number
        /// </summary>
        /// <param name="claimID"></param>
        /// <returns></returns>
        public int GetNextOrderID(int claimID)
        {
            DataTable resultsForClaim = Search("SELECT Count(*) FROM claim_status_history WHERE claim_id = " +
                claimID);

            return Convert.ToInt32(resultsForClaim.Rows[0][0]) + 1;
        }

        public string CreateChangeText()
        {
            string toReturn = string.Empty;
            if (as_is_flag)
            {
                toReturn = "-As is-";
            }
            else
            {
                if (old_status_id != new_status_id)
                {
                    if (new_status_id == -1)
                        toReturn += "Status: {Empty}";
                    else
                        toReturn += "Status: " + LinkedNewStatus.name;
                }

                if (new_revisit_date.HasValue)
                {
                    if (toReturn != string.Empty)
                        toReturn += " | ";

                    toReturn += "Revisit: " + new_revisit_date.Value.ToShortDateString();
                }
            }

            if (toReturn == string.Empty)
                toReturn = "-No change-";

            return toReturn;
        }

        public user LinkedUser
        { get { return new user(user_id); } }

        public claim_status LinkedNewStatus
        { get { return new claim_status(new_status_id); } }

        public claim_status LinkedOldStatus
        { get { return new claim_status(old_status_id); } }
    }
}
