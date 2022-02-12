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
    public class WorkOrdersController : Controller
    {
        private readonly WorkOrdersContext _context;
        private readonly IMapper _mapper; //dependency injection

        public WorkOrdersController(WorkOrdersContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: WorkOrders
        public async Task<IActionResult> Index(WorkOrdersViewModel filter)
        {
            WorkOrdersViewModel model = new WorkOrdersViewModel();
            var workOrdersContext = await _context.WorkOrders.Include(w => w.Customer).Include(w => w.Project).ToListAsync();
            if (filter != null)
            {
                if (filter.Customer != null && !String.IsNullOrEmpty(filter.Customer.Name))
                {
                    workOrdersContext = workOrdersContext.Where(x => x.Customer.Name == filter.Customer.Name).ToList();
                }
                if (filter.Project != null && !String.IsNullOrEmpty(filter.Project.Name))
                {
                    workOrdersContext = workOrdersContext.Where(w => w.Project.Name == filter.Project.Name).ToList();
                }
                if (!String.IsNullOrEmpty(filter.UserId))
                {
                    workOrdersContext = workOrdersContext.Where(x => x.UserId == filter.UserId).ToList();
                }
                if (!String.IsNullOrEmpty(filter.CustomerNote))
                {
                    workOrdersContext = workOrdersContext.Where(x => x.CustomerNote == filter.CustomerNote).ToList();
                }
                if (!String.IsNullOrEmpty(filter.PerformedWorks))
                {
                    workOrdersContext = workOrdersContext.Where(x => x.PerformedWorks == filter.PerformedWorks).ToList();
                }
            }
            List<WorkOrdersViewModel> workOrdersViewList = _mapper.Map<List<WorkOrder>, List<WorkOrdersViewModel>>(workOrdersContext); // Mapped List in WorkOrdersViewModel from WorkOrder
            model.WorkOrders = workOrdersViewList;

            model.Customers = new SelectList(_context.Customers, "CustomerId", "Name");
            model.Projects = new SelectList(_context.Projects, "Name", "Name");
            model.Users = new SelectList(_context.WorkOrders.Select(x => new SelectListItem()
            {
                Value = x.UserId,
                Text = x.UserId
            }).Distinct().ToList(), "Value", "Text");

            model.CustomerNotes = new SelectList(_context.WorkOrders.Select(x => new SelectListItem()
            {
                Value = x.CustomerNote,
                Text = x.CustomerNote
            }).Distinct().ToList(), "Value", "Text");

            model.PerformedWorksModel = new SelectList(_context.WorkOrders.Select(x => new SelectListItem()
            {
                Value = x.PerformedWorks,
                Text = x.PerformedWorks
            }).Distinct().ToList(), "Value", "Text");

            return View(model);
        }

        // GET: WorkOrders/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var workOrder = await _context.WorkOrders
                .Include(w => w.Customer)
                .Include(w => w.Project)
                .FirstOrDefaultAsync(m => m.WorkOrderId == id);
            if (workOrder == null)
            {
                return NotFound();
            }

            return View(workOrder);
        }

        // GET: WorkOrders/Create
        public IActionResult Create()
        {
            ViewData["CustomerId"] = new SelectList(_context.Customers, "CustomerId", "CustomerId");
            ViewData["ProjectId"] = new SelectList(_context.Projects, "ProjectId", "ProjectId");
            ViewData["UserId"] = new SelectList(_context.WorkOrders, "UserId", "UserId");
            return View();
        }

        // POST: WorkOrders/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("WorkOrderId,CustomerId,ProjectId,UserId,Date,CustomerNote,PerformedWorks,Hours,Minutes,IsActive,CreatedBy,CreatedOn,UpdatedBy,UpdatedOn")] WorkOrder workOrder)
        {
            if (ModelState.IsValid)
            {

                workOrder.CreatedOn = DateTime.Now; // Set the Created Data on Current Time, handdled on Backend side
                workOrder.CreatedBy = User?.Identity != null ? User.Identity.Name : ""; // User != null && User.Identity != null ? 

                _context.Add(workOrder);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CustomerId"] = new SelectList(_context.Customers, "CustomerId", "CustomerId", workOrder.CustomerId);
            ViewData["ProjectId"] = new SelectList(_context.Projects, "ProjectId", "ProjectId", workOrder.ProjectId);
            ViewData["UserId"] = new SelectList(_context.WorkOrders, "UserId", "UserId", workOrder.UserId);
            return View(workOrder);
        }

        // GET: WorkOrders/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var workOrder = await _context.WorkOrders
                .Include(x => x.Customer)
                .Include(x => x.Project)
                .FirstOrDefaultAsync(x => x.WorkOrderId == id);
            if (workOrder == null)
            {
                return NotFound();
            }

            var workOrderUser = _context.AspNetUsers.SingleOrDefault(x => x.Id == workOrder.UserId);

            ViewData["selectListCustomerName"] = new SelectList(_context.Customers, "Name", "Name", workOrder.Customer?.Name);
            ViewData["selectListProjectName"] = new SelectList(_context.Projects, "Name", "Name", workOrder.Project?.Name);
            ViewData["selectListProjectUserName"] = new SelectList(_context.AspNetUsers, "UserName", "Id", workOrderUser?.UserName);
            ViewData["CustomerNote"] = new SelectList(_context.WorkOrders, "CustomerNote", "CustomerNote");
            ViewData["PerformedWorks"] = new SelectList(_context.WorkOrders, "PerformedWorks", "PerformedWorks");
            ViewData["CreatedOn"] = new SelectList(_context.WorkOrders, "CreatedOn", "CreatedOn", workOrder.CreatedOn);
            ViewData["UpdatedOn"] = new SelectList(_context.WorkOrders, "UpdatedOn", "UpdatedOn", workOrder.UpdatedOn);
            return View(workOrder);
        }

        // POST: WorkOrders/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("WorkOrderId,CustomerId,ProjectId,UserId,Date,CustomerNote,PerformedWorks,Hours,Minutes,IsActive,CreatedBy,CreatedOn,UpdatedBy,UpdatedOn")] WorkOrder workOrder)
        {
            if (id != workOrder.WorkOrderId)
            {
                return NotFound();
            }

            workOrder.CreatedOn = DateTime.Now;
            workOrder.CreatedBy = User?.Identity != null ? User.Identity.Name : "";

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(workOrder);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!WorkOrderExists(workOrder.WorkOrderId))
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

            var workOrderUser = _context.AspNetUsers.SingleOrDefault(x => x.Id == workOrder.UserId);

            ViewData["selectListCustomerName"] = new SelectList(_context.Customers, "Name", "Name", workOrder.Customer.Name);
            ViewData["selectListProjectName"] = new SelectList(_context.Projects, "Name", "Name", workOrder.Project.Name);
            ViewData["selectListProjectUserName"] = new SelectList(_context.AspNetUsers, "UserName", "UserName", workOrderUser?.UserName);
            ViewData["CustomerNote"] = new SelectList(_context.WorkOrders, "CustomerNote", "CustomerNote");
            ViewData["PerformedWorks"] = new SelectList(_context.WorkOrders, "PerformedWorks", "PerformedWorks");
            ViewData["CreatedOn"] = new SelectList(_context.WorkOrders, "CreatedOn", "CreatedOn", workOrder.CreatedOn);
            ViewData["UpdatedOn"] = new SelectList(_context.WorkOrders, "UpdatedOn", "UpdatedOn", workOrder.UpdatedOn);
            return View(workOrder);
        }

        // GET: WorkOrders/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var workOrder = await _context.WorkOrders
                .Include(w => w.Customer)
                .Include(w => w.Project)
                .FirstOrDefaultAsync(m => m.WorkOrderId == id);
            if (workOrder == null)
            {
                return NotFound();
            }

            return View(workOrder);
        }

        // POST: WorkOrders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var workOrder = await _context.WorkOrders.FindAsync(id);
            _context.WorkOrders.Remove(workOrder);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool WorkOrderExists(int id)
        {
            return _context.WorkOrders.Any(e => e.WorkOrderId == id);
        }
    }
}
