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
    /// StudentWindow.xaml 的交互逻辑
    /// </summary>
    public partial class StudentWindow : Window
    {
        private SchoolContext _context;
        private int sno;

        public StudentWindow(int sno)
        {
            InitializeComponent();
            this.sno = sno;
            _context = new SchoolContext();
        }

        private void CourseQueryButton_Click(object sender, RoutedEventArgs e)
        {
            var courses = _context.CourseSelect
                          .Where(cs => cs.Sno == sno)
                          .Select(cs => cs.Cno)
                          .Join(_context.Course,
                                cno => cno,
                                course => course.Cno,
                                (cno, course) => course)
                          .ToList();
            CourseScheduleDataGrid.ItemsSource = courses;
        }
    }
}
