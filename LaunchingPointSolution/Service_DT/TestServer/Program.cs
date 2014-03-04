using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.ServiceModel.Description;

using LP2.Service;

namespace TestService
{
    class Program
    {
        static void Main(string[] args)
        {
            ServiceHost sh = new ServiceHost(typeof(LP_Service));
            
            sh.Open();
            Console.WriteLine("Ready and waiting at:");

            foreach (ServiceEndpoint endpoint in sh.Description.Endpoints)
            {
                Console.WriteLine("\t" + endpoint.Address.Uri);
            }
            Console.ReadLine();

            sh.Close();

        }
    }
}
