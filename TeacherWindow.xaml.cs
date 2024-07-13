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
    public class RelayCommand : ICommand
    {
        private readonly Action _execute;
        private readonly Func<bool> _canExecute;

        public RelayCommand(Action execute, Func<bool> canExecute = null)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute;
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public bool CanExecute(object parameter) => _canExecute == null || _canExecute();

        public void Execute(object parameter) => _execute();
    }
    public partial class TeacherWindow : Window, INotifyPropertyChanged
    {
        private SchoolContext _context;
        private int _tno;
        private ObservableCollection<CourseViewModel> _courses;
        private ObservableCollection<CourseViewModel> _teachingTasks;

        public ObservableCollection<CourseViewModel> Courses
        {
            get => _courses;
            set
            {
                _courses = value;
                OnPropertyChanged();
            }
        }
        public ObservableCollection<CourseViewModel> TeachingTasks
        {
            get => _teachingTasks;
            set
            {
                _teachingTasks = value;
                OnPropertyChanged();
            }
        }
        public ICommand LoadCoursesCommand { get; private set; }
        public ICommand OpenTeacherScoreQueryWindowCommand { get; private set; }  // 新添加的命令
        public ICommand LoadTeachingTasksCommand { get; private set; }
        public TeacherWindow(int tno)
        {
            InitializeComponent();
            _tno = tno;
            DataContext = this;
            _context = new SchoolContext();
            LoadCoursesCommand = new RelayCommand(LoadCourses);
            OpenTeacherScoreQueryWindowCommand = new RelayCommand(OpenGradeEntryWindow);  // 初始化新命令
            LoadTeachingTasksCommand = new RelayCommand(LoadTeachingTasks);
            Courses = new ObservableCollection<CourseViewModel>();
            TeachingTasks = new ObservableCollection<CourseViewModel>();
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

                Courses.Clear();
                foreach (var course in courses)
                {
                    Courses.Add(course);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"加载课程时出错: {ex.Message}", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void LoadTeachingTasks()
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

                TeachingTasks.Clear();
                foreach (var course in openCourses)
                {
                    TeachingTasks.Add(course);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"加载教学任务时出错: {ex.Message}", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        
        private void OpenGradeEntryWindow()  // 新添加的方法
        {
            var gradeEntryWindow = new TeacherScoreQueryWindow(_tno);
            gradeEntryWindow.ShowDialog();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public class CourseViewModel
        {
            public int Cno { get; set; }
            public string Cname { get; set; }
            public string Cstatus { get; set; }
            public float Ccredit { get; set; }
        }
    }

    // CourseViewModel 和 RelayCommand 类保持不变
}
