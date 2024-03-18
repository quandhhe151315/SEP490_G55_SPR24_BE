using AutoMapper;
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
    public class VoucherManager : IVoucherManager
    {
        private readonly IVoucherRepository _voucherRepository;
        private readonly IMapper _mapper;

        public VoucherManager(IVoucherRepository voucherRepository, IMapper mapper)
        {
            _voucherRepository = voucherRepository;
            _mapper = mapper;
        }

        public async Task<bool> CreateVoucher(VoucherDTO voucherDTO)
        {
            Voucher? voucher = await _voucherRepository.GetVoucher(voucherDTO.VoucherCode);
            if (voucher != null) return true;

            _voucherRepository.CreateVoucher(_mapper.Map<VoucherDTO, Voucher>(voucherDTO));
            _voucherRepository.Save();
            return false;
        }

        public async Task<VoucherDTO?> GetVoucher(string voucherCode)
        {
            Voucher? voucher = await _voucherRepository.GetVoucher(voucherCode);
            return voucher is null ? null : _mapper.Map<Voucher, VoucherDTO>(voucher);
        }

        public async Task<List<VoucherDTO>> GetVouchers(int userId)
        {
            List<VoucherDTO> voucherDTOs = [];
            List<Voucher> vouchers = await _voucherRepository.GetVouchers(userId);
            foreach (Voucher voucher in vouchers)
            {
                voucherDTOs.Add(_mapper.Map<Voucher, VoucherDTO>(voucher));
            }
            return voucherDTOs;
        }

        public async Task<bool> RemoveVoucher(string voucherCode)
        {
            Voucher? voucher = await _voucherRepository.GetVoucher(voucherCode);
            if(voucher == null) return false;
            _voucherRepository.RemoveVoucher(voucher);
            _voucherRepository.Save();
            return true;
        }

        public async Task<bool> UpdateVoucher(VoucherDTO VoucherDTO)
        {
            Voucher? voucher = await _voucherRepository.GetVoucher(VoucherDTO.VoucherCode);
            if (voucher == null) return false;
            voucher.VoucherCode = VoucherDTO.VoucherCode;
            voucher.UserId = VoucherDTO.UserId;
            voucher.DiscountPercentage = VoucherDTO.DiscountPercentage;
            _voucherRepository.UpdateVoucher(voucher);
            _voucherRepository.Save();
            return true;
        }
    }
}
