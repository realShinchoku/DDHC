using FluentValidation;
using OWAdministrativeService.DTOs;

namespace OWAdministrativeService.Validators;

public class StudentVerifyFormDtoValidator : AbstractValidator<StudentVerifyFormDto>
{
    public StudentVerifyFormDtoValidator()
    {
        RuleFor(x => x.FullName).NotEmpty().WithMessage("Họ và tên không được để trống");
        RuleFor(x => x.Sex).NotEmpty().WithMessage("Giới tính không được để trống");
        RuleFor(x => x.BirthDay).NotEmpty().WithMessage("Ngày sinh không được để trống");
        RuleFor(x => x.Class).NotEmpty().WithMessage("Lớp hiện đang học không được để trống");
        RuleFor(x => x.StudentCode).NotEmpty().WithMessage("Mã sinh viên không được để trống");
        RuleFor(x => x.PhoneNumber).NotEmpty().WithMessage("Số điện thoại không được để trống");
        RuleFor(x => x.Faculty).NotEmpty().WithMessage("Khoa không được để trống");
        RuleFor(x => x.IdNumber).NotEmpty().WithMessage("Số CMND/CCCD không được để trống");
        RuleFor(x => x.IdDateIssued).NotEmpty().WithMessage("Ngày cấp CMND/CCCD không được để trống");
        RuleFor(x => x.Purpose).IsInEnum().NotNull().WithMessage("Mục đích xin xác nhận không được để trống");
    }
}