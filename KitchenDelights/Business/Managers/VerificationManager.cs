using AutoMapper;
using Business.DTO;
using Business.Interfaces;
using Data.Entity;
using Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Managers
{
    public class VerificationManager : IVerificationManager
    {
        private readonly IVerificationRepository _verificationRepository;
        private readonly IMapper _mapper;

        public VerificationManager(IVerificationRepository verificationRepository, IMapper mapper)
        {
            _verificationRepository = verificationRepository;
            _mapper = mapper;
        }

        public async Task<List<VerificationDTO>> GetVerifications()
        {
            List<VerificationDTO> verificationDTOs = [];
            List<Verification> verifications = await _verificationRepository.GetVerifications();
            foreach (Verification verification in verifications)
            {
                verificationDTOs.Add(_mapper.Map<Verification, VerificationDTO>(verification));
            }
            return verificationDTOs;
        }

        public async Task<bool> CreateVerification(VerificationDTO verification)
        {
            try
            {
                _verificationRepository.CreateVerification(_mapper.Map<VerificationDTO, Verification>(verification));
                _verificationRepository.Save();
                return true;
            } catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
                return false;
            }
        }

        public async Task<bool> UpdateVerification(VerificationDTO verification)
        {
            Verification? verify = await _verificationRepository.GetVerification(verification.VerificationId.Value);
            if (verify == null) return false;

            verify.VerificationStatus = verification.VerificationStatus;
            verify.VerificationDate = DateTime.Now;

            try
            {
                _verificationRepository.UpdateVerification(verify);
                _verificationRepository.Save();
                return true;
            } catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
                return false;
            }
        }
    }
}
