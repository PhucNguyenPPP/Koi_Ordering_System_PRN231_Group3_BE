﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.DTO.General;
using Common.DTO.KoiFish;
using Common.DTO.Order;

namespace Service.Interfaces
{
    public interface IOrderService
    {
        Task<ResponseDTO> CheckValidationCreateOrder(CreateOrderDTO createOrderDTO);
        Task<ResponseDTO> CreateOrder(CreateOrderDTO createOrderDTO);
        Task<bool> CheckOrderExist(Guid orderId);
        Task<bool> UpdateOrderPackaging(Guid orderId, UpdateOrderPackagingRequest request);
        Task<ResponseDTO> GetAllHistoryOrder(Guid userId);
    }
}