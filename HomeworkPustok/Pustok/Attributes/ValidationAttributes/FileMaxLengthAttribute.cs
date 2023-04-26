using System.ComponentModel.DataAnnotations;

namespace Pustok.Attributes.ValidationAttributes
{
    public class FileMaxLengthAttribute:ValidationAttribute
    {
        private readonly int _maxLength;

        public FileMaxLengthAttribute(int maxLength)
        {
            _maxLength = maxLength;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {

            List<IFormFile> files = new List<IFormFile>();

            if (value is IFormFile)
                files.Add((IFormFile)value);
            else if (value is List<IFormFile>)
                files.AddRange((List<IFormFile>)value);

            foreach (var item in files)
            {
                if (item.Length > _maxLength)
                    return new ValidationResult($"FileLength must be equal or less than {_maxLength / 1024} kb");
            }

            return ValidationResult.Success;
        }
    }
}
