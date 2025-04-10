using System;
using System.Windows;
using laba2_db.models;

namespace laba2_db.crud
{
    public partial class Edit : Window
    {
        private readonly MyDbContext db = new MyDbContext();
        private readonly Student student;
        private readonly Grade grade;

        public Edit(int id)
        {
            InitializeComponent();

            student = db.Students.Find(id);
            grade = db.Grades.FirstOrDefault(g => g.Studentid == id);

            if (grade == null)
            {
                grade = new Grade { Studentid = id };
            }

            SurnameTextBox.Text = student.Surname;
            NameTextBox.Text = student.Name;
            PatronymicTextBox.Text = student.Patronymic;
            BirthdateTextBox.Text = student.Birthdate.ToString("dd.MM.yyyy");
            PhysicsGradeTextBox.Text = grade.Physicsgrade.ToString();
            MathGradeTextBox.Text = grade.Mathgrade.ToString();
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            student.Surname = SurnameTextBox.Text;
            student.Name = NameTextBox.Text;
            student.Patronymic = PatronymicTextBox.Text;

            if (!string.IsNullOrEmpty(BirthdateTextBox.Text) && BirthdateTextBox.Text != student.Birthdate.ToString("dd.MM.yyyy"))
            {
                if (DateOnly.TryParse(BirthdateTextBox.Text, out var newDate))
                {
                    student.Birthdate = newDate;
                }
                else
                {
                    MessageBox.Show("Неправильная дата! Формат: дд.мм.гггг");
                    return;
                }
            }

            if (int.TryParse(PhysicsGradeTextBox.Text, out int physics))
                grade.Physicsgrade = physics;

            if (int.TryParse(MathGradeTextBox.Text, out int math))
                grade.Mathgrade = math;

            if (grade.Gradeid == 0)
            {
                db.Grades.Add(grade);
            }

            try
            {
                db.SaveChanges();
                MessageBox.Show("Сохранено!");
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка сохранения: {ex.Message}");
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}