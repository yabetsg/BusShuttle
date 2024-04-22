using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using WebMvc.Models;
using DomainModel;
using WebMvc.Service;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace WebMvc.Controllers;
[Authorize]
public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public IEntryService entryService;
    public ILoopService loopService;
    public IDriverService driverService;
    public IBusService busService;
    public IStopService stopService;
    private readonly SignInManager<IdentityUser> _signInManager;
    private readonly UserManager<IdentityUser> _userManager;

    public HomeController(ILogger<HomeController> logger, IEntryService entryService, ILoopService loopService, IDriverService driverService, IBusService busService, IStopService stopService, SignInManager<IdentityUser> signInManager, UserManager<IdentityUser> userManager)
    {
        _logger = logger;
        this.entryService = entryService;
        this.loopService = loopService;
        this.driverService = driverService;
        this.busService = busService;
        this.stopService = stopService;
        _signInManager = signInManager;
        _userManager = userManager;
    }

    public IActionResult Index()
    {
        if (!_signInManager.IsSignedIn(User))
        {
            return RedirectToAction("Register");
        }
        else
        {
            return View();
        }
    }

    [AllowAnonymous]
    [HttpGet]
    public IActionResult Register()
    {
        return View("Register");
    }
    [AllowAnonymous]
    public IActionResult Login()
    {
        return View("Login");
    }

    [AllowAnonymous]
    [HttpPost]
    [ValidateAntiForgeryToken]

    public async Task<IActionResult> Register(RegisterCreateModel model)
    {

        if (ModelState.IsValid)
        {
            var user = new IdentityUser { UserName = model.UserName };
            var isFirstUser = !_userManager.Users.Any();
            var result = await _userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                if (isFirstUser)
                {
                    await _userManager.AddToRoleAsync(user, "manager");
                }
                else
                {
                    await _userManager.AddToRoleAsync(user, "driver");
                }
                _logger.LogWarning("Created a new user: {Username}",model.UserName);
                return RedirectToAction("Login");
            }
            else
            {
                _logger.LogWarning("Error creating a new user");
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                return View("Register", model);

            }
        }
        else
        {
            return View("Register", model);
        }

    }

    [AllowAnonymous]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(RegisterViewModel model)
    {
        _logger.LogInformation("--------BAD MODeL-------------");

        if (ModelState.IsValid)
        {
            var user = await _userManager.FindByNameAsync(model.UserName);
            if (user != null && await _userManager.CheckPasswordAsync(user, model.Password))
            {
                await _signInManager.SignInAsync(user, isPersistent: true);
                return RedirectToAction("Index");
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                return View(model);
            }
        }
        return View(model);
    }

    [AllowAnonymous]
    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();
        return RedirectToAction("Index");
    }

    [AllowAnonymous]
    public IActionResult AccessDenied()
    {
        return View();
    }

    [Authorize(Roles = "manager")]
    public IActionResult ViewEntry()
    {
        var loops = loopService.GetAllLoops().Select(e => LoopViewModel.FromLoop(e));
        ViewBag.LoopName = new SelectList(loops, "Name", "Name");
        return View(entryService.GetAllEntries().Select(e => EntryViewModel.FromEntry(e)));
    }

    [Authorize(Roles = "manager")]
    public IActionResult ViewLoop()
    {
        return View(loopService.GetAllLoops().Select(e => LoopViewModel.FromLoop(e)));
    }

    [Authorize(Roles = "manager")]
    public IActionResult ViewDriver()
    {
        return View(driverService.GetAllDrivers().Select(e => DriverViewModel.FromDriver(e)));
    }


    [Authorize(Roles = "manager")]
    public IActionResult ViewBus()
    {
        return View(busService.GetAllBuses().Select(e => BusViewModel.FromBus(e)));
    }

    [Authorize(Roles = "manager")]
    public IActionResult ViewStop()
    {
        return View(stopService.GetAllStops().Select(e => StopViewModel.FromStop(e)));
    }

    [Authorize(Roles = "manager")]
    public IActionResult ViewManager()
    {
        return View("ViewManager");
    }

    [Authorize(Roles = "driver")]
    public IActionResult ViewDriverConfig()
    {
        var buses = busService.GetAllBuses().Select(e => BusViewModel.FromBus(e));
        var drivers = driverService.GetAllDrivers().Select(e => DriverViewModel.FromDriver(e));
        var loops = loopService.GetAllLoops().Select(e => LoopViewModel.FromLoop(e));
        var entries = entryService.GetAllEntries().Select(e => EntryViewModel.FromEntry(e));
        // var stops = stopService.GetAllStops().Select(e => StopViewModel.FromStop(e));
        ViewBag.BusNumber = new SelectList(buses, "BusNumber", "BusNumber");
        ViewBag.FirstName = new SelectList(drivers, "FirstName", "FirstName");
        ViewBag.LastName = new SelectList(drivers, "LastName", "LastName");
        ViewBag.LoopName = new SelectList(loops, "Name", "Name");
        // ViewBag.StopName = new SelectList(stops, "Name", "Name");
        var fullNames = drivers.Select(d => new { FullName = $"{d.FirstName} {d.LastName}", Id = d.Id });
        ViewBag.FullName = new SelectList(fullNames, "FullName", "FullName");
        return View();
    }


    [Authorize(Roles = "driver")]
    public IActionResult DriverEntryView()
    {
        IEnumerable<StopViewModel> stops;
        var currentStop = TempData["CurrentStop"] as string;

        _logger.LogInformation("-------------------------------");
        _logger.LogInformation(currentStop);
        if (currentStop != null)
        {
            stops = stopService.GetAllStops().Select(e => StopViewModel.FromStop(e)).Where(stop => stop.Id != int.Parse(currentStop.ToString()));
        }
        else
        {
            stops = stopService.GetAllStops().Select(e => StopViewModel.FromStop(e));
        }
        var stop = stopService.GetAllStops().Select(e => StopViewModel.FromStop(e));

        if (currentStop != null)
        {

            int id = int.Parse(currentStop);
            int stopCount = stopService.GetAllStops().Count();
            Console.Write("COUNT" + stopCount);
            Stop nextStop;
            if (stopCount == id)
            {
                nextStop = stopService.FindStopByID(id - 1);
            }
            else
            {
                nextStop = stopService.FindStopByID(id + 1);
            }
            if (nextStop != null)
            {

                ViewBag.NextStop = nextStop.Name;
            }

        }


        var BusNumber = HttpContext.Session.GetInt32("BusNumber");
        var DriverName = HttpContext.Session.GetString("DriverName");
        var LoopName = HttpContext.Session.GetString("LoopName");
        var Stops = HttpContext.Session.GetString("Stops");

        ViewBag.BusNumber = BusNumber;
        ViewBag.DriverName = DriverName;
        ViewBag.LoopName = LoopName;

        ViewBag.Id = new SelectList(stop, "Id", "Name");
        ViewBag.StopName = new SelectList(stop, "Id", "Name");

        return View();
    }

    [Authorize(Roles = "driver")]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult CreateDriverEntry(EntryCreateModel model)
    {
        var BusNumber = HttpContext.Session.GetInt32("BusNumber");
        var DriverName = HttpContext.Session.GetString("DriverName");
        var LoopName = HttpContext.Session.GetString("LoopName");

        _logger.LogInformation("ID: " + Convert.ToString(model.StopName));
        _logger.LogInformation("Boarded: " + Convert.ToString(model.Boarded));
        _logger.LogInformation("Left Behind: " + Convert.ToString(model.LeftBehind));
        if (model.StopName != null)
        {

            ViewBag.DriverName = DriverName;
            ViewBag.LoopName = LoopName;
            ViewBag.BusNumber = (int)BusNumber;

            int stopId = int.Parse(model.StopName);
            Stop stopName = stopService.FindStopByID(stopId);

            entryService.CreateEntry((int)BusNumber, DriverName, LoopName, stopName.Name, model.Boarded, model.LeftBehind, DateTime.Now);
            TempData["SuccessMessage"] = "Your entry has been submited!";
            TempData["CurrentStop"] = model.StopName;

            _logger.LogInformation("Successfully created an entry!");
            
            return RedirectToAction("DriverEntryView");

        }
        else
        {
            _logger.LogWarning("Error creating entry!");

            var stops = stopService.GetAllStops().Select(e => StopViewModel.FromStop(e));
            ViewBag.DriverName = DriverName;
            ViewBag.LoopName = LoopName;
            ViewBag.BusNumber = (int)BusNumber;
            ViewBag.StopName = new SelectList(stops, "Id", "Name");

            return View("DriverEntryView", model);
        }

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

    [Authorize(Roles = "manager")]
    public IActionResult EditLoop([FromRoute] int id)
    {
        var loop = loopService.FindLoopByID(id);
        var loopEditModel = LoopEditModel.FromLoop(loop);
        return View(loopEditModel);
    }

    [Authorize(Roles = "manager")]
    public IActionResult EditBus([FromRoute] int id)
    {
        var bus = busService.FindBusByID(id);
        var busEditModel = BusEditModel.FromBus(bus);
        return View(busEditModel);
    }

    [Authorize(Roles = "manager")]
    public IActionResult DeleteBus([FromRoute] int id)
    {
        busService.DeleteBus(id);
        return RedirectToAction("ViewBus");

    }
    [Authorize(Roles = "manager")]
    public IActionResult DeleteLoop([FromRoute] int id)
    {
        loopService.DeleteLoop(id);
        return RedirectToAction("ViewLoop");

    }

    [Authorize(Roles = "manager")]
    public IActionResult DeleteDriver([FromRoute] int id)
    {
        driverService.DeleteDriver(id);
        return RedirectToAction("ViewDriver");

    }

    [Authorize(Roles = "manager")]
    public IActionResult DeleteStop([FromRoute] int id)
    {
        stopService.DeleteStop(id);
        return RedirectToAction("ViewStop");

    }



    [Authorize(Roles = "manager")]
    public IActionResult EditStop([FromRoute] int id)
    {
        var stop = stopService.FindStopByID(id);
        var stopEditModel = StopEditModel.FromStop(stop);
        return View(stopEditModel);
    }
    // POST: Home/EditEntry/5
    [Authorize(Roles = "manager")]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EditEntry(int id, [Bind("BusNumber,DriverName,LoopName,StopName,Boarded,LeftBehind")] EntryEditModel entry)
    {
        if (ModelState.IsValid)
        {
            entryService.UpdateEntryByID(id, entry.BusNumber, entry.DriverName, entry.LoopName, entry.StopName, entry.Boarded, entry.LeftBehind, DateTime.Now);
            return RedirectToAction("ViewEntry", new { id = id });
        }
        else
        {
            return View(entry);
        }
    }

    [Authorize(Roles = "manager")]
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

    [Authorize(Roles = "manager")]
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

    [Authorize(Roles = "manager")]
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

    [Authorize(Roles = "manager")]
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
    [Authorize(Roles = "driver")]
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


    [Authorize(Roles = "manager")]
    public IActionResult CreateDriver()
    {
        return View(new DriverCreateModel());
    }

    [Authorize(Roles = "manager")]
    public IActionResult CreateLoop()
    {
        return View(new LoopCreateModel());
    }

    [Authorize(Roles = "manager")]
    public IActionResult CreateBus()
    {
        return View(new BusCreateModel());
    }

    [Authorize(Roles = "manager")]
    public IActionResult CreateStop()
    {
        return View(new StopCreateModel());
    }

    [Authorize(Roles = "manager")]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateLoop([Bind("Name")] LoopCreateModel loop)
    {
        if (ModelState.IsValid)
        {
            loopService.CreateLoop(loop.Name);

            _logger.LogInformation("Successfully created a new loop: {Name}!",loop.Name);
            return RedirectToAction("ViewLoop");
        }
        else
        {

            return View(loop);
        }
    }

    [Authorize(Roles = "manager")]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateDriver([Bind("FirstName", "LastName")] DriverCreateModel driver)
    {
        if (ModelState.IsValid)
        {
            driverService.CreateDriver(driver.FirstName, driver.LastName);

            _logger.LogInformation("Successfully created a new driver: {First} {Last}!",driver.FirstName,driver.LastName);
            return RedirectToAction("ViewDriver");
        }
        else
        {

            _logger.LogWarning("Error creating driver!");
            return View(driver);
        }
    }

    [Authorize(Roles = "manager")]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateBus([Bind("BusNumber")] BusCreateModel bus)
    {
        if (ModelState.IsValid)
        {
            busService.CreateBus(bus.BusNumber);
            _logger.LogInformation("Successfully created a new bus: {Number}!",bus.BusNumber);
            return RedirectToAction("ViewBus");
        }
        else
        {
            _logger.LogWarning("Error creating a bus!");
            return View(bus);
        }
    }

    [Authorize(Roles = "manager")]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> FilterEntry(DateTime? date, string loopName)
    {
        var loops = loopService.GetAllLoops().Select(e => LoopViewModel.FromLoop(e));
        var entries = entryService.GetAllEntries().Select(e => EntryViewModel.FromEntry(e));
        ViewBag.LoopName = new SelectList(loops, "Name", "Name");
        IEnumerable<EntryViewModel> result = entries;
        if (date != null && loopName != null)
        {
            result = entries.Where(e => e.TimeStamp.Date == date && e.LoopName == loopName);
        }
        else if (date!=null)
        {
            result = entries.Where(e => e.TimeStamp.Date == date);
        }
        else if (loopName != null)
        {
            result = entries.Where(e => e.LoopName == loopName);
        }

        return View("ViewEntry", result);
    }

    [Authorize(Roles = "manager")]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateStop([Bind("Name")] StopCreateModel stop)
    {
        if (ModelState.IsValid)
        {
            stopService.CreateStop(stop.Name);
            _logger.LogInformation("Successfully created a new stop: {Name}!",stop.Name);
            return RedirectToAction("ViewStop");
        }
        else
        {
            _logger.LogWarning("Error creating driver!");
            return View(stop);
        }
    }



}
