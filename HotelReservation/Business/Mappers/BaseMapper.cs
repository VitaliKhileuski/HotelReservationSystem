using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using Business.Models;
using HotelReservation.Data.Entities;

namespace Business.Mappers
{
    public class UserModelsMapper<M, E>
    {
        private readonly Mapper _mapper;
        public UserModelsMapper()
        {
            _mapper = new Mapper(new MapperConfiguration(x =>
            {
                x.CreateMap<M, E>();
                x.CreateMap<E, M>();
            }));
        }

        public E FromModelToEntity(M model)
        {
            return _mapper.Map<M, E>(model);
        }

        public M FromEntityToModel(E item)
        {
            return _mapper.Map<E, M>(item);
        }

        public List<M> FromEntitiesToModels(List<E> entityItems)
        {
            return _mapper.Map<List<M>>(entityItems);
        }
    }
}
