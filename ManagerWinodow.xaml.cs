using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace WpfAppAlpha
{
    public partial class ManagerWindow : Window
    {
        private SchoolContext _context;

        public ManagerWindow()
        {
            InitializeComponent();
            _context = new SchoolContext();
            LoadTreeViewData();
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

        private void TreeViewData_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            var selectedItem = TreeViewData.SelectedItem as TreeViewItem;
            if (selectedItem == null) return;

            var tag = selectedItem.Tag as string;
            if (tag == null) return;

            if (tag.StartsWith("Major_"))
            {
                if (int.TryParse(tag.Substring(6), out int majorNo))
                {
                    var students = _context.Student.Where(s => s.MajorNo == majorNo).ToList();
                    DataGridData.ItemsSource = students;
                }
            }
            else if (tag.StartsWith("Student_"))
            {
                if (int.TryParse(tag.Substring(8), out int sno))
                {
                    var courses = (from cs in _context.CourseSelect
                            where cs.Sno == sno
                            join c in _context.Course on cs.Cno equals c.Cno
                            join t in _context.Teacher on c.Tno equals t.Tno
                            select new
                            {
                                c.Cname,
                                c.Ccredit,
                                c.Cstatus,
                                c.Ctype,
                                cs.CSscore,
                                cs.CSstatus,
                                t.Tname
                            }
                        ).ToList();
                    DataGridData.ItemsSource = courses;
                }
            }
        }

        private void DataGridData_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (DataGridData.SelectedItem is Student selectedStudent)
            {
                var editWindow = new ManagerEditStudentWindow(selectedStudent);
                if (editWindow.ShowDialog() == true)
                {
                    _context.SaveChanges();
                    RefreshDataGrid();
                }
            }
        }

         // 刷新 DataGrid 的数据
        private void RefreshDataGrid()
        {
            // 检查 TreeView 中选中的项
            if (TreeViewData.SelectedItem is TreeViewItem selectedItem)
            {
                var tag = selectedItem.Tag as string;
                if (tag == null) return;

                // 如果选中的是专业节点
                if (tag.StartsWith("Major_"))
                {
                    // 解析专业编号并从数据库中获取该专业的所有学生
                    if (int.TryParse(tag.Substring(6), out int majorNo))
                    {
                        var students = _context.Student.Where(s => s.MajorNo == majorNo).ToList();
                        // 将学生列表设置为 DataGrid 的数据源
                        DataGridData.ItemsSource = students;
                    }
                }
                // 如果选中的是学生节点
                else if (tag.StartsWith("Student_"))
                {
                    // 解析学生编号并从数据库中获取该学生选修的所有课程
                    if (int.TryParse(tag.Substring(8), out int sno))
                    {
                        var courses = (from cs in _context.CourseSelect
                                       where cs.Sno == sno
                                       join c in _context.Course on cs.Cno equals c.Cno
                                       join t in _context.Teacher on c.Tno equals t.Tno
                                       select new
                                       {
                                           c.Cname,      // 课程名称
                                           c.Ccredit,    // 课程学分
                                           c.Cstatus,    // 课程状态
                                           c.Ctype,      // 课程类型
                                           cs.CSscore,   // 课程分数
                                           cs.CSstatus,  // 课程选课状态
                                           t.Tname       // 教师名称
                                       }).ToList();
                        // 将课程列表设置为 DataGrid 的数据源
                        DataGridData.ItemsSource = courses;
                    }
                }
            }
        }
    }
}