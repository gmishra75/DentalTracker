using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Data;

namespace C_DentalClaimTracker
{
    public partial class appointment_audit : DataObject
    {
        public enum ChangeTypes
        {
            Created = 1,
            Deleted = 2,
            Broken = 3,
            Time_Operatory = 4,
            Status = 5,
            Other = 6,
            Unbroken = 7,
            CreatedBroken = 8
        }

        public List<Appointment_Audit_Change> ChangeList
        {
            get
            {
                List<Appointment_Audit_Change> allChanges = new List<Appointment_Audit_Change>();

                if (change_type == ChangeTypes.Created || change_type == ChangeTypes.CreatedBroken || change_type == ChangeTypes.Deleted)
                {
                    // Don't need to list changes if they are new or deleted
                }
                else
                {
                    // Need to list all changes
                    Type myType = (typeof(appointment_audit));
                    // Get the public methods.
                    PropertyInfo[] myArrayMethodInfo = myType.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);


                    foreach (PropertyInfo aProperty in myArrayMethodInfo)
                    {
                        if (aProperty.Name.StartsWith("n_") || aProperty.Name.Contains("DDB_LAST_MOD"))
                        {
                            // ignore

                        }
                        else
                        {
                            if (this.GetType().GetProperty("n_" + aProperty.Name) != null)
                            {
                                try
                                {
                                    // check for change
                                    var val1 = this[aProperty.Name];
                                    var val2 = this["n_" + aProperty.Name];
                                    
                                    if (val1.ToString() != val2.ToString())
                                    {
                                        allChanges.Add(new Appointment_Audit_Change(aProperty.Name, val1.ToString(), val2.ToString()));
                                    }
                                }
                                catch
                                {
                                    LoggingHelper.Log("An error occurred retrieving the change list in appointment_audit.", LogSeverity.Error);
                                    break;
                                }
                            }
                        }

                    }
                }

                return allChanges;
            }
        }

        public ChangeTypes DiscoverChangeType()
        {
            // Status List (broken = unscheduled)
            // Created - Broken - appt_id is blank, apptdate is blank
            // Created - appt_id is blank (all initial fields will also be blank)
            // Deleted - new_appt_id is blank (all new fields will also be blank)
            // Broken - new apptdate is blank
            // Unbroken - apptdate was blank, and has a new apptdate
            // Time/Operatory change change to apptdate, apptlen, time_hour, time_min, operator_id
            // Status change - change to status
            // Other change - all others

            ChangeTypes returnValue;
            

            if (CommonFunctions.DBNullToZero(this["APPTID"]) == 0)
            {
                // Just created, check for broken
                if (this["APPTDATE"] == DBNull.Value)
                    returnValue = ChangeTypes.CreatedBroken;
                else
                    returnValue = ChangeTypes.Created;
            }
            else if (CommonFunctions.DBNullToZero(this["n_APPTID"]) == 0)
            {
                returnValue = ChangeTypes.Deleted;
            }
            else if (this["n_APPTDATE"] == DBNull.Value)
            {
                returnValue = ChangeTypes.Broken;
            }
            else if (this["APPTDATE"] == DBNull.Value && this["n_APPTDATE"] != DBNull.Value)
            {
                returnValue = ChangeTypes.Unbroken;
            }
            else if (ValueChanged("APPTDATE",true) || ValueChanged("APPTLEN") || ValueChanged("TIME_HOUR")
                || ValueChanged("TIME_MINUTE") || ValueChanged("OPID"))
            {
                returnValue = ChangeTypes.Time_Operatory;
            }
            else if (ValueChanged("STATUS"))
            {
                returnValue = ChangeTypes.Status;
            }
            else
            {
                returnValue = ChangeTypes.Other;
            }


            

            return returnValue;
        }

        private bool ValueChanged(string startVal, bool isDate = false)
        {
            return CommonFunctions.DBNullToString(this[startVal]) != CommonFunctions.DBNullToString(this["n_" + startVal]);   
        }

        public List<string> UserList
        {
            get
            {
                List<string> userList = new List<string>();

                DataTable dt = Search("SELECT DISTINCT(USER_CHANGED) FROM appointment_audit");

                foreach (DataRow aUser in dt.Rows)
                {
                    userList.Add(aUser[0].ToString());
                }

                return userList;
            }
        }
    }

    public struct Appointment_Audit_Change
    {
        public string ColumnName;
        public string StartValue;
        public string EndValue;

        public Appointment_Audit_Change(string col, string start, string end)
        {
            ColumnName = col;
            StartValue = start;
            EndValue = end;
        }

        public override string ToString()
        {
            return string.Format("{0}: {1} -> {2}", ColumnName, StartValue, EndValue);
        }
    }
}
