using AutoMapper;
using SalesService.Application.Commands.Products;
using SalesService.Domain.Entities;

namespace SalesService.Application.Mappings
{
    public class ProductMappingProfile : Profile
    {
        public ProductMappingProfile()
        {
            CreateMap<CreateProductCommand, Product>()
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore());

            CreateMap<UpdateProductCommand, Product>()
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore());
        }
    }
}