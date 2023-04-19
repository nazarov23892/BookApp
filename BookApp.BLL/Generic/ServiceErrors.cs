using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace BookApp.BLL.Generic
{
    public abstract class ServiceErrors
    {
        private readonly List<ValidationResult> errors = new List<ValidationResult>();
        public IEnumerable<ValidationResult> Errors
        {
            get => errors;
        }

        public bool HasErrors
        {
            get => errors.Any();
        }

        protected void AddError(string errorMessage, params string[] memberNames)
        {
            errors.Add(new ValidationResult(
                errorMessage: errorMessage,
                memberNames: memberNames));
        }

        protected bool PerformValidationObjectProperties(object instance)
        {
            var context = new ValidationContext(
               instance: instance,
               serviceProvider: null,
               items: null);
            var results = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(
                instance: instance,
                validationContext: context,
                validationResults: results,
                validateAllProperties: true);
            if (!isValid)
            {
                foreach (var error in results)
                {
                    AddError(errorMessage: error.ErrorMessage);
                }
            }
            return isValid;
        }
    }
}
