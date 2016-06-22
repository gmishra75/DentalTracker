using System;
using System.Security.Cryptography;
using System.Text;
using System.Data;
using System.Collections.Generic;

namespace C_DentalClaimTracker
{
    public partial class user : DataObject
    {
        public user_preferences UserData
        {
            get
            {
                if (_userData == null)
                {
                    user_preferences up;
                    try
                    {
                        up = new user_preferences(id);
                    }
                    catch
                    {
                        up = new user_preferences();
                        up.PopulateDefaults(id);
                    }

                    _userData = up;


                }

                return _userData;
            }
        }

        public override string ToString()
        {
            return username;
        }

        public List<user> GetAllUsers
        {
            get
            {
                List<user> toReturn = new List<user>();

                DataTable users = Search("SELECT * FROM users WHERE id > 0 ORDER BY username");
                user userObject;

                foreach (DataRow aRow in users.Rows)
                {
                    userObject = new user();
                    userObject.Load(aRow);

                    toReturn.Add(userObject);
                }

                return toReturn;
            }
        }

        public List<user> GetActiveUsers
        {
            get
            {
                List<user> toReturn = new List<user>();

                DataTable users = Search("SELECT * FROM users WHERE id > 0 AND is_active = 1 ORDER BY username");
                user userObject;

                foreach (DataRow aRow in users.Rows)
                {
                    userObject = new user();
                    userObject.Load(aRow);

                    toReturn.Add(userObject);
                }

                return toReturn;
            }
        }

        public List<user> LoggedInUsers
        {
            get
            {
                List<user> toReturn = new List<user>();

                DataTable users = Search("SELECT * FROM users WHERE logged_in = 1");
                user userObject;

                foreach (DataRow aRow in users.Rows)
                {
                    userObject = new user();
                    userObject.Load(aRow);

                    toReturn.Add(userObject);
                }

                return toReturn;
            }
        }

        public bool SafeToImport
        {
            get { return LoggedInUsers.Count <= 1; }
        }

        public void Login()
        {
            ExecuteNonQuery("UPDATE users SET logged_in = 1 WHERE id = " + id);
        }

        public void Logout()
        {
            ExecuteNonQuery("UPDATE users SET logged_in = 0 WHERE id = " + id);
        }

        internal bool VerifyPassword(string passwordToCheck)
        {
            if (Hash(passwordToCheck) == password)
                return true;
            else
                return false;
        }

        public string Hash(string password)
        {
            if (password == "")
                return "";
            else
            {
                // Hash the password!
                string sSourceData = password;
                byte[] tmpSource;
                byte[] tmpHash;

                //Create a byte array from source data.
                tmpSource = ASCIIEncoding.ASCII.GetBytes(sSourceData);

                //Compute hash based on source data.
                tmpHash = new MD5CryptoServiceProvider().ComputeHash(tmpSource);

                int i;
                StringBuilder sOutput = new StringBuilder(tmpHash.Length);
                for (i = 0; i < tmpHash.Length; i++)
                {
                    sOutput.Append(tmpHash[i].ToString("X2"));
                }

                return sOutput.ToString();
            }
        }
    }
}
