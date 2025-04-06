using Microsoft.EntityFrameworkCore;
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
    public class ImageFileRepository : GenericRepository<ImageFile>, IImageFileRepository
    {
        public ImageFileRepository(NEContext context) : base(context)
        {
        }

        public async Task<List<ImageFile>> GetAllFileAsync()
        {
            return await _context.ImageFiles.ToListAsync();
        }
    }
}
