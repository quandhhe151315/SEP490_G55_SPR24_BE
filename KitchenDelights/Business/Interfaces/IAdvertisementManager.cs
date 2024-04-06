using Business.DTO;
using Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Interfaces
{
    public interface IAdvertisementManager
    {
        Task<bool> CreateAdvertisement(AdvertisementDTO advertisement);

        Task<bool> UpdateAdvertisement(AdvertisementDTO advertisement);

        Task<bool> UpdateAdvertisementStatus(int id, int status);

        Task<bool> DeleteAdvertisement(int id);

        Task<AdvertisementDTO?> GetAdvertisementById(int id);

        Task<List<AdvertisementDTO>> GetAdvertisements();
    }
}
