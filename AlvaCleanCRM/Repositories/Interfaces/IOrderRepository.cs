﻿using AlvaCleanCRM.Models.DTOs;
using AlvaCleanCRM.Models.RegisterModels;

namespace AlvaCleanCRM.Repositories.Interfaces
{
    public interface IOrderRepository
    {
        public Task CreateOrder(RegisterOrderDto model, string customerId);
    }
}
