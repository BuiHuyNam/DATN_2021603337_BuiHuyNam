using NE.Domain.Entitis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NE.Application.Services.Interfaces
{
    public interface IFeedbackService
    {
        Task<IEnumerable<Feedback>> GetAllFeedbackAsync();
        Task<Feedback> GetFeedbackByIdAsync(int id);
        Task AddFeedbackAsync(Feedback feedback);
        Task DeleteFeedbackAsync(int id);
        Task UpdateFeedbackAsync(Feedback feedback);
        Task<List<Feedback>> GetByIdProductAsync(int idProduct);


    }
}
