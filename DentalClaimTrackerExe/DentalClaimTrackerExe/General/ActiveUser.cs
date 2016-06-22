using System;
using System.Collections.Generic;
using System.Text;

namespace C_DentalClaimTracker
{
    static class ActiveUser
    {
        public enum ActionTypes
        {
            Login = 1,
            Logout=2,
            ViewClaim=3,
            StartCall=4,
            EndCall=5,
            StartMultiClaimCall=6,
            EndMultiClaimCall=7,
            CreateNote=8,
            ChangeClaimMultiClaim=9,
            ReviewClaim=10,
            ChangeStatus=11,
            SubmitClaim=12,
            ResendClaim=13
        }

        private static user _userObject;

        public static user UserObject
        {
            get { return _userObject; }
            set { _userObject = value; }
        }

        public static void LogAction(ActionTypes at)
        {
            LogAction(at, "");
        }

        /// <summary>
        ///  Logs a given action in the user table
        /// </summary>
        /// <param name="at"></param>
        public static void LogAction(ActionTypes at, string AdditionalNotes)
        {
            LogAction(at, 0, AdditionalNotes);
        }

        public static void LogAction(ActionTypes at, int ClaimID, string AdditionalNotes)
        {
            LogAction(at, ClaimID, 0, AdditionalNotes);
        }

        public static void LogAction(ActionTypes at, int ClaimID, int CallID, string AdditionalNotes)
        {
            user_action_log toInsert = new user_action_log();

            toInsert.user_id = UserObject.id;
            toInsert.order_id = toInsert.GetNextOrderID();
            toInsert.action_taken_time = DateTime.Now;
            toInsert.action_id = (int)at;
            toInsert.additional_notes = AdditionalNotes;

            if (ClaimID > 0)
                toInsert.claim_id = ClaimID;
            if (CallID > 0)
                toInsert.call_id = CallID;

            toInsert.Save();
        }

        /// <summary>
        /// Removes the most recent logged start call and end call actions for a user, then replaces "View Claim" with "Review Claim"
        /// Used to show a user reviewed a claim and did not take any action on a claim.
        /// </summary>
        /// <param name="claimID"></param>
        /// <param name="LogText"></param>
        internal static void LogReview(int claimID, string LogText)
        {
            // Remove the reference to "start call" and change the action log entry of view claim to "review claim"
            // If no view claim is found just create a new one

            user_action_log ual = user_action_log.FindMostRecent(claimID, UserObject.id);

            ual.DeleteLastCall(claimID, UserObject.id);

            ual.action_id = (int) ActionTypes.ReviewClaim;
            ual.additional_notes += "; " + LogText;

            ual.Save();
        }
    }
}
