using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using WebMvc.Models;
using DomainModel;
using WebMvc.Service;

namespace WebMvc.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public IEntryService entryService;
    public ILoopService loopService;
    public IDriverService driverService;

    public HomeController(ILogger<HomeController> logger, IEntryService entryService, ILoopService loopService, IDriverService driverService)
    {
        _logger = logger;
        this.entryService = entryService;
        this.loopService = loopService;
        this.driverService = driverService;
    }

    public IActionResult Index()
    {
        return View();
    }
    public IActionResult ViewEntry()
    {
        return View(entryService.GetAllEntries().Select(e => EntryViewModel.FromEntry(e)));
    }
    public IActionResult ViewLoop()
    {
        return View(loopService.GetAllLoops().Select(e=>LoopViewModel.FromLoop(e)));
    }

     public IActionResult ViewDriver()
    {
        return View(driverService.GetAllDrivers().Select(e=>DriverViewModel.FromDriver(e)));
    }


    public IActionResult ViewManager()
    {
        return View("ViewManager");
    }
    // GET: /Home/EditEntry/{id}
    public IActionResult EditEntry([FromRoute] int id)
    {
        var entry = entryService.FindEntryByID(id);
        var entryEditModel = EntryEditModel.FromEntry(entry);
        return View(entryEditModel);
    }
      public IActionResult EditDriver([FromRoute] int id)
    {
        var driver = driverService.FindDriverByID(id);
        var driverEditModel = DriverEditModel.FromDriver(driver);
        return View(driverEditModel);
    }

    public IActionResult EditLoop([FromRoute] int id)
    {
        var driver = driverService.FindDriverByID(id);
        var driverEditModel = DriverEditModel.FromDriver(driver);
        return View(driverEditModel);
    }
    // POST: Home/EditEntry/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EditEntry(int id, [Bind("BusNumber,DriverName,LoopName,StopName,Boarded,LeftBehind")] EntryEditModel entry)
    {
        if (ModelState.IsValid)
        {
            entryService.UpdateEntryByID(id, entry.BusNumber, entry.DriverName, entry.LoopName, entry.StopName, entry.Boarded, entry.LeftBehind);
            return RedirectToAction("ViewEntry", new { id = id });
        }
        else
        {
            return View(entry);
        }
    }

      [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EditDriver(int id, [Bind("FirstName,LastName")] DriverEditModel driver)
    {
        if (ModelState.IsValid)
        {
            driverService.UpdateDriverByID(id, driver.FirstName, driver.LastName);
            return RedirectToAction("ViewDriver", new { id = id });
        }
        else
        {
            return View(driver);
        }
    }

     [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EditLoop(int id, [Bind("Name")] LoopEditModel loop)
    {
        if (ModelState.IsValid)
        {
            loopService.UpdateLoopByID(id, loop.Name);
            return RedirectToAction("ViewLoop", new { id = id });
        }
        else
        {
            return View(loop);
        }
    }


    public IActionResult CreateEntry()
    {
        return View(new EntryCreateModel());
    }

     public IActionResult CreateDriver()
    {
        return View(new DriverCreateModel());
    }

    public IActionResult CreateLoop()
    {
        return View(new LoopCreateModel());
    }
    // POST: /Home/CreateEntry
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateLoop([Bind("Name")] LoopCreateModel loop)
    {
        if (ModelState.IsValid)
        {
            loopService.CreateLoop(loop.Name);
            return RedirectToAction("ViewLoop");
        }
        else
        {

            return View(loop);
        }
    }

     [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateDriver([Bind("FirstName","LastName")] DriverCreateModel driver)
    {
        if (ModelState.IsValid)
        {
            driverService.CreateDriver(driver.FirstName,driver.LastName);
            return RedirectToAction("ViewDriver");
        }
        else
        {

            return View(driver);
        }
    }

}
