using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ProjectMVC_FoxIT.Data;
using ProjectMVC_FoxIT.Models;
using ProjectMVC_FoxIT.Models.VIewModel;

namespace ProjectMVC_FoxIT.Controllers
{
    public class ProjectsController : Controller
    {
        private readonly WorkOrdersContext _context;
        private readonly IMapper _mapper;

        public ProjectsController(WorkOrdersContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: Projects
        public async Task<IActionResult> Index(ProjectViewModel filter)
        {
            ProjectViewModel model = new ProjectViewModel();
            var projectsList = await _context.Projects.Include(p => p.Customer).ToListAsync();
            if (filter != null)
            {
                if (!String.IsNullOrEmpty(filter.Name))
                {
                    projectsList = projectsList.Where(x => x.Name == filter.Name).ToList();
                }
                if (filter.Customer != null && !String.IsNullOrEmpty(filter.Customer.Name))
                {
                    projectsList = projectsList.Where(x => x.Customer.Name == filter.Customer.Name).ToList();
                }
            }

            model.ProjectNames = new SelectList(_context.Projects, "Name", "Name");
            model.Customers = new SelectList(_context.Customers, "Name", "Name");
            List<ProjectViewModel> projectViewModels = _mapper.Map<List<Project>, List<ProjectViewModel>>(projectsList);

            model.Projects = projectViewModels;
            return View(model);
        }

        // GET: Projects/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var project = await _context.Projects
                .Include(p => p.Customer)
                .FirstOrDefaultAsync(m => m.ProjectId == id);
            if (project == null)
            {
                return NotFound();
            }

            return View(project);
        }

        // GET: Projects/Create
        public IActionResult Create()
        {

            ViewData["CustomerId"] = new SelectList(_context.Customers, "CustomerId", "CustomerId");

            return View();
        }

        // POST: Projects/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ProjectId,Name,CustomerId,IsActive,CreatedBy,CreatedOn,UpdatedBy,UpdatedOn")] Project project)
        {
            if (ModelState.IsValid)
            {
                project.CreatedOn = DateTime.Now; // Set the Created Data on Current Time, handdled on Backend side
                project.CreatedBy = User?.Identity != null ? User.Identity.Name : ""; // User != null && User.Identity != null ? 

                _context.Add(project);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CustomerId"] = new SelectList(_context.Customers, "CustomerId", "CustomerId", project.CustomerId);


            return View(project);
        }

        // GET: Projects/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var project = await _context.Projects
                .Include(x => x.Customer)
                .FirstOrDefaultAsync(x => x.ProjectId == id);

            if (project == null)
            {
                return NotFound();
            }
            ViewData["SelectListName"] = new SelectList(_context.Projects, "Name", "Name", project.Name);
            ViewData["SelectListCustomerName"] = new SelectList(_context.Customers, "Name", "Name", project.Customer.Name);
            return View(project);
        }

        // POST: Projects/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ProjectId,Name,CustomerId,IsActive,CreatedBy,CreatedOn,UpdatedBy,UpdatedOn")] Project project)
        {
            if (id != project.ProjectId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    project.UpdatedOn = DateTime.Now;
                    project.UpdatedBy = User?.Identity != null ? User.Identity.Name : "";

                    _context.Update(project);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProjectExists(project.ProjectId))
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
            ViewData["SelectListName"] = new SelectList(_context.Projects, "Name", "Name", project.Name);
            ViewData["SelectListCustomerName"] = new SelectList(_context.Customers, "Name", "Name", project.Customer?.Name);
            return View(project);
        }

        // GET: Projects/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var project = await _context.Projects
                .Include(p => p.Customer)
                .FirstOrDefaultAsync(m => m.ProjectId == id);
            if (project == null)
            {
                return NotFound();
            }

            return View(project);
        }

        // POST: Projects/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var project = await _context.Projects.FindAsync(id);
            _context.Projects.Remove(project);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProjectExists(int id)
        {
            return _context.Projects.Any(e => e.ProjectId == id);
        }
    }
}
