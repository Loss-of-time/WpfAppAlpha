using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace WpfAppAlpha
{
    /// <summary>
    /// StudentCourseSelectWindow.xaml 的交互逻辑
    /// </summary>
    public partial class StudentCourseSelectWindow : Window
    {
        private SchoolContext _context;
        private int Sno;
        public StudentCourseSelectWindow(int Sno)
        {
            this.Sno = Sno;
            InitializeComponent();
            _context = new SchoolContext();
            LoadCourses();
        }

        private void LoadCourses()
        {
            var selectedCourses = (from cs in _context.CourseSelect
                                   join c in _context.Course on cs.Cno equals c.Cno
                                   join t in _context.Teacher on c.Tno equals t.Tno
                                   where cs.Sno == Sno
                                   select new 
                                   {
                                       c.Cno,
                                       c.Cname,
                                       c.Ccredit,
                                       t.Tname
                                   }).ToList();

            var selectedCourseIds = selectedCourses.Select(c => c.Cno).ToList();

            var availableCourses = _context.Course
                .Include(c => c.Teacher)
                .Where(c => !selectedCourseIds.Contains(c.Cno))
                .ToList();

            SelectedCoursesDataGrid.ItemsSource = selectedCourses;
            AvailableCoursesDataGrid.ItemsSource = availableCourses;
        }

        private void SelectButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedCourse = (Course)AvailableCoursesDataGrid.SelectedItem;
            if (selectedCourse != null)
            {
                var courseSelect = new CourseSelect { Cno = selectedCourse.Cno, Sno = Sno, CSstatus = 1 };
                _context.CourseSelect.Add(courseSelect);
                _context.SaveChanges();
                LoadCourses();
            }
        }

        private void RemoveButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedCourse = (Course)SelectedCoursesDataGrid.SelectedItem;
            if (selectedCourse != null)
            {
                var courseSelect = _context.CourseSelect.FirstOrDefault(cs => cs.Cno == selectedCourse.Cno && cs.Sno == Sno);
                if (courseSelect != null)
                {
                    _context.CourseSelect.Remove(courseSelect);
                    _context.SaveChanges();
                    LoadCourses();
                }
            }
        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            string searchText = SearchTextBox.Text;
            string searchType = (SearchComboBox.SelectedItem as ComboBoxItem)?.Content.ToString();

            //var selectedCourses = _context.CourseSelect
            //    .Include(cs => cs.Course)
            //    .ThenInclude(c => c.Teacher)
            //    .Where(cs => cs.Sno == Sno)
            //    .Select(cs => cs.Course)
            //    .ToList();
            var selectedCourseIds = (from cs in _context.CourseSelect
                                   join c in _context.Course on cs.Cno equals c.Cno
                                   where cs.Sno == Sno
                                   select c.Cno
                                   ).ToList();

            var availableCoursesQuery = (from cs in _context.CourseSelect
                                         join c in _context.Course on cs.Cno equals c.Cno
                                         join t in _context.Teacher on c.Tno equals t.Tno
                                         where !selectedCourseIds.Contains(c.Cno)
                                         select new
                                         {
                                             c.Cno,
                                             c.Cname,
                                             c.Ccredit,
                                             t.Tname,
                                         }).ToList();

            if (string.IsNullOrWhiteSpace(searchText))
            {
                AvailableCoursesDataGrid.ItemsSource = availableCoursesQuery.ToList();
                return;
            }

            if (searchType == "按课程名")
            {
                var searchedCourses = availableCoursesQuery.Where(c => c.Cname.Contains(searchText)).ToList();
                AvailableCoursesDataGrid.ItemsSource = searchedCourses;
            }
            else if (searchType == "按教师名")
            {
                var searchedCourses = availableCoursesQuery.Where(c => c.Tname.Contains(searchText)).ToList();
                AvailableCoursesDataGrid.ItemsSource = searchedCourses;
            }
        }
    }
}
