using FluentValidation;
using System;
using System.ComponentModel.DataAnnotations;
using WEBAPI.Common.Enums;
using WEBAPI.Service.ViewModels;

namespace WEBAPI.Service.Validators
{
    public class EditCategoryRequestVmValidator : AbstractValidator<EditCategoryRequestVm>
    {
        public EditCategoryRequestVmValidator()
        {
            RuleFor(u => u).NotNull().WithMessage("Category Name Can't Be Null");

            RuleFor(u => u.Name)
              .Cascade(CascadeMode.Stop)
              .NotNull().WithMessage("Category Name Can't Be Null")
              .NotEmpty().WithMessage("Category Name Can't Be Null");

        }
    }
}
