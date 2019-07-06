using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MyLibrary.Models;

namespace MyLibrary.Services
{
    public interface IMemberService
    {
        Task<Member> CreateMemberAsync(MemberViewModel memberVM);
        Task<int> UpdateMemberAsync(int id, MemberViewModel memberVM);
    }
}
