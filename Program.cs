using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LastestVersionOSPF_OK
{
    class Program
    {
        static void Main(string[] args)
        {
            OSPF myOSPF = new OSPF();
            myOSPF.Run();
            Console.Read();
        }
    }
}
