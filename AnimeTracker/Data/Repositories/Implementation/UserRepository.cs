using AnimeTracker.Data.Repositories.Implementation.Base;
using AnimeTracker.Data.Repositories.Interface;
using AnimeTracker.Models;

namespace AnimeTracker.Data.Repositories.Implementation;

public class UserRepository:Repository<Users>,IUsers
{
    public UserRepository(AppDbContext context) : base(context)
    {
    }
}