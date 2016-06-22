using System;
using System.Collections.Generic;
using System.Text;

namespace C_DentalClaimTracker
{
    partial class choice : DataObject, IComparable
    {
        public override void Save()
        {
            if (LinkedQuestion.type == C_DentalClaimTracker.question.QuestionTypes.LargeText)
            {
                TableName = "large_choices";
            }
            else
            {
                TableName = "choices";
            }
            base.Save();
        }

        public static void ClearChoices(List<choice> allChoices)
        {
            string ids = "";

            foreach (choice aChoice in allChoices)
            {
                ids += aChoice.id + ",";
            }

            if (ids.Length > 1)
            {
                ids = ids.Remove(ids.Length - 1, 1); // Remove that last comma

                choice toUse = new choice();
                toUse.ExecuteNonQuery("DELETE FROM CHOICES WHERE id IN(" + ids + ")");
            }
        }
    }
}
