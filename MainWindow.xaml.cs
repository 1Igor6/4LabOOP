using Lab4_OOP.Classes;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Lab4_OOP
{
    public class MainViewModel
    {
        public ObservableCollection<Student> Students { get; set; } = new ObservableCollection<Student>();
    }


    public partial class MainWindow : Window
    {
        public MainViewModel ViewModel { get; set; } = new MainViewModel();

        private string filePath;


        public MainWindow()
        {
            InitializeComponent();
            DataContext = ViewModel;

            string projectPath = @"C:\Users\user\Desktop\Lab4\Lab4_OOP\Lab4_OOP\Lab4_OOP";
            filePath = System.IO.Path.Combine(projectPath, "Data", "students.json");

            LoadStudents();

            this.Closing += MainWindow_Closing;

        }


        private void LoadStudents()
        {
            if (!File.Exists(filePath))
            {
                MessageBox.Show($"Файл {filePath} не знайдено.");
                return;
            }

            try
            {
                string json = File.ReadAllText(filePath);

                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };
                options.Converters.Add(new JsonStringEnumConverter());

                var studentsFromFile = JsonSerializer.Deserialize<List<Student>>(json, options);
                ViewModel.Students.Clear();

                foreach (var student in studentsFromFile)
                {
                    var validationErrors = new List<System.ComponentModel.DataAnnotations.ValidationResult>();
                    var context = new ValidationContext(student);

                    bool isValid = Validator.TryValidateObject(student, context, validationErrors, true);

                    if (student.Person != null)
                    {
                        var personContext = new ValidationContext(student.Person);
                        isValid &= Validator.TryValidateObject(student.Person, personContext, validationErrors, true);
                    }

                    if (student.Exams != null)
                    {
                        foreach (var exam in student.Exams)
                        {
                            var examContext = new ValidationContext(exam);
                            isValid &= Validator.TryValidateObject(exam, examContext, validationErrors, true);
                        }
                    }

                    if (isValid)
                    {
                        ViewModel.Students.Add(student);
                    }
                    else
                    {
                        string allErrors = string.Join("\n", validationErrors.Select(err => $"• {err.ErrorMessage}"));
                        MessageBox.Show($"Помилка у студенті \"{student?.Person?.FirstName} {student?.Person?.LastName}\":\n{allErrors}", "Помилка валідації", MessageBoxButton.OK, MessageBoxImage.Warning);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Помилка при зчитуванні студентів: {ex.Message}", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void MainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            SaveStudents();
        }


        private void SaveStudents()
        {
            try
            {
                var studentsToSave = ViewModel.Students.ToList();

                var options = new JsonSerializerOptions
                {
                    WriteIndented = true,  
                    Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping, 
                    DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
                };
  
                options.Converters.Add(new JsonStringEnumConverter());
    
                byte[] utf8Json = JsonSerializer.SerializeToUtf8Bytes(studentsToSave, options);

                string dataDir = System.IO.Path.GetDirectoryName(filePath);
                if (!Directory.Exists(dataDir)) 
                    Directory.CreateDirectory(dataDir);

                File.WriteAllBytes(filePath, utf8Json);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Помилка збереження студентів: {ex.Message}",
                    "Помилка",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }


        private void StudentsListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            EditButton.IsEnabled = DeleteButton.IsEnabled = DetailsButton.IsEnabled =
                StudentsListView.SelectedItem is Student;
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            var form = new StudentForm();
            form.Owner = this;
            if (form.ShowDialog() == true)
            {
                ViewModel.Students.Add(form.Student);
            }
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            if (StudentsListView.SelectedItem is Student selectedStudent)
            {
                var form = new StudentForm(selectedStudent)
                {
                    Owner = this
                };

                if (form.ShowDialog() == true)
                {
 
                    selectedStudent.Person.FirstName = form.Student.Person.FirstName;
                    selectedStudent.Person.LastName = form.Student.Person.LastName;
                    selectedStudent.Person.BirthDate = form.Student.Person.BirthDate;
                    selectedStudent.EducationLevel = form.Student.EducationLevel;

                    StudentsListView.Items.Refresh();
                }
            }
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (StudentsListView.SelectedItem is Student selected)
            {
                var result = MessageBox.Show(
                    $"Ви дійсно хочете видалити студента зі списку групи?",
                    "Підтвердження видалення",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Warning);

                if (result == MessageBoxResult.Yes)
                {
                    ViewModel.Students.Remove(selected);
                }
            }
        }

        private void DetailsButton_Click(object sender, RoutedEventArgs e)
        {
            if (StudentsListView.SelectedItem is Student selectedStudent)
            {
                var form = new ExamListForm(selectedStudent.Exams);
                form.Owner = this;
                bool? result = form.ShowDialog();

                if (result == true)
                {
                    selectedStudent.Exams = form.ResultExams;
                    StudentsListView.Items.Refresh();
                    SaveStudents();
                }
            }
        }

    }

}



