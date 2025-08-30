namespace SchoolManagementSystem.Application.Common.Interfaces
{
    public interface IUniqueIdGenerator
    {
        Task<string> GenerateStudentIdAsync();
        Task<string> GenerateTeacherIdAsync();
    }
}
