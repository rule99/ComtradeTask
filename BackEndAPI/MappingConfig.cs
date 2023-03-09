using AutoMapper;
using BackEndAPI.Model;
using BackEndAPI.Model.BO;
using CustomerSoapService;

namespace BackEndAPI
{
    public class MappingConfig:Profile
    {
        public MappingConfig()
        {
            CreateMap<Customer,CustomerBO>().ReverseMap();

            CreateMap<CustomerBO,PersonIdentification>().ReverseMap();

            CreateMap<Customer, PersonIdentification>().ReverseMap();

            CreateMap<CustomerBO, Person>().ReverseMap();

            CreateMap<Customer, Person>().ReverseMap();

        }
    }
}
