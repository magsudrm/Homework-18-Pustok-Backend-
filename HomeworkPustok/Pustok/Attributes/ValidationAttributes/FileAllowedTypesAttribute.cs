using System.ComponentModel.DataAnnotations;

namespace Pustok.Attributes.ValidationAttributes
{
    public class FileAllowedTypesAttribute:ValidationAttribute
    {
        private readonly string[] _types;

        public FileAllowedTypesAttribute(params string[] types)
        {
            _types = types;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            List<IFormFile> files = new List<IFormFile>();

            if (value is IFormFile)
                files.Add((IFormFile)value);
            else if (value is List<IFormFile>)
                files.AddRange((List<IFormFile>)value);

            foreach (var file in files)
            {
                if (!_types.Contains(file.ContentType))
                    return new ValidationResult($"FileType must be one of the types: {String.Join(',', _types)}");
            }

            return ValidationResult.Success;
        }
    }
}
