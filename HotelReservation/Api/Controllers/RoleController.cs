using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HotelReservation.Data;
using HotelReservation.Data.Entities;
using HotelReservation.Data.Repositories;

namespace HotelReservation.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RoleController : Controller
    {
        private readonly Context db;
        private RoleRepository roleRepository;
        public RoleController(Context context)
        {
            this.db = context;
            roleRepository = new RoleRepository(db);
        }
        [HttpGet]
        public IEnumerable<string> Get()
        {
            RoleEntity role = new RoleEntity()
            {
                Name = "Admin"
            };
            roleRepository.Create(role);
            db.SaveChanges();
            return new string[] { "value1", "value2" };
        }
    }
}
