using System.Windows;
using Connect4.Interop;
using Connect4.Wpf.ViewModels;

namespace Connect4.Wpf;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        DataContext = new MainViewModel(new NativeConnect4Engine());
    }
}
