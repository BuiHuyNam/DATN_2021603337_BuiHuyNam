﻿using NE.Domain.Entitis;
using NE.Infrastructure.Context;
using NE.Infrastructure.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NE.Infrastructure.Repositories.Implementations
{
    public class ColorRepository : GenericRepository<Color>, IColorRepository
    {
        public ColorRepository(NEContext context) : base(context)
        {
        }
    }
}
