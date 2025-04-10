using System.Linq;
using System.Windows;
using System.Windows.Controls;
using laba2_db.models;
using laba2_db.crud;

namespace laba2_db
{
    public partial class MainWindow : Window
    {
        private MyDbContext _context = new MyDbContext();

        public MainWindow()
        {
            InitializeComponent();
            LoadStudentData();
        }

        private void LoadStudentData()
        {
            var studentsWithGrades = (
                from s in _context.Students
                from g in _context.Grades.Where(x => x.Studentid == s.StudentId).DefaultIfEmpty()
                select new
                {
                    ID = s.StudentId,
                    ФИО = $"{s.Surname} {s.Name} {s.Patronymic}",
                    ДатаРождения = s.Birthdate,
                    Физика = g != null ? g.Physicsgrade : null,
                    Математика = g != null ? g.Mathgrade : null
                }
            ).ToList();

            DataGrid.ItemsSource = studentsWithGrades;

            DataGrid.AutoGeneratingColumn += (s, e) =>
            {
                if (e.PropertyName == "BirthDate")
                {
                    (e.Column as DataGridTextColumn).Binding.StringFormat = "dd.MM.yyyy";
                }
            };
        }

        private void Filter_Click(object sender, RoutedEventArgs e)
        {
            var filteredData = (
                from student in _context.Students
                where student.Surname.Contains(FilterTextBox.Text) 
                from grade in _context.Grades
                    .Where(g => g.Studentid == student.StudentId)
                    .DefaultIfEmpty() 
                select new
                {
                    ID = student.StudentId,
                    ФИО = $"{student.Surname} {student.Name} {student.Patronymic}",
                    ДатаРождения = student.Birthdate,
                    Физика = grade != null ? grade.Physicsgrade : (int?)null,
                    Математика = grade != null ? grade.Mathgrade : (int?)null
                }
            ).ToList();

            DataGrid.ItemsSource = filteredData;
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            if (DataGrid.SelectedItem == null)
            {
                MessageBox.Show("Выберите студента!");
                return;
            }

            dynamic student = DataGrid.SelectedItem;
            int id = student.ID;
            

            var answer = MessageBox.Show("Точно удалить?", "Подтверждение", MessageBoxButton.YesNo);
            if (answer == MessageBoxResult.Yes)
            {
                var s = _context.Students.Find(id);
                _context.Students.Remove(s);
                _context.SaveChanges();
                LoadStudentData();
                MessageBox.Show("Удалено!");
            }
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            Add aw = new crud.Add();
            aw.Show();
            
        }

        private void Edit_Click(object sender, RoutedEventArgs e)
        {
            if (DataGrid.SelectedItem == null)
            {
                MessageBox.Show("Выберите студента для редактирования!");
                return;
            }

            dynamic selectedStudent = DataGrid.SelectedItem;
            int studentId = selectedStudent.ID;

            Edit ew = new Edit(studentId);
            ew.ShowDialog(); 
            LoadStudentData(); 
        }
    }
}