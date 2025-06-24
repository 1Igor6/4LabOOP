using Lab4_OOP.Classes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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

namespace Lab4_OOP
{
    public partial class StudentForm : Window
    {
        public Student Student { get; private set; }

        public StudentForm()
        {
            InitializeComponent();

            Title = "Додати студента";
 
            Student = new Student(new Person("", "", DateTime.Today), EducationLevel.Бакалавр);

            DataContext = Student.Person;

            EducationLevelComboBox.ItemsSource = Enum.GetValues(typeof(EducationLevel));
            EducationLevelComboBox.SelectedItem = Student.EducationLevel;
        }

        public StudentForm(Student existingStudent)
        {
            InitializeComponent();

            if (existingStudent == null)
                throw new ArgumentNullException(nameof(existingStudent));

            Title = "Редагувати студента";

            Student = new Student(
                new Person(existingStudent.Person.FirstName, existingStudent.Person.LastName, existingStudent.Person.BirthDate),
                existingStudent.EducationLevel
            );

            DataContext = Student.Person;

            EducationLevelComboBox.ItemsSource = Enum.GetValues(typeof(EducationLevel));
            EducationLevelComboBox.SelectedItem = Student.EducationLevel;
        }


        private bool _isManuallyClosed = false;

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {

            if (!DialogResult.HasValue && !_isManuallyClosed)
            {
                var result = MessageBox.Show("Зберегти зміни перед закриттям?", "Підтвердження", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    SaveButton_Click(null, null); 
                    if (DialogResult != true)
                    {
                        e.Cancel = true;
                    }
                }
                else if (result == MessageBoxResult.No)
                {
                    DialogResult = false;
                }
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {

            var validationErrors = new List<string>();

            foreach (var propertyName in new[] { nameof(Student.Person.FirstName), nameof(Student.Person.LastName), nameof(Student.Person.BirthDate) })
            {
                string error = ((IDataErrorInfo)Student.Person)[propertyName];
                if (!string.IsNullOrEmpty(error))
                    validationErrors.Add(error);
            }

            if (validationErrors.Any())
            {
                MessageBox.Show(string.Join("\n", validationErrors), "Помилки валідації", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var person = new Person(
                Student.Person.FirstName,
                Student.Person.LastName,
                Student.Person.BirthDate
            );

            var educationLevel = (EducationLevel)EducationLevelComboBox.SelectedItem;

            Student = new Student(person, educationLevel);

            _isManuallyClosed = true;
            DialogResult = true; 
        }


        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            _isManuallyClosed = true;
            DialogResult = false;
        }
    }
}
