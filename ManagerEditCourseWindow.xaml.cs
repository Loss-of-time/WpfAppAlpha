using System.Linq;
using System.Windows;

namespace WpfAppAlpha
{
    public partial class ManagerEditCourseWindow : Window
    {
        private readonly SchoolContext _context;
        public Course Course { get; private set; }

        public ManagerEditCourseWindow(Course course)
        {
            InitializeComponent();
            _context = new SchoolContext(); // 初始化数据库上下文
            Course = course;

            // 加载学院和教师数据
            ComboBoxMajor.ItemsSource = _context.Major.ToList();
            ComboBoxTeacher.ItemsSource = _context.Teacher.ToList();

            // 设置控件的初始值
            TextBoxName.Text = course.Cname;
            ComboBoxMajor.SelectedValue = course.MajorNo;
            ComboBoxTeacher.SelectedValue = course.Tno;
            TextBoxType.Text = course.Ctype;
            TextBoxCredit.Text = course.Ccredit.ToString();
            TextBoxStatus.Text = course.Cstatus;
            TextBoxTerm.Text = course.Cterm;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            // 更新课程属性
            Course.Cname = TextBoxName.Text;
            Course.MajorNo = (int)ComboBoxMajor.SelectedValue;
            Course.Tno = (int)ComboBoxTeacher.SelectedValue;
            Course.Ctype = TextBoxType.Text;
            Course.Ccredit = float.Parse(TextBoxCredit.Text);
            Course.Cstatus = TextBoxStatus.Text;
            Course.Cterm = TextBoxTerm.Text;

            DialogResult = true;
            Close();
        }
    }
}