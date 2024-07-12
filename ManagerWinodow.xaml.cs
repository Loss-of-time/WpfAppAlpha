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
    }
}