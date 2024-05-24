using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RunGroopWebApp;
using TaskManagementSystem.Models;

namespace TaskManagementSystem.Controllers
{
    [Authorize]
    public class TaskModelsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public TaskModelsController(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        // GET: TaskModels
        
        public async Task<IActionResult> Index()
        {
            var curUser = _httpContextAccessor.HttpContext?.User.GetUserId();
            return View(await _context.TaskModel.Where(T=>T.AppUserId==curUser).ToListAsync());
            //return View(new List<TaskModel>() { new TaskModel() { Title = "test", Description = "ts", Id = 1, AppUserId = "1", DueDate = DateTime.Now, Status = "Incomplite" } });
        }

        // GET: TaskModels/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var taskModel = await _context.TaskModel
                .FirstOrDefaultAsync(m => m.Id == id);
            if (taskModel == null)
            {
                return NotFound();
            }

            return View(taskModel);
        }

        // GET: TaskModels/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: TaskModels/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,Description,DueDate,Status")] TaskModel taskModel)
        {
            var curUser = _httpContextAccessor.HttpContext?.User.GetUserId();
            if (ModelState.IsValid)
            {
                taskModel.AppUserId = curUser;
                _context.Add(taskModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(taskModel);
        }

        // GET: TaskModels/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var taskModel = await _context.TaskModel.FindAsync(id);
            if (taskModel == null)
            {
                return NotFound();
            }
            return View(taskModel);
        }

        // POST: TaskModels/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Description,DueDate,Status")] TaskModel taskModel)
        {
            var curUser = _httpContextAccessor.HttpContext?.User.GetUserId();
            if (id != taskModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    taskModel.AppUserId = curUser;
                    _context.Update(taskModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TaskModelExists(taskModel.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(taskModel);
        }

        // GET: TaskModels/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var taskModel = await _context.TaskModel
                .FirstOrDefaultAsync(m => m.Id == id);
            if (taskModel == null)
            {
                return NotFound();
            }

            return View(taskModel);
        }

        // POST: TaskModels/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var taskModel = await _context.TaskModel.FindAsync(id);
            if (taskModel != null)
            {
                _context.TaskModel.Remove(taskModel);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> UpdateStatus(int? id,string status)
        {
            if (id == null)
            {
                return NotFound();
            }

            var taskModel = await _context.TaskModel
                .FirstOrDefaultAsync(m => m.Id == id);
            if (taskModel == null)
            {
                return NotFound();
            }

            try
            {
                taskModel.Status = status;
                _context.Update(taskModel);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TaskModelExists(taskModel.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToAction(nameof(Index));
        }

        private bool TaskModelExists(int id)
        {
            return _context.TaskModel.Any(e => e.Id == id);
        }
    }
}
