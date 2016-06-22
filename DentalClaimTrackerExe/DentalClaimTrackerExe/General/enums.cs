using System;
using System.Collections.Generic;
using System.Text;

namespace C_DentalClaimTracker
{
    public enum EditModes
    {
        AddNew,
        AddDialog,
        Edit
    }

    public enum DataTypes
    {
        Text,
        Date,
        Numeric,
        Boolean
    }

    public enum DataMappingConnectionTypes
    {
        SQLServer = 0,
        MySQL = 1,
        Access = 2,
        CSV = 3
    }
}
