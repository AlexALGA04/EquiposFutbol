using System;
using System.Collections.Generic;
using System.Linq;
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
            IniciarMenu();
        }
        static void IniciarMenu()
        {
            int opcion = PedirNumero("---MENÚ---\nCargar equipo (1)\nCrear equipo (2)\nModificar equipo (3)\nElige una opción: ");
            switch(opcion)
            {
                case 1:
                    CargarEquipo();
                    break;
                case 2:
                    break;
                case 3:
                    break;
                default:
                    break;
            }
            Console.ReadKey();
        }
        static void CargarEquipo()
        {
            foreach(var c in LigaGlobal)
            {
                Console.WriteLine($"Equipo: ");
                Console.WriteLine(c.Value.Item1);
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
    }
}
