using System;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;
using System.Text;

namespace C_DentalClaimTracker
{
	public enum Alignments
	{ Right, Left, Center }


	/// <summary>
	/// Summary description for CommonFunctions.
	/// </summary>
	public sealed class CommonFunctions
	{
        /// <summary>
        /// Trims all non-printable characters from a string
        /// </summary>
        /// <param name="s"></param>
        /// <param name="replaceWith"></param>
        /// <returns></returns>
        public static string RemoveNonPrintableCharacters(string s)
        {
            StringBuilder result = new StringBuilder();
            for (int i = 0; i < s.Length; i++)
            {
                char c = s[i];
                byte b = (byte)c;
                if (b > 32)
                    result.Append(c);
            }
            return result.ToString();
        }


		/// <summary>
		/// Takes a string and inserts a space in between every character
		/// </summary>
		/// <param name="Text"></param>
		/// <returns></returns>
		public static string SpaceOutText(string text)
		{
			string toReturn = text;
			for (int i = text.Length - 1; i > 0; i--)
			{
				toReturn = toReturn.Insert(i, " ");
			}

			return toReturn;
		}

		public static void copyDirectory(DirectoryInfo srcPath, DirectoryInfo destPath, bool recursive)
		{
			if( !destPath.Exists )
				destPath.Create();

			// copy files
			foreach( FileInfo fi in srcPath.GetFiles() )
			{
				fi.CopyTo( Path.Combine(destPath.FullName, fi.Name), true );
			}

			// copy directories
			if( recursive )
			{
				foreach( DirectoryInfo di in srcPath.GetDirectories() )
				{
					copyDirectory( di, new DirectoryInfo( Path.Combine( destPath.FullName, di.Name ) ), recursive );
				}
			}
		}

		/// <summary>
		/// Removes leading, trailing, and multiple spaces from a string.
		/// </summary>
		/// <param name="text">String to trim of spaces.</param>
		/// <returns></returns>
		public static string TrimSpaces(string text) 
		{
            if (text == null)
                return "";

			while (text.IndexOf("  ") >= 0)	
			{
				text = text.Replace("  "," "); 
			} 
			return text.Trim(); 
		}

		/// <summary>
		/// Takes an object and returns 0 if it is not an integer, or returns the integers value if it is
		/// </summary>
		/// <param name="number"></param>
		/// <returns></returns>
		public static int DBNullToZero(object number)
		{
			if (number is int)
				return (int) number;
			else
				return 0;

		}

        public static decimal DBNullToZeroDec(object number)
        {
            if (number is decimal)
                return (decimal)number;
            else
                return 0;
        }

		public static string DBNullToString(object str)
		{
			if (str == null)
				return "";
			else if (str == DBNull.Value)
				return "";
			else
				return str.ToString();
		}
	

		/// <summary>
		/// Takes a number from 0-9 and converts it into its "word"
		/// 1=One
		/// </summary>
		/// <param name="Number"></param>
		/// <returns></returns>
		public static string NumberToWord(int Number)
		{
			switch(Number)
			{
				case 0: 
					return "zero";
				case 1:
					return "one";
				case 2:
					return "two";
				case 3:
					return "three";
				case 4:
					return "four";
				case 5:
					return "five";
				case 6:
					return "six";
				case 7:
					return "seven";
				case 8:
					return "eight";
				case 9:
					return "nine";
				case 10:
					return "ten";
				case 11:
					return "eleven";
				case 12:
					return "twelve";
				case 13:
					return "thirteen";
				case 14:
					return "fourteen";
				case 15:
					return "fifteen";
				case 16:
					return "sixteen";
				case 17:
					return "seventeen";
				case 18:
					return "eighteen";
				default:
					return Number.ToString();
			}
		}

		/// <summary>
		/// Takes a string and adds trailing or leading spaces to convert it to the specified length
		/// CENTER does not work AS of 7-9-03 (AJ)
		/// </summary>
		/// <param name="convstring">The string to convert</param>
		/// <param name="length">The length of the fixed length string</param>
		/// <param name="align">Alignment of the text within the string (NO CENTER)</param>
		/// <returns></returns>
		public static string ToFixedLength(string convstring, int length, Alignments align)
		{
			string fromstring = convstring;
			if (align == Alignments.Right)
			{
				while (fromstring.Length < length)
					fromstring = " " + fromstring;
				return fromstring;
			}
			else if (align == Alignments.Left)
			{
				while (fromstring.Length < length)
					fromstring += " ";
				return fromstring;
			}
			else if (align == Alignments.Center)
			{
				return fromstring;
			}
			else // never happens unlesss alignment type is uncoded for
				return "";
		}

		public static string ToYesNo(bool myVal)
		{
			if (myVal)
				return "Yes";
			else
				return "No";
		}

		public static string ToYesNo(int myVal)
		{
			if (System.Convert.ToBoolean(myVal))
				return "Yes";
			else
				return "No";
		}

        internal static bool FromYesNo(string p)
        {
            bool toReturn = false;
            try
            {
                toReturn = Convert.ToBoolean(p);
            }
            catch (Exception err)
            {
                if ((p.ToUpper() == "YES") || (p.ToUpper() == "Y"))
                    toReturn = true;
                else if ((p.ToUpper() == "NO") || (p.ToUpper() == "N"))
                    toReturn = false;
                else
                {
                    LoggingHelper.Log("Error converting yes/no to boolean in CommonFunctions.FromYesNo", LogSeverity.Error, err, false);
                    throw new Exception("Error converting Yes/No value to boolean.", err);
                }
            }

            return toReturn;
        }

		/// <summary>
		/// Mimics VB functionality to determine if a string is a number
		/// </summary>
		/// <param name="s"></param>
		/// <returns></returns>
		public static bool IsNumeric(string s)
		{
			int deccount = 0;
			foreach(char c in s.ToCharArray())
			{
				if (!char.IsNumber(c))
				{
					if (c.ToString() == ".")
					{
						deccount++;
						
					}
					else if (c.ToString() == "-")
					{
						if (s.LastIndexOf("-", 0) > 0)
						{
							return false;
						}
					}
					else
						return false;	
				}
			}

			if (deccount > 1)
				return false;
            else if (s.Length == 0)
                return false;
			else
			return true;
		}


		/// <summary>
		/// Extends a state from its abbreviation to its full name. Returns the full name.
		/// </summary>
		/// <param name="abbr"> The abbreviation for the state</param>
		/// <returns></returns>
		public static string StateAbbreviationToFullName(string abbr)
		{
			switch(abbr)
			{
				case "AL":
					return "Alabama";
				case "AK":
					return "Alaska";
				case "AS":
					return "American Samoa";
				case "AZ":
					return "Arizona";
				case "AR":
					return "Arkansas";
				case "CA":
					return "California";
				case "CO":
					return "Colorado";
				case "CT":
					return "Connecticut";
				case "DE":
					return "Delaware";
				case "DC":
					return "District of Columbia";
				case "FM":
					return "Federated States of Micronesia";
				case "FL":
					return "Florida";
				case "GA":
					return "Georgia";
				case "GU":
					return "Guam";
				case "HI":
					return "Hawaii";
				case "ID":
					return "Idaho";
				case "IL":
					return "Illinois";
				case "IN":
					return "Indiana";
				case "IA":
					return "Iowa";
				case "KS":
					return "Kansas";
				case "KY":
					return "Kentucky";
				case "LA":
					return "Louisiana";
				case "ME":
					return "Maine";
				case "MH":
					return "Marshall Islands";
				case "MD":
					return "Maryland";
				case "MA":
					return "Massachusettes";
				case "MI":
					return "Michigan";
				case "MN":
					return "Minnesota";
				case "MS":
					return "Mississippi";
				case "MO":
					return "Missouri";
				case "MT":
					return "Montana";
				case "NE":
					return "Nebraska";
				case "NV":
					return "Nevada";
				case "NH":
					return "New Hampshire";
				case "NJ":
					return "New Jersey";
				case "NM":
					return "New Mexico";
				case "NY":
					return "New York";
				case "NC":
					return "North Carolina";
				case "ND":
					return "South Dakota";
				case "MP":
					return "Northern Mariana Islands";
				case "OH":
					return "Ohio";
				case "OK":
					return "Oklahoma";
				case "OR":
					return "Oregon";
				case "PW":
					return "Palua";
				case "PA":
					return "Pennsylvania";
				case "PR":
					return "Puerto Rico";
				case "RI":
					return "Rhode Island";
				case "SC":
					return "South Carolina";
				case "SD":
					return "South Dakota";
				case "TN":
					return "Tennessee";
				case "TX":
					return "Texas";
				case "UT":
					return "Utah";
				case "VT":
					return "Vermont";
				case "VI":
					return "Virgin Islands";
				case "VA":
					return "Virginia";
				case "WA":
					return "Washington";
				case "WV":
					return "West Virginia";
				case "WI":
					return "Wisconsin";
				case "WY":
					return "Wyoming";
				default:
					return abbr;
			}
		}

		/// <summary>
		/// Validates a file name (does not include path)
		/// </summary>
		/// <param name="filename"></param>
		/// <returns></returns>
		public static bool FileNameInvalid(string filename)
		{
			char[] InvalidCharArray = new char[20];

			InvalidCharArray = ("*/?\\$+:".ToCharArray());
			
			if (filename.IndexOfAny(InvalidCharArray) >= 0)
				return true;
			else
				return false;
		}

		/// <summary>
		/// Validates that a folder is valid (strange, does not check for
		/// drive specification). Invalid if filename included, invalid if
		/// starts with a \.
		/// </summary>
		/// <param name="foldername"></param>
		/// <returns></returns>
		public static bool FolderNameValid(string foldername)
		{
			char[] InvalidCharArray = new char[20];

			InvalidCharArray = ("*?/$+.".ToCharArray());
			
			if (foldername.IndexOfAny(InvalidCharArray) >= 0)
			{
				return false;
			}
			else if (foldername.StartsWith(@"\"))
			{
				return false;
			}
			else
			{
				return true;
			}

		}

		/// <summary>
		/// Takes a word and returns whether or not an "a" or an "an" should be placed 
		/// in front of it. If the first letter of the next word is a vowel or h,
		/// then an. If not a. Example: a tonberry, an hour. Always lowercase
		/// </summary>
		/// <param name="nextWord"> The word after the a or an</param>
		/// <returns>Returns either a or an, always lowercase</returns>
		public static string AorAn(string nextWord)
		{
            if (nextWord.Length <= 0)
            {
                return "";
            }
            else
            {
                bool vowel;

                string _char = nextWord.Substring(0, 1);

                if (char.IsNumber(_char, 0))
                {
                    _char = NumberToWord(System.Convert.ToInt32(_char)).Substring(0, 1);
                }


                if (_char.ToUpper() == "A" ||
                    _char.ToUpper() == "E" ||
                    _char.ToUpper() == "I" ||
                    _char.ToUpper() == "O" ||
                    _char.ToUpper() == "U" ||
                    _char.ToUpper() == "H")
                {
                    vowel = true;
                }
                else
                {
                    vowel = false;
                }


                if (vowel)
                    return "an";
                else
                    return "a";

            }
												 
		}

		/// <summary>
		/// Removes any formatting characters from text (chr(10), (13), and tab))
		/// And replaces them with code values. Use FormmatedTextFromDB to retrieve
		/// </summary>
		/// <param name="toCheck">The string to remove formatting characters from</param>
		/// <returns></returns>
		public static string FormattedTextToDB(string toCheck)
		{
			string toReturn = "";
			foreach(char testChar in toCheck.ToCharArray())
			{
				switch(System.Convert.ToInt32((testChar))) 
				{
					case 13:
						// carriage return
						toReturn += "{<CR>}";
						break;
					case 10:	
						// line feed
						toReturn += "{<LF>}";
						break;
					case 9:
						// tab
						break;
					case 39:
						// apostrophe
						toReturn += "{<AP>}";
						break;
					case 8217:
						// apostrophe from word
						toReturn += "{<AP>}";
						break;
						
					default:
						toReturn += testChar.ToString();
						break;

				}
			}
			return toReturn;

		}

		/// <summary>
		/// Adds any formatting to text encoded using the FormattedTextToDB function
		/// </summary>
		/// <param name="toFormat">Encoded text to decode</param>
		/// <returns></returns>
		public static string FormattedTextFromDB(string toFormat)
		{
			string toReturn = toFormat;
			char CR;
			char LF;

			CR = System.Convert.ToChar(13);
			LF = System.Convert.ToChar(10);

			toReturn = toReturn.Replace("{<CR>}", CR.ToString());
			toReturn = toReturn.Replace("{<LF>}", LF.ToString());
			toReturn = toReturn.Replace("{<AP>}", "'");
			return toReturn;
		}

		
		public static void CreateDLLRegisterBatchFile(string strLocation, string strFileName, string strLibName)
		{
			
			System.IO.StreamWriter sw = new System.IO.StreamWriter(strLocation + strFileName, true);
			
			sw.WriteLine("rem Autogenerated batch file from The Team Computer Services");
			sw.WriteLine("rem Unregister the DLL");
			sw.WriteLine("regsvr32 " + strLibName + " /u");
			sw.WriteLine("regsvr32 " + strLibName);
			
			
			sw.Close();
		}

		/// <summary>
		/// Takes the integer representation for a month and returns the string 
		/// </summary>
		/// <param name="month"></param>
		public static string MonthIntToYear(int month)
		{
			string toReturn;
			switch(month)
			{
				case 1:
					toReturn = "January";
					break;
				case 2:
					toReturn = "February";
					break;
				case 3:
					toReturn = "March";
					break;
				case 4:
					toReturn = "April";
					break;
				case 5:
					toReturn = "May";
					break;
				case 6:
					toReturn = "June";
					break;
				case 7:
					toReturn = "July";
					break;
				case 8:
					toReturn = "August";
					break;
				case 9:
					toReturn = "September";
					break;
				case 10:
					toReturn = "October";
					break;
				case 11:
					toReturn = "November";
					break;
				case 12:
					toReturn = "December";
					break;
				default:
					toReturn = "Unrecognized Month";
					break;
			}

			return toReturn;

		}		
	

		/// <summary>
		/// Takes a string and converts the last comma in it to the word "and". 
		/// Used when constructing sentences with lists.
		/// </summary>
		/// <param name="Sentence">String to be formatted.</param>
		/// <returns>Formatted string.</returns>
		public static string ReplaceFinalCommaWithAnd(string Sentence)
		{
			if (Sentence.IndexOf(",") >= 0)
			{
				Sentence = Sentence.Substring(0, Sentence.LastIndexOf(",")) + 
					Sentence.Substring(Sentence.LastIndexOf(","), 1).Replace(",", " and") + 
					Sentence.Substring(Sentence.LastIndexOf(",") + 1);
			}

			return Sentence;
		}

		public static void StartFile(string FileName)
		{
			if (File.Exists(FileName))
			{
				try
				{
					System.Diagnostics.Process.Start(FileName);
				}
				catch(Exception err)
				{
                    LoggingHelper.Log("Error starting file in CommonFunctions.StartFile", LogSeverity.Error, err, false);
#if DEBUG
					MessageBox.Show("The program encountered an error trying to open the file. Possible problems " +
						"include an unrecognized file type.\n\n" + err.Message);
#endif
				}
			}
			else
			{
				throw new FileNotFoundException();
			}
		}

		public static void OpenDirectory(string dirName)
		{
			if (Directory.Exists(dirName))
			{
				try
				{
					System.Diagnostics.Process.Start(dirName);
				}
				catch(Exception err)
				{
                    LoggingHelper.Log("Error opening folder in CommonFunctions.OpenDirectory", LogSeverity.Error, err, false);
#if DEBUG
					MessageBox.Show("The program encountered an error trying to open the file. Possible problems " +
						"include an unrecognized file type.\n\n" + err.Message);
#endif
				}
			}
			else
			{
				throw new FileNotFoundException();
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="parent"></param>
		/// <param name="toShow"></param>
		/// <param name="pTop"></param>
		/// <param name="pLeft"></param>
		/// <param name="pHeight"></param>
		/// <param name="pWidth"></param>
		/// <param name="showTime">In seconds, the amount of time to reveal the panel</param>
		/// <param name="Show">Is the panel being revealed or hidden</param>
		public static void HideRevealPanel(Form parent, Panel toShow, int pTop, int pLeft, int pHeight, int pWidth, decimal showTime, bool Reveal)
		{
			const int LOOPDELAY = 50;
			
			

			int numincrements = System.Convert.ToInt32((showTime / LOOPDELAY) * 1000);
			
			if (Reveal)
			{
				parent.Controls.Add(toShow);
				toShow.Visible = true;
				toShow.BringToFront();
				toShow.Refresh();
				// Showing panel
				toShow.Height = 0;
				toShow.Width = 0;
				toShow.Top = pTop;
				toShow.Left = pLeft;
				for (decimal i = 1; i < numincrements; i++)
				{
					toShow.Height = System.Convert.ToInt32((i / numincrements) * pHeight);
					toShow.Width = System.Convert.ToInt32((i / numincrements) * pWidth);
					toShow.Refresh();
					System.Threading.Thread.Sleep(LOOPDELAY);
				}

				toShow.Left = pLeft;
				toShow.Top = pTop;
				toShow.Width = pWidth;
				toShow.Height = pHeight;
			}
			else 
			{
				// Hiding Panel
				toShow.Width = pWidth;
				toShow.Height = pHeight;

				for (decimal i = numincrements; i > 0; i--)
				{
					toShow.Height = System.Convert.ToInt32((i / numincrements) * pHeight);
					toShow.Width = System.Convert.ToInt32((i / numincrements) * pWidth);
					System.Threading.Thread.Sleep(LOOPDELAY);
				}

				
				toShow.Height = 0;
				toShow.Width = 0;
				toShow.Visible = false;
				parent.Controls.Remove(toShow);
			}

		}

		public static byte[] ToByteArray(string toUse)
		{
			System.Text.ASCIIEncoding  encoding=new System.Text.ASCIIEncoding();
			return encoding.GetBytes(toUse);
		}

		public static string ToString(byte[] toUse)
		{
			System.Text.ASCIIEncoding enc = new System.Text.ASCIIEncoding();
			return enc.GetString(toUse);
		}


        /// <summary>
        /// Used for rounding a number - for example, rounding 
        /// to the nearest 50 or 100
        /// </summary>
        /// <param name="value">The value to be rounded.</param>
        /// <param name="roundTo">Round to the nearest (does not have to be 10-based)</param>
        /// <returns></returns>
        public static int Round(decimal value, int roundTo)
        {
            return (int) Decimal.Round((value / roundTo), 0) * roundTo;
        }

        /// <summary>
        /// Removes formatting characters like “, ’, ” and replaces them with the standard
        /// charcters like " or '
        /// </summary>
        /// <param name="toEdit"></param>
        /// <returns></returns>
        public static string RemoveSpecialWordFormatting(string toEdit)
        {
            string toUse = toEdit;
            toUse = toUse.Replace("“", "\"");
            toUse = toUse.Replace("”", "\"");
            toUse = toUse.Replace("’", "'");

            return toUse;
        }

        /// <summary>
        /// Takes a string and makes it safe for database searches. Assumes '' is a ' already prepared
        /// for the database, and will not convert it to ''''.
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public static string MakeDataSafe(string toMakeSafe)
        {
            toMakeSafe = toMakeSafe.Replace("'", "''");
            while (toMakeSafe.Contains("''''"))
                toMakeSafe = toMakeSafe.Replace("''''", "''");

            return toMakeSafe;
        }

        /// <summary>
        /// Removes periods, hyphens, parentheses, and extra spaces in a phone number
        /// </summary>
        /// <param name="phone"></param>
        /// <returns>Phone Formatted with a space following the 3rd and 7th characters</returns>
        public static string RemovePhoneFormatting(string phone)
        {
            string[] toRemove = { ".", "-", "(", ")", " " };

            foreach (string r in toRemove)
                phone = phone.Replace(r, "");

            if (phone.Length > 3)
                phone = phone.Insert(3, " ");
            if (phone.Length > 8)
                phone = phone.Insert(8, " ");

            return phone;
        }

        /// <summary>
        /// Returns the filesize as a string. Second parameter specifies formatting
        /// 0=bytes,1=kb,2=mb
        /// </summary>
        /// <param name="Path"></param>
        /// <param name="p">0=bytes,1=kb,2=mb</param>
        /// <returns></returns>
        internal static string FileSize(string fPath, int p)
        {
            if (File.Exists(fPath))
            {
                FileInfo fi = new FileInfo(fPath);


                switch (p)
                {
                    case 0:
                        return fi.Length.ToString() + " b";
                    case 1:
                        return (fi.Length / 1024) + " KB";
                    case 2:
                        return (fi.Length / 1045876) + " MB";
                }

                throw new Exception("Programmer error: Invalid filesize parameter.");
            }
            else
                return "0";
        }

        public static string FormatTextForMultiLine(string toFormat)
        {
            return toFormat.Replace("\r", "").Replace("\n", "\r\n");
        }

        internal static bool IsDate(string sToCheck)
        {
            try
            {
                System.Convert.ToDateTime(sToCheck);
                return true;
            }
            catch
            {
                return false;
            }
        }

        internal static string FormatName(string firstName, string lastName)
        {
            return FormatName(firstName, "", lastName);
        }

        internal static string FormatName(string firstName, string middleInitial, string lastName)
        {
            return CommonFunctions.TrimSpaces(firstName + " " + middleInitial + " " + lastName);
        }

        internal static FormattedName GetFormattedName(string unformattedName)
        {
            FormattedName toReturn;
            string workingValue = unformattedName.Trim();

            if (workingValue.Contains(" "))
            {
                toReturn.FirstName = workingValue.Substring(0, workingValue.IndexOf(" "));
            }
            else
            {
                toReturn.FirstName = workingValue;
                toReturn.MiddleInitial = "";
                toReturn.LastName = "";
                return toReturn;
            }

            // Remove everything up to the first space
            workingValue = workingValue.Remove(0, toReturn.FirstName.Length + 1);

            if (workingValue.Length > 1)
            {
                if (workingValue[1].ToString() == " ")
                {
                    // If the character is one "letter" long, then assume it's the middle initial
                    toReturn.MiddleInitial = workingValue[0].ToString();

                    workingValue = workingValue.Remove(0, 2);
                }
                else
                {
                    toReturn.MiddleInitial = "";
                }
            }
            else
            {
                toReturn.MiddleInitial = "";
            }

            toReturn.LastName = CommonFunctions.TrimSpaces(workingValue);

            return toReturn;
        }

        public static string FormatSSN(string toFormat)
        {
            string workingString = toFormat;

            if (workingString.Length > 3)
            {
                workingString = workingString.Insert(3, "-");

                if (workingString.Length > 6)
                {
                    workingString = workingString.Insert(6, "-");
                }
            }

            return workingString;
        }


        public struct FormattedName
        {
            public string FirstName;
            public string MiddleInitial;
            public string LastName;
        }

        /// <summary>
        /// Creates a string that is valid to use in folders and file names from a given date
        /// 9/18/09 12:00 becomes 9-18-09 12 00
        /// </summary>
        /// <param name="dateToUse"></param>
        /// <returns></returns>
        internal static object DateToFilePathFormat(DateTime dateToUse)
        {
            return dateToUse.ToString("yyyy-MM-dd") + " " + dateToUse.ToString("HH-mm");
        }

        internal static string ToMySQLDateTime(DateTime lastWrite)
        {
            return lastWrite.ToString("yyyy-MM-dd HH:mm:ss");
            
        }

        internal static string ToMySQLDate(DateTime lastWrite)
        {
            return lastWrite.Year + "-" +
                    lastWrite.Month.ToString("00") + "-" + lastWrite.Day.ToString("00");
        }

        internal static string ToSQLServerDateTime(DateTime val)
        {
            return val.ToString("yyyy-MM-dd HH:mm:ss");
        }
    }
}
