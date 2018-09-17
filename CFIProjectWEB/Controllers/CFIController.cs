using CFIProjectWEB.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CFIProjectWEB.Controllers
{
    public class CFIController : Controller
    {
        // GET: CFIValidSubject
        public ActionResult Index()
        {
            var list = DBRepository.DefaultDBRepository.GetValidSubjects();
            ViewBag.ValidSubjectList = new SelectList(list,"Name","Name");
            return View();
        }

        [HttpPost]
        public ActionResult MainPage()
        {
            string name = Request.Form["ValidSubjects"];
            string sortingBy = Request.Form["SortingBY"];
            string filteringBy = Request.Form["FilteringBY"];
            string filteringValue = Request.Form["FilteringValue"];

            if(String.IsNullOrEmpty(name))
            {
                name = this.TempData["ValidSubjects"] as string;
            }
            CFIValidSubject sb = new CFIValidSubject { Name = name };
            var list = DBRepository.DefaultDBRepository.GetDetails(sb);

            //Deal with filtering data
            if(!String.IsNullOrEmpty(filteringBy) && !String.IsNullOrEmpty(filteringValue))
            {
                switch (filteringBy)
                {
                    case "Campus":
                        list = list.Where(x => x.Campus.Contains(filteringValue)).ToList();
                        break;
                    case "Lecturer":
                        list = list.Where(x => x.Lecturer.Contains(filteringValue)).ToList();
                        break;
                }
            }

            //Deal with sorting data
            if(!String.IsNullOrEmpty(sortingBy))
            {
                switch (sortingBy)
                {
                    case "Campus":
                        list = list.OrderBy(x => x.Campus).ToList();
                        break;
                    case "Lecturer":
                        list = list.OrderBy(x => x.Lecturer).ToList();
                        break;
                    case "Room":
                        list = list.OrderBy(x => x.Room).ToList();
                        break;
                }
            }

            this.TempData["ValidSubjects"] = name;
            ViewBag.Details = list;
            return View();
        }
    }
}