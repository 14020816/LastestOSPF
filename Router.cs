using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LastestVersionOSPF_OK
{
  
    class Metric
    {
        public int ID;
        public int metric;
        public Metric(int id, int _metric)
        {
            ID = id;
            metric = _metric;
        }
    }
    class Router
    {
        public int ID { get; set; }
        public List<Metric> myLSA = new List<Metric>(); 
        public int[,] LSDB {get; set;}

        public List<Router> ListConnected = new List<Router>();
        public List<Router> Neighbour = new List<Router>();

        // Kiem tra xem co ket noi voi mot router khong
        public bool CheckConected (Router Destination)
        {
            for(int i = 0 ; i < ListConnected.Count ; i++ )
            {
                if (ListConnected[i].ID == Destination.ID)
                    return true;
            }
            return false;
        }

        public void NewConected(Router newR, int Metric)
        {
            ListConnected.Add(newR);
            Metric tmp = new Metric(newR.ID, Metric);
            myLSA.Add(tmp);
        }

        public bool CheckNeigbour(Router Destination)
        {
            for (int i = 0; i < Neighbour.Count; i++)
            {
                if (Neighbour[i].ID == Destination.ID)
                    return true;
            }
            return false;
        }
        void UpdateLSA()
        {
            foreach(Router t in Neighbour)
            {
                Metric tmp = new Metric(0,0);
                tmp.ID = t.ID;
                tmp.metric = 4;
            }
        }

        void UpdateLSDB(Router Destinarion, int NumberNode)
        {
            for(int i = 0 ; i < NumberNode ; i++)
            {
                for(int j = i ; j < NumberNode ; j++)
                {
                    LSDB[Destinarion.ID, Destinarion.myLSA[i].ID] = Destinarion.myLSA[i].metric;
                }
            }
            
        }
        // Gui Hello toi mot router khac
        public void SendHello(ref int Time, Router Destination)
        {
            
                Console.WriteLine("Router {0} send hello packet to broadcass adress at time {1}ms\n", this.ID, Time);
                if (CheckConected(Destination) && !CheckNeigbour(Destination))
                {
                    Random mt = new Random();
                    Console.WriteLine("Router 192.168.{0}.0 discovered new neighbour ID = 192.168.{1}.0\n", this.ID, Destination.ID);
                    this.Neighbour.Add(Destination);
                    Destination.Neighbour.Add(this);
                    Time++;
                    // myLSA.Add(new Metric(Destination.ID, 3 * mt.Next(5)));
                }
                else if (CheckNeigbour(Destination) && !CheckConected(Destination))
                {
                    Console.WriteLine("Neighbour 192.168.{0}.0 not response , either it down or the link has some problem. Send LSA to other neighbour to advertise them somethings has changed\n");
                    int tmp = Neighbour.FindIndex(0, c => c.ID == Destination.ID);
                    Neighbour.RemoveAt(tmp);

                }
                else if (!CheckConected(Destination) && !CheckNeigbour(Destination))
                {
                    Console.WriteLine("Time out\n");
                }
           
        }

        public void SendLSA(ref int Time, Router Destination)
        {
            Console.WriteLine("Router 192.168.{1}.0 send LSA to 192.168.{0}.0 at time {2}ms\n", Destination.ID, this.ID,Time);
            Destination.UpdateLSDB(this, 6);
            
        }

        public void SPF()
        {

        }
        
    }

    enum EventType
    {
        SendHello, SendLSA , UpdateLSA , UpdateLSDB, SFP, RouteTable
    };
    class Event
    {
        public int ID { get; set; }

        public int IDDestination { get; set; }
        public int Time { get; set; }
        public int Type { get; set; }
    }
}
