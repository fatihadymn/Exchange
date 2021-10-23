using Exchange.Items.Queries;
using FluentValidation;
using System.Collections.Generic;
using System.Linq;

namespace Exchange.Items.Validators
{
    public class GetAllCurrenciesQueryValidators : AbstractValidator<GetAllCurrenciesQuery>
    {
        public GetAllCurrenciesQueryValidators()
        {
            List<string> fields = new List<string>() { "rate", "code", "", null };

            RuleFor(x => x.SortingField).Must(x => fields.Contains(x?.ToLower()))
                                        .WithMessage($"Please use this fields for Sorting Fields: {string.Join(',', fields.Where(x=>!string.IsNullOrEmpty(x)))}");
        }
    }
}
