using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MyLibrary.Data;
using MyLibrary.Models;
using MyLibrary.Services;

namespace MyLibrary.Controllers
{
    public class MembersController : Controller
    {
        private readonly LibraryContext _context;
        private readonly IMemberService _memberService;

        public MembersController(LibraryContext context, IMemberService memberService)
        {
            _context = context;
            _memberService = memberService;
        }

        // GET: Members
        public async Task<IActionResult> Index(
            string sortOrder,
            string currentFilter,
            string searchString,
            int? page)
        {
            ViewData["CurrentSort"] = sortOrder;
            ViewData["NameSortParm"] = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewData["CurrentFilter"] = searchString;

            var members = from b in _context.Members
                           select b;

            if (!String.IsNullOrEmpty(searchString))
            {
                members = members.Where(b => b.LastName.Contains(searchString));
            }
            switch (sortOrder)
            {
                case "name_desc":
                    members = members.OrderByDescending(b => b.LastName);
                    break;
                default:
                    members = members.OrderBy(b => b.LastName);
                    break;
            }
            int pageSize = 3;

            //MemberViewModel memberVM = new MemberViewModel
            //{
            //}

            return View(await PaginatedList<Member>.CreateAsync(members.AsNoTracking(), page ?? 1, pageSize));
        }

        // GET: Members/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var member = await _context.Members
                .Include(m => m.Rentals)
                    .ThenInclude(m => m.Book)
                .AsNoTracking()
                .SingleOrDefaultAsync(m => m.ID == id);

            if (member == null)
            {
                return NotFound();
            }

            return View(member);
        }

        // GET: Members/Create
        public IActionResult Create()
        {
            BooksDropDownList();
            return View();
        }

        // POST: Members/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("FirstName,LastName,BookID")] MemberViewModel memberVM)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    Member member = await _memberService.CreateMemberAsync(memberVM);
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (DbUpdateException)
            {
                ModelState.AddModelError("", "Unable to save changes. " +
                    "Try again, and if the problem persists " +
                    "see your system administrator.");
            }

            BooksDropDownList();
            return View(memberVM);
        }

        // GET: Members/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var member = await _context.Members
                .AsNoTracking()
                .SingleOrDefaultAsync(m => m.ID == id);


            MemberViewModel memberVM = new MemberViewModel
            {
                FirstName = member.FirstName,
                LastName = member.LastName
            };

            return View(memberVM);
        }

        // POST: Members/Edit/5
        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditPost(int id, MemberViewModel memberVM)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var status = await _memberService.UpdateMemberAsync(id, memberVM);

                    return RedirectToAction(nameof(Index));
                }
            }
            catch (DbUpdateException)
            {
                ModelState.AddModelError("", "Unable to save changes.");
            }

            return View(memberVM);
        }

        private void BooksDropDownList(object selectedBook = null)
        {
            var booksQuery = from b in _context.Books
                                   orderby b.Title
                                   select b;
            ViewBag.BookID = new SelectList(booksQuery.AsNoTracking(), "BookID", "Title", selectedBook);
        }

        // GET: Members/Delete/5
        public async Task<IActionResult> Delete(int? id, bool? saveChangesError = false)
        {
            if (id == null)
            {
                return NotFound();
            }

            var member = await _context.Members
                .Include(m => m.Rentals)
                .AsNoTracking()
                .SingleOrDefaultAsync(m => m.ID == id);
            if (member == null)
            {
                return NotFound();
            }

            if (saveChangesError.GetValueOrDefault())
            {
                ViewData["ErrorMessage"] =
                    "Delete failed. Try again, and if the problem persists " +
                    "see your system administrator.";
            }

            return View(member);
        }

        // POST: Members/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            Member member = await _context.Members
                .SingleAsync(m => m.ID == id);

            var rentals = await _context.Rentals
                .Where(r => r.MemberID == id)
                .ToListAsync();

            _context.Members.Remove(member);

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
