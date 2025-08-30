using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SchoolManagementSystem.Application.Common.Interfaces;
using SchoolManagementSystem.Application.Common.Interfaces.Repositories;
using SchoolManagementSystem.Application.Common.Interfaces.Securities;
using SchoolManagementSystem.Infrastructure.Data;
using SchoolManagementSystem.Infrastructure.Repository;
using SchoolManagementSystem.Infrastructure.Security;

namespace SchoolManagementSystem.Infrastructure
{
    public static class DependencyInjection
    {

        public static IServiceCollection AddInfrastructure(this IServiceCollection  services, IConfiguration configuration)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
            });

            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IStudentRepository, StudentRepository>();
            services.AddScoped<ITeacherRepository, TeacherRepository>();
            services.AddScoped<ICourseRepository, CourseRepository>();
            services.AddScoped<ISuspensionRequestRepository, SuspensionRequestRepository>();
            services.AddScoped<IStudentCourseRepository, StudentCourseRepository>();

            services.AddScoped<IPasswordHasher, PasswordHasher>();
            services.AddScoped<IUniqueIdGenerator, UniqueIdGenerator>();

            return services;
        }
    }
}
