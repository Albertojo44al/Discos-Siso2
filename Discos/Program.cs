using System;
using Discos;
namespace PlanificacionDisco
{
    public static class Program
    {
        static void Main(string[] args)
        {
            Random rnd = new Random();
            int tracks = rnd.Next(4, 10);
            int head = rnd.Next(1, tracks);
            Console.WriteLine("***** DISCO *****");
            Console.WriteLine("Número de pistas: " + tracks);
            Console.WriteLine("Posición del cabezal: " + head);
            Console.WriteLine("Algoritmos:");
            Console.WriteLine("1. FIFO\n2. SSTF\n3. SCAN");
            Console.WriteLine("Seleccione un algortimo: ");
            int plan = Convert.ToInt32(Console.ReadLine());
            Disc disco = new Disc(tracks, plan, head);
            disco.IniciarHilos();
            
            Console.WriteLine("**** SALIENDO ****");
        }
    }

}
