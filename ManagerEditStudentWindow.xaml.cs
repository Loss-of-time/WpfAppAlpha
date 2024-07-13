using System.Windows;

namespace WpfAppAlpha
{
    public partial class ManagerEditStudentWindow : Window
    {
        public Student Student { get; private set; }

        public ManagerEditStudentWindow(Student student)
        {
            InitializeComponent();
            Student = student;

            TextBoxName.Text = student.Sname;
            TextBoxSex.Text = student.Ssex;
            TextBoxHome.Text = student.Shome;
            TextBoxYear.Text = student.Syear.ToString();
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            Student.Sname = TextBoxName.Text;
            Student.Ssex = TextBoxSex.Text;
            Student.Shome = TextBoxHome.Text;
            Student.Syear = int.Parse(TextBoxYear.Text);

            DialogResult = true;
            Close();
        }
    }
}