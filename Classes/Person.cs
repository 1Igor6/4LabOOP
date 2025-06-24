using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using Lab4_OOP.Classes.Validation;

namespace Lab4_OOP.Classes
{
    public class Person : IDataErrorInfo, INotifyPropertyChanged
    {
        private string firstName;
        private string lastName;
        private DateTime birthDate;

        public Person() { } 


        public Person(string firstName, string lastName, DateTime birthDate)
        {
            this.firstName = firstName;
            this.lastName = lastName;
            this.birthDate = birthDate;
        }

        [Required(ErrorMessage = "Ім'я є обов'язковим.")]
        [MaxLength(30, ErrorMessage = "Ім'я не може бути довше 30 символів.")] 
        [RegularExpression(@"^[А-ЯІЇЄҐ][а-яіїєґ’' \-]*$", ErrorMessage = "Ім'я має починатися з великої літери\nта містити лише українські букви, пробіли, дефіси або апострофи.")]
        public string FirstName
        {
            get => firstName;
            set
            {
                firstName = value;
                OnPropertyChanged(nameof(FirstName));
            }
        }

        [Required(ErrorMessage = "Прізвище є обов'язковим.")]
        [MaxLength(30, ErrorMessage = "Прізвище не може бути довше 30 символів.")]
        [RegularExpression(@"^[А-ЯІЇЄҐ][а-яіїєґ’' \-]*$", ErrorMessage = "Прізвище має починатися з великої літери\nта містити лише українські букви, пробіли, дефіси або апострофи.")]
        public string LastName
        {
            get => lastName;
            set
            {
                lastName = value;
                OnPropertyChanged(nameof(LastName));
            }
        }

        [Required(ErrorMessage = "Дата народження є обов'язковою.")]
        [MinAge(16, ErrorMessage = "Вік студента має бути більше 16 років.")]
        public DateTime BirthDate
        {
            get => birthDate;
            set
            {
                birthDate = value;
                OnPropertyChanged(nameof(BirthDate));
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
            return $"{firstName} {lastName}, народжений: {birthDate.ToShortDateString()}";
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
