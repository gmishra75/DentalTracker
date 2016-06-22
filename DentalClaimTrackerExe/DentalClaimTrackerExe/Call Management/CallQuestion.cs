using System;
using System.Collections.Generic;
using System.Text;

namespace C_DentalClaimTracker
{
    public class CallQuestion
    {
        private call _call;
        private question _question;
        private choice _choice;
        private List<CallQuestion> _subQuestions;

        public CallQuestion() 
        {
            _subQuestions = new List<CallQuestion>();
        }

        public CallQuestion(question question, call call)
        {
            _subQuestions = new List<CallQuestion>();
            _call = call;            
            Question = question;                        
        }
        
        public CallQuestion(choice choice)
        {
            _subQuestions = new List<CallQuestion>();
            _choice = choice;

            if (choice != null)
            {
                _call = choice.LinkedCall;
                Question = choice.LinkedQuestion;
            }
        }

        public bool IsAnswered
        {
            get 
            {
                if (Choice != null)
                    return true;
                foreach (CallQuestion cq in SubQuestions)
                {
                    if (cq.IsAnswered)
                        return true;
                }
                return false;
            }
        }

        public call Call
        {
            get { return _call; }
            set { _call = value; }
        }

        public question Question
        {
            get { return _question; }
            set 
            { 
                _question = value;
                foreach (question q in value.SubQuestions)
                {
                    SubQuestions.Add(new CallQuestion(q, Call));
                }
            }
        }

        public choice Choice
        {
            get { return _choice; }
            set { _choice = value; }
        }

        public List<CallQuestion> SubQuestions
        {
            get { return _subQuestions; }
            set { _subQuestions = value; }
        }
    }
}
