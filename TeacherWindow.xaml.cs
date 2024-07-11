using System.Windows;
using Microsoft.EntityFrameworkCore;

namespace WpfAppAlpha
{
    public partial class TeacherWindow : Window
    {
        private SchoolContext _context;
        private int Tno;
        public TeacherWindow(int Tno)
        {
            InitializeComponent();
            
            this.Tno = Tno;

            LoadData();
        }

        private void LoadData()
        {
            _context = new SchoolContext();
            _context.Course.Include(c => c.Teacher).Load();
            _context.CourseSelect.Include(cs => cs.Course).ThenInclude(c => c.Teacher).Load();

            CourseScheduleDataGrid.ItemsSource = _context.Course.Select(c => new
            {
                c.Cno,
                c.Cname,
                c.Ctype,
                c.Ccredit,
                c.Cstatus,
                c.Cterm,
                c.Tno,
                c.Teacher.Tname
            }).Where(c => c.Tno == Tno).ToList();

        }
    }
}
