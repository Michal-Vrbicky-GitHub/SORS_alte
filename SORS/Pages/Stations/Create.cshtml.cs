using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using SORS.Data;
using SORS.Data.Models;

namespace SORS.Pages.Stations
{
    //[Authorize]
    public class CreateModel : PageModel
	{
		private readonly SORS.Data.ApplicationDbContext _context;
		public List<string> BadAlertEmails { get; set; } = new List<string>();//if a non-valid email is posted, the emails are loaded here so they can be preserved on the page
		public CreateModel(SORS.Data.ApplicationDbContext context)
		{
			_context = context;
		}
		/*
        public IActionResult OnGet()
        {
            return Page();
        }*/
        public void OnGet()
		{
			Station = new Station(); 
			LoadPreviousEmails();
		}

		[BindProperty]
		public Station Station { get; set; } = default!;
		

		// To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
		public async Task<IActionResult> OnPostAsync()
		{
		  if (!ModelState.IsValid || _context.Stations == null || Station == null)
			{
				return Page();
			}
            if (Station.LvlMax <= Station.LvlMin){
				string errMsg = "LvlMax must be greater than LvlMin.";
                ModelState.AddModelError(nameof(Station.LvlMax), errMsg);
                ModelState.AddModelError(nameof(Station.LvlMin), errMsg);
                return Page();
            }
            if (Station.AlertDelay <= 0) { 	
                ModelState.AddModelError(nameof(Station.AlertDelay), "AlertDelay must be a positive number.");
                return Page();
            }

            var alertEmails = Request.Form["alertEmail"];
            List<string> invalidEmails = new List<string>();
            foreach (var email in alertEmails)
            {
                if (!string.IsNullOrEmpty(email) && !IsValidEmail(email))
                {
                    invalidEmails.Add(email);
                }
            }

            if (invalidEmails.Count > 0)
            {
                foreach (var email in invalidEmails)
                {
                    BadAlertEmails.Add(email);
                }
                ModelState.AddModelError("AlertEmail", "One or more email addresses are invalid.");
                return Page();
            }
            _context.Stations.Add(Station);
			await _context.SaveChangesAsync();

            // emails
            foreach (var email in alertEmails)
            {
                if (!string.IsNullOrEmpty(email))
                {
					if (!IsValidEmail(email))
					{
						ModelState.AddModelError("AlertEmail", "Invalid email address format.");
						foreach (var emailToPage in alertEmails){
							BadAlertEmails.Add(emailToPage);
						}
                        return Page();
                        return RedirectToPage("./Create?mails="/**/);
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
		public static bool IsValidEmail(string email)
		{
			try
			{
				var addr = new System.Net.Mail.MailAddress(email);
				return addr.Address == email;
			}
			catch
			{
				return false;
			}
		}

		private void LoadPreviousEmails(){

		}
	}
}
