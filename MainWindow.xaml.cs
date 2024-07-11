using System.Windows;

namespace WpfAppAlpha
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            // Determine which radio button is checked
            string role = "";
            if (StudentRadioButton.IsChecked == true)
            {
                role = "Student";
            }
            else if (TeacherRadioButton.IsChecked == true)
            {
                role = "Teacher";
            }
            else if (AdminRadioButton.IsChecked == true)
            {
                role = "Manager";
            }

            // Navigate to the respective window
            Window newWindow = null;
            switch (role)
            {
                case "Student":
                    //newWindow = new StudentWindow(); // Ensure you have created a StudentWindow.xaml
                    break;
                case "Teacher":
                    //newWindow = new TeacherWindow(); // Ensure you have created a TeacherWindow.xaml
                    break;
                case "Manager":
                    newWindow = new ManagerWindow();
                    break;
            }

            if (newWindow != null)
            {
                this.Hide();
                newWindow.Show();
                newWindow.Closed += (s, args) => this.Show();
            }
        }
    }
}
