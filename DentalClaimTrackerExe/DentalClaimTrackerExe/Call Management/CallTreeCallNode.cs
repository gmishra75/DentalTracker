using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace C_DentalClaimTracker
{
    internal class CallTreeCallNode : TreeNode
    {
        private call _call;

        public CallTreeCallNode(call call)
            : base()
        {
            this.Text = call.operatordata;
            if (call.updated_on.HasValue)
            {
                this.Text += " - " + call.updated_on.Value.ToString("MM/dd/yyyy hh:mm tt");
            }
            Call = call;
        }       

        public call Call
        {
            get { return _call; }
            private set { _call = value; }
        }
    }
}
