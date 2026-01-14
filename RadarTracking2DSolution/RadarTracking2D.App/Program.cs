using RadarTracking2D.WPF.Views;
using System.Windows;

namespace RadarTracking2D.App;

static class Program
{
    [STAThread]
    static void Main()
    {
        var app = new Application();
        var window = new MainWindow();
        app.Run(window);  // this is where the UI and timer in MainWindow are started
    }
}

// more comments
// tests
// improvements in visuals