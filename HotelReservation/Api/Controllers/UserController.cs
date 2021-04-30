using HotelReservation.Data.Repositories;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HotelReservation.Data;
using HotelReservation.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace HotelReservation.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : Controller
    {
        private readonly Context _db;
        private readonly UserRepository _userRepository;
        public UserController(Context context)
        {
            _db = context;
            _userRepository = new UserRepository(_db);
        }
        [HttpGet]
        public IEnumerable<UserEntity> Get()
        {
            return _userRepository.GetAll();
        }

        [HttpGet]
        [Route("{id:int}")]
        public UserEntity Get(int id)
        {
            return _userRepository.Get(id);
        }
        [HttpPost]
        public void Post([FromBody] UserEntity user)
        {
            _userRepository.Create(user);
        }

        [HttpDelete]
        [Route("{id:int}")]
        public void DeleteUser(int id)
        {
            _userRepository.Delete(id);
            _db.SaveChanges();
        }


    }
}
