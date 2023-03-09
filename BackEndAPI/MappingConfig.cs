using AutoMapper;
using BackEndAPI.Model;
using BackEndAPI.Model.BO;

namespace BackEndAPI
{
    public class MappingConfig:Profile
    {
        public MappingConfig()
        {
            CreateMap<Customer,CustomerBO>().ReverseMap();
        }
    }
}
