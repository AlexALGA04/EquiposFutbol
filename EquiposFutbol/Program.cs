using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace EquiposFutbol
{
    internal class Program
    {
        public static Dictionary<string, (int, string[])> LigaGlobal = new Dictionary<string, (int, string[])>{
            { "uno", (1, new string[] { "a", "b", "c" }) },
            { "dos", (2, new string[] { "d", "e", "f" }) },
            { "tres", (3, new string[] { "g", "h", "i" }) }
        };

        static void Main(string[] args)
        {
            CargarInfo();
            IniciarMenu();
        }
        static void IniciarMenu()
        {
            int opcion;
            while (true)
            {
                opcion = PedirNumero("----MENÚ----\nCargar equipo (1)\nCrear equipo (2)\nModificar equipo (3)\nSalir (0)\nElige una opción: ");
                
                switch (opcion)
                {
                    case 1:
                        CargarEquipo();
                        break;
                    case 2:
                        CrearEquipo();
                        break;
                    case 3:
                        break;
                    case 0:
                        return;
                    default:
                        break;
                }
            }
        }
        static void CargarInfo()
        {

        }
        static void CargarEquipo()
        {
            Console.WriteLine("----------");
            foreach (var c in LigaGlobal)
            {
                Console.WriteLine($"Equipo: {c.Key}");
                Console.WriteLine($"Puntuación: {c.Value.Item1}");
                Console.Write("Jugadores: ");
                foreach (string jugador in c.Value.Item2)
                {
                    Console.Write($"{jugador}   ");
                }
                Console.WriteLine("\n----------");
            }
        }
        static int PedirNumero(string mensaje)
        {
            Console.Write(mensaje);
            string numeroString = Console.ReadLine();
            bool canInt;
            int numero;
            do
            {
                canInt = int.TryParse(numeroString, out numero);
                if (!canInt)
                {
                    Console.WriteLine("Has escrito un formato erroneo, vuelvelo a intentar");
                    Console.Write(mensaje);
                    numeroString = Console.ReadLine();
                }
            } while (!canInt);
            return numero;
        }

        static void CrearEquipo()
        {

            using (StreamWriter writer = new StreamWriter(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Test.txt")))
            {
                foreach (var item in LigaGlobal)
                {
                    writer.Write(item.Key);
                    writer.Write(item.Value.Item1);
                    foreach (var c in item.Value.Item2)
                    {
                        writer.Write(c);

                    }
                    writer.WriteLine();
                }
            }
            Console.WriteLine("El archivo ha sido escrito en: " + Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Test.txt"));
        }
    }
}
