﻿using FluentValidation;
using MediatR;
using SalesService.Application.Abstracts;
using SalesService.Domain.Entities;
using Shared.Abstracts;
using Shared.Exceptions;

namespace SalesService.Application.Queries.Services
{
    public record GetServicesByIdQuery(string ServiceId) : IRequest<Service>, IHaveCompanyId
    {
        public string CompanyId { get; set; } = string.Empty;
    }

    public class GetServicesByIdQueryValidator : AbstractValidator<GetServicesByIdQuery>
    {
        public GetServicesByIdQueryValidator()
        {
            RuleFor(x => x.ServiceId)
                .NotNull()
                .NotEmpty()
                .WithMessage($"{nameof(Service.Id)} is required.");

            RuleFor(x => x.CompanyId)
                .NotNull()
                .NotEmpty()
                .WithMessage($"{nameof(Service.CompanyId)} is required.");
        }
    }

    public class GetServicesByIdQueryHandler : IRequestHandler<GetServicesByIdQuery, Service>
    {
        private readonly IServiceRepository _serviceRepository;

        public GetServicesByIdQueryHandler(IServiceRepository serviceRepository)
        {
            _serviceRepository = serviceRepository;
        }

        public async Task<Service> Handle(GetServicesByIdQuery request, CancellationToken cancellationToken)
        {
            var service = await _serviceRepository.GetServiceByIdAsync(request.ServiceId);
            if(service == null)
                throw new NotFoundException($"Services with id: {request.ServiceId} not found.");

            if (service.CompanyId != request.CompanyId)
                throw new ForbiddenException("You are not able to get product from company where you are not registered in");

            return service;
        }
    }
}
