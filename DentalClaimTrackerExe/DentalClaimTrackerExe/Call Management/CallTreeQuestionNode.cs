using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace C_DentalClaimTracker
{
    internal class CallTreeQuestionNode : TreeNode
    {
        private question _question;
        private choice _choice;

        public CallTreeQuestionNode() { }
        
        public CallTreeQuestionNode(question question)
            : base(question.text)
        {
            _question = question;            
        }

        public CallTreeQuestionNode(choice choice) :this(choice.LinkedQuestion)
        {
            _choice = choice;
            this.Text = Question.text + ": " + choice.answer;
        }

        public question Question
        {
            get { return _question; }
            set { _question = value; }
        }

        public choice Choice
        {
            get { return _choice; }
            set { 
                _choice = value;
                if (Question == null)
                {
                    Question = _choice.LinkedQuestion;
                }
            }
        }
    }
}
