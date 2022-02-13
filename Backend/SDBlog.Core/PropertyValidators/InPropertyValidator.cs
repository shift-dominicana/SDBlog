using FluentValidation.Validators;
using Microsoft.Extensions.Localization;
using Seguridad.DataModel.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Seguridad.Core.PropertyValidators
{
    public class InPropertyValidator<T, TProperty> : PropertyValidator where T : class, IEntityBase, new()
    {
        private readonly TProperty[] _validOptions;
        private readonly IStringLocalizer _localizer;
        private readonly bool _validateAtInsert;
        public InPropertyValidator(
            //IStringLocalizer localizer,
            bool validateAtInsert = true,
            params TProperty[] validOptions)
            : base()
        {
            //_localizer = localizer;
            _validateAtInsert = validateAtInsert;
            _validOptions = validOptions;
        }

        protected override bool IsValid(PropertyValidatorContext context)
        {
            var instance = context.InstanceToValidate as T ?? new T();
            if (!_validateAtInsert && instance.Id == 0) return true;

            string formatted;
            if (_validOptions == null || _validOptions.Length == 0)
            {
                throw new ArgumentException("At least one valid option is expected", nameof(_validOptions));
            }
            else if (_validOptions.Length == 1)
            {
                formatted = _validOptions[0].ToString();
            }
            else
            {
                var result = new StringBuilder();
                foreach (var option in _validOptions.Take(_validOptions.Count() - 1))
                {
                    result.Append($"'{option.ToString()}' ");
                }
                result.Append($"{_localizer["or"]} '{_validOptions.Last()}'");
                formatted = result.ToString();
            }
            context.MessageFormatter.AppendArgument("formatted", formatted);
            
            return _validOptions.Any(validOption => validOption.ToString().Equals(context.PropertyValue));
        }
    }
}
