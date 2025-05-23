﻿using Domain.Entities;
using Repository.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Repositories
{
    public interface IClientProfileRepository : IRepository<ClientProfile>
    {
        Task<ClientProfile?> GetByUserIdAsync(int userId);
        Task<(string Name, string Surname)?> GetNameSurnameByUserIdAsync(int userId);

    }
}
