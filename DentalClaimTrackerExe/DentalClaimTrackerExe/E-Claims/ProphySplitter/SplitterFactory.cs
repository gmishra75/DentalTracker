using System;
using System.Data.SqlClient;

namespace NHDG.ProphySplitter {
	/// <summary>A class that produces splitter objects.</summary>
	public class SplitterFactory {
		/// <summary>Builds a splitter object based on the parameters.</summary>
		/// <param name="conn">The database connection to retreive the rule details from.</param>
		/// <param name="splitRuleID">The database ID of the rule this Splitter represents.</param>
		/// <param name="ruleType">The type of rule this splitter should implement.</param>
		/// <param name="procedureCode">The ADA procedure code this splitter operates on.</param>
		/// <param name="carrier">The insurance carrier this splitter is for.</param>
		/// <returns>An initialized splitter object.</returns>
		static public Splitter GetSplitter(int splitRuleID, string ruleType, string procedureCode, string carrier) {
			Splitter s;
			
			switch (ruleType) {
				case "H":
					s = new HardCodeSplitter(splitRuleID, procedureCode, carrier);
					break;

				case "R":
					s = new RemainderSplitter(splitRuleID, procedureCode, carrier);
					break;

				case "P":
					s = new PercentageSplitter(splitRuleID, procedureCode, carrier);
					break;

				default:
					throw new ArgumentException("Invalid split rule type (" + ruleType + ").", "ruleType");
			}

			return s;
		}
	}
}
