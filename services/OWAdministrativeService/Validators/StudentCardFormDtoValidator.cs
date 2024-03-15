using FluentValidation;
using OWAdministrativeService.DTOs;
using OWAdministrativeService.Models;

namespace OWAdministrativeService.Validators;

public class StudentCardFormDtoValidator : AbstractValidator<StudentCardFormDto>
{
    public StudentCardFormDtoValidator()
    {
        RuleFor(x => x.FullName).NotEmpty().WithMessage("Họ và tên không được để trống");
        RuleFor(x => x.BirthDay).NotEmpty().WithMessage("Ngày sinh không được để trống");
        RuleFor(x => x.CurrentClass).NotEmpty().WithMessage("Lớp hiện tại không được để trống");
        RuleFor(x => x.FirstClass).NotEmpty().WithMessage("Lớp đầu tiên không được để trống");
        RuleFor(x => x.StudentCode).NotEmpty().WithMessage("Mã sinh viên không được để trống");
        RuleFor(x => x.Course).NotEmpty().WithMessage("Khóa học không được để trống");
        RuleFor(x => x.StudentType).NotEmpty().WithMessage("Loại sinh viên không được để trống");
        RuleFor(x => x.Reason).IsInEnum().NotNull().WithMessage("Lý do không được để trống");
        RuleFor(x => x.Photo3X4).NotEmpty().WithMessage("Ảnh 3x4 không được để trống")
            .When(x => x.Reason == Reason.FirstCreate);
        RuleFor(x => x.FrontIdPhoto).NotEmpty().WithMessage("Ảnh mặt trước CMND không được để trống")
            .When(x => x.Reason == Reason.FirstCreate);
        RuleFor(x => x.BackIdPhoto).NotEmpty().WithMessage("Ảnh mặt sau CMND không được để trống")
            .When(x => x.Reason == Reason.FirstCreate);
    }
}