using AnimeTracker.Data.Repositories.Interface.Base;
using AnimeTracker.Models;

namespace AnimeTracker.Data.Repositories.Interface;

public interface ICategoryRepository:IRepository<Category>
{
    
    
    Category FindByName(string categoryName);

}