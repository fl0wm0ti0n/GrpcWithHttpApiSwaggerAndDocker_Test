using System;
using System.Threading.Tasks;
using Grpc.Core;
using Grpc.Net.Client;
using GrpcClient_Test.Protos;

namespace GrpcClient_Test
{
    class Program
    {
        static async Task Main(string[] args)
        {
            // Customer Client
            var channel = GrpcChannel.ForAddress("https://localhost:55004");
            var customerClient = new Customer.CustomerClient(channel);

            // Specify userID to request
            var clientRequested = new CustomerLookupRequest { UserId = 2 };
            CustomerGrpcModel customer = null;

            //try till server is online (Docker is starting slowly - regardless if debugging order is setup..)
            bool trye = false;
            while (!trye)
            {
                System.Threading.Thread.Sleep(20000);
                try
                {
                    //reply = await client.SayHelloAsync(input, new CallOptions().WithWaitForReady(true));

                    // Get specified Customer
                    customer = await customerClient.GetCustomerInfoAsync(clientRequested);
                    trye = true;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    trye = false;
                }
            }

            // Get specified customer
            Console.WriteLine();
            Console.WriteLine($"Customer with id {clientRequested.UserId}");
            Console.WriteLine();
            // writeout specified Customer
            Console.WriteLine($"{ customer.FirstName } { customer.LastName }");

            // Get stream of new customers
            Console.WriteLine();
            Console.WriteLine("New Customer List:");
            Console.WriteLine();
            using (var call = customerClient.GetNewCustomers(new NewCustomerRequest()))
            {
                while (await call.ResponseStream.MoveNext())
                {
                    var currentCustomer = call.ResponseStream.Current;
                    Console.WriteLine($"{ currentCustomer.FirstName } { currentCustomer.LastName } { currentCustomer.EmailAddress } { currentCustomer.Age } ");
                }
            }

            // Get Repeated List of customers
            Console.WriteLine();
            Console.WriteLine("repeated Customer List:");
            Console.WriteLine();

            var customerList = await customerClient.ListNewCustomersAsync(new ListCustomerRequest());

            foreach (var currentCustomer in customerList.Customers)
            {
                Console.WriteLine($"{currentCustomer.FirstName} {currentCustomer.LastName} {currentCustomer.EmailAddress} {currentCustomer.Age} ");
            }

            // Get Repeated List of customers
            Console.WriteLine();
            Console.WriteLine("Create Customer:");
            Console.WriteLine();

            var clientCreateRequest = new CreateCustomerRequest
            {
                Customer = new CustomerGrpcModel
                {
                    FirstName = "Heinz",
                    LastName = "Müller",
                    EmailAddress = "Heinz.Mueller@test.at",
                    Age = 6,
                    IsAlive = true
                }
            };
            var newcustomerReply = await customerClient.CreateCustomerAsync(clientCreateRequest);
            Console.WriteLine($"{newcustomerReply.FirstName} {newcustomerReply.LastName} {newcustomerReply.EmailAddress} {newcustomerReply.Age} ");

            Console.ReadLine();
        }
    }
}