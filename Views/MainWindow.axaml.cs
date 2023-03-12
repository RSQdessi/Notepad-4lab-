using Avalonia.Controls;
using Avalonia.Interactivity;
using Notepad_4.ViewModels;

namespace Notepad_4.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainWindowViewModel();
        }

        public void DoubleTap(object sender, RoutedEventArgs e)
        {

            var mvm = (MainWindowViewModel?)DataContext;

            if (mvm == null) return;

            var src = e.Source;
            if (src == null) return;

            var name = src.GetType().Name;
            if (name == "Image" || name == "ContentPresenter" || name == "TextBlock") mvm.DoubleTap();
        }
    }
}