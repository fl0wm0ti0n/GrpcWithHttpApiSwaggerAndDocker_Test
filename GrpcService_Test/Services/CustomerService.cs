using System;
using System.Collections.Generic;
using System.Linq;
using Grpc.Core;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using GrpcService_Test.Models;
using GrpcService_Test.Protos;

namespace GrpcService_Test.Services
{
    public class CustomerService : Customer.CustomerBase
    {
        private readonly ILogger<CustomerService> _logger;

        List<CustomerModel> _savedCustomers = new List<CustomerModel>();

        public CustomerService(ILogger<CustomerService> logger)
        {
            _logger = logger;
            _savedCustomers.Add(new CustomerModel { FirstName = "Tim", LastName = "Benz", Id = 1, age = 50});
            _savedCustomers.Add(new CustomerModel { FirstName = "Sue", LastName = "Storm", Id = 2, age = 50 });
            _savedCustomers.Add(new CustomerModel { FirstName = "Flo", LastName = "Gabriel", Id = 3, age = 20 });
            _savedCustomers.Add(new CustomerModel { FirstName = "Jan", LastName = "Gabriel", Id = 4, age = 34});
        }


        public override Task<CustomerGrpcModel> GetCustomerInfo(CustomerLookupRequest request, ServerCallContext context)
        {
            Console.WriteLine("Called GetCustomerInfo");
            CustomerGrpcModel output = new CustomerGrpcModel();

            if (request.UserId == 1)
            {
                output.FirstName = "Jamie";
                output.LastName = "Smith";
            }
            else if (request.UserId == 2)
            {
                output.FirstName = "Jane";
                output.LastName = "Doe";
            }
            else
            {
                output.FirstName = "´Greg";
                output.LastName = "Thomas";
            }
            return Task.FromResult(output);
        }
        public override async Task GetNewCustomers(NewCustomerRequest request, IServerStreamWriter<CustomerGrpcModel> responseStream, ServerCallContext context)
        {
            Console.WriteLine("Called GetNewCustomers");
            List<CustomerGrpcModel> customers = new List<CustomerGrpcModel>
            {
                new CustomerGrpcModel
                {
                    FirstName = "Jamie",
                    LastName = "Smith",
                    EmailAddress = "Jamie@Smith.com",
                    Age = 14,
                    IsAlive = true
                },
                new CustomerGrpcModel
                {
                    FirstName = "Jane",
                    LastName = "Doe",
                    EmailAddress = "Jane@Doe.com",
                    Age = 28,
                    IsAlive = true
                },
                new CustomerGrpcModel
                {
                    FirstName = "Greg",
                    LastName = "Thomas",
                    EmailAddress = "Greg@Thomas.com",
                    Age = 119,
                    IsAlive = false
                }
            };

            foreach (var cust in customers)
            {
                await Task.Delay(1000);
                await responseStream.WriteAsync(cust);
            }
        }

        public override Task<ListCustomerReply> ListNewCustomers(ListCustomerRequest request, ServerCallContext context)
        {
            Console.WriteLine("Called ListNewCustomers");
            ListCustomerReply customers = new ListCustomerReply();
            customers.Customers.Add(new CustomerGrpcModel
            {
                FirstName = "Jamie",
                LastName = "Smith",
                EmailAddress = "Jamie@Smith.com",
                Age = 14,
                IsAlive = true
            });
            customers.Customers.Add(new CustomerGrpcModel
            {
                FirstName = "Jane",
                LastName = "Doe",
                EmailAddress = "Jane@Doe.com",
                Age = 28,
                IsAlive = true
            });
            customers.Customers.Add(new CustomerGrpcModel
            {
                FirstName = "Greg",
                LastName = "Thomas",
                EmailAddress = "Greg@Thomas.com",
                Age = 119,
                IsAlive = false
            });

            return Task.FromResult(customers);
        }

        public override Task<CustomerGrpcModel> CreateCustomer(CreateCustomerRequest request, ServerCallContext context)
        {
            Console.WriteLine("Called CreateCustomer");
            var customerModel = new CustomerModel();
            // map Customer
            customerModel.FirstName = request.Customer.FirstName;
            customerModel.LastName = request.Customer.LastName;
            customerModel.EmailAddress = request.Customer.EmailAddress;
            customerModel.age = request.Customer.Age;
            customerModel.isAlive = request.Customer.IsAlive;

            return Task.FromResult(request.Customer);
        }

        public override Task<ListCustomerNamesReply> GetCustomerNames(CustomerLookupNameRequest request, ServerCallContext context)
        {
            Console.WriteLine("Called GetCustomerNames");
            ListCustomerNamesReply names = new ListCustomerNamesReply();

            foreach (var savedC in _savedCustomers)
            {
                if (request.Age == savedC.age)
                {
                    names.CustomerNames.Add(savedC.FirstName);
                }
            }

            return Task.FromResult(names);
        }
    }
}