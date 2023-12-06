using AnimeTracker.Data.Repositories.Implementation.Base;
using AnimeTracker.Data.Repositories.Interface;
using AnimeTracker.Models;

namespace AnimeTracker.Data.Repositories.Implementation;

public class UserAnimeLists:Repository<UserAnimeList>,IUserAnimeList
{
    public UserAnimeLists(AppDbContext context) : base(context)
    {
    }
}