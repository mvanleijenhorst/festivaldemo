using FestivalDemo.WebServer.Domain.Models;

namespace FestivalDemo.WebServer.Domain.Repository
{
    public interface IFestivalRepository
    {
        Festival GetFestival();
    }
}
