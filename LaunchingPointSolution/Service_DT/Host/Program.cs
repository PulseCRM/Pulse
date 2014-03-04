using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;

namespace LP2Service
{
    class Program
    {
        static void Main(string[] args)
        {
            using (ServiceHost host = new ServiceHost(typeof(LP2Service)))
            {
                host.Open();
                Console.WriteLine("WCF Service has been started at the following URLs:");
                foreach (Uri ea in host.BaseAddresses)
                {
                    Console.WriteLine(ea.ToString());
                }
                Console.ReadLine();
                host.Close();
            }
        }
    }
}
