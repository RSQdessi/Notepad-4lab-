using Avalonia.Controls;
using Notepad_4.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notepad_4.Models
{
    public abstract class Notepad
    {
        public Notepad(string name) 
        {
            Header = name;
        }

        public string Header { get; set; }  

        public string Image { get; set; }   

        public string SourceName { get; set; }
    }
}
