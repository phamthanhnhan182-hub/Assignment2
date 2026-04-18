using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Assignment2_Exer3_Final.Data;

namespace Assignment2_Exer3_Final.Pages.Orders
{
    public class IndexModel : PageModel
    {
        private readonly Assignment2_Exer3_Final.Data.Assignment2_Exer3_FinalContext _context;

        public IndexModel(Assignment2_Exer3_Final.Data.Assignment2_Exer3_FinalContext context)
        {
            _context = context;
        }

        public IList<Order> Order { get;set; } = default!;

        public async Task OnGetAsync()
        {
            Order = await _context.Order.ToListAsync();
        }
    }
}
