using FluentValidation;
using WEBAPI.Service.ViewModels;

namespace WEBAPI.Service.Validators
{
    public class EditProductRequestVmValidator : AbstractValidator<EditProductRequestVm>
    {
        public EditProductRequestVmValidator()
        {
            RuleFor(u => u).NotNull().WithMessage("Category Name Can't Be Null");

            RuleFor(u => u.Name)
              .Cascade(CascadeMode.Stop)
              .NotNull().WithMessage("Category Name Can't Be Null")
              .NotEmpty().WithMessage("Category Name Can't Be Null");

            RuleFor(u => u.Price)
                .Cascade(CascadeMode.Stop)
                .GreaterThan(0).WithMessage("Product Price Can't Be Less Than Zero");

            RuleFor(u => u.CategoryId)
                .Cascade(CascadeMode.Stop)
                .GreaterThan(0).WithMessage("Category Id Can't Be Less Than Zero");


        }
    }
}
