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
    public class CustomersController : Controller
    {
        private readonly WorkOrdersContext _context;
        private readonly IMapper _mapper;

        public CustomersController(WorkOrdersContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: Customers
        public async Task<IActionResult> Index(CustomerViewModel filter)
        {

            CustomerViewModel model = new CustomerViewModel();
            var customerList = await _context.Customers.ToListAsync();

            if (filter != null)
            {
                if (!String.IsNullOrEmpty(filter.Name))
                {
                    customerList = customerList.Where(x => x.Name == filter.Name).ToList();
                }
                if (!String.IsNullOrEmpty(filter.Address))
                {
                    customerList = customerList.Where(x => x.Address == filter.Address).ToList();
                }
                if (!String.IsNullOrEmpty(filter.Edb))
                {
                    customerList = customerList.Where(x => x.Edb== filter.Edb).ToList();
                }
            }

            model.CustomerNames = new SelectList(_context.Customers, "Name", "Name");
            model.CustomerAddress = new SelectList(_context.Customers, "Address", "Address");
            model.CustomerEdbs = new SelectList(_context.Customers, "Edb", "Edb");

            List<CustomerViewModel> customerViewModels = _mapper.Map<List<Customer>, List<CustomerViewModel>>(customerList);
            model.Customers = customerViewModels;

            return View(model);
        }

        // GET: Customers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customer = await _context.Customers
                .FirstOrDefaultAsync(m => m.CustomerId == id);
            if (customer == null)
            {
                return NotFound();
            }

            return View(customer);
        }

        // GET: Customers/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Customers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CustomerId,Name,Address,City,Phone,Email,Web,Edb,IsActive,CreatedBy,CreatedOn,UpdatedBy,UpdatedOn")] Customer customer)
        {
            if (ModelState.IsValid)
            {
                customer.CreatedOn = DateTime.Now; // Set the Created Data on Current Time, handdled on Backend side
                customer.CreatedBy = User?.Identity != null ? User.Identity.Name : ""; // User != null && User.Identity != null ? 

                _context.Add(customer);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(customer);
        }

        // GET: Customers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customer = await _context.Customers.FindAsync(id);
            if (customer == null)
            {
                return NotFound();
            }

            ViewData["CustomersListName"] = new SelectList(_context.Customers, "Name", "Name", customer.Name);
            ViewData["CustomerListAddress"] = new SelectList(_context.Customers, "Address", "Address", customer.Address);
            ViewData["CustomerListEdb"] = new SelectList(_context.Customers, "Edb", "Edb", customer.Edb);
            return View(customer);
        }

        // POST: Customers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CustomerId,Name,Address,City,Phone,Email,Web,Edb,IsActive,CreatedBy,CreatedOn,UpdatedBy,UpdatedOn")] Customer customer)
        {
            if (id != customer.CustomerId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    customer.UpdatedOn = DateTime.Now;
                    customer.UpdatedBy = User?.Identity != null ? User.Identity.Name : "";

                    _context.Update(customer);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CustomerExists(customer.CustomerId))
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
            ViewData["CustomersListName"] = new SelectList(_context.Customers, "Name", "Name", customer.Name);
            ViewData["CustomerListAddress"] = new SelectList(_context.Customers, "Address", "Address", customer.Address);
            ViewData["CustomerListEdb"] = new SelectList(_context.Customers, "Edb", "Edb", customer.Edb);
            return View(customer);
        }

        // GET: Customers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customer = await _context.Customers
                .FirstOrDefaultAsync(m => m.CustomerId == id);
            if (customer == null)
            {
                return NotFound();
            }

            return View(customer);
        }

        // POST: Customers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var customer = await _context.Customers.FindAsync(id);
            _context.Customers.Remove(customer);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CustomerExists(int id)
        {
            return _context.Customers.Any(e => e.CustomerId == id);
        }
    }
}
