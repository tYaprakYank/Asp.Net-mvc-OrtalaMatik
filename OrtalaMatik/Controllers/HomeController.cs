using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using OrtalaMatik.Models;

namespace OrtalaMatik.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    Atp11bContext db = new Atp11bContext();
    public IActionResult Index()
    {
        return View();

        //return Content("Merhaba Mvc");
    }
    [Route("/privacy")]
    public IActionResult Privacy()
    {
        return View();
    }
    [Route("/contact")]

    public IActionResult Contact()
    {
        return View();
    }
    [Route("/todolist")]

    public IActionResult Todolist()
    {
        var model = new TodoViewModel()
        {
            Todos = db.Todos.OrderByDescending(x => x.Id).ToList()
        };
        return View(model);
    }

    [HttpPost]
    [IgnoreAntiforgeryToken]
    [Route("/add-todo")]
    public IActionResult AddTodo(Todo postedData)
    {
        Todo toAdd = new Todo();
        toAdd.Title = postedData.Title;
        toAdd.Iscomplated = false;
        db.Add(toAdd);
        db.SaveChanges();
        return Redirect("/todolist");
    }


    [Route("/update-todo/{id}")]
    public IActionResult UpdateTodo(int id)
    {
        Todo toUpdate = db.Todos.Find(id)!;
        toUpdate.Iscomplated = !toUpdate.Iscomplated;
        db.Entry(toUpdate).CurrentValues.SetValues(toUpdate);
        db.SaveChanges();
        return Content(toUpdate.Iscomplated.ToString()!);
    }
    [Route("/remove-todo/{id}")]
    public IActionResult RemoveTodo(int id)
    {
        Todo toDelete = db.Todos.Find(id)!;
        db.Remove(toDelete);
        db.SaveChanges();
        return Redirect("/todolist");
    }

    [HttpPost]
    [IgnoreAntiforgeryToken]
    [Route("/add-lesson")]
    public IActionResult AddDers(Lesson postedData)
    {
        Lesson dersAdd = new Lesson();
        dersAdd.Lessonname = postedData.Lessonname;
        dersAdd.Lessoncredit = postedData.Lessoncredit;
        dersAdd.Lessonnote = postedData.Lessonnote;


        db.Add(dersAdd);
        db.SaveChanges();
        return Redirect("/Ortalmatik");
    }
    
    [Route("/remove-lesson/{id}")]
    public IActionResult RemoveLesson(int id)
    {
        Lesson lessonDelete = db.Lessons.Find(id)!;
        db.Remove(lessonDelete);
        db.SaveChanges();
        return Redirect("/Ortalmatik");
    }


    [Route("/Ortalmatik")]
    public IActionResult Ortalmatik()
    {
       var model = new LessonViewModel()
        {
            Lessons = db.Lessons.OrderByDescending(x=> x.Id).ToList()
        };
        return View(model);
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
