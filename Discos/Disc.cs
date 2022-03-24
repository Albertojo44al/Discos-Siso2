using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Discos
{
    internal class Disc
    {
        int num_tracks { get; set; }
        public int head { get; set; }
        Random rand = new Random();
        readonly object candado = new object();
        int Plan { get; set; }
        ConcurrentQueue<int> requestQueue = new ConcurrentQueue<int>();
        List<int> planificationQueue = new List<int>();
        int[] auxQueue;


        public Disc(int tracks, int plan, int cab)
        {
            num_tracks = tracks;
            Plan = plan;
            head = cab;
        }

        public void IniciarHilos()
        {
            Thread thread = new Thread(new ThreadStart(FillRequestQueue));
            thread.Start();

            Thread threadPlan = new Thread(new ThreadStart(PlanQueue));
            threadPlan.Start();

            thread.Join();
            threadPlan.Join();
        }

        public void Scan()
        {
            List<int> cola = new List<int>();
            int i = 0;

            planificationQueue.Add(head);
            planificationQueue.Add(num_tracks);
            planificationQueue.Add(0);
            planificationQueue.Sort();

            int tam = planificationQueue.Count();

            for (i = 0; i < tam; i++)
            {
                if (head == planificationQueue[i])
                {
                    break;
                }
            }
            int k = i;
            if (k < num_tracks / 2)
            {
                for (i = k; i < tam; i++)
                {
                    cola.Add(planificationQueue[i]);
                }
                for (i = k - 1; i >= 0; i--)
                {
                    cola.Add(planificationQueue[i]);
                }
            }
            else
            {
                for (i = k; i >= 0; i--)
                {
                    cola.Add(planificationQueue[i]);
                }
                for (i = k + 1; i < tam; i++)
                {
                    cola.Add(planificationQueue[i]);
                }
            }
 

        }

        public void SSTF()
        {
            int aux;
            int tam = planificationQueue.Count;
            auxQueue = new int[tam];
            fill();

            for (int i = 0; i < tam; i++)
            {
                for (int j = i + 1; j < tam; j++)
                {
                    if (auxQueue[i] > auxQueue[j])
                    {
                        aux = auxQueue[i];
                        auxQueue[i] = auxQueue[j];
                        auxQueue[j] = aux;

                        aux = planificationQueue[i];
                        planificationQueue[i] = planificationQueue[j];
                        planificationQueue[j] = aux;
                    }
                }
            }
        }


        public void FillRequestQueue()
        {
            int seconds = 0;
            while (seconds < 60)
            {
                int random = rand.Next(1, num_tracks + 1);
                lock (candado)
                {
                    requestQueue.Enqueue(random);
                    printSoli();
                }
                Thread.Sleep(1000);
                seconds++;
            }
        }
        public void PlanQueue()
        {
            int seconds = 0;
            switch (Plan)
            {
                case 1:
                    while (seconds < 60)
                    {
                        lock (candado)
                        {
                            Console.Write("FIFO: ");
                            planificationQueue = requestQueue.ToList();
                            printPlan();
                        }
                        Thread.Sleep(1000);
                        seconds++;
                    }
                    break;
                case 2:
                    while (seconds < 60)
                    {
                        lock (candado)
                        {
                            Console.Write("SSTF: ");
                            planificationQueue = requestQueue.ToList();
                            SSTF();
                            printPlan();
                        }
                        Thread.Sleep(1000);
                        seconds++;
                    }
                    break;
                case 3:
                    while (seconds < 60)
                    {
                        lock (candado)
                        {
                            Console.Write("SCAN: ");
                            planificationQueue = requestQueue.ToList();
                            Scan();
                            printPlan();
                        }
                        Thread.Sleep(1000);
                        seconds++;
                    }
                    break;
                default:
                    break;
            }
        }
        

        private void printSoli()
        {
            Console.Write("Solicitudes: ");
            Console.Write("{ ");
            foreach (var elem in requestQueue)
            {
                Console.Write(elem + " ");
            }
            Console.WriteLine("}");
        }

        private void printPlan()
        {
            Console.Write("{ ");
            foreach (var elem in planificationQueue)
            {
                Console.Write(elem + " ");
            }
            Console.WriteLine("}");
        }

        private void fill()
        {
            int tam = planificationQueue.Count;
            for (int i = 0; i < tam; i++)
            {
                var check = Math.Abs(head - planificationQueue[i]);
                auxQueue[i] = check;
            }
        }
    }
}
