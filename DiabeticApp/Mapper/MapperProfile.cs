using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using DiabeticApp.ApiInfrastructure.Models;
using DiabeticApp.Models;

namespace DiabeticApp.Mapper
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            this.CreateMap<LoginApiModel, LoginViewModel>()
                .ForMember(x => x.Username, o => o.MapFrom(s => s.Username))
                .ForMember(x => x.Password, o => o.MapFrom(s => s.Password));
        }
    }
}
