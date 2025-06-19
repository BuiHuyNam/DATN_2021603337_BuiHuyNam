using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using DinkToPdf;
using DinkToPdf.Contracts;
using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.CodeAnalysis.RulesetToEditorconfig;
using System.Drawing.Printing;
using PaperKind = DinkToPdf.PaperKind;
using NE.Application.Dtos.OrderDto;
using System.Net.Http;

namespace NE.WebApp.Controllers
{
    public class OuputOrder : Controller
    {
        private const string ApiUrl = "https://localhost:7099/api/order";

        private readonly IRazorViewEngine _viewEngine;
        private readonly ITempDataProvider _tempDataProvider;
        private readonly IServiceProvider _serviceProvider;
        private readonly IConverter _converter;
        private readonly HttpClient _httpClient;

        public OuputOrder(IRazorViewEngine viewEngine,
                          ITempDataProvider tempDataProvider,
                          IServiceProvider serviceProvider,
                          IConverter converter,
                          HttpClient httpClient)
        {
            _viewEngine = viewEngine;
            _tempDataProvider = tempDataProvider;
            _serviceProvider = serviceProvider;
            _converter = converter;
            _httpClient = httpClient;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> ExportToPdf(int id)
        {
            var invoice = await _httpClient.GetFromJsonAsync<OrderViewDto>($"{ApiUrl}/{id}");

            var html = await RenderViewAsync("ExportToPdf", invoice);

            var doc = new HtmlToPdfDocument
            {
                GlobalSettings = { PaperSize = PaperKind.A4 },
                Objects = { new ObjectSettings { HtmlContent = html } }
            };

            var pdf = _converter.Convert(doc);
            return File(pdf, "application/pdf", $"HoaDon_{invoice.Id}.pdf");
        }

  
        private async Task<string> RenderViewAsync(string viewName, object model)
        {
            var actionContext = new ActionContext(HttpContext, RouteData, ControllerContext.ActionDescriptor);
            using var sw = new StringWriter();

            var viewResult = _viewEngine.FindView(actionContext, viewName, false);
            if (!viewResult.Success)
                throw new Exception($"Không tìm thấy view '{viewName}'");

            var viewContext = new ViewContext(
                actionContext,
                viewResult.View,
                new ViewDataDictionary(new EmptyModelMetadataProvider(), new ModelStateDictionary())
                {
                    Model = model
                },
                new TempDataDictionary(HttpContext, _tempDataProvider),
                sw,
                new HtmlHelperOptions()
            );

            await viewResult.View.RenderAsync(viewContext);
            return sw.ToString();
        }

   
    }
}
