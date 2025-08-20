using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SMS_Clean_Arch.Domain.Interfaces;
using SMS_Clean_Arch.Domain.Interfaces.Repositories;
using SMS_Clean_Arch.Infrastructure.Repository;
using SMS_Clean_Arch.Infrastructure.SMS_Clean_Arch_Context;



namespace SMS_Clean_Arch.Infrastructure
{
    public static class DependencyInjection 
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<SMSCleanArchContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));


            // Add Services
            services.AddScoped<ISmsCleanArchContext>(provider => provider.GetService<SMSCleanArchContext>()!);
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IDepartmentRepository, DepartmentRepository>();
            services.AddScoped<ISubjectReppository, SubjectRespository>();

            return services;
        }
    }
}
