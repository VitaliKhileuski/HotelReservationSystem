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
        public IEnumerable<RoleEntity> Get()
        {
            return roleRepository.GetAll();
        }
        [HttpGet]
        [Route("{id:int}")]
        public RoleEntity Get(int id)
        {
            return roleRepository.Get(id);
        }

        [HttpPost]
        public void Post([FromBody] RoleEntity role)
        {
            roleRepository.Create(role);
            db.SaveChanges();
        }
        [HttpDelete]
        [Route("{id:int}")]
        public void RemoveRole(int id)
        {
            roleRepository.Delete(id);
            db.SaveChanges();
        }
    }
}
