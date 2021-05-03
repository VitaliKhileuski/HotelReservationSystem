using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using HotelReservation.Data;
using HotelReservation.Data.Entities;
using HotelReservation.Data.Repositories;

namespace HotelReservation.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RoleController : Controller
    {
        private readonly Context _db;
        private readonly RoleRepository _roleRepository;
        public RoleController(Context context)
        {
            this._db = context;
            _roleRepository = new RoleRepository(_db);
        }
        [HttpGet]
        public IEnumerable<RoleEntity> Get()
        {
            return _roleRepository.GetAll();
        }
        [HttpGet]
        [Route("{id:int}")]
        public RoleEntity Get(int id)
        {
            return _roleRepository.Get(id);
        }

        [HttpPost]
        public void Post([FromBody] RoleEntity role)
        {
            _roleRepository.Create(role);
            _db.SaveChanges();
        }
        [HttpDelete]
        [Route("{id:int}")]
        public void RemoveRole(int id)
        {
            _roleRepository.Delete(id);
            _db.SaveChanges();
        }
    }
}