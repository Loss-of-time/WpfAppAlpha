using System.Windows;

namespace WpfAppAlpha
{
    public partial class ManagerEditTeacherWindow : Window
    {
        private readonly Teacher _teacher;

        public ManagerEditTeacherWindow(Teacher teacher)
        {
            InitializeComponent();
            _teacher = teacher;
            TextBoxName.Text = _teacher.Tname;
            TextBoxSex.Text = _teacher.Tsex;
            TextBoxPassword.Text = _teacher.Tpassword;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            _teacher.Tname = TextBoxName.Text;
            _teacher.Tsex = TextBoxSex.Text;
            _teacher.Tpassword = TextBoxPassword.Text;
            DialogResult = true;
            Close();
        }
    }
}
