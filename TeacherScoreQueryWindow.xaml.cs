using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using Microsoft.EntityFrameworkCore;


namespace WpfAppAlpha
{
    public partial class TeacherScoreQueryWindow : Window, INotifyPropertyChanged
    {
        private readonly SchoolContext _context;
        private readonly int _tno;
        private ObservableCollection<CourseViewModel> _courses;
        private ObservableCollection<StudentGradeViewModel> _students;
        private CourseViewModel _selectedCourse;

        public ObservableCollection<CourseViewModel> Courses
        {
            get => _courses;
            set
            {
                _courses = value;
                OnPropertyChanged(nameof(Courses));
            }
        }

        public ObservableCollection<StudentGradeViewModel> Students
        {
            get => _students;
            set
            {
                _students = value;
                OnPropertyChanged(nameof(Students));
            }
        }

        public CourseViewModel SelectedCourse
        {
            get => _selectedCourse;
            set
            {
                _selectedCourse = value;
                OnPropertyChanged(nameof(SelectedCourse));
                LoadStudentsForCourse();
            }
        }

        public TeacherScoreQueryWindow(int tno)
        {
            InitializeComponent();
            DataContext = this;
            _tno = tno;
            _context = new SchoolContext();
            LoadCourses();
        }
        public class CourseViewModel
        {
            public int Cno { get; set; }
            public string Cname { get; set; }
            public float Ccredit { get; set; }
            public string Cstatus { get; set; }
            public int StudentCount { get; set; }
        }
        private void LoadCourses()
        {
            var courses = _context.Course
                .Where(c => c.Tno == _tno)
                .Select(c => new CourseViewModel
                {
                    Cno = c.Cno,
                    Cname = c.Cname,
                    Ccredit = c.Ccredit,
                    StudentCount = _context.CourseSelect.Count(cs => cs.Cno == c.Cno)
                })
                .ToList();

            Courses = new ObservableCollection<CourseViewModel>(courses);
        }

        private void LoadStudentsForCourse()
        {
            if (SelectedCourse == null) return;

            var students = _context.CourseSelect
                .Where(cs => cs.Cno == SelectedCourse.Cno)
                .Select(cs => new StudentGradeViewModel
                {
                    Sno = cs.Sno,
                    Sname = cs.Student.Sname,
                    ClassId = cs.Student.MajorNo,  // Assuming MajorNo is used as ClassId
                    Grade = cs.CSstatus  // Assuming CSstatus is used to store the grade
                })
                .ToList();

            Students = new ObservableCollection<StudentGradeViewModel>(students);
        }

        private void SubmitGrades_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedCourse == null || Students == null)
            {
                MessageBox.Show("请先选择课程并确保有学生数据。", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            try
            {
                foreach (var student in Students)
                {
                    var courseSelect = _context.CourseSelect
                        .FirstOrDefault(cs => cs.Cno == SelectedCourse.Cno && cs.Sno == student.Sno);

                    if (courseSelect != null)
                    {
                        courseSelect.CSstatus = student.Grade;
                    }
                }

                _context.SaveChanges();
                MessageBox.Show("成绩已成功提交。", "成功", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"提交成绩时发生错误: {ex.Message}", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class StudentGradeViewModel
    {
        public int Sno { get; set; }
        public string Sname { get; set; }
        public int ClassId { get; set; }
        public int Grade { get; set; }
    }
}