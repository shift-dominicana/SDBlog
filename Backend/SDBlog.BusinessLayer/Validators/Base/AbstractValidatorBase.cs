using FluentValidation;
using SDBlog.Core.Base;

namespace SDBlog.BusinessLayer.Validators.Base
{
    public class AbstractValidatorBase<TModel> : AbstractValidator<TModel> where TModel : class
    {
        public AbstractValidatorBase()
        {
            //validaciones             
        }
    }
}
