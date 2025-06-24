using Lab4_OOP.Classes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
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
    public partial class ExamForm : Window
    {
        public Exam Exam { get; private set; }

        public ExamForm()
        {
            InitializeComponent();

            Exam = new Exam("", 0, DateTime.Today);

            DataContext = Exam;
        }

        public ExamForm(Exam existingExam) : this()
        {
            InitializeComponent();
            
            if (existingExam == null)
                throw new ArgumentNullException(nameof(existingExam));

            Title = "Редагувати іспит"; ;

            Exam = new Exam(existingExam.Subject, existingExam.Grade, existingExam.ExamDate);

            DataContext = Exam;
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

            foreach (var propertyName in new[] { nameof(Exam.Subject), nameof(Exam.Grade), nameof(Exam.ExamDate) })
            {
                string error = ((IDataErrorInfo)Exam)[propertyName];
                if (!string.IsNullOrEmpty(error))
                    validationErrors.Add(error);
            }

            if (validationErrors.Any())
            {
                MessageBox.Show(string.Join("\n", validationErrors), "Помилки валідації", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            Exam = new Exam(SubjectTextBox.Text, int.Parse(GradeTextBox.Text), ExamDatePicker.SelectedDate.Value);
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
