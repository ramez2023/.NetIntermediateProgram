using FluentValidation;
using FluentValidation.Results;
using System;
using WEBAPI.Common.Exceptions.Business;

namespace WEBAPI.Service.Validators
{
    public static class ValidatorHandler
    {
        public static void Validate<M>(this M model, Func<AbstractValidator<M>> validatorFactory) where M : class
        {
            var validator = validatorFactory();
            ValidationResult validationResult = validator.Validate(model);
            if (!validationResult.IsValid)
                ThrowValidationException(validationResult);
        }

        private static void ThrowValidationException(ValidationResult validationResult)
        {
            foreach (var error in validationResult.Errors)
            {
                throw new InvalidParameterValidationException(error.ErrorMessage);
            }
        }
    }
}
