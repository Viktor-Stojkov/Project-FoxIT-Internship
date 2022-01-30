using AutoMapper;
using ProjectMVC_FoxIT.Models;
using ProjectMVC_FoxIT.Models.VIewModel;

namespace ProjectMVC_FoxIT.Mappers
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<WorkOrder, WorkOrdersViewModel>();
            CreateMap<Project, ProjectViewModel>();
            CreateMap<Customer, CustomerViewModel>();
        }
    }
}
