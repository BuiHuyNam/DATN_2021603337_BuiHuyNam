using Microsoft.EntityFrameworkCore.Metadata;
using NE.Application.Services.Interfaces;
using NE.Domain.Entitis;
using NE.Infrastructure.Repositories.Interfaces;
using NE.Infrastructure.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NE.Application.Services.Implementations
{
    public class FeedbackService : IFeedbackService
    {
        private readonly IUnitOfWork _unitOfWork;

        public FeedbackService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task AddFeedbackAsync(Feedback feedback)
        {
            await _unitOfWork.Feedbacks.AddAsync(feedback);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteFeedbackAsync(int id)
        {
            var feedback = await _unitOfWork.Feedbacks.GetByIdAsync(id);
            if (feedback != null)
            {
                await _unitOfWork.Feedbacks.Delete(feedback);
                await _unitOfWork.SaveChangesAsync();
            }
            else
            {
                throw new Exception("Feedback does not exist!");
            }
        }

        public async Task<IEnumerable<Feedback>> GetAllFeedbackAsync()
        {
            return await _unitOfWork.Feedbacks.GetAllAsync();
        }

        public async Task<Feedback> GetFeedbackByIdAsync(int id)
        {
            var feedback = await _unitOfWork.Feedbacks.GetByIdAsync(id);
            if (feedback == null)
            {
                throw new Exception("Feedback does not exist!");
            }
            return feedback;
        }

        public async Task UpdateFeedbackAsync(Feedback feedback)
        {
            var feedbackUpdate = await _unitOfWork.Feedbacks.GetByIdAsync(feedback.Id);
            if (feedbackUpdate == null)
            {
                throw new Exception("Feedback does not exist!");
            }
            feedback.ProductId = feedbackUpdate.ProductId;
            feedback.UserId = feedbackUpdate.UserId;
            feedback.Star = feedbackUpdate.Star;
            feedback.Image = feedbackUpdate.Image;
            await _unitOfWork.Feedbacks.Update(feedback);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
