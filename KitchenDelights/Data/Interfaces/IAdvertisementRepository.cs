using Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Interfaces
{
    public interface IAdvertisementRepository
    {
        void CreateAdvertisement(Advertisement advertisement);
        void UpdateAdvertisement(Advertisement advertisement);
        void DeleteAdvertisement(Advertisement advertisement);
        Task<Advertisement?> GetAdvertisementById(int id);
        Task<List<Advertisement>> GetAdvertisements();
        void Save();
    }
}
