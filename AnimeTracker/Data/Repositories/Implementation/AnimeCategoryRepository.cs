using AnimeTracker.Data.Repositories.Implementation.Base;
using AnimeTracker.Data.Repositories.Interface;
using AnimeTracker.Data.Repositories.Interface.Base;
using AnimeTracker.Models;

namespace AnimeTracker.Data.Repositories.Implementation;

public class AnimeCategoryRepository:Repository<AnimeCategory>,IAnimeCategory
{
    public AnimeCategoryRepository(AppDbContext context) : base(context)
    {
    }
}