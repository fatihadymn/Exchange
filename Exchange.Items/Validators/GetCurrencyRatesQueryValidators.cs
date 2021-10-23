using Exchange.Items.Models.Enum;
using Exchange.Items.Queries;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Exchange.Items.Validators
{
    public class GetCurrencyRatesQueryValidators : AbstractValidator<GetCurrencyRatesQuery>
    {
        public GetCurrencyRatesQueryValidators()
        {
            List<string> codes = Enum.GetNames(typeof(Codes)).ToList();

            RuleFor(x => x.Code).NotNull()
                                .WithMessage("Code can not be null")
                                .NotEmpty()
                                .WithMessage("Code can not be empty")
                                .Must(x => codes.Contains(x.ToUpper()))
                                .WithMessage($"Code should be these values : ({string.Join(',', Enum.GetNames(typeof(Codes)).ToList())})");

        }
    }
}
