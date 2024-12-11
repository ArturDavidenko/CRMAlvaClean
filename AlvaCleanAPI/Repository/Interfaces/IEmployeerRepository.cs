﻿using AlvaCleanAPI.Models;
using AlvaCleanAPI.Models.DTOs;
using System.IdentityModel.Tokens.Jwt;

namespace AlvaCleanAPI.Repository.Interfaces
{
    public interface IEmployeerRepository
    {
        public Task RegisterNewEmployeer(RegisterEmployeerModel model);

        public Task<JwtSecurityToken> LoginEmployeer(LoginEmployeerModel model);

        public Task DeleteEmployeer(string employeerId);

        public Task<Employeer> GetEmployeer(string employeerId);

        public Task UpdateEmployeer(EmployeerDto toUpdateEmployeer, string employeerId);

        public Task<List<Employeer>> GetListOfEmployeers();


    }
}