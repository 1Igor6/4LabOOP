using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab4_OOP.Classes
{
    public enum EducationLevel
    {
        Бакалавр,

        Спеціаліст,

        Магістр
    }

    [JsonObject(IsReference = true)]
    public class Student :/* INotifyDataErrorInfo,*/ INotifyPropertyChanged
    {
        private Person person;
        private EducationLevel educationLevel;
        private List<Exam> exams;

        public Student()
        {
            person = new Person();
            exams = new List<Exam>();
        }


        public Student(Person person, EducationLevel educationLevel)
        {
            this.person = person;
            this.educationLevel = educationLevel;
            this.exams = new List<Exam>();
        }

        public Student(Person person, EducationLevel educationLevel, List<Exam> exams)
        {
            this.person = person;
            this.educationLevel = educationLevel;
            this.exams = exams;
        }

        public Person Person
        {
            get => person;
            set
            {
                person = value;
                OnPropertyChanged(nameof(Person));
            }
        }

        public EducationLevel EducationLevel
        {
            get => educationLevel;
            set
            {
                educationLevel = value;
                OnPropertyChanged(nameof(EducationLevel));
            }
        }

        public List<Exam> Exams
        {
            get => exams;
            set
            {
                exams = value;
                OnPropertyChanged(nameof(Exams));
                OnPropertyChanged(nameof(ExamsSummary));
            }
        }

        public string FirstName => person?.FirstName ?? string.Empty;
        public string LastName => person?.LastName ?? string.Empty;
        public DateTime BirthDate => person?.BirthDate ?? default;

        public string ExamsSummary
        {
            get
            {
                if (exams == null || exams.Count == 0)
                    return "Немає складених іспитів";

                int count = exams.Count;
                double avg = GetAverageGrade();
                return $"Складено іспитів: {count}\nСередній бал: {avg:F2}";
            }
        }


        public void AddExam(Exam exam)
        {
            exams.Add(exam);
        }

        public double GetAverageGrade()
        {
            return exams.Count > 0 ? exams.Average(e => e.Grade) : 0.0;
        }

        public override string ToString()
        {
            string examInfo = exams.Count > 0
                ? string.Join("\n", exams.Select(e => " - " + e.ToString()))
                : "Немає складених іспитів";
            return $"Студент: {person}\nОсвітній рівень: {educationLevel}\nІспити:\n{examInfo}";
        }

        public string ToStringShort()
        {
            return $"Прізвище: {person.LastName}, Середній бал: {GetAverageGrade():F2}";
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
