using System.Linq;
using System.Windows;

namespace WpfAppAlpha
{
    public partial class ManagerEditStudentWindow : Window
    {
        private readonly SchoolContext _context;
        public Student Student { get; private set; }

        public ManagerEditStudentWindow(Student student)
        {
            InitializeComponent();
            _context = new SchoolContext(); // 初始化数据库上下文
            Student = student;

            // 加载学院数据
            ComboBoxMajor.ItemsSource = _context.Major.ToList();

            // 设置控件的初始值
            TextBoxSno.Text = student.Sno.ToString();
            TextBoxName.Text = student.Sname;
            TextBoxSex.Text = student.Ssex;
            TextBoxHome.Text = student.Shome;
            TextBoxYear.Text = student.Syear.ToString();
            TextBoxPassword.Text = student.Spassword;
            ComboBoxMajor.SelectedValue = student.MajorNo;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            // 更新学生属性
            Student.Sname = TextBoxName.Text;
            Student.Ssex = TextBoxSex.Text;
            Student.Shome = TextBoxHome.Text;
            Student.Syear = int.Parse(TextBoxYear.Text);
            Student.Spassword = TextBoxPassword.Text;
            Student.MajorNo = (int)ComboBoxMajor.SelectedValue;

            DialogResult = true;
            Close();
        }
    }
}