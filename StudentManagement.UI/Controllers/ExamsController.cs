﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using StudentManagement.BLL.Services;
using StudentManagement.Models;
using StudentManagement.UI.Filters;

namespace StudentManagement.UI.Controllers
{
    [RoleAuthorize(2)]
    public class ExamsController : Controller
    {
        private readonly IGroupService _groupService;
        private readonly IExamService _examService;
        public ExamsController(IGroupService groupService, IExamService examService)
        {
            _groupService = groupService;
            _examService = examService;
        }

        public IActionResult Index(int pageNumber=1, int pageSize=10)
        {
            return View(_examService.GetAll(pageNumber, pageSize));
        }

        [HttpGet]
        public IActionResult Create() 
        {
            var groups = _groupService.GetAllGroups();
            ViewBag.AllGroups = new SelectList(groups, "Id", "Name");
            return View();
        }

        [HttpPost]
        public IActionResult Create(CreateExamsViewModel vm)
        {
           _examService.AddExam(vm);
            return RedirectToAction("Index");
        }
    }
}
