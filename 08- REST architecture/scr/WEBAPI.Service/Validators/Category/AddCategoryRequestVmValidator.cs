using FluentValidation;
using WEBAPI.Service.ViewModels;

namespace WEBAPI.Service.Validators
{
    public class AddCategoryRequestVmValidator : AbstractValidator<AddCategoryRequestVm>
    {
        public AddCategoryRequestVmValidator()
        {
            RuleFor(u => u).NotNull().WithMessage("Category Name Can't Be Null");

            RuleFor(u => u.Name)
                .Cascade(CascadeMode.Stop)
                .NotNull().WithMessage("Category Name Can't Be Null")
                .NotEmpty().WithMessage("Category Name Can't Be Null");

        }
    }
}
