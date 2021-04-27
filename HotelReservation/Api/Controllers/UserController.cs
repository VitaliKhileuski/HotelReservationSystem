using HotelReservation.Data.Repositories;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HotelReservation.Data;
using HotelReservation.Data.Entities;

namespace HotelReservation.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : Controller
    {
        private readonly Context db;
        private UserRepository userRepository;
        public UserController(Context context)
        {
            this.db = context;
            userRepository = new UserRepository(db);
        }
        [HttpGet]
        public IEnumerable<string> Get()
        {
            var users = userRepository.GetAll();
            return new string[] {"value1", "value2"};
        }
    }
}
