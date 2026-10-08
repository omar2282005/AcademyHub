using AcademyHub.ViewModel;
using AutoMapper;

namespace AcademyHub.Mapping
{
    public class MappingProfile:Profile
    {
        public MappingProfile()
        {
            CreateMap<Course, CourseListItemViewModel>()
         .ForMember(
             destination => destination.CourseId,
             options => options.MapFrom(source => source.Id)
         )
         .ForMember(
             destination => destination.CourseName,
             options => options.MapFrom(source => source.Title)
         )
         .ForMember(
             destination => destination.InstructorName,
             options => options.MapFrom(source => source.Instructor.Name)
         );
            CreateMap<Enrollment, EnrolledStudentViewModel>()
        .ForMember(
            destination => destination.StudentName,
            options => options.MapFrom(source => source.Student.Name)
        );
            // Details 
            CreateMap<Course, CourseDetailsViewModel>()
        .ForMember(
                d => d.CourseId,
                op => op.MapFrom(source => source.Id))
                  .ForMember(
                d => d.CourseName,
                op => op.MapFrom(s => s.Title))
                  .ForMember(
                d => d.InstructorName,
                op => op.MapFrom(s => s.Instructor.Name))
                  .ForMember(
                  d => d.InstructorBio,
                  op => op.MapFrom(s =>
                      s.Instructor.Profile != null
                          ? s.Instructor.Profile.Bio
                          : null
                              ))
                  .ForMember(
                d => d.StudentCount,
                op => op.MapFrom(s => s.Enrollments.Count))
                  .ForMember(
                d => d.Students,
                op => op.MapFrom(s => s.Enrollments)
        );

            CreateMap<CreateCourseViewModel, Course>()
                .ForMember(
                d => d.Id,
                op => op.Ignore())
                .ForMember(
                d => d.CreatedByUserId,
                op => op.Ignore())
                .ForMember(
                d => d.Enrollments,
                op => op.Ignore())
                .ForMember(
                d => d.Instructor,
                op => op.Ignore());

            CreateMap<Course, EditCourseViewModel>()
                .ForMember(
                    destination => destination.CourseId,
                    options => options.MapFrom(source => source.Id)
                )
                .ForMember(
                    destination => destination.Instructors,
                    options => options.Ignore()
                );
        }
    }
}
