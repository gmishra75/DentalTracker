using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

namespace C_DentalClaimTracker
{
    partial class company_contact_info
    {
        public PhoneObject phone_Object
        {
            get
            {
                if (this["phone"] is string)
                {
                    PhoneObject toReturn = new PhoneObject(this["phone"].ToString());
                    return toReturn;
                }
                else
                    return new PhoneObject();
            }
            set
            {
                this["phone"] = value.UnformattedPhone;
            }
        }

        public int GetNextOrderID()
        {
            int toReturn;

            DataTable maxOrderID = Search("SELECT MAX(order_ID) from company_contact_info WHERE company_id = " + company_id);

            int lastID = CommonFunctions.DBNullToZero(maxOrderID.Rows[0][0]);

            if (lastID == 0)
                toReturn = 1;
            else
                toReturn = lastID + 1;

            return toReturn;

        }
    }
}
