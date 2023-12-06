using AnimeTracker.Controllers;
using AnimeTracker.Data.Repositories.Interface.Base;
using AnimeTracker.Models;

namespace AnimeTracker.Data.Repositories.Interface;

public interface IAnimeRepository:IRepository<Anime>
{

    List<Anime> FindByName(string search,string type,string cate);

}