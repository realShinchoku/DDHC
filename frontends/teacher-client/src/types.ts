export interface KeyCount {
    name: string;
    count: number;
}

export interface Student {
    id: string;
    name: string;
    email: string;
    phone: string;
    studentCode: string;
    class: string;
    grade: string;
    faculty: string;
    status: UserStatus;
    birthDay: Date;
}

export interface Lesson {
    id: string;
    subjectId: string;
    startTime: Date;
    endTime: Date;
    createdAt: Date;
    updatedAt: Date;
    name: string;
    code: string;
    room: string;
    isEnded: boolean;
    teacherName: string;
    openAttendanceTime: string;
    closeAttendanceTime: string;
    studentEmails: string[];
    wifi: KeyCount[];
    bluetooth: KeyCount[];
}

export interface Subject {
    id: string;
    teacherId: string;
    room: string;
    name: string;
    code: string;
    isEnded: boolean;
    dateStart: Date;
    dateEnd: Date;
    lessons: Lesson[];
    students: Student[];
    studentsCount: number;
    lessonsCount: number;
    currentLessonsCount: number;
}

export interface AttendanceStudent {
    studentId: string;
    studentName: string;
    studentCode: string;
    lessons: AttendanceLesson[];
}

export interface AttendanceLesson {
    lessonId: string;
    startTime: string;
    attendedTime: string;
    type?: AttendanceType;
}

export enum AttendanceType {
    Presence,
    Late,
    Absent
}

export interface Teacher {
    id: string;
    name: string;
    email: string;
    phone: string;
    teacherCode: string;
    faculty: string;
    status: UserStatus;
    birthDay: Date;
}


export enum UserStatus {
    Inactive = 0,
    Active = 1,
}