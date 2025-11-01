using System.Collections.Generic;
using API.Domain.Models;

namespace API.Domain.IRepository
{
    public interface IPlayerRepository
    {
        List<PlayerDM> GetAll();
        PlayerDM Get(string userName);
        PlayerDM Update(string userName, PlayerDM player);
        PlayerDM Create(string userName);
    }
}