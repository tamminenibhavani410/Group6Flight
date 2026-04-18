using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace Group6Flight.Models
{
    public class FutureDateAttribute : ValidationAttribute, IClientModelValidator
    {
        private int maxYears;
        public FutureDateAttribute(int years)
        {
            maxYears = years;
        }
        protected override ValidationResult IsValid(object? value, ValidationContext ctx)
        {
            if (value is DateTime dateToCheck)
            {
                DateTime today = DateTime.Today;
                DateTime maxDate = today.AddYears(maxYears);
                if (dateToCheck > today && dateToCheck <= maxDate)
                {
                    return ValidationResult.Success!;
                }
            }
            return new ValidationResult(GetMsg(ctx.DisplayName ?? "Date"));
        }

        public void AddValidation(ClientModelValidationContext ctx)
        {
            if (!ctx.Attributes.ContainsKey("data-val"))
                ctx.Attributes.Add("data-val", "true");
            ctx.Attributes.Add("data-val-futuredate-years",
                maxYears.ToString());
            ctx.Attributes.Add("data-val-futuredate",
                GetMsg(ctx.ModelMetadata.DisplayName ?? ctx.ModelMetadata.Name ?? "Date"));
        }

        private string GetMsg(string name) =>
            base.ErrorMessage ?? $"{name} must be a valid future date within {maxYears} years.";
    }
}
