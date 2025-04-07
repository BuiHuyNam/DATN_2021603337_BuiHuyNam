using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NE.Application.Dtos.BrandDto;
using NE.Application.Dtos.Feedback;
using NE.Application.Services.Implementations;
using NE.Application.Services.Interfaces;
using NE.Domain.Entitis;

namespace NE.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FeedbackController : ControllerBase
    {
        private readonly IFeedbackService _feedbackService;
        private readonly IMapper _mapper;

        public FeedbackController(IFeedbackService feedbackService, IMapper mapper)
        {
            _feedbackService = feedbackService;
            _mapper = mapper;
        }

        [HttpPost()]
        public async Task<ActionResult> AddFeedback(FeedbackCreateDto feedbackCreateDto)
        {
            var feedback = _mapper.Map<Feedback>(feedbackCreateDto);
            await _feedbackService.AddFeedbackAsync(feedback);
            return Ok();
        }

        [HttpGet()]
        public async Task<ActionResult> GetAllFeedback()
        {
            var feedbacks = await _feedbackService.GetAllFeedbackAsync();
            var feedbackDto = _mapper.Map<IEnumerable<FeedbackViewDto>>(feedbacks);
            return Ok(feedbackDto);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetFeedbackByIdAsync(int id)
        {
            var feedback = await _feedbackService.GetFeedbackByIdAsync(id);
            var feedbackDto = _mapper.Map<FeedbackViewDto>(feedback);
            return Ok(feedbackDto);
        }

        [HttpPut]
        public async Task<ActionResult> UpdateFeedback(FeedbackUpdateDto feedbackUpdateDto)
        {
            var feedbackUpdate = _mapper.Map<Feedback>(feedbackUpdateDto);
            await _feedbackService.UpdateFeedbackAsync(feedbackUpdate);
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteFeedback(int id)
        {
            await _feedbackService.DeleteFeedbackAsync(id);
            return Ok();
        }
    }
}
