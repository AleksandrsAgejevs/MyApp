using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MyLibrary.Data;
using MyLibrary.Models;

namespace MyLibrary.Controllers
{
    public class RentalsController : Controller
    {
        private readonly LibraryContext _context;

        public RentalsController(LibraryContext context)
        {
            _context = context;
        }

        // GET: Rentals
        public async Task<IActionResult> Index(
            string sortOrder,
            string currentFilter,
            string searchString,
            int? page)
        {
            ViewData["CurrentSort"] = sortOrder;
            ViewData["NameSortParm"] = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewData["DateSortParm"] = sortOrder == "Date" ? "date_desc" : "Date";

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewData["CurrentFilter"] = searchString;

            var rentals = from s in _context.Rentals
                          .Include(r => r.Book)
                          .Include(r => r.Member)
                            //.ThenInclude(r => r.FullName)
                          select s;
            if (!String.IsNullOrEmpty(searchString))
            {
                rentals = rentals.Where(s => s.Member.LastName.Contains(searchString));
            }
            switch (sortOrder)
            {
                case "name_desc":
                    rentals = rentals.OrderByDescending(s => s.Member.FullName);
                    break;
                case "Date":
                    rentals = rentals.OrderBy(s => s.RentalDate);
                    break;
                case "date_desc":
                    rentals = rentals.OrderByDescending(s => s.RentalDate);
                    break;
                default:
                    rentals = rentals.OrderBy(s => s.Member.FullName);
                    break;
            }

            int pageSize = 3;
            return View(await PaginatedList<Rental>.CreateAsync(rentals.AsNoTracking(), page ?? 1, pageSize));
        }
        

        // GET: Rentals/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var rental = await _context.Rentals
                .Include(r => r.Book)
                .Include(r => r.Member)
                .AsNoTracking()
                .SingleOrDefaultAsync(m => m.RentalID == id);
            if (rental == null)
            {
                return NotFound();
            }

            return View(rental);
        }

        // GET: Rentals/Create
        public IActionResult Create()
        {
            BookDropDownList();
            MemberDropDownList();
            return View();
        }

        // POST: Rentals/Create
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("RentalID,BookID,MemberID,RentalDate")] Rental rental)
        {
            if (ModelState.IsValid)
            {
                _context.Add(rental);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            BookDropDownList(rental.BookID);
            MemberDropDownList(rental.MemberID);
            return View(rental);
        }

        // GET: Rentals/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var rental = await _context.Rentals
                .AsNoTracking()
                .SingleOrDefaultAsync(m => m.RentalID == id);
            if (rental == null)
            {
                return NotFound();
            }
            BookDropDownList(rental.BookID);
            MemberDropDownList(rental.MemberID);
            return View(rental);
        }

        // POST: Rentals/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditPost(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var rentalToUpdate = await _context.Rentals
                 .SingleOrDefaultAsync(s => s.RentalID == id);
            if (await TryUpdateModelAsync<Rental>(rentalToUpdate,
                "",
                s => s.BookID, s => s.MemberID, s => s.RentalDate))
            {
                try
                {
                    await _context.SaveChangesAsync();

                }
                catch (DbUpdateException /* ex */)
                {
                    //Log the error (uncomment ex variable name and write a log.)
                    ModelState.AddModelError("", "Unable to save changes. " +
                        "Try again, and if the problem persists, " +
                        "see your system administrator.");
                }
                return RedirectToAction(nameof(Index));
            }
            BookDropDownList(rentalToUpdate.BookID);
            MemberDropDownList(rentalToUpdate.MemberID);
            return View(rentalToUpdate);
        }

        private void BookDropDownList(object selectedBook = null)
        {
            var booksQuery = from b in _context.Books
                               orderby b.Title
                               select b;
            ViewBag.BookID = new SelectList(booksQuery.AsNoTracking(), "BookID", "Title", selectedBook);
        }

        private void MemberDropDownList(object selectedMember = null)
        {
            var membersQuery = from m in _context.Members
                               orderby m.FullName
                               select m;
            ViewBag.MemberID = new SelectList(membersQuery.AsNoTracking(), "ID", "FullName", selectedMember);
        }

        // GET: Rentals/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var rental = await _context.Rentals
                .Include(r => r.Book)
                .Include(r => r.Member)
                .FirstOrDefaultAsync(m => m.RentalID == id);
            if (rental == null)
            {
                return NotFound();
            }

            return View(rental);
        }

        // POST: Rentals/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var rental = await _context.Rentals.FindAsync(id);
            _context.Rentals.Remove(rental);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RentalExists(int id)
        {
            return _context.Rentals.Any(e => e.RentalID == id);
        }
    }
}
