using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lab4_OOP.Classes.Validation;

namespace Lab4_OOP.Classes
{
    [JsonObject(IsReference = true)]
    public class Exam : IDataErrorInfo, INotifyPropertyChanged    
    {
        private string subject;
        private int grade;
        private DateTime examDate;

        public Exam(string subject, int grade, DateTime examDate)
        {
            this.subject = subject;
            this.grade = grade;
            this.examDate = examDate;
        }

        [Required(ErrorMessage = "Назва предмету обов'язкова.")]
        [MaxLength(30, ErrorMessage = "Назва предмету не може бути довше 30 символів.")]
        [RegularExpression(@"^[А-ЯІЇЄҐ][а-яіїєґ’' \-]*$", ErrorMessage = "Назва предмету має починатися з великої літери\nта містити лише українські букви, пробіли, дефіси або апострофи.")]
        public string Subject
        {
            get => subject;
            set
            {
                subject = value;
                OnPropertyChanged(nameof(Subject));
            }
        }

        [Required(ErrorMessage = "Оцінка за іспит обов'язкова.")]
        [Range(1, 100, ErrorMessage = "Оцінка має бути в межах від 1 до 100.")]
        public int Grade
        {
            get => grade;
            set
            {
                grade = value;
                OnPropertyChanged(nameof(Grade));
            }
        }

        [Required(ErrorMessage = "Дата складання іспиту обов'язкова.")]
        [MaxDate(ErrorMessage = "Дата складання іспиту не може бути у майбутньому.")]
        public DateTime ExamDate
        {
            get => examDate;
            set
            {
                examDate = value;
                OnPropertyChanged(nameof(ExamDate));
            }
        }


        public string this[string columnName]
        {
            get
            {
                var results = new List<ValidationResult>();
                var context = new ValidationContext(this) { MemberName = columnName };
                var value = GetType().GetProperty(columnName)?.GetValue(this);

                bool isValid = Validator.TryValidateProperty(value, context, results);
                if (!isValid)
                    return results[0].ErrorMessage;

                return null;
            }
        }

        public string Error => null;

        public override string ToString()
        {
            return $"{subject}: оцінка {grade}, дата складання: {examDate.ToShortDateString()}";
        }


        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
