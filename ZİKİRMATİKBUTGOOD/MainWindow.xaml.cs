using System.Windows;

namespace ZİKİRMATİKBUTGOOD
{
    public partial class MainWindow : Window
    {
        // This is where we store the current count
        private int zikirCount = 0;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void ZikirButton_Click(object sender, RoutedEventArgs e)
        {
            zikirCount++;
            ZikirLabel.Text = zikirCount.ToString();
        }

        private void ResetButton_Click(object sender, RoutedEventArgs e)
        {
            zikirCount = 0;
            ZikirLabel.Text = "0";
        }
    }
}
