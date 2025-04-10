using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Numerics;
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
using System.Xml.Linq;
using laba2_db.models;

namespace laba2_db.crud
{
    /// <summary>
    /// Логика взаимодействия для Add.xaml
    /// </summary>
    public partial class Add : Window
    {
        private readonly MyDbContext _context = new MyDbContext();
        public Add()
        {
            InitializeComponent();
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(SurnameTextBox.Text) ||
                    string.IsNullOrWhiteSpace(NameTextBox.Text))
                {
                    MessageBox.Show("Заполните фамилию и имя студента!",
                                 "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (!DateOnly.TryParseExact(BirthdateTextBox.Text, "yyyy-MM-dd",
                    CultureInfo.InvariantCulture, DateTimeStyles.None, out DateOnly birthDate))
                {
                    MessageBox.Show("Введите дату в формате ГГГГ-ММ-ДД!",
                                 "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

               
                var newStudent = new Student
                {
                    Surname = SurnameTextBox.Text,
                    Name = NameTextBox.Text,
                    Patronymic = string.IsNullOrWhiteSpace(PatronymicTextBox.Text) ? null : PatronymicTextBox.Text,
                    Birthdate = birthDate
                };

                _context.Students.Add(newStudent);
                _context.SaveChanges(); 

                
                var newGrade = new Grade
                {
                    Studentid = newStudent.StudentId,
                    Physicsgrade = 0, 
                    Mathgrade = 0     
                };

                _context.Grades.Add(newGrade);
                _context.SaveChanges(); 

                MessageBox.Show("Студент успешно добавлен!", "Успех",
                              MessageBoxButton.OK, MessageBoxImage.Information);

                MainWindow mw = new MainWindow();
                mw.Show();
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при сохранении: {ex.Message}", "Ошибка",
                              MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
