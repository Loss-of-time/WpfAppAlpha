using System;
using System.Linq;
using System.Windows;
using Microsoft.EntityFrameworkCore;

namespace WpfAppAlpha
{
    public partial class TeacherCourseApplyWindow : Window
    {
        private int teacherNo { get; }
        public TeacherCourseApplyWindow(int Tno)
        {
            InitializeComponent();
            LoadMajors();
            teacherNo = Tno;
        }

        private void LoadMajors()
        {
            using (var context = new SchoolContext())
            {
                var majors = context.Major.ToList();
                MajorComboBox.ItemsSource = majors;
                MajorComboBox.DisplayMemberPath = "MajorName";
                MajorComboBox.SelectedValuePath = "MajorNo";
            }
        }

        private void ApplyButton_Click(object sender, RoutedEventArgs e)
        {
            var courseName = CourseNameTextBox.Text;
            var majorNo = (int)MajorComboBox.SelectedValue;
            var courseType = CourseTypeTextBox.Text;
            var courseCredit = float.Parse(CourseCreditTextBox.Text);
            var courseTerm = CourseTermTextBox.Text;

            using (var context = new SchoolContext())
            {
                // 获取当前最大的Cno值
                // int maxCno = context.Course.Max(c => (int?)c.Cno) ?? 0;

                // 设置新课程的Cno值
                var newCourse = new Course
                {
                    // Cno = maxCno + 1, // 主键值递增
                    Cname = courseName,
                    MajorNo = majorNo,
                    Tno = teacherNo,
                    Ctype = courseType,
                    Ccredit = courseCredit,
                    Cterm = courseTerm,
                    Cstatus = "待审核" // 初始状态为“待审核”
                };

                context.Course.Add(newCourse);
                context.SaveChanges();
            }

            MessageBox.Show("Course application submitted successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            this.Close();
        }
    }
}