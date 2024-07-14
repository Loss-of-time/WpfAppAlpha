using System.Windows;

namespace WpfAppAlpha
{
    public partial class TeacherInfoWindow : Window
    {
        private readonly SchoolContext _context;
        public Teacher Teacher { get; private set; }

        public TeacherInfoWindow(Teacher teacher)
        {
            InitializeComponent();
            _context = new SchoolContext();
            Teacher = teacher;

            // 设置控件的初始值
            TextBoxTno.Text = teacher.Tno.ToString();
            TextBoxName.Text = teacher.Tname;
            TextBoxSex.Text = teacher.Tsex;
            TextBoxPassword.Password = teacher.Tpassword;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            // 更新教师信息
            Teacher.Tname = TextBoxName.Text;
            Teacher.Tsex = TextBoxSex.Text;
            Teacher.Tpassword = TextBoxPassword.Password;

            _context.Teacher.Update(Teacher);
            _context.SaveChanges();

            DialogResult = true;
            Close();
        }
    }
}