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

    public HomeController(ILogger<HomeController> logger,IEntryService entryService)
    {
        _logger = logger;
        this.entryService = entryService;
    }

    public IActionResult Index()
    {
       return View(entryService.GetAllEntries().Select(e => EntryViewModel.FromEntry(e)));
    }

    // GET: /Home/EditEntry/{id}
    public IActionResult EditEntry([FromRoute] int id)
    {
        var entry = entryService.FindEntryByID(id);
        var entryEditModel = EntryEditModel.FromEntry(entry);
        return View(entryEditModel);
    }

    // POST: Home/EditEntry/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EditEntry(int id, [Bind("BusNumber,DriverName,LoopName,StopName,Boarded,LeftBehind")] EntryEditModel entry)
    {
        if (ModelState.IsValid)
        {
            entryService.UpdateEntryByID(id,entry.BusNumber,entry.DriverName,entry.LoopName,entry.StopName,entry.Boarded,entry.LeftBehind);
            return RedirectToAction("ViewEntry", new { id = id });
        }
        else
        {
            return View(entry);
        }
    }



    public IActionResult ViewEntry([FromRoute] int id)
    {
        var entry = entryService.FindEntryByID(id);
        return View(EntryViewModel.FromEntry(entry));
    }


    public IActionResult CreateEntry()
    {
        return View(new EntryCreateModel());
    }
    // POST: /Home/CreateEntry
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateEntry([Bind("BusNumber,DriverName,LoopName,StopName,Boarded,LeftBehind")] EntryCreateModel entry)
    {
        if (ModelState.IsValid)
        {
            entryService.CreateEntry(entry.BusNumber, entry.DriverName, entry.LoopName,entry.StopName,entry.Boarded,entry.LeftBehind);
            return RedirectToAction("Index");
        }
        else
        {

            return View(entry);
        }
    }

}
