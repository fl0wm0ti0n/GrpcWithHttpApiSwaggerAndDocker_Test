using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Grpc.Core;
using Grpc.Net.Client;
using GrpcApiGw_Test.Contracts;
using GrpcApiGw_Test.Models;
using GrpcApiGw_Test.Protos;
using GrpcApiGw_Test.Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace GrpcApiGw_Test.Controller
{

    public class CustomerController : ICustomerController
    {
        private readonly ICustomerApiClient _customerApiClient;
        private readonly ICustomerGrpcClient _customerGrpcClient;

        private readonly bool _isGrpc = false;
        public CustomerController(ICustomerApiClient customerApiClient, ICustomerGrpcClient customerGrpcClient/*, IServerStreamWriter<CustomerGrpcModel> responseStream*/)
        {
            _customerApiClient = customerApiClient;
            _customerGrpcClient = customerGrpcClient;
            //_responseStream = responseStream;
        }

        public CustomerGrpcModel GetCustomerInfo(CustomerLookupRequest request)
        {
            CustomerGrpcModel output = new CustomerGrpcModel();
            CustomerModel modeloutput = new CustomerModel();
            if (_isGrpc)
            {
                output = _customerGrpcClient.GetCustomerInfo(request).Result;

                return output;
            }

            modeloutput = _customerApiClient.GetCustomerInfo(request).Result;
            //TODO Automapper
            output.Age = modeloutput.Age;
            output.EmailAddress = modeloutput.EmailAddress;
            output.FirstName = modeloutput.FirstName;
            output.IsAlive = modeloutput.IsAlive;
            output.LastName = modeloutput.LastName;

            return output;
        }

        public async Task<bool> GetNewCustomers(IServerStreamWriter<CustomerGrpcModel> responseStream)
        { 
            return await _customerGrpcClient.GetNewCustomers(responseStream);
        }

        public ListCustomerReply ListNewCustomers(ListCustomerRequest request)
        {
            var listcutomers = _customerGrpcClient.ListNewCustomers();
            return listcutomers.Result;
        }

        public CustomerGrpcModel CreateCustomer(CreateCustomerRequest request)
        {
            var customer = _customerGrpcClient.CreateCustomer(request);
            return customer.Result;
        }

        public ListCustomerNamesReply GetCustomerNames(CustomerLookupNameRequest request)
        {
            var names = _customerGrpcClient.GetCustomerNames(request);
            return names.Result;
        }
    }
}