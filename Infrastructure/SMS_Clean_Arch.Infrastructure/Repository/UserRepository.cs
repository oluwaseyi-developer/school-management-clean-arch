using Microsoft.EntityFrameworkCore;
using SMS_Clean_Arch.Domain.Entities;
using SMS_Clean_Arch.Domain.Interfaces;
using SMS_Clean_Arch.Domain.Interfaces.Repositories;
using SMS_Clean_Arch.Infrastructure.SMS_Clean_Arch_Context;


namespace SMS_Clean_Arch.Infrastructure.Repository
{
    public class UserRepository : RepositoryBase<User>, IUserRepository
    {
        public UserRepository(SMSCleanArchContext context) : base(context) { }
    }
}
