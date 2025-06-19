using Microsoft.AspNetCore.Mvc;
using NE.Application.Dtos.VNPay;
using NE.WebApp.Service;

namespace NE.WebApp.Controllers
{
    public class PaymentController : Controller
    {

        private readonly IVnPayService _vnPayService;
        public PaymentController(IVnPayService vnPayService)
        {

            _vnPayService = vnPayService;
        }

        [HttpPost]                     
        public IActionResult CreatePaymentUrlVnpay(PaymentInformationModel model)
        {
            var url = _vnPayService.CreatePaymentUrl(model, HttpContext);

            return Redirect(url);
        }


   

    }

}
