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
    public class AuthorsController : Controller
    {
        private readonly LibraryContext _context;

        public AuthorsController(LibraryContext context)
        {
            _context = context;
        }

        // GET: Authors
        public async Task<IActionResult> Index(
            string sortOrder,
            string currentFilter,
            string searchString,
            int? page)
        {
            ViewData["CurrentSort"] = sortOrder;
            ViewData["IdSortParm"] = String.IsNullOrEmpty(sortOrder) ? "id_desc" : ""; 
            ViewData["NameSortParm"] = sortOrder == "Name" ? "name_desc" : "Name";

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewData["CurrentFilter"] = searchString;

            var authors = from a in _context.Authors
                           select a;
            if (!String.IsNullOrEmpty(searchString))
            {
                authors = authors.Where(a => a.FullName.Contains(searchString));
            }
            switch (sortOrder)
            {
                case "id_desc":
                    authors = authors.OrderByDescending(a => a.AuthorID);
                    break;
                case "Name":
                    authors = authors.OrderBy(a => a.FullName);
                    break;
                case "name_desc":
                    authors = authors.OrderByDescending(a => a.FullName);
                    break;
                default:
                    authors = authors.OrderBy(a => a.AuthorID);
                    break;
            }

            int pageSize = 3;
            return View(await PaginatedList<Author>.CreateAsync(authors.AsNoTracking(), page ?? 1, pageSize));
        }

        // GET: Authors/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var author = await _context.Authors
                .Include(s => s.Books)
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.AuthorID == id);

            if (author == null)
            {
                return NotFound();
            }

            return View(author);
        }

        // GET: Authors/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Authors/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,FullName,BookID")] Author author)
        {
            if (ModelState.IsValid)
            {
                _context.Add(author);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(author);
        }

        // GET: Authors/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var author = await _context.Authors
                .Include(c => c.Books)
                .AsNoTracking()
                .SingleOrDefaultAsync(m => m.AuthorID == id);

            if (author == null)
            {
                return NotFound();
            }
            return View(author);
        }

        // POST: Authors/Edit/5
        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditPost(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var authorToUpdate = await _context.Authors.SingleOrDefaultAsync(s => s.AuthorID == id);
            if (await TryUpdateModelAsync<Author>(
                authorToUpdate,
                "",
                s => s.FullName))
            {
                try
                {
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateException )
                {
                    ModelState.AddModelError("", "Unable to save changes. " +
                        "Try again, and if the problem persists, " +
                        "see your system administrator.");
                }
            }
            return View(authorToUpdate);
        }

        // GET: Authors/Delete/5
        public async Task<IActionResult> Delete(int? id, bool? saveChangesError = false)
        {
            if (id == null)
            {
                return NotFound();
            }

            var author = await _context.Authors
                .AsNoTracking()
                .SingleOrDefaultAsync(m => m.AuthorID == id);
            if (author == null)
            {
                return NotFound();
            }

            if (saveChangesError.GetValueOrDefault())
            {
                ViewData["ErrorMessage"] =
                    "Delete failed. Try again, and if the problem persists " +
                    "see your system administrator.";
            }

            return View(author);
        }

        // POST: Authors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            Author author = await _context.Authors
                .Include(i => i.Books)
                .SingleAsync(i => i.AuthorID == id);

            var books = await _context.Books
                .Where(d => d.AuthorID == id)
                .ToListAsync();

            _context.Authors.Remove(author);

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
