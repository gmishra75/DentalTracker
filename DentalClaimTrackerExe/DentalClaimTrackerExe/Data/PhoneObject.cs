using System;

namespace C_DentalClaimTracker
{
	/// <summary>
	/// Phone Object is used for dealing with phone numbers. It takes a string, 
	/// formatted 123 456 7890 x150 and returns the various parts.
	/// </summary>
	public class PhoneObject
	{
		string _unformattedPhone;
		public PhoneObject()
		{ _unformattedPhone = ""; }

		public PhoneObject(string PhoneUnformatted)
		{
			this._unformattedPhone = PhoneUnformatted;
		}

		public PhoneObject(string sAreaCode, string sPrefix, string sPostFix, string sExtension)
		{
			this.SetPhoneFromPieces(sAreaCode, sPrefix, sPostFix, sExtension);
		}


		public string UnformattedPhone
		{
			get { return this._unformattedPhone; }
			set { this._unformattedPhone = value; }
		}

		/// <summary>
		/// Specifies whether the Phone Object recognizes the current Phone as a 
		/// US Format string. If it is not US formatted, most functions will not work.
		/// The only recognized format is 123 456 7890 x1004 where 1004 is the extension
		/// </summary>
		public bool IsUSFormat
		{
			get 
			{
				string strWorkingPhone = this._unformattedPhone.Replace(" ", "");

				if (strWorkingPhone.IndexOf("x") >= 0)
				{
					// Remove Extension
					strWorkingPhone = strWorkingPhone.Substring(0, strWorkingPhone.IndexOf("x")- 1);
				}
			
				if (strWorkingPhone.Length == 10)
				{
					// 12 = Area Code + Phone
					return true;
				}
				else if (strWorkingPhone.Length == 7)
				{
					// 8 = Phone, no Area Code
					return true;
				}
				else
				{
					// Not a recognized format
					return false;
				}
			}
		}

		/// <summary>
		/// Returns "" if there is no area code. Will not work with international
		/// phone numbers.
		/// </summary>
		/// <param name="phone"></param>
		public string AreaCode
		{
			get
			{
				// First, have to remove the extension, then check the length of the phone
			
				string strWorkingPhone = this._unformattedPhone.Replace(" ", "");

				if (strWorkingPhone.IndexOf("x") >= 0)
				{
					// Remove Extension
					strWorkingPhone = strWorkingPhone.Substring(0, strWorkingPhone.IndexOf("x") -1);
				}
			
				if (strWorkingPhone.Length == 10)
				{
					// 12 = Area Code + Phone
					return strWorkingPhone.Substring(0, 3);
				}
				else
				{
					return "";
				}
			}

		}

		/// <summary>
		/// If there is no area code, returns the first three characters. Will not work with international
		/// phone numbers.
		/// </summary>
		/// <param name="phone"></param>
		public string Prefix
		{
			get 
			{
				// First, have to remove the extension, then check the length of the phone
			
				string strWorkingPhone = this._unformattedPhone.Replace(" ", "");

				if (strWorkingPhone.IndexOf("x") >= 0)
				{
					// Remove Extension
					strWorkingPhone = strWorkingPhone.Substring(0, strWorkingPhone.IndexOf("x") -1);
				}
			
				if (strWorkingPhone.Length == 10)
				{
					// 12 = Area Code + Phone
					return strWorkingPhone.Substring(3, 3);
				}
				else if (strWorkingPhone.Length > 3)
				{
					// No area code, but phone exists
					return strWorkingPhone.Substring(0, 3);
				}
				else
				{
					return "";
				}
			}

		}

		/// <summary>
		/// Returns the last four characters of a phone number.
		/// </summary>
		/// <param name="phone"></param>
		public string Postfix
		{
			get 
			{
				// First, have to remove the extension, then check the length of the phone
                string strWorkingPhone = this._unformattedPhone.Replace(" ", "").Replace(" ", "");
			
				if (strWorkingPhone.IndexOf("x") >= 0)
				{
					// Remove Extension
					strWorkingPhone = strWorkingPhone.Substring(0, strWorkingPhone.IndexOf("x") -1);
				}
			
			
				if (strWorkingPhone.Length == 10)
				{
					// 12 = Area Code + Phone
					return strWorkingPhone.Substring(6, 4);
				}
				else if (strWorkingPhone.Length > 6)
				{
					// No area code
					return strWorkingPhone.Substring(3, 4);
				}
				else
				{
					return "";
				}
			}
		}

		/// <summary>
		/// Returns the extension of a phone number. Returns "" if no extension.
		/// </summary>
		/// <param name="phone"></param>
		public string Extension
		{
			get
			{


				// First, have to remove the extension, then check the length of the phone
				string strWorkingPhone = this._unformattedPhone.Replace(" ", "");
			
				if (strWorkingPhone.IndexOf("x") >= 0)
				{
					// Has extension
					return strWorkingPhone.Substring(strWorkingPhone.IndexOf("x") + 1, strWorkingPhone.Length - strWorkingPhone.IndexOf("x") - 1);
				}
				else
				{
					// No extension
					return "";
				}

			}
		}

		/// <summary>
		/// Puts the given pieces into appropriate US format phone number
		/// </summary>
		/// <param name="AreaCode">Area Code</param>
		/// <param name="Prefix">Prefix/First three digits</param>
		/// <param name="Postfix">Postfix/Last four digits</param>
		/// <param name="Extension">Extension</param>
		public void SetPhoneFromPieces(string sAreaCode, string sPrefix, string sPostfix, string sExtension)
		{
			this._unformattedPhone = "";
			if (sAreaCode.Length > 0)
			{
				this._unformattedPhone = sAreaCode + " ";
			}

			if (sPrefix.Length > 0)
			{			
				this._unformattedPhone += sPrefix + " ";
			}
			
			this._unformattedPhone += sPostfix;
			
			if (sExtension.Length > 0)
			{
				
				this._unformattedPhone += " x" + sExtension;
			}
		}

		/// <summary>
		/// Puts the given pieces into appropriate US format phone number
		/// </summary>
		/// <param name="AreaCode">Area Code</param>
		/// <param name="Prefix">Prefix/First three digits</param>
		/// <param name="Postfix">Postfix/Last four digits</param>
		/// <param name="Extension">Extension</param>
		public void SetPhoneFromPieces(string sAreaCode, string sPrefix, string sPostfix)
		{
			this._unformattedPhone = "";
			if (sAreaCode.Length > 0)
			{
				this._unformattedPhone = sAreaCode + " ";
			}
			
			if (sPrefix.Length > 0)
			{			
				this._unformattedPhone += sPrefix + " ";
			}
			
			this._unformattedPhone += sPostfix;
		}

		/// <summary>
		/// Returns the phone number in a formatted layout, ie (123) 456-7890 x5000
		/// </summary>
		/// <returns></returns>
		public string FormattedPhone
		{
			get 
			{
				string phoneFormatted = "";

				if (this.IsUSFormat)
				{
					if (this.AreaCode.Length > 0) 
						phoneFormatted += "(" + this.AreaCode + ") ";
			
					if (this._unformattedPhone.Length > 0)
					{
						phoneFormatted += this.Prefix + "-" + this.Postfix;
					}
			
					if (this.Extension.Length > 0)
					{
						phoneFormatted += " x" + this.Extension;
					}
				}
				else
				{
					phoneFormatted = this._unformattedPhone;
				}

				return phoneFormatted;
			}
		}

        /// <summary>
        /// Returns a phone number without the extension and no spaces
        /// </summary>
        public string UnformattedPhoneNoExtension
        {
            get
            {
                return this.AreaCode + this.Prefix + this.Postfix;
            }
        }

	}
}
