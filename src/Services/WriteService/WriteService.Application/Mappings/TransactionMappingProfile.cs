using AutoMapper;
using Shared.DTOs;
using Shared.Entities;
using WriteService.Application.Commands.Transactions;

namespace WriteService.Application.Mappings
{
    public class TransactionMappingProfile : Profile
    {
        public TransactionMappingProfile()
        {
            CreateMap<CreateTransactionCommand, Transaction>()
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore());

            CreateMap<UpdateTransactionCommand, Transaction>()
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore());

            CreateMap<Transaction, TransactionDto>();

            CreateMap<TransactionDto, Transaction>()
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore());
        }
    }
}
