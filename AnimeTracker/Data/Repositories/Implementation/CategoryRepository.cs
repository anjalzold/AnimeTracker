using AnimeTracker.Data.Repositories.Implementation.Base;
using AnimeTracker.Data.Repositories.Interface;
using AnimeTracker.Models;

namespace AnimeTracker.Data.Repositories.Implementation;

public class CategoryRepository:Repository<Category>,ICategoryRepository
{
    public CategoryRepository(AppDbContext context) : base(context)
    {
    }
}