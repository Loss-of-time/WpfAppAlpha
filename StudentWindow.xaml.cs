using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Windows;

namespace WpfAppAlpha
{
    public partial class StudentWindow : Window
    {
        private SchoolContext _context;
        private int _sno;

        public StudentWindow(int sno)
        {
            InitializeComponent();
            _sno = sno;
            _context = new SchoolContext();
        }

        private void CourseQueryButton_Click(object sender, RoutedEventArgs e)
        {
            var courses = _context.CourseSelect
                          .Where(cs => cs.Sno == _sno)
                          .Select(cs => cs.Cno)
                          .Join(_context.Course,
                                cno => cno,
                                course => course.Cno,
                                (cno, course) => course)
                          .ToList();
            CourseScheduleDataGrid.ItemsSource = courses;
        }

        private void CourseSelectButton_Click(object sender, RoutedEventArgs e)
        {
            StudentCourseSelectWindow studentCourseSelectWindow = new StudentCourseSelectWindow(_sno);
            studentCourseSelectWindow.Closed += (s, args) => this.Show();
            this.Hide();
            studentCourseSelectWindow.Show();
        }

        private void ScoreQueryButton_Click(object sender, RoutedEventArgs e)
        {
            var coursesWithGrades = _context.CourseSelect
                .Where(cs => cs.Sno == _sno)
                .Select(cs => new
                {
                    cs.Course.Cno,
                    cs.Course.Cname,
                    cs.CSstatus,
                    cs.Course.Ccredit,
                    cs.CSscore
                })
                .ToList();
            CourseScheduleDataGrid.ItemsSource = coursesWithGrades;
        }

        private void PersonalInfoButton_Click(object sender, RoutedEventArgs e)
        {
            var student = _context.Student.FirstOrDefault(s => s.Sno == _sno);
            if (student != null)
            {
                var infoWindow = new StudentInfoWindow(student);
                infoWindow.ShowDialog();
            }
            else
            {
                MessageBox.Show("未找到该学生的信息", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
