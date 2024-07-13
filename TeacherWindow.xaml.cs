using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using Microsoft.EntityFrameworkCore;

namespace WpfAppAlpha
{
    public partial class TeacherWindow : Window
    {
        private SchoolContext _context;
        private int _tno;

        public TeacherWindow(int tno)
        {
            InitializeComponent();
            _tno = tno;
            _context = new SchoolContext();
            LoadCourses();
        }

        private void LoadCourses()
        {
            try
            {
                var courses = _context.Course
                    .Where(c => c.Tno == _tno)
                    .Select(c => new CourseViewModel
                    {
                        Cno = c.Cno,
                        Cname = c.Cname,
                        Cstatus = c.Cstatus,
                        Ccredit = c.Ccredit
                    })
                    .ToList();
                CourseTeachingDataGrid.ItemsSource = courses;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"加载课程时出错: {ex.Message}", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        public void CourseTasksButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var openCourses = _context.Course
                    .Where(c => c.Tno == _tno && c.Cstatus == "已开课")
                    .Select(c => new CourseViewModel
                    {
                        Cno = c.Cno,
                        Cname = c.Cname,
                        Cstatus = c.Cstatus,
                        Ccredit = c.Ccredit
                    })
                    .ToList();
                TeachScheduleDataGrid.ItemsSource = openCourses;

            }
            catch (Exception ex)
            {
                MessageBox.Show($"加载教学任务时出错: {ex.Message}", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        
        private void ScoreQueryButton_Click(object sender, RoutedEventArgs e) 
        {
            var newWindow = new TeacherScoreQueryWindow(_tno);
            newWindow.Show();
        }
        
        private void CourseApplyButton_Click(object sender, RoutedEventArgs e)
        {
            var newWindow = new TeacherCourseApplyWindow(_tno);
            newWindow.Show();
        }

        private void UpdateCourseButton_Click(object sender, RoutedEventArgs e)
        {
            LoadCourses();
        }
        
        public class CourseViewModel
        {
            public int Cno { get; set; }
            public string Cname { get; set; }
            public string Cstatus { get; set; }
            public float Ccredit { get; set; }
        }
    }
}
