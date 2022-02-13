using FluentValidation;
using SDBlog.BusinessLayer.Validators.Base;
using SDBlog.DataModel.Context;
using SDBlog.DataModel.Entities.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDBlog.BusinessLayer.Validators.Users
{
    public class UserValidator : AbstractValidatorBase<User>
    {
        public UserValidator() 
        {
            RuleFor(x => x.FirstName).NotNull().Length(10,20);
        }
    }
}
