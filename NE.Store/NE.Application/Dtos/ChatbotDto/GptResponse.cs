using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NE.Application.Dtos.ChatbotDto
{
    public class GptResponse
    {
        public List<Choice> choices { get; set; }
    }
}
