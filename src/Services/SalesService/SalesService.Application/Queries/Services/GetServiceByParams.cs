using FluentValidation;
using MediatR;
using SalesService.Application.Abstracts;
using SalesService.Application.Commands.Abstracts;
using SalesService.Domain.Entities;

namespace SalesService.Application.Queries.Services
{
    public record GetServicesByParamsQuery(string? Id, string? Name, string? Category, decimal? MinPrice, decimal? MaxPrice, int? MinDuration, int? MaxDuration) 
        : IRequest<IEnumerable<Service>>, IHaveCompanyId
    {
        public string CompanyId { get; set; } = string.Empty;
    }

    public class GetServicesByParamsQueryValidator : AbstractValidator<GetServicesByParamsQuery>
    {
        public GetServicesByParamsQueryValidator()
        {
            RuleFor(x => x.Name)
                .Must(Validator.BeValidString)
                .WithMessage($"{nameof(Service.Name)} can only contain letters and numbers")
                .When(x => !string.IsNullOrEmpty(x.Name));

            RuleFor(x => x.Category)
                .Must(Validator.BeValidString)
                .WithMessage($"{nameof(Service.Category)} can only contain letters and numbers")
                .When(x => !string.IsNullOrEmpty(x.Category));

            RuleFor(x => x.MinPrice)
                .GreaterThanOrEqualTo(0)
                .WithMessage("Min price must be greater or equal to 0.")
                .When(x => x.MinPrice != null);

            RuleFor(x => x.MaxPrice)
                .GreaterThan(0)
                .WithMessage("Max price must be greater than 0.")
                .Must((query, maxPrice) => maxPrice >= query.MinPrice)
                .WithMessage("Max price must be greater than or equal to Min price.")
                .When(x => x.MaxPrice != null);

            RuleFor(x => x.MinDuration)
                .GreaterThanOrEqualTo(0)
                .WithMessage("Min duration must be greater or equal to 0.")
                .When(x => x.MinDuration != null);

            RuleFor(x => x.MaxDuration)
                .GreaterThanOrEqualTo(0)
                .WithMessage("Min duration must be greater or equal to 0.")
                .Must((query, maxDuration) => maxDuration >= query.MinPrice)
                .WithMessage("Max duration must be greater than or equal to Min duration.")
                .When(x => x.MaxDuration != null);

            RuleFor(x => x.CompanyId)
                .NotNull()
                .NotEmpty()
                .WithMessage($"{nameof(Service.CompanyId)} is required.")
                .Length(17)
                .WithMessage($"{nameof(Service.CompanyId)} must be exactly 17 characters long.");
        }
    }

    public class GetServicesByParamsQueryHandler : IRequestHandler<GetServicesByParamsQuery, IEnumerable<Service>>
    {
        private readonly IServiceRepository _serviceRepository;

        public GetServicesByParamsQueryHandler(IServiceRepository serviceRepository)
        {
            _serviceRepository = serviceRepository;
        }

        public async Task<IEnumerable<Service>> Handle(GetServicesByParamsQuery request, CancellationToken cancellationToken)
        {
            var query = _serviceRepository.GetQueryable();

            // IMPORTANT STATEMENT (User can only get services from declared company)
            query = query.Where(s => s.CompanyId == request.CompanyId);

            if (!string.IsNullOrEmpty(request.Id))
                query = query.Where(s => s.Id == request.Id);

            if (!string.IsNullOrEmpty(request.Name))
                query = query.Where(s => s.Name.Contains(request.Name));

            if (!string.IsNullOrEmpty(request.Category))
                query = query.Where(s => s.Category.Contains(request.Category));

            if (request.MinPrice.HasValue)
                query = query.Where(s => s.Price >= request.MinPrice.Value);

            if (request.MaxPrice.HasValue)
                query = query.Where(s => s.Price <= request.MaxPrice.Value);

            if (request.MinDuration.HasValue)
                query = query.Where(s => s.Duration <= request.MinDuration.Value);

            if (request.MaxDuration.HasValue)
                query = query.Where(s => s.Duration <= request.MaxDuration.Value);

            var services = await _serviceRepository.GetFilteredServicesAsync(query, cancellationToken);

            return services;
        }
    }
}
