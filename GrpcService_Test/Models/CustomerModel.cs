using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GrpcService_Test.Models
{
    /// <summary>
    /// Represents one specific CustomerModel
    /// </summary>
    public class CustomerModel
    {
        private int _id;
        public CustomerModel()
        {
            _id++;
        }

        /// <summary>
        /// ID froM SQL
        /// </summary>
        public int Id { get; set; } = 0;

        /// <summary>
        /// The users first name
        /// </summary>
        public string FirstName { get; set; } = "";

        /// <summary>
        /// The users last name
        /// </summary>
        public string LastName { get; set; } = "";

        /// <summary>
        /// The Users Emailaddress
        /// </summary>
        public string EmailAddress { get; set; } = "";

        /// <summary>
        /// Users Status
        /// </summary>
        public bool isAlive { get; set; } = true;

        /// <summary>
        /// The USers Age
        /// </summary>
        public int age { get; set; } = 0;

        //}
    }
}
