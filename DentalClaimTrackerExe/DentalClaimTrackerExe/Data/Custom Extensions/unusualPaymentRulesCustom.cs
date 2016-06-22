using System;
using System.Collections.Generic;
using System.Text;

namespace C_DentalClaimTracker
{
    
    partial class unusual_payment_rules : DataObject
    {
        // Changes to these enums will need to be manually reflected in the ctlUnusualPaymentRule class

        public enum OperatorCodes
        {
            LessThan = 0,
            EqualTo = 1,
            GreaterThan = 2
        }

        public enum AmountTypeCodes
        {
            Dollars = 0,
            Percent = 1
        }
        

    }
}
