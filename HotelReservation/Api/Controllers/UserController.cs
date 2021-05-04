using HotelReservation.Data.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Business.Models;
using Business.Services;
using HotelReservation.Data;
using HotelReservation.Data.Entities;

namespace HotelReservation.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : Controller
    {
        private readonly UsersService _usersService;
        public UserController(UsersService usersService)
        {
            _usersService = usersService;
        }
        [HttpGet]
        public   IEnumerable<UserModel> Get()
        {
            return _usersService.GetAll();
        }

        [HttpGet]
        [Route("{id:int}")]
        public UserModel GetById(int id)
        {
           return _usersService.GetById(id);
        }
        [HttpPost]
        public void Post([FromBody] UserEntity user)
        {
            
        }

        [HttpDelete]
        [Route("{id:int}")]
        public void DeleteUser(int id)
        {
            _usersService.DeleteById(id);
        }


    }
}