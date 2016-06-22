using System;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using System.Xml.Xsl;
using System.Diagnostics;

namespace NHDG.NHDGCommon {
	/// <summary>Miscellaneous utility functions.</summary>
	public class Utilities {
		/// <summary>Format a phone number so that it is suitable for display.</summary>
		/// <param name="phone">The phone number to format.</param>
		/// <returns>The formatted phone number.</returns>
		static public string FormatPhoneNumber(string phone) {
			string temp = StripNonNumeric(phone);

			if (temp.Length == 10) {
				// (NPA) NXX-LINE
				return "(" + temp.Substring(0, 3) + ") " + temp.Substring(3, 3) + "-" + temp.Substring(6, 4);
			} else if (temp.Length == 7) {
				// NXX-LINE
				return temp.Substring(0, 3) + "-" + temp.Substring(3, 4);
			} else {
				// Unknown length, return original string.
				return phone;
			}
		}


		/// <summary>Format a zip code so that it is suitable for display.</summary>
		/// <param name="zip">The zip code to format.</param>
		/// <returns>The formatted zip code.</returns>
		static public string FormatZipCode(string zip) {
			string temp = StripNonNumeric(zip);

			if (temp.Length == 5) {
				return temp;
			} else if (temp.Length == 9) {
				return temp.Substring(0,5) + "-" + temp.Substring(5,4);
			} else {
				return zip;
			}
		}


		/// <summary>Format a credit card so that it is suitable for display.</summary>
		/// <param name="cc">The credit card to format.</param>
		/// <returns>The formatted credit card.</returns>
		static public string FormatCreditCard(string cc) {
			string temp = StripNonNumeric(cc);

			if ((temp.Length == 16) && ((temp.Substring(0,1) == "4") || (temp.Substring(0,1) == "5") || (temp.Substring(0,1) == "6"))) {
				return temp.Substring(0,4) + "-" + temp.Substring(4,4) + "-" + temp.Substring(8,4) + "-" + temp.Substring(12,4);
			} else if ((temp.Length == 15) && (temp.Substring(0,1) == "3")) {
				return temp.Substring(0,4) + "-" + temp.Substring(4,6) + "-" + temp.Substring(10,5);
			} else {
				return cc;
			}
		}

        /// <summary>Format currency so that it is suitable for XML output (no dollar sign, no comma).</summary>
        /// <param name="amount">The currency to format.</param>
        /// <returns>The formatted currency.</returns>
        static public string FormatCurrencyForXML(int amount)
        {
            float tmpAmount = (float)amount / 100.0f;
            return tmpAmount.ToString("0.00");
        }

		/// <summary>Format currency so that it is suitable for display.</summary>
		/// <param name="amount">The currency to format.</param>
		/// <returns>The formatted currency.</returns>
		static public string FormatCurrency(int amount) {
			float tmpAmount = (float)amount / 100.0f;
			System.Globalization.NumberFormatInfo nfi = new System.Globalization.NumberFormatInfo();
			nfi.CurrencyNegativePattern = 2;
			nfi.CurrencySymbol = "$";
			return tmpAmount.ToString("c", nfi);
		}

		/// <summary>Take a formatted currency and translate it into Dentrix-currency.</summary>
		/// <param name="amount">The value to unformat.</param>
		/// <returns>The currency in Dentrix-form.</returns>
		static public int UnFormatCurrency(string amount) {
			string temp = StripNonNumeric(amount);
			if (amount.IndexOf("-") >= 0) {
				return int.Parse(temp) * -1;
			} else {
				return int.Parse(temp);
			}
		}


		/// <summary>Format a Dentrix-style time so that it is suitable for display.</summary>
		/// <param name="hour">The hour (24 hour clock).</param>
		/// <param name="min">The minutes.</param>
		/// <returns>The formatted time.</returns>
		static public string FormatTime(int hour, int min) {
			string hourStr;
			string minuteStr;
			string ampm;

			if (hour > 12) {
				hourStr = (hour - 12).ToString();
			} else {
				hourStr = hour.ToString();
			}

			if (min < 10) {
				minuteStr = "0" + min.ToString();
			} else {
				minuteStr = min.ToString();
			}

			if (hour < 12) {
				ampm = "am";
			} else {
				ampm = "pm";
			}

			return hourStr + ":" + minuteStr + ampm;
		}


		/// <summary>Remove all non-numeric characters from a string.</summary>
		/// <param name="input">The string to clean.</param>
		/// <returns>The string devoid of all non-numeric characters.</returns>
		static public string StripNonNumeric(string input) {
			string temp = input;

			if (input == null) { return null; }

			for (int i = (input.Length - 1); i >= 0; i--) {
				if (!char.IsDigit(input[i])) {
					temp = temp.Remove(i, 1);
				}
			}

			return temp;
		}


		/// <summary>Create a string that consists of a smaller string repeated multiple times.</summary>
		/// <param name="input">The string to repeat.</param>
		/// <param name="count">How many times to repeat it.</param>
		/// <returns>The resulting string.</returns>
		static public string Repeat(string input, int count) {
			string temp = string.Empty;

			for (int i = 0; i < count; i++) {
				temp += input;
			}

			return temp;
		}


		/// <summary>Truncate a string at a given length.</summary>
		/// <param name="input">The string to truncate.</param>
		/// <param name="len">The maximum length the result should be.</param>
		/// <returns>The truncated string.</returns>
		/// <remarks>If the input string is smaller in length than the provided length, the
		/// original string is returned unaltered.</remarks>
		static public string Truncate(string input, int len) {
			if (input.Length <= len) {
				return input;
			} else {
				return input.Substring(0, len);
			}
		}


		/// <summary>Performs a MOD 10 Hash Check on the input to determine if the CC is valid.</summary>
		/// <param name="input">The credit card number to check.</param>
		/// <returns>Whether or not the credit card number is valid.</returns>
		static public bool ValidateCreditCard(string input) {
			string temp = StripNonNumeric(input);
			int sum = 0;
			int digit;
			bool even = true;

			if ((temp == null) || (temp.Length < 1)) { return false; }

			if (temp.Substring(temp.Length-8,8) == "00000000") { return true; }
			
			for (int i = (temp.Length - 1); i >= 0; i--) {
				digit = int.Parse(temp[i].ToString());
				even = !even;
				if (even) { digit *= 2; }
				
				if (digit > 9) {
					digit = 1 + (digit - 10);
				}

				sum += digit;
			}

			return (sum % 10 == 0);
		}


		/// <summary>Serializes an object into an XML file.</summary>
		/// <param name="obj">The object to serialize.</param>
		/// <param name="type">The object's type.</param>
		/// <param name="filename">The file to write to.</param>
		static public void SerializeToFile(object obj, System.Type type, string filename) {
			XmlSerializer xmlser = new XmlSerializer(type);
			TextWriter writer = new StreamWriter(filename, false, System.Text.Encoding.UTF8);
			xmlser.Serialize(writer, obj);
			writer.Close();
		}


		/// <summary>Deserializes an object from an XML file.</summary>
		/// <param name="type">The object's type.</param>
		/// <param name="filename">The file to read from.</param>
		/// <returns>The deserialized object.</returns>
		static public object DeserializeFromFile(System.Type type, string filename) {
			object temp;
			XmlSerializer xmlser = new XmlSerializer(type);
			FileStream fs = new FileStream(filename, FileMode.Open, System.IO.FileAccess.Read, System.IO.FileShare.Read);
			temp = xmlser.Deserialize(fs);
			fs.Close();
			return temp;
		}


		/// <summary>Applies a stylesheet to a serialized object and then prints it as HTML.</summary>
		/// <param name="obj">The object to transform and print.</param>
		/// <param name="type">The object's type.</param>
		/// <param name="stylesheet">The path to the stylesheet to use.</param>
		static public void TransformAndPrintHTML(object obj, System.Type type, string stylesheet) {
			MemoryStream ms = new MemoryStream();
			XmlDocument xml = new XmlDocument();
			XmlSerializer xmlSer = new XmlSerializer(type);
			XslTransform transformer  = new XslTransform();
			string fileName = Path.GetTempFileName();
			ProcessStartInfo psi = new ProcessStartInfo();
			Process proc;

			// Serialize it.
			xmlSer.Serialize(ms, obj);
			ms.Position = 0;
			xml.Load(ms);
			ms.Close();

			// Apply transformation.
			transformer.Load(stylesheet);
			FileInfo fileInfo = new FileInfo(fileName);
			fileName = Path.ChangeExtension(fileName, ".html");
			fileInfo.MoveTo(fileName);

			// Put the HTML into a temp file.
			FileStream fs = new FileStream(fileName, FileMode.Open);
			StreamWriter writer = new StreamWriter(fs);
			transformer.Transform(xml, null, writer);
			writer.Close();
			fs.Close();

			// Print it.
			psi.FileName = AppSettings.SettingsManager.Instance.Settings.Printing.HTMLCommand;
			psi.Arguments = AppSettings.SettingsManager.Instance.Settings.Printing.HTMLArguments + " \"" + fileName + "\"";
			proc = Process.Start(psi);
			proc.WaitForExit();
			proc.Close();
			fileInfo.Delete();
		}
	}
}
