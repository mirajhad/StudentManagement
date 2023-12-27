﻿using StudentManagement.Data.Entities;
using StudentManagement.Data.UnitOfWork;
using StudentManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentManagement.BLL.Services
{
    public class AccountService : IAccountService
    {
        private IUnitOfWork _unitOfWork;

        public AccountService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public bool AddTeacher(UserViewModel vm)
        {
            try
            {
                Users model = new Users
                {
                    Name = vm.Name,
                    UserName = vm.UserName,
                    Password = vm.Password,
                    Role = (int)EnumRoles.Teacher
                };
                _unitOfWork.GenericRepository<Users>().Add(model);
                _unitOfWork.Save();
            }
            catch (Exception)
            {

                throw;
            }
            return true;
        }

        public PagedResult<TeacherViewModel> GetAllTeacher(int pageNumber, int PageSize)
        {
            try
            {
                int excludeRecords = (PageSize * pageNumber) - PageSize;
                List<TeacherViewModel> teacherViewModel = new List<TeacherViewModel>();

                var usersList = _unitOfWork.GenericRepository<Users>()
                    .GetAll().Where(x => x.Role == (int)EnumRoles.Teacher)
                    .Skip(excludeRecords).Take(PageSize).ToList();

                teacherViewModel = ListInfo(usersList);
                var result = new PagedResult<TeacherViewModel>
                {
                    Data = teacherViewModel,
                    TotalItems = _unitOfWork.GenericRepository<Users>()
                    .GetAll().Where(x => x.Role == (int)EnumRoles.Teacher).Count(),
                    PageNumber = pageNumber,
                    PageSize = PageSize
                };
                return result;
            }
            catch (Exception)
            {

                throw;
            }
        }

        private List<TeacherViewModel> ListInfo(List<Users> usersList)
        {
           return usersList.Select(x => new TeacherViewModel(x)).ToList();
        }

        public LoginViewModel Login(LoginViewModel loginViewModel)
        {

            var user = _unitOfWork.GenericRepository<Users>()
               .GetAll()
               .FirstOrDefault(a => a.UserName == loginViewModel.UserName.Trim()
               && a.Password == loginViewModel.Password
               && a.Role == loginViewModel.Role);
            if (user != null)
            {
                loginViewModel.Id = user.Id;
                return loginViewModel;
            }
            return null;

        }
    }
}
