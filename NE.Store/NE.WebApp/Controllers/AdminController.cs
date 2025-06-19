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

        //Thống kê đơn hàng 6 tháng gần nhất
        [HttpGet("StatisticalOrder")]
        public async Task<IActionResult> StatisticalOrder()
        {
            var responseOrder = await _httpClient.GetFromJsonAsync<List<OrderViewDto>>(ApiUrlOrder);

            var now = DateTime.Now;
            var startDate = new DateTime(now.Year, now.Month, 1).AddMonths(-5);

            var filteredOrders = responseOrder
                .Where(o => o.CreatedDate >= startDate)
                .ToList();

            var months = new List<string>();
            var cancelledList = new List<int>();
            var waitingList = new List<int>();
            var soldList = new List<int>();

            for (int i = 0; i < 6; i++)
            {
                var month = startDate.AddMonths(i);
                var monthStart = new DateTime(month.Year, month.Month, 1);
                var monthEnd = monthStart.AddMonths(1);

                var monthOrders = filteredOrders
                    .Where(o => o.CreatedDate >= monthStart && o.CreatedDate < monthEnd);

                months.Add(month.ToString("MM/yyyy"));
                cancelledList.Add(monthOrders.Count(o => o.Status == 6 || o.Status == 7));
                waitingList.Add(monthOrders.Count(o => o.Status == 1 || o.Status == 2));
                soldList.Add(monthOrders.Count(o => o.Status == 3 || o.Status == 4 || o.Status == 5));
            }

            ViewBag.Months = months;
            ViewBag.Cancelled = cancelledList;
            ViewBag.Waiting = waitingList;
            ViewBag.Sold = soldList;

            return View();
        }


        //Thông kê đơn hàng 10 ngày gần nhất
        [HttpGet("StatisticalOrderDays")]
        public async Task<IActionResult> StatisticalOrderDays()
        {
            var responseOrder = await _httpClient.GetFromJsonAsync<List<OrderViewDto>>(ApiUrlOrder);

            var now = DateTime.Now.Date; // lấy ngày hiện tại (không tính giờ)
            var startDate = now.AddDays(-9); // 15 ngày: hôm nay + 14 ngày trước đó

            var filteredOrders = responseOrder
                .Where(o => o.CreatedDate.Date >= startDate && o.CreatedDate.Date <= now)
                .ToList();

            var days = new List<string>();
            var cancelledList = new List<int>();
            var waitingList = new List<int>();
            var soldList = new List<int>();

            for (int i = 0; i < 10; i++)
            {
                var day = startDate.AddDays(i);

                var dayOrders = filteredOrders
                    .Where(o => o.CreatedDate.Date == day);

                days.Add(day.ToString("dd/MM/yyyy"));
                cancelledList.Add(dayOrders.Count(o => o.Status == 6 || o.Status == 7));
                waitingList.Add(dayOrders.Count(o => o.Status == 1 || o.Status == 2));
                soldList.Add(dayOrders.Count(o => o.Status == 3 || o.Status == 4 || o.Status == 5));
            }

            ViewBag.Days = days;
            ViewBag.Cancelled = cancelledList;
            ViewBag.Waiting = waitingList;
            ViewBag.Sold = soldList;

            return View();
        }


        //Thống kê doanh thu 6 tháng gần nhất
        [HttpGet("StatisticalRevenueMonth")]
        public async Task<IActionResult> StatisticalRevenueMonth()
        {
            var responseOrder = await _httpClient.GetFromJsonAsync<List<OrderViewDto>>(ApiUrlOrder);
            int countOrder = responseOrder?.Count ?? 0;
            ViewBag.CountOrder = countOrder;

            //Thống kê doanh thu (đơn hàng đã giao  (status == 5))
            int totalRevenue = (int)(responseOrder?.Where(s => s.Status == 5).Sum(o => o.TotalMoney) ?? 0);
            ViewBag.TotalRevenue = totalRevenue;

            //Thống kê số đơn hàng đã giao
            int countDelivered = (int)(responseOrder?.Where(s => s.Status == 5).Count() ?? 0);
            ViewBag.CountDelivered = countDelivered;

            var now = DateTime.Now;

            // Tạo danh sách 6 tháng gần nhất (thay vì 12)
            var last6Months = Enumerable.Range(0, 6)
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

            foreach (var m in last6Months)
            {
                int monthKey = m.Year * 100 + m.Month;
                months.Add(monthKey);
                revenues.Add(revenueDict.TryGetValue(monthKey, out var value) ? value : 0);
            }

            ViewBag.Months = months;
            ViewBag.Revenues = revenues;
            return View();
        }


        //Thống kê doanh thu 10 ngày gần nhất
        [HttpGet("StatisticalRevenueDays")]
        public async Task<IActionResult> StatisticalRevenueDays()
        {
            var responseOrder = await _httpClient.GetFromJsonAsync<List<OrderViewDto>>(ApiUrlOrder);

            int countOrder = responseOrder?.Count ?? 0;
            ViewBag.CountOrder = countOrder;

            // Tổng doanh thu tất cả đơn đã giao (status == 5)
            int totalRevenue = (int)(responseOrder?.Where(s => s.Status == 5).Sum(o => o.TotalMoney) ?? 0);
            ViewBag.TotalRevenue = totalRevenue;

            // Số đơn đã giao
            int countDelivered = (int)(responseOrder?.Where(s => s.Status == 5).Count() ?? 0);
            ViewBag.CountDelivered = countDelivered;

            var now = DateTime.Today;

            // Tạo danh sách 10 ngày gần nhất, từ 9 ngày trước đến hôm nay
            var last10Days = Enumerable.Range(0, 10)
                .Select(i => now.AddDays(-9 + i))  // thứ tự ngày tăng dần
                .ToList();

            // Nhóm doanh thu theo ngày (yyyyMMdd)
            var revenueDict = responseOrder?
                .Where(s => s.Status == 5)
                .GroupBy(s => s.CreatedDate.Date)
                .ToDictionary(
                    g => g.Key,
                    g => (int)g.Sum(o => o.TotalMoney)
                ) ?? new Dictionary<DateTime, int>();

            var labels = new List<string>();
            var revenues = new List<int>();

            foreach (var day in last10Days)
            {
                labels.Add(day.ToString("dd/MM/yyyy"));
                revenues.Add(revenueDict.TryGetValue(day, out var val) ? val : 0);
            }

            ViewBag.Labels = labels;       // List<string> ngày dạng "15/05/2025"
            ViewBag.Revenues = revenues;   // List<int> doanh thu từng ngày

            return View();
        }



    }
}
