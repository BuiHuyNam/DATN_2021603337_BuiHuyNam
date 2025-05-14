using Microsoft.AspNetCore.Mvc;
using NE.Application.Dtos.OrderDto;
using NE.Application.Dtos.UserDto;
using NE.WebApp.Service;
using System.Net.Http;

namespace NE.WebApp.Controllers
{
    [Route("admin")]
    public class AdminController : Controller
    {
        private readonly HttpClient _httpClient;

        private const string ApiUrlOrder = "https://localhost:7099/api/order";
        private const string ApiUrlUser = "https://localhost:7099/api/user";

        public AdminController(HttpClient httpClient)
        {
            _httpClient = httpClient;
         
        }

        [HttpGet("home")]
        public async Task< IActionResult> Index()
        {
            var responseUser = await _httpClient.GetFromJsonAsync<List<UserViewDto>>(ApiUrlUser);
            int countUser = responseUser?.Count ?? 0; // ; // Đếm số lượng user, fallback về 0 nếu null

            ViewBag.CountUser = countUser; // Truyền qua View nếu cần

            var responseOrder = await _httpClient.GetFromJsonAsync<List<OrderViewDto>>(ApiUrlOrder);
            int countOrder = responseOrder?.Count ?? 0;
            ViewBag.CountOrder = countOrder;

            //Thống kê doanh thu (đơn hàng đã giao  (status == 5))
            int totalRevenue = (int)(responseOrder?.Where(s=>s.Status == 5).Sum(o => o.TotalMoney) ?? 0);      
            ViewBag.TotalRevenue = totalRevenue;

            //Thống kê số đơn hàng đã giao
            int countDelivered = (int)(responseOrder?.Where(s => s.Status == 5).Count() ?? 0);
            ViewBag.CountDelivered = countDelivered;

            var now = DateTime.Now;

            // Tạo danh sách 12 tháng gần nhất dưới dạng { Year = yyyy, Month = MM }
            var last12Months = Enumerable.Range(0, 12)
                .Select(i => now.AddMonths(-i))
                .Select(d => new { Year = d.Year, Month = d.Month })
                .OrderBy(d => d.Year).ThenBy(d => d.Month)
                .ToList();

            // Group doanh thu theo { Year, Month }
            var revenueDict = responseOrder?
     .Where(s => s.Status == 5)
     .GroupBy(s => s.CreatedDate.Year * 100 + s.CreatedDate.Month) // int key: yyyyMM
     .ToDictionary(
         g => g.Key,
         g => (int)g.Sum(o => o.TotalMoney)
     ) ?? new Dictionary<int, int>();


            var months = new List<int>();
            var revenues = new List<int>();

            foreach (var m in last12Months)
            {
                int monthKey = m.Year * 100 + m.Month;
                months.Add(monthKey);
                revenues.Add(revenueDict.TryGetValue(monthKey, out var value) ? value : 0);
            }


            ViewBag.Months = months;
            ViewBag.Revenues = revenues;


            //// Thống kê doanh thu theo tháng
            //var monthlyRevenue = responseOrder?
            //    .Where(s => s.Status == 5)
            //    .GroupBy(s => new { s.CreatedDate.Year, s.CreatedDate.Month })
            //    .OrderBy(g => g.Key.Year).ThenBy(g => g.Key.Month)
            //    .Select(g => new
            //    {
            //        Month = $"{g.Key.Month:00}/{g.Key.Year}", // format: 04/2025
            //        Revenue = g.Sum(o => o.TotalMoney)
            //    })
            //    .ToList();

            //// Truyền vào ViewBag để vẽ biểu đồ
            //ViewBag.Months = monthlyRevenue?.Select(x => x.Month).ToList();
            //ViewBag.Revenues = monthlyRevenue?.Select(x => x.Revenue).ToList();


            return View();
        }
    }
}
