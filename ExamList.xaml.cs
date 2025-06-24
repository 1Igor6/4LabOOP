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
using System.IO;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;


namespace Lab4_OOP
{
    public partial class ExamListForm : Window
    {
        public List<Exam> ResultExams { get; private set; }

        public ExamListForm(List<Exam> exams)
        {
            InitializeComponent();

            ResultExams = exams.Select(e => new Exam(e.Subject, e.Grade, e.ExamDate)).ToList();
            ExamListView.ItemsSource = ResultExams;
            this.Closing += Window_Closing;

        }

        private void ExamListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            EditButton.IsEnabled = DeleteButton.IsEnabled = 
                ExamListView.SelectedItem is Exam;
        }


        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            var form = new ExamForm();
            if (form.ShowDialog() == true)
            {
                ResultExams.Add(form.Exam);
                ExamListView.Items.Refresh();
            }
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (ExamListView.SelectedItem is Exam selected)
            {
                var result = MessageBox.Show(
                    $"Ви дійсно хочете видалити іспит зі списку зданих іспитів?",
                    "Підтвердження видалення",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Warning);

                if (result == MessageBoxResult.Yes)
                {
                    ResultExams.Remove(selected);
                    ExamListView.Items.Refresh();
                }
            }
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            if (ExamListView.SelectedItem is Exam selected)
            {
                var form = new ExamForm(selected);
                if (form.ShowDialog() == true)
                {
                    selected.Subject = form.Exam.Subject;
                    selected.Grade = form.Exam.Grade;
                    selected.ExamDate = form.Exam.ExamDate;
                    ExamListView.Items.Refresh();
                }
            }
        }

        private bool _isManuallyClosed = false;
        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            _isManuallyClosed = true;
            DialogResult = true;
            Close();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            _isManuallyClosed = true;
            DialogResult = false;
            Close();
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            if (!DialogResult.HasValue)
            {
                var result = MessageBox.Show("Зберегти зміни перед закриттям?", "Підтвердження", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    DialogResult = true;
                }
                else if (result == MessageBoxResult.No)
                {
                    DialogResult = false;
                }
                else
                {
                    e.Cancel = true;
                }
            }
        }

    }

}
