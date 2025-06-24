using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab4_OOP.Classes.Validation
{
    public class MinAge : ValidationAttribute
    {
        private int MinimumAge { get; }

        public MinAge(int minimumAge)
        {
            MinimumAge = minimumAge;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null)
            {
                return ValidationResult.Success;
            }

            if (value is DateTime birthDate)
            {
                var today = DateTime.Today;
                var age = today.Year - birthDate.Year;



                if (age < MinimumAge)
                {
                    var errorMessage = FormatErrorMessage(validationContext.DisplayName);
                    return new ValidationResult(errorMessage);
                }

                return ValidationResult.Success;
            }

            return new ValidationResult("Неправильний тип даних для атрибута MinAge (очікується DateTime).");
        }
    }
}
