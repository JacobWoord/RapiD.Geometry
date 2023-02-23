using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RapiD.Geometry.Models
{
    public static class Utils2D
    {


        public static string GetAppDataFolder()
        {
            string foldername = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\Net Designer\";
            if (!Directory.Exists(foldername))
                Directory.CreateDirectory(foldername);
            return foldername;
        }

      

    }
}



