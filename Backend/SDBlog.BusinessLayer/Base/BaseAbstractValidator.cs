using FluentValidation;
using System.Collections.Generic;

namespace SDBlog.BusinessLayer.Validators.Base
{
    public class BaseAbstractValidator<TModel> : AbstractValidator<TModel> where TModel : class
    {
        public readonly IEnumerable<TModel> _collection;
        public readonly TModel _entity;

        public BaseAbstractValidator(IEnumerable<TModel> collection)
        {
            _collection = collection;
            //validaciones             
        }

        public BaseAbstractValidator()
        {
            //validaciones             
        }

        // {
        // Se trato de implementar este metodo para llevar esto a todos los validadores,
        // en el cual se pasara el nombre del campo en String y se buscara si existe.
        // pero tuvimos problemas para pasarle el parametro con un predicate en el MUST de RulesFor.
        //
        // RuleFor(x => x.Codigo).Must(EsUnico(x => x.Codigo,"Codigo"))
        //
        // }

        //public bool EsUnico(string nuevoValor, string campo)
        //{
        //    return _collection.All(x =>
        //      x.Equals(_entity) || x.GetType().GetProperty(campo).ToString() != nuevoValor);
        //}
    }
}
