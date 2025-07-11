﻿using Microsoft.EntityFrameworkCore;
using NE.Domain.Entitis;
using NE.Infrastructure.Context;
using NE.Infrastructure.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NE.Infrastructure.Repositories.Implementations
{
    public class ProductRepository : GenericRepository<Product>, IProductRepository
    {
        public ProductRepository(NEContext context) : base(context)
        {
            
        }

        public override async Task<IEnumerable<Product>> GetAllAsync()
        {
            return await _context.Set<Product>()
                .Include(c => c.Category)
                .Include(b => b.Brand)
                .Include(pc => pc.ProductColors)
                    .ThenInclude(c => c.Color)
                .Include(p => p.ProductColors)
                    .ThenInclude(pc => pc.ImageFiles)
                .ToListAsync();
        }

        public async override Task<Product> GetByIdAsync(int id)
        {
            return await _context.Set<Product>()
                .Include(c => c.Category)
                .Include(b => b.Brand)
                .Include(pc => pc.ProductColors)
                    .ThenInclude(c => c.Color)
                .Include(p => p.ProductColors)
                    .ThenInclude(pc => pc.ImageFiles)
                .FirstOrDefaultAsync(bp => bp.Id == id);
        }

            public IQueryable<Product> GetAllForPaging()
            {
                return _context.Set<Product>()
                  
                    .Include(c => c.Category)
                    .Include(b => b.Brand)
                    .Include(pc => pc.ProductColors)
                        .ThenInclude(c => c.Color)
                    .Include(p => p.ProductColors)
                        .ThenInclude(pc => pc.ImageFiles);
            }

        public Task<List<Product>> Top5NewProduct()
        {
            return _context.Set<Product>()
                .Where(p => p.IsActive == true)
                 .Include(c => c.Category)
                .Include(b => b.Brand)
                .Include(pc => pc.ProductColors)
                    .ThenInclude(c => c.Color)
                .Include(p => p.ProductColors)
                    .ThenInclude(pc => pc.ImageFiles)
                .OrderByDescending(p => p.CreatedDate)
                .Take(5)
                .ToListAsync();
        }
    }
}
