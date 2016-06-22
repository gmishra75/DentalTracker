using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

namespace C_DentalClaimTracker
{
    public partial class user_action_log
    {
        /// <summary>
        ///  Returns the next available 0-based order ID for the current 
        /// </summary>
        /// <returns></returns>
        public int GetNextOrderID()
        {
            DataTable dt = Search("SELECT MAX(order_id) FROM user_action_log WHERE user_id = " + user_id);

            return CommonFunctions.DBNullToZero(dt.Rows[0][0]) + 1; 
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public user LinkedUser
        {
            get { return new user(user_id); }
        }

        /// <summary>
        /// Deletes the last logged call with the given claim ID and user ID
        /// </summary>
        /// <param name="claimID"></param>
        /// <param name="userID"></param>
        internal void DeleteLastCall(int claimID, int userID)
        {
            
            ExecuteNonQuery("DELETE FROM user_action_log WHERE action_id = " + (int)C_DentalClaimTracker.ActiveUser.ActionTypes.StartCall +
                " AND user_id = " + userID + " AND claim_id = " + claimID + " AND order_id IN" +
                "(SELECT TOP 1 order_id FROM user_action_log WHERE user_id = " + userID + " AND claim_id = " + claimID + " ORDER BY order_id desc)");

            ExecuteNonQuery("DELETE TOP (1) FROM user_action_log WHERE action_id = " + (int)C_DentalClaimTracker.ActiveUser.ActionTypes.EndCall +
                " AND user_id = " + userID + " AND claim_id = " + claimID + " AND order_id IN" +
                "(SELECT TOP 1 order_id FROM user_action_log WHERE user_id = " + userID + " AND claim_id = " + claimID + " ORDER BY order_id desc)");
        }

        /// <summary>
        /// Finds and returns the most recent View Claim action for a given claim id and user id
        /// </summary>
        /// <param name="claimID"></param>
        /// <param name="p"></param>
        /// <returns></returns>
        internal static user_action_log FindMostRecent(int claimID, int userID)
        {
            user_action_log toReturn = new user_action_log();
            DataTable matches = toReturn.Search("SELECT TOP 1 * FROM user_action_log WHERE action_id = " + (int)C_DentalClaimTracker.ActiveUser.ActionTypes.ViewClaim +
                " AND user_id = " + userID + " AND claim_id = " + claimID + " ORDER BY order_id desc");

            if (matches.Rows.Count > 0)
            {
                toReturn.Load(matches.Rows[0]);
            }
            else
            {
                toReturn.user_id = userID;
                toReturn.order_id = toReturn.GetNextOrderID();
                toReturn.action_taken_time = DateTime.Now;
                toReturn.action_id = (int)C_DentalClaimTracker.ActiveUser.ActionTypes.ReviewClaim;
                toReturn.additional_notes = "";
                toReturn.claim_id = claimID;
                
            }

            return toReturn;
        }
    }
}
