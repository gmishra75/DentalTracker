using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

namespace C_DentalClaimTracker
{
    public partial class question
    {
        public enum QuestionTypes
        {
            Category = 1,
            YesNo = 2,
            MultipleChoice = 3,
            NormalText = 4,
            SmallText = 5,
            LargeText = 6,
            Date = 7,
            Numeric = 8,
            Header = 9,
            Spacer = 10
        }

        /// <summary>
        /// Returns a list of questions that apply if this question was chosen as a category. Starts at the lowest level parent and continues down
        /// </summary>
        /// <returns></returns>
        internal List<question> GetCategoryQuestions()
        {
            List<question> toReturn = new List<question>();
            List<question> parentQuestions = new List<question>();
            int p = parent;

            while (p > 0)
            {
                question q = new question(p);
                parentQuestions.Insert(0, q);
                p = q.parent;
            }

            foreach (question q in parentQuestions)
            {
                toReturn.AddRange(q.GetSubQuestions());
            }

            toReturn.AddRange(GetSubQuestionsRecursive());


            return toReturn;
        }

        private List<question> GetSubQuestionsRecursive()
        {
            List<question> toReturn = new List<question>();
            DataTable matches = Search("SELECT * FROM QUESTIONS WHERE parent = " + id + " AND is_classification <> 1 " +
                "ORDER BY order_id");

            foreach (DataRow aRow in matches.Rows)
            {

                question toAdd = new question();
                toAdd.Load(aRow);

                toReturn.Add(toAdd);
                toReturn.AddRange(toAdd.GetSubQuestionsRecursive());
            }

            return toReturn;
        }

        public List<question> GetSubQuestions()
        {
            List<question> toReturn = new List<question>();
            DataTable matches = Search("SELECT * FROM QUESTIONS WHERE parent = " + id + " AND is_classification <> 1 " +
                " ORDER BY order_id");

            foreach (DataRow aRow in matches.Rows)
            {
                question toAdd = new question();
                toAdd.Load(aRow);

                toReturn.Add(toAdd);
            }

            return toReturn;
        }



        internal List<int> GetParentIDs()
        {
            List<int> toReturn = new List<int>();

            question thisQuestion = new question(this.id);

            while (thisQuestion.parent > 0)
            {
                toReturn.Add(thisQuestion.parent);
                thisQuestion = thisQuestion.ParentQuestion;
            }

            return toReturn;
        }

        /// <summary>
        /// 
        /// </summary>
        public List<multiple_choice_answer> GetMultipleChoiceAnswers()
        {
            List<multiple_choice_answer> toReturn = new List<multiple_choice_answer>();
            DataTable linkedNotes = Search("SELECT * FROM multiple_choice_answers WHERE question_id = " + id + " order by order_id");
            foreach (DataRow aAnswer in linkedNotes.Rows)
            {
                multiple_choice_answer m = new multiple_choice_answer();
                m.Load(aAnswer);
                toReturn.Add(m);
            }

            toReturn.Sort();

            _multipleChoiceAnswers = toReturn;

            return toReturn;
        }

        public override void Delete()
        {
            foreach (question aQuestion in SubQuestions)
                aQuestion.Delete();

            base.Delete();
        }

        public bool HasParent
        {
            get { return parent < 1; }
        }

        public question ParentQuestion
        {
            get
            {
                return new question(parent);
            }
        }

        public bool ParentIsFork
        {
            get
            {
                if (parent < 1)
                    return false;
                else
                {
                    return ParentQuestion.is_fork;
                }
            }
        }

        internal List<question> SubCategories()
        {
            List<question> toReturn = new List<question>();
            DataTable dt = Search("SELECT * FROM questions WHERE parent = " + 
                id + " AND is_classification = 1 ORDER BY order_id");


            question workingQuestion;
            foreach (DataRow aRow in dt.Rows)
            {
                workingQuestion = new question();
                workingQuestion.Load(aRow);

                toReturn.Add(workingQuestion);
            }

            return toReturn;
        }

        /// <summary>
        /// Returns a list of all classifications that are children of the current classification. Returns an 
        /// empty list (count = 0) if none exist.
        /// </summary>
        /// <returns></returns>
        internal List<question> GetSubClassifications()
        {
            List<question> toReturn = new List<question>();
            question q = new question();
            DataTable dt = Search("SELECT * FROM questions WHERE parent = " +
                id + " AND is_classification = 1 ORDER BY order_id");

            foreach (DataRow aRow in dt.Rows)
            {
                q = new question();
                q.Load(aRow);
                toReturn.Add(q);
            }

            return toReturn;
        }
    }
}
