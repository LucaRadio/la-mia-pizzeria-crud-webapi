using System.ComponentModel.DataAnnotations;

namespace la_mia_pizzeria_static.Validations
{
    public class DescriptionValidateAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value == null)
            {
                return new ValidationResult("Description cannot be empty!");
            }
            int count = 0;
            foreach (char item in (string)value)
            {
                bool isSpace = item == ' ';
                if (isSpace)
                {
                    count++;
                }
            }
            if (count >= 5)
            {
                return ValidationResult.Success;
            }
            else
            {
                return new ValidationResult("Description must have at least 5 spaces!");
            }
        }
    }
}
