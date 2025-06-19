using NE.Application.Dtos.UserDto;
using NE.Domain.Entitis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NE.Application.Dtos.Feedback
{
    public class FeedbackViewDto
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public int Star { get; set; }
        public string Image { get; set; }

        public int UserId { get; set; }
        public UserViewDto User { get; set; }
        public int ProductId { get; set; }
        public DateTime CreatedDate { get; set; }

    }
}
