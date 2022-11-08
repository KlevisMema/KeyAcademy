using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using KeyAcademy_2.Models;
using System.Net;
using Microsoft.Extensions.Options;

namespace KeyAcademy_2.Controllers;

public class HomeController : Controller
{
    private readonly IOptions<KeyAcademy_2.AppSettingsValues.Uri> _uri;

    public HomeController(IOptions<KeyAcademy_2.AppSettingsValues.Uri> uri)
    {
        _uri = uri;
    }

    [HttpGet]
    /// <summary>
    /// Main Layout
    /// </summary>
    /// <returns>A html page reprezenting the home layout</returns>
    public IActionResult Index()
    {
        return View();
    }

    [HttpGet]
    /// <summary>
    /// Get all holiday requests
    /// </summary>
    /// <returns>List of holiday requests</returns>
    public async Task<ActionResult<List<TaskDto>>> GetAllTasks()
    {
        try
        {
            var client = new HttpClient();

            client.BaseAddress = new Uri(_uri.Value.HolidaysRequestsBase);

            var response = await client.GetAsync(_uri.Value.HolidaysRequestsEndPoint);
            var tasks = await response.Content.ReadAsAsync<List<TaskDto>>();

            client.Dispose();

            return View(tasks);
        }
        catch(HttpRequestException ex)
        {
            return Content(ex.Message);
        }
        catch (Exception)
        {
            return BadRequest();
        }
    }

    /// <summary>
    ///     Approve holiday request 
    /// </summary>
    /// <param name="id">Id of the holiday request</param>
    public async Task<IActionResult> ApproveRequestHoliday(string id)
    {
        try
        {
            var client = new HttpClient();

            client.BaseAddress = new Uri(_uri.Value.BaseUri);

            var message = new HttpRequestMessage
                (
                    HttpMethod.Post,
                    client.BaseAddress + _uri.Value.ApproveeRejectRequestHoliday + $"{id}/{true}"
                );

            var result = await client.SendAsync(message);
            var resultContent = await result.Content.ReadAsStringAsync();

            client.Dispose();

            return RedirectToAction("Index");
        }
        catch (HttpRequestException ex)
        {
            return Content(ex.Message);
        }
        catch (Exception)
        {
            return BadRequest();
        }
    }

    /// <summary>
    ///     Reject holiday request
    /// </summary>
    /// <param name="id">Id of the holiday request</param>
    public async Task<IActionResult> RejectRequestHoliday(string id)
    {
        try
        {
            var client = new HttpClient();

            client.BaseAddress = new Uri(_uri.Value.BaseUri);

            var message = new HttpRequestMessage
                (
                    HttpMethod.Post,
                    client.BaseAddress + _uri.Value.ApproveeRejectRequestHoliday + $"{id}/{false}"
                );

            var result = await client.SendAsync(message);
            var resultContent = await result.Content.ReadAsStringAsync();

            client.Dispose();

            return RedirectToAction("Index");
        }
        catch (HttpRequestException ex)
        {
            return Content(ex.Message);
        }
        catch (Exception)
        {
            return BadRequest();
        }
    }

    [HttpGet]
    /// <summary>
    ///     Dispaly holiday request form
    /// </summary>
    public IActionResult GetCreateHolidayRequest()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    /// <summary>
    /// Creates a holiday request
    /// </summary>
    public async Task<IActionResult> PostCreateHolidayRequest(RequestHoliday requestHoliday)
    {
        try
        {
            if (ModelState.IsValid)
            {
                var client = new HttpClient();

                client.BaseAddress = new Uri(_uri.Value.BaseUri);

                var result = await client.PostAsJsonAsync(_uri.Value.CreateHolidayRequest, requestHoliday);

                client.Dispose();

                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }
        catch (HttpRequestException ex)
        {
            return Content(ex.Message);
        }
        catch (Exception)
        {
            return BadRequest();
        }
    }

    [HttpGet]
    /// <summary>
    /// Privacy Layout
    /// </summary>
    /// <returns>A html page reprezenting the privacy layout</returns>
    public IActionResult Privacy()
    {
        return View();
    }


    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}