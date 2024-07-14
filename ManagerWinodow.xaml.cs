using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace WpfAppAlpha
{
    public partial class ManagerWindow : Window
    {
        private readonly SchoolContext _context;

        public ManagerWindow()
        {
            InitializeComponent();
            _context = new SchoolContext();
            LoadTreeViewData();
            LoadTeacherData();
        }

        private void LoadTreeViewData()
        {
            var majors = _context.Major.ToList();
            foreach (var major in majors)
            {
                var majorNode = new TreeViewItem { Header = major.MajorName, Tag = "Major_" + major.MajorNo };
                TreeViewData.Items.Add(majorNode);

                var students = _context.Student.Where(s => s.MajorNo == major.MajorNo).ToList();
                foreach (var student in students)
                {
                    var studentNode = new TreeViewItem { Header = student.Sname, Tag = "Student_" + student.Sno };
                    majorNode.Items.Add(studentNode);
                }
            }
        }

        private void LoadTeacherData()
        {
            var teachers = _context.Teacher.ToList();
            TeacherDataGridData.ItemsSource = teachers;
        }

        private void TreeViewData_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            var selectedItem = TreeViewData.SelectedItem as TreeViewItem;

            if (selectedItem?.Tag is not string tag) return;

            if (tag.StartsWith("Major_"))
            {
                if (!int.TryParse(tag.AsSpan(6), out var majorNo)) return;
                var students = _context.Student.Where(s => s.MajorNo == majorNo).ToList();
                StudentDataGridData.ItemsSource = students;
            }
            else if (tag.StartsWith("Student_"))
            {
                if (!int.TryParse(tag[8..], out var sno)) return;
                var courses = (from s in _context.Student
                               where s.Sno == sno
                               join cs in _context.CourseSelect on s.Sno equals cs.Sno
                               join c in _context.Course on cs.Cno equals c.Cno
                               select c).ToList();
                StudentDataGridData.ItemsSource = courses;
            }
        }

        private void StudentDataGridData_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (StudentDataGridData.SelectedItem is not Student selectedStudent) return;
            var editWindow = new ManagerEditStudentWindow(selectedStudent);
            if (editWindow.ShowDialog() == true)
            {
                _context.SaveChanges();
                RefreshDataGrid();
            }
        }

        private void TeacherDataGridData_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (TeacherDataGridData.SelectedItem is not Teacher selectedTeacher) return;
            var editWindow = new ManagerEditTeacherWindow(selectedTeacher);
            if (editWindow.ShowDialog() == true)
            {
                _context.SaveChanges();
                LoadTeacherData(); // Refresh the teacher data grid after editing
            }
        }

        private void AddTeacherButton_Click(object sender, RoutedEventArgs e)
        {
            var newTeacher = new Teacher();
            var editWindow = new ManagerEditTeacherWindow(newTeacher);
            if (editWindow.ShowDialog() == true)
            {
                _context.Teacher.Add(newTeacher);
                _context.SaveChanges();
                LoadTeacherData(); // Refresh the teacher data grid after adding
            }
        }

        private void DeleteTeacherButton_Click(object sender, RoutedEventArgs e)
        {
            if (TeacherDataGridData.SelectedItem is not Teacher selectedTeacher) return;
            var result = MessageBox.Show($"确定要删除教师 {selectedTeacher.Tname} 吗？", "确认删除", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (result == MessageBoxResult.Yes)
            {
                _context.Teacher.Remove(selectedTeacher);
                _context.SaveChanges();
                LoadTeacherData(); // Refresh the teacher data grid after deleting
            }
        }

        // 刷新 DataGrid 的数据
        private void RefreshDataGrid()
        {
            if (TreeViewData.SelectedItem is not TreeViewItem selectedItem) return;
            if (selectedItem.Tag is not string tag) return;
            if (!tag.StartsWith("Student_")) return;

            if (!int.TryParse(tag[8..], out var sno)) return;

            var courses = (from s in _context.Student
                           where s.Sno == sno
                           join cs in _context.CourseSelect on s.Sno equals cs.Sno
                           join c in _context.Course on cs.Cno equals c.Cno
                           select c).ToList();
            StudentDataGridData.ItemsSource = courses;
        }
    }
}
