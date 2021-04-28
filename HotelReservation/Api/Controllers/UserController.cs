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
        private readonly Context db;
        private readonly UserRepository userRepository;
        public UserController(Context context)
        {
            this.db = context;
            userRepository = new UserRepository(db);
        }
        [HttpGet]
        public IEnumerable<UserEntity> Get()
        {
            return userRepository.GetAll();
        }

        [HttpGet]
        [Route("{id:int}")]
        public UserEntity Get(int id)
        {
            return userRepository.Get(id);
        }
        [HttpPost]
        public void Post([FromBody] UserEntity user)
        {
            userRepository.Create(user);
        }

        [HttpDelete]
        [Route("{id:int}")]
        public void DeleteUser(int id)
        {
            userRepository.Delete(id);
            db.SaveChanges();
        }


    }
}
