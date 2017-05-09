using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Game.Mvc.Models;
using Microsoft.Ajax.Utilities;
using Microsoft.AspNet.Identity;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    [Authorize]
    public class MinesController : Controller
    {
        ApplicationDbContext dbContext = new ApplicationDbContext();
        // GET: Mines
        public ActionResult Index()
        {
            var userId = this.User.Identity.GetUserId();
            var user = dbContext.Users.Find(userId);
            var city = user.Cities.First();
            this.UpdteResources(city);
            return View(city);
        }
        public ActionResult Details(int mineId)
        {
            var userId = this.User.Identity.GetUserId();
            var user = dbContext.Users.Find(userId);
            var city = user.Cities.First();
            
            var mine = dbContext.Mines.Find(mineId);
            return View(mine);
        }
        private void UpdteResources(City city)
        {
            var start = DateTime.Now;
            foreach (var res in city.Resources)
            {
                foreach (var mine in city.Mines)
                {
                    if (mine.Type == res.Type)
                    {
                        res.Value += mine.GetProductionPerHour() * (start - res.LastUpdate).TotalHours;
                    }
                }
                res.LastUpdate = start;
            }
            dbContext.SaveChanges();

        }
        [HttpPost]
        public ActionResult Upgrade(int mineId, bool fastUpgrade)
        {
            var mine = this.dbContext.Mines.Find(mineId);
            var city = mine.City;
            var needed = mine.GetUpgradeRequirments();
            var have = city.Resources;
            if(fastUpgrade)
            {
                needed = needed.Select(n => (amount: n.amount * 10, type: n.type))
                    .ToArray();
            }
            var r = needed
                .Join(have, n => n.type, h => h.Type, (n, h) => (needed: n, have: h));
            if(!r.All(_=>_.needed.amount <= _.have.Value))
            {
                return View(new MessageViewModel
                {
                    Message = $"You do not have enough resources"
                });
            }
            foreach(var item in r)
            {
                item.have.Value -= item.needed.amount;
            }
            mine.Level++;
            mine.UpgradeCompleteAt = DateTime.Now.AddHours(0.5 * mine.Level);
            this.dbContext.SaveChanges();
            return View(new MessageViewModel
            {
                Message = $"Mine id = {mineId} {fastUpgrade}"
            }
            );
        }
    }
}