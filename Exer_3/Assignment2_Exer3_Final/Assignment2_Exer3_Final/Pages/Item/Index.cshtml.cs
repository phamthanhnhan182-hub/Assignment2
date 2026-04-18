using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Assignment2_Exer3_Final.Data;
using Assignment2_Exer3_Final.Models;

namespace Assignment2_Exer3_Final.Pages.Item
{
    public class IndexModel : PageModel
    {
        private readonly Assignment2_Exer3_Final.Data.Assignment2_Exer3_FinalContext _context;

        public IndexModel(Assignment2_Exer3_Final.Data.Assignment2_Exer3_FinalContext context)
        {
            _context = context;
        }

        // Thay dòng 22 cũ bằng dòng này:
        public IList<Assignment2_Exer3_Final.Models.Item> Item { get; set; } = default!;
        public async Task OnGetAsync()
        {
            Item = await _context.Item.ToListAsync();
        }
    }
}
