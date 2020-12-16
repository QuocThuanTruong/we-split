using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeSplit.Converter;

namespace WeSplit.Utilities
{
    class AppUtilities
    {
        private static AppUtilities _appInstance;
        private AbsolutePathConverter _absolutePathConverter = new AbsolutePathConverter();

        private AppUtilities() {}
        public static AppUtilities GetAppInstance()
        {
            if (_appInstance == null)
            {
                _appInstance = new AppUtilities();
            }
            else
            {
                //Do Nothing
            }

            return _appInstance;
        }

        public void createIDDirectory(int ID)
        {
            string path = (string)(_absolutePathConverter.Convert($"Images/Journeys/{ID}", null, null, null));

            if (Directory.Exists(path))
            {
                //Do Nothing
            }
            else
            {
                Directory.CreateDirectory(path);
            }
        }

        public void createSitesDirectory()
        {
            string path = (string)(_absolutePathConverter.Convert($"Images/Sites", null, null, null));

            if (Directory.Exists(path))
            {
                //Do Nothing
            }
            else
            {
                Directory.CreateDirectory(path);
            }
        }

        public void copyImageToIDirectory(int ID, string srcPath, string nameFile, bool isSite)
        {
            var destPath = "";

            if (isSite)
            {
                destPath = (string)_absolutePathConverter.Convert($"Images/Sites/{ID}.{getTypeOfImage(srcPath)}", null, null, null);
            } else
            {
                destPath = (string)_absolutePathConverter.Convert($"Images/Journeys/{ID}/{nameFile}.{getTypeOfImage(srcPath)}", null, null, null);
            }

            File.Copy(srcPath, destPath, true);
        }

        public string getTypeOfImage(string uriImage)
        {
            string result = "";

            int index = uriImage.Length - 1;

            while (uriImage[index] != '.')
            {
                result = uriImage[index--] + result;
            }

            return result;
        }
    }
}
