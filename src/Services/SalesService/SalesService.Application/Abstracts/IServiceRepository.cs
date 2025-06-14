﻿using SalesService.Domain.Entities;

namespace SalesService.Application.Abstracts
{
    public interface IServiceRepository
    {
        Task AddServiceAsync(Service service);
        Task UpdateServiceAsync(Service service);
        Task DeleteServiceAsync(Service service);
        Task DeleteManyServicesByCompanyIdAsync(string companyId);
        Task<Service?> GetServiceByIdAsync(string id);
        IQueryable<Service> GetQueryable();
        Task<List<Service>> GetFilteredServicesAsync(IQueryable<Service> query, CancellationToken cancellationToken);
    }
}
