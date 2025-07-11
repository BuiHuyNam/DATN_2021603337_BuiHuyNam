﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NE.Application.Dtos.BaseDto
{
    public class PageResultDto<T>
    {
        public int Page { get; set; }
        public int PageSize { get; set; }
        public int TotalItem { get; set; }
        public int TotalPage => (int)Math.Ceiling((float)TotalItem / PageSize);
        public List<T> Items { get; set; }

    }
}
