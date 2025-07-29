using AutoMapper;
using SkillHubApi.Models;
using SkillHubApi.Dtos;
using System;

namespace SkillHubApi.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Tag, TagDto>();
            CreateMap<TagDto, Tag>();
           CreateMap<TagCreateDto, Tag>()
            .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore());
            CreateMap<TagUpdateDto, Tag>(); 

            CreateMap<User, UserDto>();
            CreateMap<UserCreateDto, User>();
            CreateMap<UserUpdateDto, User>();

            CreateMap<LessonCreateDto, Lesson>()
                .ForMember(dest => dest.Difficulty, opt => opt.MapFrom(src =>
                    Enum.IsDefined(typeof(DifficultyLevel), src.Difficulty)
                        ? (DifficultyLevel)Enum.Parse(typeof(DifficultyLevel), src.Difficulty)
                        : DifficultyLevel.Beginner));

            CreateMap<LessonUpdateDto, Lesson>()
                .ForMember(dest => dest.Difficulty, opt => opt.MapFrom(src =>
                    Enum.IsDefined(typeof(DifficultyLevel), src.Difficulty)
                        ? (DifficultyLevel)Enum.Parse(typeof(DifficultyLevel), src.Difficulty)
                        : DifficultyLevel.Beginner));

            CreateMap<Lesson, LessonDto>()
                .ForMember(dest => dest.Difficulty, opt => opt.MapFrom(src => src.Difficulty.ToString()));

            CreateMap<Review, ReviewDto>()
            .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.CreatedAt));
            CreateMap<ReviewCreateDto, Review>()
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore());
            CreateMap<ReviewUpdateDto, Review>()
                .ForMember(dest => dest.Id, opt => opt.Ignore());

            CreateMap<LessonTag, LessonTagDto>();
            CreateMap<LessonTagCreateDto, LessonTag>();
            CreateMap<LessonTagUpdateDto, LessonTag>();

            CreateMap<LessonEnrollment, LessonEnrollmentDto>();
            CreateMap<LessonEnrollmentCreateDto, LessonEnrollment>();

            CreateMap<Attendance, AttendanceDto>();
            CreateMap<AttendanceCreateDto, Attendance>();
            CreateMap<AttendanceUpdateDto, Attendance>();

            CreateMap<ReportedReview, ReportedReviewDto>();
            CreateMap<ReportedReviewUpdateDto, ReportedReview>();
            CreateMap<ReportedReviewCreateDto, ReportedReview>();

            CreateMap<FileResource, FileResourceDto>();
            CreateMap<FileResourceCreateDto, FileResource>();
            CreateMap<FileResourceUpdateDto, FileResource>();

            CreateMap<ReportRequest, ReportRequestDto>();
            CreateMap<ReportRequestCreateDto, ReportRequest>();
            CreateMap<ReportRequestUpdateDto, ReportRequest>();
        }
    }
}
