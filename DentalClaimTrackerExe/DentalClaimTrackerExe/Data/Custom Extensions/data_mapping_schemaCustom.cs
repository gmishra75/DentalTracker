using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Windows.Forms;

namespace C_DentalClaimTracker
{
    partial class data_mapping_schema
    {
        public List<data_mapping_schema_data> LinkedSchemaData
        {
            get
            {
                List<data_mapping_schema_data> toReturn = new List<data_mapping_schema_data>();
                data_mapping_schema_data aMapping;
                DataTable matches = Search("SELECT * FROM data_mapping_schema_data WHERE schema_id = " + id);

                foreach (DataRow aRow in matches.Rows)
                {
                    aMapping = new data_mapping_schema_data();
                    aMapping.Load(aRow);

                    toReturn.Add(aMapping);
                }

                return toReturn;
            }
        }


        /// <summary>
        /// Returns schema name
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return schema_name;
        }

        public string GetConnectionString(bool isOleDb)
        {
            string strConnectionString;
            if (data_type == DataMappingConnectionTypes.SQLServer)
            {
                if (isOleDb)
                {
                    // Initiate Connection
                    strConnectionString = "Provider=SQLOLEDB;uid=" + user_name +
                        ";Database=" + database_name + ";" +
                        "Server=" + server_name + ";pwd=";
                    strConnectionString += GetPassword();
                }
                else
                {
                    strConnectionString = "Server=" + server_name + ";Database=" + database_name +
                        ";User ID=" + user_name + ";Trusted_Connection=false;Password=" + GetPassword();
                }

            }
            else if (data_type == DataMappingConnectionTypes.MySQL)
            {
                throw new Exception("This connection type (" + data_type.ToString() + ") is not supported yet.");

                // Initiate Connection
                /*
                strConnectionString = "Driver={MySQL OleDb 5.1 Driver};Server=" + server_name + ";user=" + user_name;

                if (allow_password_save)
                {
                    strConnectionString += ";password=" + pw;
                }
                else
                {
                    MessageBox.Show("Password saving not enabled. Using blank password.");
                    strConnectionString += ";password=" + pw;
                }

                strConnectionString += ";database=" + database_name + ";";
                */
            }
            else
            {
                throw new Exception("This connection type (" + data_type.ToString() + ") is not supported yet.");
            }

            return strConnectionString;
        }

        private string GetPassword()
        {
            if (allow_password_save)
            {
                return pw;
            }
            else
            {
               // MessageBox.Show("Password saving not enabled. Using blank password.");
                return "";
            }
        }

        public static data_mapping_schema GetDefaultSchema
        {
            get
            {
                // Current hard-coded, but at some point should allow this to be set.
                return new data_mapping_schema(3);
            }
        }

        
    }
}
