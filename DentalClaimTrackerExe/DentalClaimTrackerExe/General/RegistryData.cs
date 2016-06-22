using System;
using System.Collections.Generic;
using System.Text;

namespace C_DentalClaimTracker.General
{
    class RegistryData
    {
        private static string _AppRegyPath = "Software\\TwinSparks\\Dental Claim Tracker";
        private const string COMPUTER_NAME = "computer_name";

        public static Microsoft.Win32.RegistryKey AppCuKey
        {
            get
            {
                Microsoft.Win32.RegistryKey _appCuKey;

                    _appCuKey = Microsoft.Win32.Registry.LocalMachine.OpenSubKey(_AppRegyPath, true);
                    if (_appCuKey == null)
                        _appCuKey = Microsoft.Win32.Registry.LocalMachine.CreateSubKey(_AppRegyPath);
                
                return _appCuKey;
            }
        }


        private static string GetStringFromRegistry(string key)
        {
            return (string)AppCuKey.GetValue(key);
        }

        private static void SaveStringToRegistry(string key, string value)
        {
            AppCuKey.SetValue(key, value);
        }

        public static string LocalComputerName
        {
            get
            {
                try
                {
                    return GetStringFromRegistry(COMPUTER_NAME);
                }
                catch
                {
                    return "defaultname";
                }
            }
            set
            {
                SaveStringToRegistry(COMPUTER_NAME, value);
            }
        }


    }
}
