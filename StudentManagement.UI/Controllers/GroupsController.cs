﻿using Microsoft.AspNetCore.Mvc;
using StudentManagement.BLL.Services;
using StudentManagement.Models;

namespace StudentManagement.UI.Controllers
{
    public class GroupsController : Controller
    {
        private IGroupService _groupService;
        private IStudentService _studentService;

        public GroupsController(IGroupService groupService, IStudentService studentService)
        {
            _groupService = groupService;
            _studentService = studentService;
        }

        public IActionResult Index(int pageNumber, int pageSize=10)
        {
            return View(_groupService.GetAll(pageNumber, pageSize));
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(GroupViewModel viewModel)
        {
          var vm = _groupService.AddGroup(viewModel);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Details(int id) 
        {
            GroupStudentViewModel vm = new GroupStudentViewModel();
            var group = _groupService.GetGroup(id);
            var students = _studentService.GetAll();
            vm.GroupId = group.Id;
            foreach(var student in students)
            {
                vm.StudentList.Add(new CheckBoxTable
                {
                    Id = student.Id,
                    Name = student.Name,
                    IsChecked = false
                });
            }
            return View(vm);
        }
    }
}
