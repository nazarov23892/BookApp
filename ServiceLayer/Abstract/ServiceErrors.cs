using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace ServiceLayer.Abstract
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
    }
}
