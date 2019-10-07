using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TestTaskNETCore.Models;


namespace TestTaskNETCore.Controllers
{
    public class HomeController : Controller
    {
        private readonly LinkContext db;

        public HomeController(LinkContext context)
        {
            db = context;
        }

        public string GenerateRandomShortUrl()
        {
            string cutUrl = "";
            int j;
            Random random = new Random();

            for (int i = 0; i < 8; i++)
            {
                j = random.Next(0, 35);
                if (j < 10)
                    j += 48;
                else
                    j += 87;

                if (j % 2 == 0)
                    cutUrl += char.ConvertFromUtf32(j).ToUpper();
                else
                    cutUrl += char.ConvertFromUtf32(j);

            }
            return "http://test.co/" + cutUrl;
        }

        public async Task<IActionResult> Index()
        {
            return View(await db.Links.ToListAsync());
        }

        [HttpGet]
        public IActionResult CreateLink()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateLink(string longURL)
        {
            Link link = new Link()
            {
                LongURL = longURL,
                DateTime = DateTime.UtcNow,
                RedirectCount = 0,
            };
            if (HasProtocol(longURL))
            {
                link.ShortURL = GenerateRandomShortUrl();
                db.Links.Add(link);
                await db.SaveChangesAsync();
            }
            else
            {
                return RedirectToAction("CreateLink");
            }
            return RedirectToAction("Index");

        }

        public bool HasProtocol(string checkUrl)
        {
            if (checkUrl.Length > 5)
            {
                if (checkUrl.StartsWith("http://") || checkUrl.StartsWith("https://"))
                    return true;
                else
                    return false;
            }
            else
                return false;
        }

        public async Task<IActionResult> RedirectToLong(string shortURL)
        {
            Link link = await db.Links.FirstOrDefaultAsync(u => u.ShortURL == shortURL);

            link.RedirectCount++;
            await db.SaveChangesAsync();
            return Redirect(link.LongURL);
        }
        
        public async Task<IActionResult> Delete(int? id)
        {
            if (id != null)
            {
                Link link = await db.Links.FirstOrDefaultAsync(p => p.LinkId == id);
                if (link != null)
                {
                    db.Links.Remove(link);
                    await db.SaveChangesAsync();
                    return RedirectToAction("Index");
                }
            }
            return NotFound();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}