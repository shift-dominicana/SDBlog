using FluentValidation.Validators;
using LinqKit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Linq.Expressions;
using Seguridad.DataModel.Abstract;
using Seguridad.Core.Models.Enums;

namespace SGI.Core.PropertyValidators
{
    public class UniqueValidator<T> : PropertyValidator where T : class, IEntityBase, new()
    {

        private Expression<Func<T, bool>> _predicate;
        private readonly DbContext _context;

        public UniqueValidator(
            DbContext context,
            IStringLocalizer localizer,
            Expression<Func<T, bool>> predicate = null)
            : base(localizer["{PropertyName} must be unique, the value already exist."])
        {
            _context = context;
            _predicate = predicate ?? PredicateBuilder.New<T>(true);
        }

        protected override bool IsValid(PropertyValidatorContext context)
        {
            var instance = context.Instance as T ?? new T();
            var func = (context.PropertyName + " = " + context.PropertyValue
                        + "| " + "Id" + " != " + instance.Id)
                .AsPredicate<T>(AndOrOperator.And);

            _predicate = _predicate.And(func);// Extension.And(_predicate, func);

            return !_context.Set<T>().Where(func).Any();
        }
    }
}
