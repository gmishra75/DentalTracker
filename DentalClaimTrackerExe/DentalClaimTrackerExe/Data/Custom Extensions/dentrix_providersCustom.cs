using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Windows.Forms;

namespace C_DentalClaimTracker
{
    public partial class dentrix_providers : DataObject
    {
        public dentrix_providers GetProvider(string ProviderId)
        {
            dentrix_providers toReturn = new dentrix_providers();
            toReturn.RSCID = ProviderId;
            DataTable matches = toReturn.Search();

            if (matches.Rows.Count > 0)
                toReturn.Load(matches.Rows[0]);
            else
                toReturn = null;

            return toReturn;
        }
    }
}
