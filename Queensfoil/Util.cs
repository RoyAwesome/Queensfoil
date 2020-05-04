using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Queensfoil
{
    public static class Util
    {
        public delegate void Destiny2FolderSet();

        public static event Destiny2FolderSet OnDestiny2FolderSet;

        public static void SetDestiny2Folder(string folder)
        {
            if(!IsValidDestiny2Folder(folder))
            {
                return;
            }

            Properties.Settings.Default.DestinyPath = folder;
            Properties.Settings.Default.Save();

            OnDestiny2FolderSet?.Invoke();
        }

        public static bool IsValidDestiny2Folder(string folder)
        {
            if (File.Exists(Path.Combine(folder, "destiny2.exe")))
            {
                return true;
            }

            return false;
        }

        public static bool HasValidD2Directory
        {
            get
            {
                return IsValidDestiny2Folder(Properties.Settings.Default.DestinyPath);
            }
        }

        public static string D2Path
        {
            get
            {
                return Properties.Settings.Default.DestinyPath;
            }
        }
    }
}
