﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class BuildingController : Controller
    {
        ApplicationDbContext dbContext = new ApplicationDbContext();
        // GET: Building
        public ActionResult Index()
        {
            var userId = this.User.Identity.GetUserId();
            var user = dbContext.Users.Find(userId);
            var city = user.Cities.First();
            return View(city);
        }
        public ActionResult Details(int buildingId)
        {
            var userId = this.User.Identity.GetUserId();
            var user = dbContext.Users.Find(userId);
            var city = user.Cities.First();
            var building = dbContext.Buildings.Find(buildingId);
            return View(building);
        }
        public ActionResult Build(int buildingId)
        {
           
            return View(new BuildViewModel
            {
                BuildingId = buildingId,
                BuildingTypes = this.dbContext.BuildingTypes
                .Select(b=> new SelectListItem
                {
                    Value = b.BuildingTypeId.ToString(),
                    Text = b.Name
                })
            }
                );
        }
       
    }
    public class BuildViewModel
    {
        public int BuildingId { get; set; }
        public IEnumerable<SelectListItem> BuildingTypes { get; set; }
        public int? SelectedBuildingType { get; set; }

    }
}