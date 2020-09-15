using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PayDel.Data.DatabaseContext;
using PayDel.Data.Models;
using PayDel.Repo.Infrastructures;

namespace PayDel.Presentation.Controllers.Api
{

    [AllowAnonymous]
    
    public class HomeController : Controller
    {
        private readonly IUnitOfWork<PayDelDbContext> _context;
        public HomeController(IUnitOfWork<PayDelDbContext> dbContext)
        {
            _context = dbContext;
        }
        //[Route("home/index/{id}")]
        [HttpGet]
        public async Task<ActionResult> Index(string id)
        {
            var photo = await _context._PhotoRepository
                .GetAsNoTrackingByIdAsync(p=>p.Id == id); // tracking not required
                

            if (photo == null)
            {
                return NotFound();
            }

            // Use strongly typed data rather than ViewData.

            return View("~/Views/Home/Index.cshtml", photo);
        }
        //[Route("home/index")]
        [HttpPost]
        public async Task<ActionResult> Index(Photo photo)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            var departmentToUpdate = await _context._PhotoRepository
                .GetAsNoTrackingByIdAsync(m => m.Id == photo.Id);

            if (departmentToUpdate == null)
            {
                return View(departmentToUpdate);
            }

            var s = await _context._PhotoRepository.UpdateSaveConcurrency(photo);


            return RedirectToAction("Index",new { id= photo.Id});
        }

       


    }
}
