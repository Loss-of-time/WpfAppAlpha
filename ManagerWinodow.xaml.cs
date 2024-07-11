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
                var majorNode = new TreeViewItem { Header = major.MajorName, Tag = major.MajorNo };
                TreeViewData.Items.Add(majorNode);

                var students = _context.Student.Where(s => s.MajorNo == major.MajorNo).ToList();
                foreach (var student in students)
                {
                    var studentNode = new TreeViewItem { Header = student.Sname, Tag = student.Sno };
                    majorNode.Items.Add(studentNode);
                }
            }
        }

        private void TreeViewData_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            var selectedItem = TreeViewData.SelectedItem as TreeViewItem;
            if (selectedItem == null) return;

            if (selectedItem.Tag is int majorNo)
            {
                var students = _context.Student.Where(s => s.MajorNo == majorNo).ToList();
                DataGridData.ItemsSource = students;
            }
            else if (selectedItem.Tag is int sno)
            {
                var courses = _context.CourseSelect
                                      .Where(cs => cs.Sno == sno)
                                      .Select(cs => cs.Cno)
                                      .Join(_context.Course,
                                            cno => cno,
                                            course => course.Cno,
                                            (cno, course) => course)
                                      .ToList();
                DataGridData.ItemsSource = courses;
            }
        }
    }
}
