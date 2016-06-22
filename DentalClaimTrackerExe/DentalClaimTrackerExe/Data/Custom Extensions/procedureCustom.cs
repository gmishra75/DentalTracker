using System;
using System.Collections.Generic;
using System.Text;

namespace C_DentalClaimTracker
{
    partial class procedure
    {
        /// <summary>
        /// CUSTOM: Get/Set for the int amount. Throws DataObjectException if the data is null.
        /// Value altered by 100 to account for the fact that incoming values are stored in cents
        /// </summary>
        public int amount
        {
            get
            {
                if (this["amount"] is int)
                    return ((int)this["amount"] / 100);
                else
                    throw new DataObjectException("Property value is empty or invalid. (int - amount)");
            }
            set
            {
                this["amount"] = value * 100;
            }
        }

        /// <summary>
        /// CUSTOM: Gets a tooth range string formatted like so: 1-3. Also converts any tooth numbers to letters if necessary
        /// </summary>
        public string tooth_range_string
        {
            get
            {
                string toReturn;
                if (tooth_range_start == tooth_range_end)
                    toReturn = ToothToLetter(tooth_range_start);
                else
                    toReturn = ToothToLetter(tooth_range_start) + "-" + 
                        ToothToLetter(tooth_range_end);

                return toReturn;
            }
        }

        public static string ToothToLetter(int toConvert)
        {
            if ((toConvert > 32) && (toConvert < 54))
            {
                char c = Convert.ToChar(toConvert + 32);

                return c.ToString();
            }
            else
                return toConvert.ToString();

        }
    }
}
