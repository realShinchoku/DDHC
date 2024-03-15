export interface Form {
    id: string;
    email: string;
    body: StudentCardForm | StudentVerifyForm;
    fileSrc: string;
    fileName: string;
    type: FormType;
    createdAt: Date;
    updatedAt: Date;
    status: FormStatus;
    dateToGetResult?: Date;
    note: string;
}

export interface Response {
    items: Form[];
    count: number;
    totalForms: number;
    approvedForms: number;
}

export enum FormStatus {
    Pending,
    Approved,
    Rejected
}

export const getFormStatus = (status: FormStatus): string => {
    switch (status) {
        case FormStatus.Pending:
            return 'Chưa duyệt';
        case FormStatus.Approved:
            return 'Đã chấp thuận';
        case FormStatus.Rejected:
            return 'Đã từ chối';
        default:
            return '';
    }
}

export enum FormType {
    Card,
    Verify
}

export const getFormType = (type: FormType): string => {
    switch (type) {
        case FormType.Card:
            return 'Đơn xin cấp thẻ sinh viên';
        case FormType.Verify:
            return 'Đơn xin xác nhận sinh viên';
        default:
            return '';
    }
}

export interface StudentCardForm {
    FullName: string;
    BirthDay: string;
    CurrentClass: string;
    FirstClass: string;
    StudentCode: string;
    Course: string;
    Reason: Reason;
    StudentType: string;
    Photo3X4: string;
    FrontIdPhoto: string;
    BackIdPhoto: string;
    Code: string;
    CreatedDate: string;
    CardReturnDate: string;
}

export enum Reason {
    FirstCreate,
    PrintingError,
    Damaged,
    ReturningToSchool,
    Lost
}

export const getReason = (reason: Reason): string => {
    switch (reason) {
        case Reason.FirstCreate:
            return 'Làm thẻ lần đầu';
        case Reason.PrintingError:
            return 'Làm lại thẻ do lỗi in ấn';
        case Reason.Damaged:
            return 'Làm lại thẻ do gẫy, hỏng';
        case Reason.ReturningToSchool:
            return 'Làm lại thẻ do trở lại học cùng khóa sau';
        case Reason.Lost:
            return 'Làm lại thẻ do mất thẻ';
        default:
            return '';

    }
}

export interface StudentVerifyForm {
    FullName: string;
    Sex: string;
    BirthDay: string;
    Class: string;
    StudentCode: string;
    PhoneNumber: string;
    Faculty: string;
    IdNumber: string;
    IdDateIssued: string;
    Purpose: Purpose;
    Code: string;
}

export enum Purpose {
    TaxReduction,
    MilitaryServicePostponement,
    PartTimeJob,
    TemporaryResidence,
    VisaApplication,
    StudyPeriodConfirmation,
    Other
}

export const getPurpose = (purpose: Purpose): string => {
    switch (purpose) {
        case Purpose.TaxReduction:
            return 'Xác nhận đang là sinh viên để giảm trừ Thuế thu nhập cá nhân';
        case Purpose.MilitaryServicePostponement:
            return 'Xác nhận đang là sinh viên để xin tạm hoãn Nghĩa vụ Quân sự';
        case Purpose.PartTimeJob:
            return 'Xác nhận đang là sinh viên để đi xin việc làm thêm';
        case Purpose.TemporaryResidence:
            return 'Xác nhận đang là sinh viên để xin tạm trú tạm vắng';
        case Purpose.VisaApplication:
            return 'Xác nhận đang là sinh viên để xin làm Visa';
        case Purpose.StudyPeriodConfirmation:
            return 'Xác nhận khoảng thời gian đã từng học tập tại trường (Dành cho sinh viên đã Thôi học, Tốt nghiệp)';
        default:
            return 'Mục đích khác';
    }
}