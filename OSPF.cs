using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LastestVersionOSPF_OK
{
    class OSPF
    {
        static int Time;
        public int TimeLimit { get; set; }
        public List<Router> Topo = new List<Router>();
        public  int Node { get; set; }
        public int[,] Graph { get; set; }

        List<Event> ListEvent = new List<Event>();
        public void Intizi()
        {
            //Console.WriteLine("Number Node ?");
            //Node = Console.Read();
            Node = 6;
            for(int i = 0 ; i < Node ; i++)
            {
                Router tmp = new Router();
                tmp.ID = i;
                Topo.Add(tmp);
            }

           int [,] Graph = {{0,7,15,3,0,1},{7,0,0,4,21,0},{15,0,0,25,0,0}, {2,4,25,0,8,0},{0,21,0,8,0,0}, {1,0,0,0,0,0}};
           this.Graph = Graph;
            for(int i = 0 ; i < Node ; i++)
            {
                for(int j = i ; j < Node ; j++)
                {
                    if(Graph[i,j] !=0 && i!= j)
                    {
                        Topo[i].NewConected(Topo[j], Graph[i, j]);
                        Topo[j].NewConected(Topo[i], Graph[i, j]);
                    }
                }
            }
        }

        public void InsertEvent(Event newE)
        {
            if(ListEvent.Count == 0)
            {
                ListEvent.Add(newE);
            }
            else if(ListEvent[ListEvent.Count - 1].Time <= newE.Time)
            {
                ListEvent.Add(newE);
            }
            else if(ListEvent[0].Time == newE.Time)
            {
                ListEvent.Insert(1, newE);
            }
            else if(ListEvent[0].Time > newE.Time)
            {
                ListEvent.Insert(0, newE);
            }
            else
            {
                for(int i = 0 ; i < ListEvent.Count ; i++)
                {
                    if (newE.Time >= ListEvent[i].Time && newE.Time <= ListEvent[i + 1].Time)
                    {
                        ListEvent.Insert(i, newE);
                        break;
                    }
                }
            }
        }
        public void TurnOn ()
        {
            for(int i = 0 ; i < Node ; i++)
            {
                Random rdn = new Random();
                int tmp;
                tmp = i;
                Console.WriteLine("Router 192.168.{0}.0 turn on at {1}ms ", Topo[i].ID,tmp );
                Event newEvent = new Event();
                newEvent.ID = Topo[i].ID;
                newEvent.Type = (int)EventType.SendHello;
                newEvent.Time = tmp;
                InsertEvent(newEvent);

            }
        }

        public void Run()
        {
            Intizi();
            TurnOn();
            while (ListEvent.Count > 0)
            {
                Event DoNow = new Event();
                DoNow = ListEvent[0];
                if(DoNow.Type == (int)EventType.SendHello)
                {
                   
                    for (int i = 0; i < Topo.Count; i++ )
                    {   
                       // Time = DoNow.Time;
                        int TimeNow = DoNow.Time;
                        Topo[DoNow.ID].SendHello(ref TimeNow, Topo[i]);
                        Event newEvent = new Event();
                        newEvent.Type = (int)EventType.SendLSA;
                        newEvent.ID = DoNow.ID;
                        newEvent.IDDestination = Topo[i].ID;
                        newEvent.Time = TimeNow;
                        InsertEvent(newEvent);

                    }
                ListEvent.RemoveAt(0);
                       

                }
                else if(DoNow.Type == (int)EventType.SendLSA)
                {
                    int TimeNow = DoNow.Time;
                    Topo[DoNow.ID].SendLSA(ref TimeNow, Topo[DoNow.IDDestination]);
                    ListEvent.RemoveAt(0);

                }
            }
        }
        
    }
}
