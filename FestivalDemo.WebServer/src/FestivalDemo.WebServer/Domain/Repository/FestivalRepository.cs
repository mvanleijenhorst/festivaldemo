using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FestivalDemo.WebServer.Domain.Models;

namespace FestivalDemo.WebServer.Domain.Repository
{
    public class FestivalRepository : IFestivalRepository
    {
        private readonly Festival _festival;

        public FestivalRepository()
        {
            _festival = new Festival();
        }

        public Festival GetFestival()
        {
            return _festival;
        }
    }
}
