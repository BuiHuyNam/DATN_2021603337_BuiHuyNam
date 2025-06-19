using Microsoft.AspNetCore.Mvc;
using NE.Application.Dtos.ChatbotDto;
using NE.Application.Dtos.ProductDto;
using NE.Domain.Entitis;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace NE.WebApp.Controllers
{
    [Route("Chatbot")]
    public class ChatbotController : Controller
    {
        // Nội tuyến service đơn giản
        public interface IChatbotService
        {
            Task<string> AskGptAsync(string userMessage);
        }

        public class ChatbotService : IChatbotService
        {
            private const string ApiUrl = "https://localhost:7099/api/product";

            private readonly HttpClient _httpClient;
            //private readonly string _apiKey = "sk-proj-GRs7VHewIzrbZHd49DMjT3BlbkFJ4UBCOphysLKws13qzLHh"; 

            //private readonly string _apiKey = "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"; 


            public ChatbotService()
            {
                _httpClient = new HttpClient();
            }

            public async Task<string> AskGptAsync(string userMessage)
            {
                try
                {
                    _httpClient.DefaultRequestHeaders.Authorization =
                        new AuthenticationHeaderValue("Bearer", _apiKey);
                    var product = await _httpClient.GetFromJsonAsync<List<ProductViewDto>>(ApiUrl);
                    // Tạo chuỗi nối tên sản phẩm và giá
                    string productListString = string.Join(", ", product.Where(p=>p.IsActive == true).Select(p => $"{p.ProductName} ({p.Price:N0} VND)"));

                    var requestBody = new
                    {
                        //model = "gpt-3.5-turbo",
                        model = "gpt-4o-mini",
                        store = true,

                        messages = new[]
                        {
                            new { role = "system", content = "Bạn là một trợ lý AI luôn trả lời bằng tiếng Việt." },
                            new { role = "system", content = "Website bán hàng điện tử của Nam Electronics chuyên laptop, điện thoại, phụ kiện." },
                            new { role = "system", content = "Bạn chỉ trả lời các câu hỏi liên quan đến website Nam Electronics." },

                            new { role = "system", content = "Nếu hỏi về sản phẩm thì trả lời theo kiểu liệt kê đúng trọng tâm." },

                            new { role = "system", content = productListString },



                            new { role = "user", content = userMessage }
                        }
                    };

                    var response = await _httpClient.PostAsJsonAsync("https://api.openai.com/v1/chat/completions", requestBody);

                    if (!response.IsSuccessStatusCode)
                        return "Lỗi khi gọi GPT";

                    var result = await response.Content.ReadFromJsonAsync<GptResponse>();

                    return result?.choices?[0]?.message?.content?.Trim() ?? "Hệ thống đang bảo trì.";
                }
                catch (Exception ex)
                {
                    return "Lỗi hệ thống: " + ex.Message;
                }
            }
        }

         

            private readonly IChatbotService _chatbotService;

        public ChatbotController()
        {
            // Tạo trực tiếp service ở đây (chỉ nên dùng để test)
            _chatbotService = new ChatbotService();
        }

        [HttpPost("SendMessage")]
        public async Task<IActionResult> SendMessage([FromForm] string userMessage)
        {
            if (string.IsNullOrWhiteSpace(userMessage))
            {
                return Json(new { reply = "Bạn chưa nhập nội dung." });
            }

            var reply = await _chatbotService.AskGptAsync(userMessage);
            return Json(new { reply });
        }
    }
}
