using Microsoft.AspNetCore.Mvc;
using NE.Application.Dtos.ColorDto;
using NE.Application.Dtos.OrderDto;
using NE.Application.Dtos.ProductDto;
using NE.Application.Dtos.UserDto;
using NE.Domain.Entitis;
using System.IdentityModel.Tokens.Jwt;

namespace NE.WebApp.Controllers
{
    public class UserController : Controller
    {

        private readonly HttpClient _httpClient;
        private const string ApiUrl = "https://localhost:7099/api/user";
        private const string ApiUrlOrder = "https://localhost:7099/api/order";


        public UserController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        [HttpGet("admin/User")]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost("admin/IsActiveUser")]
        public async Task<IActionResult> IsActiveUser(IsActiveUserDto isActiveUserDto)
        {
            var response = await _httpClient.PutAsJsonAsync(ApiUrl + "/IsActiveUser", isActiveUserDto);
            return RedirectToAction("Index", "User");
        }

        [HttpGet("Profile")]
        public async Task<IActionResult> Profile()
        {
            var token = HttpContext.Session.GetString("JwtToken");
            if (string.IsNullOrEmpty(token))
            {
                return RedirectToAction("Login", "Auth");
            }
            else
            {
                // Giải mã Token để lấy UserId
                var handler = new JwtSecurityTokenHandler();
                var jwtToken = handler.ReadJwtToken(token);
                var userIdClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == "nameid");

                if (userIdClaim == null)
                {
                    return RedirectToAction("Login", "Auth");
                }

                if (!int.TryParse(userIdClaim.Value, out int userId))
                {
                    return RedirectToAction("Login", "Auth");
                }

                _httpClient.DefaultRequestHeaders.Authorization =
                                        new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);


                var response = await _httpClient.GetFromJsonAsync<UserViewDto>($"{ApiUrl}/{userId}");
                return View(response);
            }

        }

        [HttpPost("UpdateInfor")]
        public async Task<IActionResult> UpdateInfor(UpdateInforUserDto updateInforUserDto)
        {
            var token = HttpContext.Session.GetString("JwtToken");
            if (string.IsNullOrEmpty(token))
            {
                return RedirectToAction("Login", "Auth");
            }
            else
            {
                // Giải mã Token để lấy UserId
                var handler = new JwtSecurityTokenHandler();
                var jwtToken = handler.ReadJwtToken(token);
                var userIdClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == "nameid");

                if (userIdClaim == null)
                {
                    return RedirectToAction("Login", "Auth");
                }

                if (!int.TryParse(userIdClaim.Value, out int userId))
                {
                    return RedirectToAction("Login", "Auth");
                }

                updateInforUserDto.Id = userId;

                _httpClient.DefaultRequestHeaders.Authorization =
                                        new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);


                var response = await _httpClient.PutAsJsonAsync(ApiUrl, updateInforUserDto);

                if (!response.IsSuccessStatusCode)
                {
                    TempData["Error"] = "Lưu thất bại!";
                }
                else
                {
                    TempData["Success"] = "Lưu thành công!";
                }
                return RedirectToAction("Profile", "User");


            }


        }

        [HttpGet("HistoryOrder")]
        public async Task<IActionResult> HistoryOrder()
        {
            var token = HttpContext.Session.GetString("JwtToken");
            if (string.IsNullOrEmpty(token))
            {
                return RedirectToAction("Login", "Auth");
            }
            else
            {
                // Giải mã Token để lấy UserId
                var handler = new JwtSecurityTokenHandler();
                var jwtToken = handler.ReadJwtToken(token);
                var userIdClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == "nameid");

                if (userIdClaim == null)
                {
                    return RedirectToAction("Login", "Auth");
                }

                if (!int.TryParse(userIdClaim.Value, out int userId))
                {
                    return RedirectToAction("Login", "Auth");
                }



                _httpClient.DefaultRequestHeaders.Authorization =
                                        new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);


                var response = await _httpClient.GetFromJsonAsync<List<OrderViewDto>>($"{ApiUrlOrder}/UserId/{userId}");


                return View(response);
            }
        }

        [HttpGet("ChangePassword")]
        public IActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost("ChangePassword")]
        public async Task<IActionResult> ChangePassword(UpdatePasswordDto updatePasswordDto)
        {
            var token = HttpContext.Session.GetString("JwtToken");
            if (string.IsNullOrEmpty(token))
            {
                return RedirectToAction("Login", "Auth");
            }
            else
            {
                // Giải mã Token để lấy UserId
                var handler = new JwtSecurityTokenHandler();
                var jwtToken = handler.ReadJwtToken(token);
                var userIdClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == "nameid");

                if (userIdClaim == null)
                {
                    return RedirectToAction("Login", "Auth");
                }

                if (!int.TryParse(userIdClaim.Value, out int userId))
                {
                    return RedirectToAction("Login", "Auth");
                }

                updatePasswordDto.Id = userId;

                _httpClient.DefaultRequestHeaders.Authorization =
                                        new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

              



                var response = await _httpClient.PutAsJsonAsync($"{ApiUrl}/updatePassword", updatePasswordDto);


                if (!response.IsSuccessStatusCode)
                {
                    if (updatePasswordDto.NewPassword != updatePasswordDto.ComfirmPassword)
                    {
                        ModelState.AddModelError("ComfirmPassword", "Mật khẩu xác nhận không khớp.");
                        return View(updatePasswordDto);
                    }
                    else
                    {
                        ModelState.AddModelError("Password", "Mật khẩu không đúng.");
                        return View(updatePasswordDto);
                    }

                }
                else
                {
                    TempData["Success"] = "Thay đổi thành công!";
                }
                return RedirectToAction("ChangePassword", "User");


            }
        }

        [HttpGet("Admin/UserDetail/{id}")]
        public async Task<IActionResult> UserDetail(int id)
        {
            var result = await _httpClient.GetFromJsonAsync<UserViewDto>($"{ApiUrl}/{id}");
            return View(result);
        }

        [HttpGet("Admin/EditUser/{id}")]

        public async Task<IActionResult> EditUser(int id) {
            var result = await _httpClient.GetFromJsonAsync<UserViewDto>($"{ApiUrl}/{id}");
            return View(result);
        }

        [HttpPost("Admin/EditUser/{id}")]

        public async Task<IActionResult> EditUser(UpdateInforUserDto updateInforUserDto)
        {
            var response = await _httpClient.PutAsJsonAsync(ApiUrl, updateInforUserDto);

            if (!response.IsSuccessStatusCode)
            {
                TempData["Error"] = "Sửa thất bại!";
            }
            else
            {
                TempData["Success"] = "Sửa thành công!";
            }
            return RedirectToAction("Index", "User");
        }
    }
}
