using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notepad_4.Models
{
    public class Direct : Notepad
    {
        public Direct(string name) : base(name)
        {
            SourceName = name;
            Image = "Assets/live-folder.png";
        }

        public Direct(DirectoryInfo directInfo) : base(directInfo.Name)
        {
            SourceName = directInfo.FullName;
            Image = "Assets/live-folder.png";
        }
    }
}
