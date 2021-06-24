using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using GrpcApiGw_Test.Models;
using GrpcApiGw_Test.Protos;

namespace GrpcApiGw_Test.Contracts
{
    public interface ICustomerApiClient : IApiClientBase
    {
        Task<Uri> CreateCustomerAsync(CustomerModel customer);
        Task<CustomerModel> GetCustomerAsync();
        Task<CustomerModel> UpdateCustomerAsync(CustomerModel customer);
        Task<HttpStatusCode> DeleteCustomerAsync(string id);
        Task<CustomerModel> GetCustomerInfo(CustomerLookupRequest customerRequest);
        Task<CustomerModel> CreateCustomer(CreateCustomerRequest customer);
        Task<CustomerModel> ListNewCustomers();
        Task<CustomerModel> GetCustomerNames(CustomerLookupNameRequest age);
    }
}
