﻿using Business.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Interfaces
{
    public interface IRoleManager
    {
        Task<List<RoleDTO>> GetRoles();
    }
}
