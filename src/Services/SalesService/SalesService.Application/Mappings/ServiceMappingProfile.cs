using AutoMapper;
using SalesService.Application.Commands.Services;
using SalesService.Domain.Entities;

namespace SalesService.Application.Mappings
{
    public class ServiceMappingProfile : Profile
    {
        public ServiceMappingProfile()
        {
            CreateMap<CreateServiceCommand, Service>()
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore());

            CreateMap<UpdateServiceCommand, Service>()
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore());
        }
    }
}
