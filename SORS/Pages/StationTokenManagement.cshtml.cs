using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SORS.Data;
using SORS.Data.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SORS.Pages
{
    public class StationTokenManagementModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public StationTokenManagementModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<Station> Stations { get; set; } = new List<Station>();

        public async Task OnGetAsync()
        {
            Stations = await _context.Stations.ToListAsync();
        }

        public async Task<IActionResult> OnPostGenerateTokenAsync(int stationId)
        {
            var station = await _context.Stations.FindAsync(stationId);

            if (station == null)
            {
                return NotFound();
            }

            // Generate new token (replace with your logic)
            station.Token = GenerateNewToken();

            await _context.SaveChangesAsync();

            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostClearTokenAsync(int stationId)
        {
            var station = await _context.Stations.FindAsync(stationId);

            if (station == null)
            {
                return NotFound();
            }

            // Clear the token (set to empty string)
            station.Token = "";

            await _context.SaveChangesAsync();

            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostEditStationAsync(int stationId, string newName, int newLvlMin, int newLvlMax, int newAlertDelay)
        {
            var station = await _context.Stations.FindAsync(stationId);

            if (station == null)
            {
                return NotFound();
            }

            station.Name = newName;
            station.LvlMin = newLvlMin;
            station.LvlMax = newLvlMax;
            station.AlertDelay = newAlertDelay;

            await _context.SaveChangesAsync();

            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostEditTokenAsync(int stationId, string newToken)
        {
            var station = await _context.Stations.FindAsync(stationId);

            if (station == null)
            {
                return NotFound();
            }

            station.Token = newToken;

            await _context.SaveChangesAsync();

            return RedirectToPage();
        }

        private string GenerateNewToken()
        {
            // Implement your token generation logic here (e.g., generate a random string)
            return Guid.NewGuid().ToString();
        }
    }
}
