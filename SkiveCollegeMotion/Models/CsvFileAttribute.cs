using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using System.Linq;

// This is made for validating IFormFile having an extension of csv,
// however it can easily be extended for use with other/multiple extensions.
public sealed class CsvFileAttribute : ValidationAttribute
{
    private const string DefaultErrorMessage = "Vælg venligst en CSV fil";

    protected override ValidationResult IsValid(object file, ValidationContext validationContext)
    {
        // This is made for IFormFile compatibility, for strings use built in FileExtensionsAttribute.
        if (((IFormFile)file).FileName.ToLower().EndsWith(".csv"))
        {
            return ValidationResult.Success;
        }
        else
        {
            return new ValidationResult(ErrorMessage ?? DefaultErrorMessage);
        }
    }
}