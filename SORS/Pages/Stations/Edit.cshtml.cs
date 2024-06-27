using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SORS.Data;
using SORS.Data.Models;
using SORS.Pages.Stations;

namespace SORS.Pages.Stations
{
    public class EditModel : PageModel
    {
        private readonly SORS.Data.ApplicationDbContext _context;

        public EditModel(SORS.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Station Station { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.Stations == null)
            {
                return NotFound();
            }

            var station = await _context.Stations
                .Include(s => s.AlertEmails) // Zahrnout spojené emaily
                .FirstOrDefaultAsync(m => m.StationID == id);

            if (station == null)
            {
                return NotFound();
            }

            Station = station;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(Station).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!StationExists(Station.StationID))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            var existingAlertEmails = _context.AlertEmails.Where(e => e.StationID == Station.StationID);
            _context.AlertEmails.RemoveRange(existingAlertEmails);

            var alertEmails = Request.Form["alertEmail"];
            foreach (var email in alertEmails)
            {
                if (!string.IsNullOrEmpty(email))
                {
                    if (!CreateModel.IsValidEmail(email))
                    {
                        ModelState.AddModelError("AlertEmail", "Invalid email address format.");
                        return Page();
                    }
                    var alertEmail = new AlertEmail
                    {
                        Email = email,
                        StationID = Station.StationID
                    };
                    _context.AlertEmails.Add(alertEmail);
                }
            }
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }

        private bool StationExists(int id)
        {
            return _context.Stations.Any(e => e.StationID == id);
        }
    }
}
