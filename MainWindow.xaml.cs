using System;
using System.Linq;
using System.Windows;
using Microsoft.EntityFrameworkCore;

namespace WpfAppAlpha
{
    public partial class MainWindow : Window
    {
        private readonly SchoolContext _context;
        private const string ADMIN_USERNAME = "admin";
        private const string ADMIN_PASSWORD = "admin";

        public MainWindow()
        {
            InitializeComponent();
            _context = new SchoolContext();
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            string username = UsernameTextBox.Text;
            string password = PasswordBox.Password;
            string role = GetSelectedRole();

            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("请输入用户名和密码。", "登录错误", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            bool isAuthenticated = false;
            Window newWindow = null;

            switch (role)
            {
                case "Student":
                    var student = _context.Student.FirstOrDefault(s => s.Sno.ToString() == username && s.Spassword == password);
                    if (student != null)
                    {
                        isAuthenticated = true;
                        newWindow = new StudentWindow(student.Sno);
                    }
                    break;
                case "Teacher":
                    var teacher = _context.Teacher.FirstOrDefault(t => t.Tno.ToString() == username && t.Tpassword == password);
                    if (teacher != null)
                    {
                        isAuthenticated = true;
                        newWindow = new TeacherWindow(teacher.Tno);
                    }
                    break;
                case "Manager":
                    if (username == ADMIN_USERNAME && password == ADMIN_PASSWORD)
                    {
                        isAuthenticated = true;
                        newWindow = new ManagerWindow();
                    }
                    break;
            }

            if (isAuthenticated)
            {
                this.Hide();
                newWindow.Show();
                newWindow.Closed += (s, args) => this.Show();
            }
            else
            {
                MessageBox.Show("用户名或密码错误。", "登录失败", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        string GetSelectedRole()
        {
            if (StudentRadioButton.IsChecked == true)
                return "Student";
            if (TeacherRadioButton.IsChecked == true)
                return "Teacher";
            if (AdminRadioButton.IsChecked == true)
                return "Manager";
            return string.Empty;
        }
    }
}
