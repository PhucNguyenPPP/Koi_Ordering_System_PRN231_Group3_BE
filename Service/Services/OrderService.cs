﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Common.DTO.General;
using Common.DTO.KoiFish;
using Common.DTO.Order;
using Common.Enum;
using DAL.Entities;
using DAL.UnitOfWork;
using Microsoft.IdentityModel.Tokens;
using Service.Interfaces;

namespace Service.Services
{
    public class OrderService : IOrderService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        public OrderService(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<ResponseDTO> CheckValidationCreateOrder(CreateOrderDTO createOrderDTO) 
        {
            var customer = _unitOfWork.User.GetAllByCondition(c => c.UserId == createOrderDTO.CustomerId /*&& c.Role.RoleName.Equals(RoleEnum.Customer)*/);
            if (customer.IsNullOrEmpty()) 
                return new ResponseDTO("Customer not exist!", 400, false);

            Guid farm = Guid.NewGuid();
            for(int i = 0; i < createOrderDTO.CartId.Count; i++)
            {
                var cart = await _unitOfWork.Cart.GetByCondition(c => c.CartId == createOrderDTO.CartId[i]);
                if (cart == null)
                    return new ResponseDTO("Cart not exist!", 400, false);

                var koi = await _unitOfWork.Koi.GetByCondition(c=> c.KoiId == cart.KoiId && c.Status == true);
                if (koi == null)
                    return new ResponseDTO("Koi not exist!", 400, false);
                if(i == 0) farm = koi.FarmId;
                
                if(i > 0)
                {
                    if(farm != koi.FarmId)
                    {
                        return new ResponseDTO("Unable to select Koi from different farms!", 400, false);
                    }
                }
            }
            

            var storage = _unitOfWork.StorageProvince.GetAllByCondition(c => c.Country.Equals("Vietnam"));
            if (!storage.Any(c => c.StorageProvinceId.Equals(createOrderDTO.StorageVietNamId)))
                return new ResponseDTO("Invalid Viet Nam storage province!", 400, false);

            return new ResponseDTO("Check validation successfully!", 200, true);
        } 

        public async Task<bool> CreateOrder(CreateOrderDTO createOrderDTO)
        {
            var order = _mapper.Map<Order>(createOrderDTO);
            if (createOrderDTO == null)
            {
                throw new ArgumentNullException(nameof(createOrderDTO), "createOrderDTO is null");
            }

            if (order == null)
            {
                throw new NullReferenceException("Mapping from CreateOrderDTO to Order failed.");
            }
            //OrderId
            var orderId = Guid.NewGuid();
            order.OrderId = orderId;

            //OrderNumber
            Random rand = new Random();
            string num = "";
            var orderNum = _unitOfWork.Order.GetAll();
            do
            {
                num = "B" + rand.Next(999);
                order.OrderNumber = num;
            } while (orderNum.Any(c => c.OrderNumber == num));
            
            //OrderCreateDate
            order.CreatedDate = DateTime.Now;

            //Order Status
            order.Status = true.ToString();
            List<Guid> koiId = new List<Guid>();
            decimal totalPrice = 0;
            decimal shippingFee = 0;
            Guid? jpnStorage = Guid.NewGuid();
            for (int i = 0; i < createOrderDTO.CartId.Count(); i++)
            {
                var cart = await _unitOfWork.Cart.GetByCondition(c => c.CartId == createOrderDTO.CartId[i]);
                koiId.Add(cart.KoiId);

                //update Koi
                var koi = _unitOfWork.Koi.GetAllByCondition(c => c.KoiId == koiId[i]).FirstOrDefault();
                koi.Status =false;
                koi.OrderId = orderId;
                _unitOfWork.Koi.Update(koi);

                totalPrice += cart.Koi.Price;
                if(i == 0)
                {
                    var farm = _unitOfWork.KoiFarm.GetAllByCondition(c => c.KoiFarmId == cart.Koi.FarmId).FirstOrDefault();
                    
                    jpnStorage = farm.StorageProvinceId;
                    shippingFee = _unitOfWork.ShippingFee
                    .GetAllByCondition(c => c.StorageProvinceVnId == createOrderDTO.StorageVietNamId
                    && c.StorageProvinceJpId == jpnStorage)
                    .Select(c=> c.Price).FirstOrDefault();
                    order.ShippingFee = shippingFee.ToString();
                }
                if(i == createOrderDTO.CartId.Count() - 1)
                {
                    totalPrice += shippingFee;
                    order.TotalPrice = totalPrice;
                    

                    await _unitOfWork.Order.AddAsync(order);
                    await _unitOfWork.SaveChangeAsync();
                }
                _unitOfWork.Cart.Delete(cart);
            }
            OrderStorage orderStorage1 = new OrderStorage();
            orderStorage1.OrderId = orderId;
            orderStorage1.Status = true;
            if(jpnStorage != null) orderStorage1.StorageProvinceId = (Guid)jpnStorage;
            orderStorage1.OrderStorageId = Guid.NewGuid();
            await _unitOfWork.OrderStorage.AddAsync(orderStorage1);
            await _unitOfWork.SaveChangeAsync();

            OrderStorage orderStorage2 = new OrderStorage();
            orderStorage2.OrderId = orderId;
            orderStorage2.Status = true;
            orderStorage2.StorageProvinceId = createOrderDTO.StorageVietNamId;
            orderStorage2.OrderStorageId = Guid.NewGuid();
            await _unitOfWork.OrderStorage.AddAsync(orderStorage2);

            return await _unitOfWork.SaveChangeAsync();
        }

        public async Task<bool> CheckOrderExist(Guid orderId)
        {
            var order = await _unitOfWork.Order.GetByCondition(c => c.OrderId == orderId);
            if(order == null)
            {
                return false;
            }
            return true;
        }
    }
}
