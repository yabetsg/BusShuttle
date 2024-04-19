using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using WebMvc.Models;
using DomainModel;
using WebMvc.Service;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace WebMvc.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public IEntryService entryService;
    public ILoopService loopService;
    public IDriverService driverService;
    public IBusService busService;
    public IStopService stopService;

    public HomeController(ILogger<HomeController> logger, IEntryService entryService, ILoopService loopService, IDriverService driverService, IBusService busService, IStopService stopService)
    {
        _logger = logger;
        this.entryService = entryService;
        this.loopService = loopService;
        this.driverService = driverService;
        this.busService = busService;
        this.stopService = stopService;
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
        return View(loopService.GetAllLoops().Select(e => LoopViewModel.FromLoop(e)));
    }

    public IActionResult ViewDriver()
    {
        return View(driverService.GetAllDrivers().Select(e => DriverViewModel.FromDriver(e)));
    }


    public IActionResult ViewBus()
    {
        return View(busService.GetAllBuses().Select(e => BusViewModel.FromBus(e)));
    }

    public IActionResult ViewStop()
    {
        return View(stopService.GetAllStops().Select(e => StopViewModel.FromStop(e)));
    }
    public IActionResult ViewManager()
    {
        return View("ViewManager");
    }
    public IActionResult ViewDriverConfig()
    {
        var buses = busService.GetAllBuses().Select(e => BusViewModel.FromBus(e));
        var drivers = driverService.GetAllDrivers().Select(e => DriverViewModel.FromDriver(e));
        var loops = loopService.GetAllLoops().Select(e => LoopViewModel.FromLoop(e));
        var entries = entryService.GetAllEntries().Select(e => EntryViewModel.FromEntry(e));
        var stops = stopService.GetAllStops().Select(e => StopViewModel.FromStop(e));
        ViewBag.BusNumber = new SelectList(buses, "BusNumber", "BusNumber");
        ViewBag.FirstName = new SelectList(drivers, "FirstName", "FirstName");
        ViewBag.LastName = new SelectList(drivers, "LastName", "LastName");
        ViewBag.LoopName = new SelectList(loops, "Name", "Name");
        ViewBag.StopName = new SelectList(stops, "Name", "Name");
        var fullNames = drivers.Select(d => new { FullName = $"{d.FirstName} {d.LastName}", Id = d.Id });
        ViewBag.FullName = new SelectList(fullNames, "FullName", "FullName");
        return View();
    }



    public IActionResult DriverEntryView()
    {

        var BusNumber = HttpContext.Session.GetInt32("BusNumber");
        var DriverName = HttpContext.Session.GetString("DriverName");
        var LoopName = HttpContext.Session.GetString("LoopName");

        var stops = stopService.GetAllStops().Select(e => StopViewModel.FromStop(e));
        ViewBag.BusNumber = BusNumber;
        ViewBag.DriverName = DriverName;
        ViewBag.LoopName = LoopName;

        ViewBag.StopName = new SelectList(stops, "Name", "Name");

        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult CreateDriverEntry(EntryCreateModel model)
    {
        if (ModelState.IsValid)
        {
            entryService.CreateEntry(model.BusNumber, model.DriverName, model.LoopName, model.StopName, model.Boarded, model.LeftBehind);
            return new EmptyResult();

        }

         var BusNumber = HttpContext.Session.GetInt32("BusNumber");
        var DriverName = HttpContext.Session.GetString("DriverName");
        var LoopName = HttpContext.Session.GetString("LoopName");

        var stops = stopService.GetAllStops().Select(e => StopViewModel.FromStop(e));
        ViewBag.BusNumber = BusNumber;
        ViewBag.DriverName = DriverName;
        ViewBag.LoopName = LoopName;

        ViewBag.StopName = new SelectList(stops, "Name", "Name");
        
        return View("DriverEntryView",model);
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
        var loop = loopService.FindLoopByID(id);
        var loopEditModel = LoopEditModel.FromLoop(loop);
        return View(loopEditModel);
    }
    public IActionResult EditBus([FromRoute] int id)
    {
        var bus = busService.FindBusByID(id);
        var busEditModel = BusEditModel.FromBus(bus);
        return View(busEditModel);
    }

    public IActionResult EditStop([FromRoute] int id)
    {
        var stop = stopService.FindStopByID(id);
        var stopEditModel = StopEditModel.FromStop(stop);
        return View(stopEditModel);
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

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EditBus(int id, [Bind("BusNumber")] BusEditModel bus)
    {
        if (ModelState.IsValid)
        {
            busService.UpdateBusByID(id, bus.BusNumber);
            return RedirectToAction("ViewBus", new { id = id });
        }
        else
        {
            return View(bus);
        }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EditStop(int id, [Bind("Name")] StopEditModel stop)
    {
        if (ModelState.IsValid)
        {
            stopService.UpdateStopByID(id, stop.Name);
            return RedirectToAction("ViewStop", new { id = id });
        }
        else
        {
            return View(stop);
        }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateEntry(int busNumber, string driverName, string loopName)
    {
        if (ModelState.IsValid)
        {
            HttpContext.Session.SetInt32("BusNumber", busNumber);
            HttpContext.Session.SetString("DriverName", driverName);
            HttpContext.Session.SetString("LoopName", loopName);

            return RedirectToAction("DriverEntryView");
        }
        var buses = busService.GetAllBuses().Select(e => BusViewModel.FromBus(e));
        var drivers = driverService.GetAllDrivers().Select(e => DriverViewModel.FromDriver(e));
        var loops = loopService.GetAllLoops().Select(e => LoopViewModel.FromLoop(e));
        ViewBag.BusNumber = new SelectList(buses, "BusNumber", "BusNumber");
        ViewBag.FirstName = new SelectList(drivers, "FirstName", "FirstName");
        ViewBag.LastName = new SelectList(drivers, "LastName", "LastName");
        ViewBag.LoopName = new SelectList(loops, "Name", "Name");
        var fullNames = drivers.Select(d => new { FullName = $"{d.FirstName} {d.LastName}", Id = d.Id });
        ViewBag.FullName = new SelectList(fullNames, "FullName", "FullName");
        return View("ViewDriverConfig");
    }



    public IActionResult CreateDriver()
    {
        return View(new DriverCreateModel());
    }


    public IActionResult CreateLoop()
    {
        return View(new LoopCreateModel());
    }
    public IActionResult CreateBus()
    {
        return View(new BusCreateModel());
    }


    public IActionResult CreateStop()
    {
        return View(new StopCreateModel());
    }
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
    public async Task<IActionResult> CreateDriver([Bind("FirstName", "LastName")] DriverCreateModel driver)
    {
        if (ModelState.IsValid)
        {
            driverService.CreateDriver(driver.FirstName, driver.LastName);
            return RedirectToAction("ViewDriver");
        }
        else
        {

            return View(driver);
        }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateBus([Bind("BusNumber")] BusCreateModel bus)
    {
        if (ModelState.IsValid)
        {
            busService.CreateBus(bus.BusNumber);
            return RedirectToAction("ViewBus");
        }
        else
        {
            return View(bus);
        }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateStop([Bind("Name")] StopCreateModel stop)
    {
        if (ModelState.IsValid)
        {
            stopService.CreateStop(stop.Name);
            return RedirectToAction("ViewStop");
        }
        else
        {
            return View(stop);
        }
    }

}
