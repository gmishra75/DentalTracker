using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Windows.Forms;

namespace C_DentalClaimTracker
{
    public partial class call : DataObject
    {
        /// <summary>
        /// Override. Deletes all references to the call in notes, then erases call.
        /// Also erases answers/choices made for this call
        /// </summary>
        public override void Delete()
        {
            ExecuteNonQuery("DELETE FROM notes WHERE call_id = " + id);
            ExecuteNonQuery("DELETE FROM choices WHERE call_id = " + id);
            ExecuteNonQuery("DELETE FROM large_choices WHERE call_id = " + id);
            base.Delete();
        }


        /// <summary>
        /// Converts the call to a text format where choice made are visible
        /// </summary>
        /// <returns></returns>
        internal string DisplayAsText()
        {
            string toReturn = operatordata + " " + created_on.Value.ToShortDateString() + " " + created_on.Value.ToShortTimeString();
            toReturn += "\nCall Length: " + System.Convert.ToInt32(DurationSeconds / 60) + " minutes";

            foreach (choice aChoice in GetCallChoices())
            {
                string toAdd = "";
                List<string> parentCatNames = new List<string>();
                question workingQuestion;
                if (aChoice.LinkedQuestion.parent > 0)
                {
                    workingQuestion = aChoice.LinkedQuestion;
                    parentCatNames.Insert(0, workingQuestion.text);
                    while (workingQuestion.parent > 0)
                    {
                        workingQuestion = workingQuestion.ParentQuestion;
                        parentCatNames.Insert(0, workingQuestion.text);
                    }
                }
                else
                {
                    parentCatNames.Insert(0, aChoice.LinkedQuestion.text);
                }

                foreach (string aCatName in parentCatNames)
                {
                    if (toAdd == string.Empty)
                        toAdd = aCatName;
                    else
                        toAdd += " -> " + aCatName;
                }

                toAdd += ": " + aChoice.answer;
                toReturn += "\n" + toAdd;
            }

            return toReturn;
        }

        public List<choice> GetCallChoices()
        {
            List<choice> toReturn = new List<choice>();

            DataTable allChoices = Search("SELECT * FROM choices WHERE call_id = " + id +
                " UNION SELECT * FROM large_choices WHERE call_id = " + id);

            foreach(DataRow aRow in allChoices.Rows)
            {
                choice toAdd = new choice();
                toAdd.Load(aRow);
                toReturn.Add(toAdd);
            }

            return toReturn;
        }

        public List<CallQuestion> GetCallQuestions()
        {
            List<CallQuestion> toReturn = new List<CallQuestion>();

            List<question> claimQuestions = LinkedClaim.Questions;
            foreach (question q in claimQuestions)
            {
                toReturn.Add(new CallQuestion(q, this));
            }

            List<choice> callChoices = new List<choice>();
            DataTable linkedChoices = Search("SELECT * FROM choices WHERE call_id = " + id +
                " UNION SELECT * FROM large_choices WHERE call_id = " + id);
            foreach (DataRow aChoice in linkedChoices.Rows)
            {
                choice c = new choice();
                c.Load(aChoice);
                c.LinkedCall = this;

                CallQuestion cq = FindCallQuestion(c.question_id, toReturn);
                if (cq != null)
                {
                    c.LinkedQuestion = cq.Question;
                    cq.Choice = c;
                }
            }

            return toReturn;
        }

        private CallQuestion FindCallQuestion(int id, List<CallQuestion> list)
        {
            foreach (CallQuestion cq in list)
            {
                if (cq.Question.id == id)
                    return cq;
                if (cq.SubQuestions.Count > 0)
                {
                    CallQuestion subcq = FindCallQuestion(id, cq.SubQuestions);
                    if (subcq != null)
                        return subcq;
                }
            }
            return null;
        }

        /// <summary>
        /// Returns a list of all questions that were answered on this call (use only on calls that have been saved)
        /// </summary>
        public List<notes> GetNotes()
        {
            List<notes> toReturn = new List<notes>();
            DataTable linkedNotes = Search("SELECT * FROM notes WHERE call_id = " + id
                + " ORDER BY created_on desc");
            foreach (DataRow aNote in linkedNotes.Rows)
            {
                notes n = new notes();
                n.Load(aNote);
                toReturn.Add(n);
            }

            // toReturn.Sort();

            return toReturn;
        }

        /// <summary>
        /// Removes all answers for this question without affecting the notes or erasing the call itself
        /// </summary>
        internal void ClearAnswers()
        {
            string commandText = "DELETE FROM choices WHERE call_id = " + id;
            ExecuteNonQuery(commandText);

            commandText = "DELETE FROM large_choices WHERE call_id = " + id;
            ExecuteNonQuery(commandText);    
        }

        public override string ToString()
        {
            return operatordata + " " +
                created_on.Value.ToShortDateString() + " " + created_on.Value.ToShortTimeString() + " " +
                 System.Convert.ToInt32(DurationSeconds / 60) + " minutes";
        }

        /// <summary>
        /// Adds a claim to a given call (if that claim is notalready there).
        /// </summary>
        /// <param name="LinkedClaim"></param>
        internal void AddClaim(claim LinkedClaim)
        {
            if (!CallHasClaim(LinkedClaim))
            {
                LinkedClaim.ExecuteNonQuery("INSERT INTO call_claim_ids (call_id, claim_id) VALUES ("
                    + id + ", " + LinkedClaim.id + ")");
            }
        }

        /// <summary>
        /// Looks to see if a call has the particular claim already in the call_claim_ids table 
        /// (for calls that accept multiple claims)
        /// </summary>
        /// <param name="LinkedClaim"></param>
        /// <returns></returns>
        private bool CallHasClaim(claim LinkedClaim)
        {
            if (LinkedClaim.Search("SELECT * FROM call_claim_ids WHERE call_id = " + id +
                " AND claim_id = " + LinkedClaim.id).Rows.Count > 0)
                return true;
            else
                return false;
        }

        public question LinkedStatus
        {
            get
            {
                if (call_status == 0)
                    return null;
                else
                    return new question(call_status);
            }
        }
    }
}
