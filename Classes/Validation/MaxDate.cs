using System;
using System.ComponentModel.DataAnnotations;

namespace Lab4_OOP.Classes.Validation
{
    public class MaxDate : ValidationAttribute
    {
        private DateTime MaxAllowedDate { get; }

        public MaxDate()
        {
            MaxAllowedDate = DateTime.Today;
        }

        public MaxDate(int year, int month, int day)
        {
            MaxAllowedDate = new DateTime(year, month, day);
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null)
            {
                return ValidationResult.Success;
            }

            if (value is DateTime inputDate)
            {
                if (inputDate > MaxAllowedDate)
                {
                    var errorMessage = FormatErrorMessage(validationContext.DisplayName);
                    return new ValidationResult(errorMessage);
                }

                return ValidationResult.Success;
            }

            return new ValidationResult("Неправильний тип даних для атрибута MaxDate (очікується DateTime).");
        }
    }
}






























//using System;
//using System.Collections.Generic;
//using System.ComponentModel.DataAnnotations;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace Lab4_OOP.Classes.Validation
//{
//    public class MaxDate : ValidationAttribute
//    {
//        private DateTime _maxDate;

//        public MaxDate()
//        {
//            _maxDate = DateTime.Today;
//        }
//        public MaxDate(int year, int month, int day)
//        {
//            _maxDate = new DateTime(year, month, day);
//        }


//        public override bool IsValid(object value)
//        {
//            if (value is DateTime date)
//            {
//                return date <= _maxDate;
//            }
//            return false;
//        }
//    }
//}
