﻿using AutoMapper;
using Business.DTO;
using Business.Interfaces;
using Data.Entity;
using Data.Interfaces;
using Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Managers
{
    public class AdvertisementManager : IAdvertisementManager
    {
        private readonly IAdvertisementRepository _advertisementRepository;
        private readonly IMapper _mapper;

        public AdvertisementManager(IAdvertisementRepository advertisementRepository, IMapper mapper)
        {
            _advertisementRepository = advertisementRepository;
            _mapper = mapper;
        }

        public async Task<bool> CreateAdvertisement(AdvertisementDTO advertisement)
        {
            try
            {
                _advertisementRepository.CreateAdvertisement(_mapper.Map<AdvertisementDTO, Advertisement>(advertisement));
                _advertisementRepository.Save();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<bool> DeleteAdvertisement(int id)
        {
            Advertisement? advertisement = await _advertisementRepository.GetAdvertisementById(id);
            if (advertisement == null) return false;

            _advertisementRepository.DeleteAdvertisement(advertisement);
            _advertisementRepository.Save();
            return true;
        }

        public async Task<AdvertisementDTO?> GetAdvertisementById(int id)
        {
            Advertisement? advertisement = await _advertisementRepository.GetAdvertisementById(id);
            return advertisement is null ? null : _mapper.Map<Advertisement, AdvertisementDTO>(advertisement);
        }

        public async Task<List<AdvertisementDTO>> GetAdvertisements()
        {
            List<Advertisement> advertisements = await _advertisementRepository.GetAdvertisements();
            List<AdvertisementDTO> advertisementDTOs = new List<AdvertisementDTO>();
            foreach (Advertisement advertisement in advertisements)
            {
                advertisementDTOs.Add(_mapper.Map<Advertisement, AdvertisementDTO>(advertisement));
            }
            return advertisementDTOs;
        }

        public async Task<bool> UpdateAdvertisement(AdvertisementDTO advertisementDTO)
        {
            Advertisement? advertisement = await _advertisementRepository.GetAdvertisementById(advertisementDTO.AdvertisementId.Value);
            if (advertisement == null) return false;

            advertisement.AdvertisementImage = advertisementDTO.AdvertisementImage;
            advertisement.AdvertisementLink = advertisementDTO.AdvertisementLink;
            _advertisementRepository.UpdateAdvertisement(advertisement);
            _advertisementRepository.Save();
            return true;
        }

        public async Task<bool> UpdateAdvertisementStatus(int id, int status)
        {
            Advertisement? advertisement = await _advertisementRepository.GetAdvertisementById(id);
            if (advertisement == null) return false;

            advertisement.AdvertisementStatus = status;
            _advertisementRepository.UpdateAdvertisement(advertisement);
            _advertisementRepository.Save();
            return true;
        }
    }
}
