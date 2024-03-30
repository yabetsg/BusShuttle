using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Driver.Models;

namespace Driver.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        List<string> busNumbers = new List<string> { "990", "820", "111" };
        List<string> drivers = new List<string> { "yabets g" };
        List<string> loopOptions = new List<string> { "Green", "Red", "Blue" };


        ViewBag.BusNumbers = busNumbers;
        ViewBag.LoopOptions = loopOptions;
        ViewBag.Drivers = drivers;

        return View();
    }

    [HttpPost]
    [Route("/form")]
    public IActionResult FormSubmission(string busNumber, string loopOption, string driver)
    {
        // Process the form data here (you can access the selected values of the dropdowns via the parameters)
        // For example, you can pass the selected values to another view
        ViewBag.SelectedBusNumber = busNumber;
        ViewBag.SelectedLoopOption = loopOption;
        ViewBag.SelectedDriver = driver;
        ViewBag.Stops = new List<string> { "Stop 1", "Stop 2", "Stop 3" };
        return View("FormSubmissionView");
    }




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
