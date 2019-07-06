using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MyLibrary.Data;
using MyLibrary.Models;

namespace MyLibrary.Services
{
    public class MemberService : IMemberService
    {
        private readonly LibraryContext _context;

        public MemberService(LibraryContext context)
        {
            _context = context;
        }

        public async Task<Member> CreateMemberAsync(MemberViewModel memberVM)
        {
            Member member = new Member
            {
                FirstName = memberVM.FirstName,
                LastName = memberVM.LastName
            };
            _context.Add(member);

            Rental rental = new Rental
            {
                MemberID = member.ID,
                BookID = memberVM.BookID,
                RentalDate = DateTime.Now
            };
            _context.Add(rental);

            await _context.SaveChangesAsync();

            return member;
        }

        public async Task<int> UpdateMemberAsync(int id, MemberViewModel memberVM)
        {
            var memberToUpdate = await _context.Members
                .Include(m => m.Rentals)
                    .ThenInclude(m => m.Book)
                .SingleOrDefaultAsync(m => m.ID == id);

            memberToUpdate.FirstName = memberVM.FirstName;
            memberToUpdate.LastName = memberVM.LastName;

            _context.Update(memberToUpdate);
            return await _context.SaveChangesAsync();
        }
    }
}
