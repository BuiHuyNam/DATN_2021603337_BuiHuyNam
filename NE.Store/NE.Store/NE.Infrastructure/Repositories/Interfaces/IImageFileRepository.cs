﻿using NE.Domain.Entitis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NE.Infrastructure.Repositories.Interfaces
{
    public interface IImageFileRepository  :IGenericRepository<ImageFile>
    {
        Task<List<ImageFile>> GetAllFileAsync();


    }
}
