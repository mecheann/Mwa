using FluentValidation;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dci.Mnm.Mwa.Core;

namespace Dci.Mnm.Mwa.Domain
{
    public class UserValidator : AbstractValidator<User>
    {
        public UserValidator(IStringLocalizer<User> localizer)
        {
            RuleFor(x => x.Id).NotEmpty().WithMessage(x => localizer[AppConst.LocalizationString.IdRequired]);
            RuleFor(x => x.FirstName).NotEmpty();
            RuleFor(x => x.Email).NotEmpty();
        }
    }
}
