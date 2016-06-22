using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Windows.Forms;
using System.Data.Objects.DataClasses;
using System.Runtime.Serialization;
using System.Data.Objects;

namespace C_DentalClaimTracker
{
    public partial class claim_status : DataObject
    {
        
        public override string ToString()
        {
            return name;
        }

        
        internal DataTable GetVisibleStatus()
        {
            user_visible = true;
            
            return Search(SearchSQL + " ORDER BY orderid");
        }
    }
}
