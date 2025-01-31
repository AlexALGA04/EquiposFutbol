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
        public static Dictionary<string, (int, string[])> LigaGlobal = new Dictionary<string, (int, string[])>()
        /*{
            { "uno", (1, new string[] { "a", "b", "c" }) },
            { "dos", (2, new string[] { "d", "e", "f" }) },
            { "tres", (3, new string[] { "g", "h", "i" }) }
        }*/;

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
                        MostrarEnPantalla();
                        break;
                    case 2:
                        CrearEquipo();
                        break;
                    case 3:
                        ModificarEquipo();
                        break;
                    case 0:
                        return;
                    default:
                        break;
                }
            }
        }

        static void MostrarEnPantalla()
        {
            Console.WriteLine("----------");
            foreach (var c in LigaGlobal)
            {
                Console.WriteLine($"Equipo: {c.Key}");
                Console.WriteLine($"Puntuación: {c.Value.Item1}");
                Console.Write("Jugadores: ");
                foreach (string jugador in c.Value.Item2)
                {
                    Console.Write($"{jugador}, ");
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
            int puntuacion;
            int numeroDeJugadores;
            int indice = 0;
            string equipo;
            string casillaVacia;
            string[] jugadores;

            Console.Clear();
            Console.WriteLine("========================");
            Console.WriteLine("===== Crear Equipo =====");
            Console.WriteLine("========================");
            Console.Write("Dime el nombre del equipo: \n> ");

            equipo = Console.ReadLine();

            puntuacion = PedirNumero("Dime la puntuacion global que tiene: \n> ");

            numeroDeJugadores = PedirNumero("Dime cuantos jugadores tiene tu equipo: \n> ");

            jugadores = new string[numeroDeJugadores];

            Console.WriteLine("Escribelos uno por uno: ");

            while(indice < numeroDeJugadores)
            {
                Console.Write("> ");
                casillaVacia = Console.ReadLine();
                if (!String.IsNullOrWhiteSpace(casillaVacia))
                {
                    jugadores[indice] = casillaVacia;
                    indice++;
                }
                else
                    Console.WriteLine("No sirven Casillas en blanco");
            }

            LigaGlobal.Add(equipo, (puntuacion, jugadores));

            using (StreamWriter writer = new StreamWriter(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Test.json")))
            {
                foreach (var item in LigaGlobal)
                {

                    writer.Write($"{item.Key},{item.Value.Item1},");

                    for(int i = 0; i < item.Value.Item2.Length; i++) 
                    {
                        writer.Write(item.Value.Item2[i] + ((i == item.Value.Item2.Length - 1)? "" : ",") );
                    }
                    writer.WriteLine();
                }
            }
            //Console.WriteLine("El archivo ha sido escrito en: " + Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Test.json"));
        }

        static void ModificarEquipo()
        {
            foreach (var item in LigaGlobal)
            {

            }
        }

        static void CargarInfo()
        {
            int indice = 0;

            LigaGlobal.Clear();

            using (StreamReader reader = new StreamReader(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Test.json")))
            {
                //uno, 1, a, b, c
                string texto;
                
                string[] separadas;

                while ((texto = reader.ReadLine()) != null)
                {
                    separadas = texto.Split(',');
                    // el primero es el nombre del equipo, el segundo es la puntuacion y a partir de ahi cada jugador
                    LigaGlobal.Add(separadas[0], (int.Parse(separadas[1]), separadas.Skip(2).ToArray()));

                    indice++;
                }
                if (indice <= 0)
                    Console.WriteLine("No Existe ningun equipo, ves a crearlos");
            }
        }
    }
}
