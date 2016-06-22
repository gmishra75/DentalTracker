using System;
using System.Collections.Generic;
using System.Text;

namespace C_DentalClaimTracker
{
    public partial class user_preferences
    {
        public void PopulateDefaults(int id)
        {
            // Creates a record with the given id, populates it with default values
            user_id = id;
            open_eclaims_form = false;
            open_search_form = true;
            search_form_sent_date = -40;
            claim_form_maximized = false;
            claim_form_width = 1160;
            claim_form_height = 750;
            claim_form_top = 10;
            claim_form_left = 10;
            show_my_claims_first = true;
            Save();
        }
    }
}
