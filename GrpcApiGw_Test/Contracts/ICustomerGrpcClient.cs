using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Grpc.Core;
using GrpcApiGw_Test.Models;
using GrpcApiGw_Test.Protos;

namespace GrpcApiGw_Test.Contracts
{
    public interface ICustomerGrpcClient
    {
        Task<CustomerGrpcModel> GetCustomerInfo(CustomerLookupRequest customerRequest);
        Task<bool> GetNewCustomers(IServerStreamWriter<CustomerGrpcModel> responseStream);
        Task<ListCustomerReply> ListNewCustomers();
        Task<CustomerGrpcModel> CreateCustomer(CreateCustomerRequest request);
        Task<ListCustomerNamesReply> GetCustomerNames(CustomerLookupNameRequest request);
    }
}
