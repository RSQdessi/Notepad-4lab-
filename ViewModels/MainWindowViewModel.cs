using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Styling;
using DynamicData.Binding;
using Notepad_4.Models;
using ReactiveUI;
using System;
using System.Collections.ObjectModel;
using System.Dynamic;
using System.IO;
using System.Text;
using static System.Net.WebRequestMethods;

namespace Notepad_4.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {

        private ObservableCollection<Notepad> NotepadColl;

        public ObservableCollection<Notepad> NotepadList {  get => NotepadColl;  set => this.RaiseAndSetIfChanged(ref NotepadColl, value); }

        private int curInd;

        

        private bool visibleNote;

        bool visibilityNote { get => visibleNote; set => this.RaiseAndSetIfChanged(ref visibleNote, value); }


        private bool visibleExp;

        bool visibilityExp { get => visibleExp; set => this.RaiseAndSetIfChanged(ref visibleExp, value); }


        private string path = Directory.GetCurrentDirectory();

        private bool visible_open, visible_save;

        private string TextInBox, TextInFolder;

        private string SaveButtonText = string.Empty;

        public MainWindowViewModel() 
        {
            visibleNote = true;
            visibleExp = false;
            visible_open = false;
            visible_save = false;
            TextInBox = string.Empty;
            TextInFolder = string.Empty;
            SaveButtonText = "Открыть";


            NotepadColl = new ObservableCollection<Notepad>();

            //NotepadColl.Clear();

            GetPath(path);
        }

        public string TextDop
        {
            get => TextInFolder;
            set
            {
                this.RaiseAndSetIfChanged(ref TextInFolder, value);
                if (TextInFolder != "") ButtonText = "Сохранить";
            }
        }

        public string ButtonText
        {
            get => SaveButtonText;
            set
            {
                this.RaiseAndSetIfChanged(ref SaveButtonText, value);
            }
        }

        public string Text
        {
            get => TextInBox;
            set
            {
                this.RaiseAndSetIfChanged(ref TextInBox, value);
            }
        }


        public int CurrentIndexProp
        {
            get => curInd;
            set
            {
                this.RaiseAndSetIfChanged(ref curInd, value);
                if (visible_open == true && visible_save == false)
                {
                    if (NotepadColl[curInd] is Files) TextDop = NotepadColl[curInd].Header;
                    else TextDop = "";
                    ButtonText = "Открыть";
                }
                else if (visible_save == true && visible_open == false)
                {
                    if (NotepadColl[curInd] is Files)
                    {
                        ButtonText = "Сохранить";
                        TextDop = NotepadColl[curInd].Header;
                    }
                    else
                    {
                        ButtonText = "Открыть";
                        TextDop = "";
                    }
                }
            }
        }


        public bool VisibleOpen
        {
            get => visible_open;
            set
            {
                this.RaiseAndSetIfChanged(ref visible_open, value);
            }
        }

        public bool VisibleSave
        {
            get => visible_save;
            set
            {
                this.RaiseAndSetIfChanged(ref visible_save, value);
            }
        }

        public void returnBack()
        {
            TextInFolder = string.Empty;
            visibilityNote = true;
            visibilityExp = false;
            VisibleOpen = false;
            VisibleSave = false;
        }


        public void openExpl()
        {
            TextDop = string.Empty;
            visibilityExp = true;
            visibilityNote = false;
            VisibleOpen = true;
            VisibleSave = false;
        }

        public void saveExpl()
        {
            TextDop = "";
            CurrentIndexProp = 0;
            visibilityExp = true;
            visibilityNote = false;
            VisibleOpen = false;
            VisibleSave = true;
        }

        public void ButtonClick()
        {
            if (VisibleOpen == true) ButtonOpen();
            else if (VisibleSave == true) ButtonSave();
        }


        public void ButtonOpen()
        {
            if (NotepadColl[CurrentIndexProp] is Direct)
            {
                if (NotepadColl[CurrentIndexProp].Header == "..")
                {
                    var patt = Directory.GetParent(path);
                    if (patt != null)
                    {
                        GetPath(patt.FullName);
                        path = patt.FullName;
                    }
                    else if (patt == null) GetPath("");
                }
                else
                {
                    var temp_path = NotepadColl[curInd].SourceName;
                    GetPath(NotepadColl[CurrentIndexProp].SourceName);
                    path = temp_path;
                }
            }
            else
            {
                FileLoad(NotepadColl[curInd].SourceName);
                returnBack();
            }
        }


        public void ButtonSave()
        {
            if (NotepadColl[CurrentIndexProp] is Direct && TextDop == "")
            {
                ButtonText = "Открыть";
                if (NotepadColl[CurrentIndexProp].Header == "..")
                {
                    var patt = Directory.GetParent(path);
                    if (patt != null) GetPath(patt.FullName);
                    else if (patt == null) GetPath("");
                    path = patt.FullName;
                }
                else
                {
                    var temp_path = NotepadColl[curInd].SourceName;
                    GetPath(NotepadColl[CurrentIndexProp].SourceName);
                    path = temp_path;
                }
            }
            else if (NotepadColl[CurrentIndexProp] is Files || TextDop != "")
            {
                if (TextDop == NotepadColl[curInd].Header) FileSave(NotepadColl[CurrentIndexProp].SourceName, 0);
                else
                {
                    var temp_pat = path;
                    temp_pat += "\\" + TextDop;
                    FileSave(temp_pat, 1);
                }
                returnBack();
            }
        }


        public void FileLoad(string ppath)
        {
            string new_text = String.Empty;
            StreamReader sr = new StreamReader(NotepadColl[curInd].SourceName);
            while (sr.EndOfStream != true)
            {
                new_text += sr.ReadLine() + "\n";
            }
            Text = new_text;
        }

        public async void FileSave(string ppath, int flag)
        {
            if (flag == 0)
            {
                using (StreamWriter writer = new StreamWriter(ppath))
                {
                    writer.Write(Text);
                }
            }
            else
            {
                using (FileStream fstream = new FileStream(ppath, FileMode.OpenOrCreate))
                {
                    byte[] buffer = Encoding.Default.GetBytes(Text);
                    await fstream.WriteAsync(buffer, 0, buffer.Length);
                }
            }
        }


        public void GetPath(string warPath) 
        {
            NotepadColl.Clear();
            if (warPath != "")
            {
                var dirinfo = new DirectoryInfo(warPath);
                NotepadColl.Add(new Direct(".."));
                foreach (var directory in dirinfo.GetDirectories())
                {
                    NotepadColl.Add(new Direct(directory));
                }
                foreach (var fileinfo in dirinfo.GetFiles())
                {
                    NotepadColl.Add(new Files(fileinfo));
                }
            }
            else if (warPath == "")
            {
                foreach (var disk in Directory.GetLogicalDrives())
                {
                    NotepadColl.Add(new Direct(disk));
                }
            }
            CurrentIndexProp = 0;
        }


        public void DoubleTap()
        {
            if (VisibleOpen == true) ButtonOpen();
            else if (VisibleSave == true) ButtonSave();
        }
    }
}