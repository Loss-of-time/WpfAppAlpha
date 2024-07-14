using System.Windows;

namespace WpfAppAlpha
{
    public partial class StudentInfoWindow : Window
    {
        private readonly SchoolContext _context;
        public Student Student { get; private set; }

        public StudentInfoWindow(Student student)
        {
            InitializeComponent();
            _context = new SchoolContext();
            Student = student;

            // 设置控件的初始值
            TextBoxSno.Text = student.Sno.ToString();
            TextBoxName.Text = student.Sname;
            TextBoxSex.Text = student.Ssex;
            TextBoxHome.Text = student.Shome;
            TextBoxYear.Text = student.Syear.ToString();
            TextBoxPassword.Password = student.Spassword;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            // 更新学生信息
            Student.Sname = TextBoxName.Text;
            Student.Ssex = TextBoxSex.Text;
            Student.Shome = TextBoxHome.Text;
            Student.Syear = int.Parse(TextBoxYear.Text);
            Student.Spassword = TextBoxPassword.Password;

            _context.Student.Update(Student);
            _context.SaveChanges();

            DialogResult = true;
            Close();
        }
    }
}