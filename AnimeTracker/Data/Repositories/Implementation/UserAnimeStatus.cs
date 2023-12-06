using AnimeTracker.Data.Repositories.Implementation.Base;
using AnimeTracker.Data.Repositories.Interface;

namespace AnimeTracker.Data.Repositories.Implementation;

public class UserAnimeStatus:Repository<Models.UserAnimeStatus>,IUserAnimeStatus
{
    public UserAnimeStatus(AppDbContext context) : base(context)
    {
    }
}