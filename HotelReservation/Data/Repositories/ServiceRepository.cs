using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HotelReservation.Data.Entities;
using HotelReservation.Data.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HotelReservation.Data.Repositories
{
    public class ServiceRepository : IRepository<ServiceEntity>, IRepositoryAsync<ServiceEntity>
    {
        private readonly Context _db;
        public ServiceRepository(Context context)
        {
            _db = context;
        }
        public void Create(ServiceEntity service)
        {
            _db.Services.Add(service);
            _db.SaveChanges();
        }

        public async Task CreateAsync(ServiceEntity service)
        {
            await _db.Services.AddAsync(service);
            await _db.SaveChangesAsync();
        }

        public void Delete(int id)
        {
            ServiceEntity service = _db.Services.Find(id);
            if (service != null)
            {
                _db.Services.Remove(service);
            }
            _db.SaveChanges();
        }

        public async Task DeleteAsync(int id)
        {
            var service = await _db.Services.FindAsync(id);

            if (service != null)
            {
                _db.Services.Remove(service);
            }

            await _db.SaveChangesAsync();
        }

        public IEnumerable<ServiceEntity> Find(Func<ServiceEntity, bool> predicate)
        {
            return _db.Services.Where(predicate).ToList();
        }

        public ServiceEntity Get(int id)
        {
            return _db.Services.Find(id);
        }

        public IEnumerable<ServiceEntity> GetAll()
        {
            return _db.Services;
        }

        public async Task<ServiceEntity> GetAsync(int id)
        {
            return await _db.Services.FirstOrDefaultAsync(x => x.Id == id);
        }

        public void Update(ServiceEntity service)
        {
            _db.Entry(service).State = EntityState.Modified;
            _db.SaveChanges();
        }
    }
}
