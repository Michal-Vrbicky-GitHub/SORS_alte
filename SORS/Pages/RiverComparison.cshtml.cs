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
    public class RiverComparisonModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public RiverComparisonModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public List<string> StationNames { get; set; } = new List<string>();
        public List<int> MinValues { get; set; } = new List<int>();
        public List<int> MaxValues { get; set; } = new List<int>();
        public List<int> LatestValues { get; set; } = new List<int>();

        public async Task OnGetAsync()
        {
            var stations = await _context.Stations.ToListAsync();

            foreach (var station in stations)
            {
                var reports = await _context.Report
                    .Where(r => r.StationId == station.StationID)
                    .ToListAsync();

                if (reports.Any())
                {
                    var minReport = reports.Min(r => r.Value);
                    var maxReport = reports.Max(r => r.Value);
                    var latestReport = reports.OrderByDescending(r => r.TimeStamp).First().Value;

                    StationNames.Add(station.Name ?? station.StationID.ToString());
                    MinValues.Add(minReport);
                    MaxValues.Add(maxReport);
                    LatestValues.Add(latestReport);
                }
            }
        }

        public async Task<IActionResult> OnGetJsonAsync()
        {
            var stations = await _context.Stations.ToListAsync();
            var stationNames = new List<string>();
            var minValues = new List<int>();
            var maxValues = new List<int>();
            var latestValues = new List<int>();

            foreach (var station in stations)
            {
                var reports = await _context.Report
                    .Where(r => r.StationId == station.StationID)
                    .ToListAsync();

                if (reports.Any())
                {
                    var minReport = reports.Min(r => r.Value);
                    var maxReport = reports.Max(r => r.Value);
                    var latestReport = reports.OrderByDescending(r => r.TimeStamp).First().Value;

                    stationNames.Add(station.Name ?? station.StationID.ToString());
                    minValues.Add(minReport);
                    maxValues.Add(maxReport);
                    latestValues.Add(latestReport);
                }
            }

            return new JsonResult(new { stationNames, minValues, latestValues, maxValues });
        }
    }
}
