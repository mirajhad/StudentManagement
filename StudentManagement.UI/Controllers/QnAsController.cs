﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using StudentManagement.BLL.Services;
using StudentManagement.Models;

namespace StudentManagement.UI.Controllers
{
    public class QnAsController : Controller
    {
        private IExamService _examService;
        private IQnAsService _iqnAsService;

        public QnAsController(IExamService examService, IQnAsService iqnAsService)
        {
            _examService = examService;
            _iqnAsService = iqnAsService;
        }

        public IActionResult Index(int pageNumber = 1, int pageSize = 10)
        {
            var QnAs = _iqnAsService.GetAll(pageNumber, pageSize);
            return View(QnAs);
        }
        [HttpGet]
        public IActionResult Create()
        {
            var exams = _examService.GetAllExams();
            ViewBag.examsList = new SelectList(exams, "Id", "Title");
            return View();
        }

        [HttpPost]
        public IActionResult Create(CreateQnAsViewModel viewModel)
        {
            _iqnAsService.AddQnAs(viewModel);
            return RedirectToAction("Index");
        }
    }
}
