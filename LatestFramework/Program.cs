using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LatestFramework
{
    class Program
    {
        static void Main(string[] args)
        {
            const string data = "Hello";

            var json = JsonConvert.SerializeObject(data);

        }
    }
}
