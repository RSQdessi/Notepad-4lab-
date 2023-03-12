using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notepad_4.Models
{
    public class Files : Notepad
    {
        public Files(string name) : base(name)
        {
            SourceName = name;
            Image = "Assets/files.png";
        }

        public Files(FileInfo fileInfo) : base(fileInfo.Name)
        {
            SourceName = fileInfo.FullName;
            Image = "Assets/files.png";
        }
    }
}
    