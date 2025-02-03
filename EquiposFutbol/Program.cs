using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace EquiposFutbol
{
    internal class Program
    {
        public static Dictionary<string, (int, string[])> LigaGlobal = new Dictionary<string, (int, string[])>()

        //Ejemplo estructura de diccionario, la llave es el nombre del equipo, el numero como primer valor son los puntos del equipo, y la array de strings como
        //segundo valor son los jugadores
        /*{
            { "uno", (1, new string[] { "a", "b", "c" }) },
            { "dos", (2, new string[] { "d", "e", "f" }) },
            { "tres", (3, new string[] { "g", "h", "i" }) }
        }*/;

        static void Main(string[] args)
        {
            //Empieza el programa cargando en el diccionario lo que se encuentra en el archivo y luego inicia el menú del programa.
            CargarInfo();
            IniciarMenu();
        }
        static void IniciarMenu()
        {
            //Pregunta al usuario que quiere hacer, según lo que eliga, llamara a las funciones correspondientes de su eleccion
            int opcion;
            while (true)
            {
                opcion = PedirNumero("----MENÚ----\nCargar equipo (1)\nCrear equipo (2)\nBorrar equipo (3)\nModificar equipo (4)\nSalir (0)\nElige una opción: ");
                
                switch (opcion)
                {
                    case 1:
                        MostrarEnPantalla();
                        break;
                    case 2:
                        CrearEquipo();
                        break;
                    case 3:
                        BorrarEquipo();
                        break;
                    case 4:
                        ModificarEquipo();
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
            int indice = 0;

            //Declaración de la ruta del archivo
            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Test.json");

            LigaGlobal.Clear();

            //Si no existe lo crea
            if(!File.Exists(path))
            {
                File.WriteAllText(path, "");
            }

            //Iniciar el archivo para empezar a leerlo
            using (StreamReader reader = new StreamReader(path))
            {
                string texto;
                string[] separadas;

                string equipo;
                int puntuacion;
                List<string> jugadores = new List<string>();

                //Bucle que recorre todas las líneas que tenga el archivo, porque cada línea es un equipo diferente
                while ((texto = reader.ReadLine()) != null)
                {
                    //Separa todos los datos por comas
                    separadas = texto.Split(',');

                    equipo = separadas[0];
                    puntuacion = int.Parse(separadas[1]);
                    jugadores = separadas.Skip(2).ToList();

                    // el primero es el nombre del equipo, el segundo es la puntuacion y a partir de ahi cada jugador
                    LigaGlobal.Add(equipo, (puntuacion, jugadores.ToArray()));

                    indice++;
                }
                //Si no hay ningún equipo, el programa se lo dice al usuario
                if (indice <= 0)
                    Console.WriteLine("No Existe ningun equipo, ves a crearlos");
            }
        }

        static void MostrarEnPantalla()
        {
            int indice = 0;
            //Recorre todas las instancias (equipos) del diccionario y las imprime con formato.
            Console.WriteLine("----------");
            foreach (var c in LigaGlobal)
            {
                Console.WriteLine($"Equipo: {c.Key}");
                Console.WriteLine($"Puntuación: {c.Value.Item1}");
                Console.Write("Jugadores: ");
                foreach (string jugador in c.Value.Item2)
                {
                    Console.Write($"{jugador}"+ ((indice == c.Value.Item2.Length - 1) ? "" : ", "));
                    indice++;
                }
                Console.WriteLine("\n----------");
            }
        }

        static void CrearEquipo()
        {
            int puntuacion;
            int numeroDeJugadores;
            int indice = 0;
            string equipo;
            string nombre;
            string[] jugadores;

            Console.Clear();
            Console.WriteLine("========================");
            Console.WriteLine("===== Crear Equipo =====");
            Console.WriteLine("========================");
            equipo = PedirTextoSinComas("Dime el nombre del equipo: ");

            //Si el equipo ya existe no se creara y pedira el nombre de nuevo
            while(LigaGlobal.ContainsKey(equipo)) {
                Console.WriteLine("Este equipo ya existe, vuelve a probar");
                equipo = PedirTextoSinComas("Dime el nombre del equipo: \n");
            }

            puntuacion = PedirNumero("Dime la puntuacion global que tiene: \n> ");

            numeroDeJugadores = PedirNumero("Dime cuantos jugadores tiene tu equipo: \n> ");

            jugadores = new string[numeroDeJugadores];

            Console.WriteLine("Escribelos uno por uno: ");

            //Bucle que va pidiendo los jugadores y los guarda en una array
            while(indice < numeroDeJugadores)
            {
                nombre = PedirTextoSinComas("> ");
                if (!String.IsNullOrWhiteSpace(nombre))
                {
                    jugadores[indice] = nombre;
                    indice++;
                }
                else
                    Console.WriteLine("No sirven Casillas en blanco");
            }

            //Se crea la nueva instanacia del equipo con sus valores y se añade al archivo de texto.
            LigaGlobal.Add(equipo, (puntuacion, jugadores));

            SobreecribirArchivo();
        }

        static void BorrarEquipo()
        {
            string equipo;

            Console.Clear();
            Console.WriteLine("========================");
            Console.WriteLine("=== Borrar Equipo ===");
            Console.WriteLine("========================");
            Console.WriteLine("Equipos actuales: ");

            //Comprueba si hay algún equipo que puedas borrar
            if (LigaGlobal.Count > 0)
            {

                //Imprime los equipos disponibles
                foreach (var item in LigaGlobal)
                {
                    Console.Write("> " + item.Key + "\n");
                }

                Console.Write("Elige uno a eliminar: ");
                equipo = Console.ReadLine();

                //Si el usuario elige un equipo que no existe, vuelve a pedir al usuario que escriba bien el nombre del equipo
                while (!LigaGlobal.ContainsKey(equipo))
                {
                    Console.WriteLine("El equipo que has elegido no existe, vuelve a probar");
                    Console.Write("Elige uno a eliminar: ");
                    equipo = Console.ReadLine();
                }

                //Elimina el equipo del diccionario y de el archivo de texto
                LigaGlobal.Remove(equipo);
                SobreecribirArchivo();
            }
            else
                Console.WriteLine("No puedes borrar equipos porque no hay");
        }

        static void ModificarEquipo()
        {
            string equipo;

            Console.Clear();
            Console.WriteLine("========================");
            Console.WriteLine("=== Modificar Equipo ===");
            Console.WriteLine("========================");
            Console.WriteLine("Equipos actuales: ");

            //Comprueba hay equipos para modificar
            if (LigaGlobal.Count > 0)
            {

                //Te muestra los equipos que hay
                foreach (var item in LigaGlobal)
                {
                    Console.Write("> " + item.Key + "\n");
                }

                Console.Write("Elige uno a modificar: ");
                equipo = Console.ReadLine();
                while (!LigaGlobal.ContainsKey(equipo))
                {
                    Console.WriteLine("El equipo que has elegido no existe, vuelve a probar");
                    Console.Write("Elige uno a modificar: ");
                    equipo = Console.ReadLine();
                }

                //Le da elegir al usuario si quiere modificar los puntos o los jugadores del equipo
                Console.WriteLine($"¿Que quieres modificar de {equipo}?\nPuntos (1)\nJugadores (2)");
                int opcion = PedirNumero("Elige: ");
                switch(opcion)
                {
                    case 1:
                        ModificarPuntos(equipo);
                        break;
                    case 2:
                        ModificarJugadores(equipo);
                        break;
                    default:
                        Console.WriteLine("Esa opción no existe");
                        break;
                }
            }
            else
                Console.WriteLine("No puedes modificar equipos porque no existe ninguno");
        }
        static void ModificarPuntos(string equipo)
        {
            //Le pide al usuario los puntos generales nuevos del equipo y los guarda tanto en el diccionario como en el archivo de texto
            int puntos = PedirNumero("Dime la nueva puntuación global del equipo: ");
            LigaGlobal[equipo] = (puntos, LigaGlobal[equipo].Item2);
            SobreecribirArchivo();
        }
        static void ModificarJugadores(string equipo)
        {
            //Pregunta si quiere eliminar o añadir jugadores.
            int opcion = PedirNumero("¿Que quieres hacer con los jugadores?\nEliminar jugador (1)\nAñadir jugadores (2)\nElige: ");
            switch(opcion)
            {
                case 1:
                    EliminarJugador(equipo);
                    break;
                case 2:
                    AñadirJugadores(equipo);
                    break;
                default:
                    Console.WriteLine("Esa opción no existe");
                    break;
            }
        }
        static void EliminarJugador(string equipo)
        {
            Console.WriteLine($"Lista de jugadores de {equipo}");
            List<string> jugadores = LigaGlobal[equipo].Item2.ToList();
            int puntuacion = LigaGlobal[equipo].Item1;

            //Comprueba si hay jugadores que eliminar en el equipo
            if (jugadores.Count() != 0)
            {

                //Imprime los jugadores que hay
                foreach (var item in jugadores)
                {
                    Console.WriteLine($"> {item}");
                }

                Console.Write("Elige un jugador: ");
                string jugador = Console.ReadLine();

                //Si el jugador no existe lo vuelve a pedir al usuario
                while (!jugadores.Contains(jugador))
                {
                    Console.WriteLine("Ese jugador no existe, vuelve a intentarlo");
                    Console.Write("Elige un jugador: ");
                    jugador = Console.ReadLine();
                }

                //Elimina de la array de jugadores el jugador seleccionado por el usuario, se lo asigna al diccionario y al archivo de texto.
                jugadores.Remove(jugador);
                LigaGlobal[equipo] =  (puntuacion, jugadores.ToArray());
                SobreecribirArchivo();
            }
            else
                Console.WriteLine("No puedes eliminar jugadores, porque no los hay");
        }
        static void AñadirJugadores(string equipo)
        {
            List<string> jugadores = LigaGlobal[equipo].Item2.ToList();
            int puntuacion = LigaGlobal[equipo].Item1;
            int numJugadores = PedirNumero("¿Cuantos jugadores nuevos quieres añadir?: ");
            string jugador;

            //Le va preguntando el nombre de tantos jugadores como haya indicado el usuario
            for(int i = 0; i < numJugadores; i++)
            {
                jugador = PedirTextoSinComas("Nombre de jugador: ");
                jugadores.Add(jugador);
            }

            //Añade los nuevos jugadores en el diccionario 
            LigaGlobal[equipo] = (puntuacion, jugadores.ToArray());
            SobreecribirArchivo();
        }
        static void SobreecribirArchivo()
        {
            //Declaración de la ruta del archivo
            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Test.json");
            string equipo;
            int puntuacion;
            List<string> jugadores = new List<string>();

            //Si no existe el archivo lo crea
            if (!File.Exists(path))
            {
                File.WriteAllText(path, "");
            }

            //Inicia la escritura del archivo
            using (StreamWriter writer = new StreamWriter(path))
            {

                //Bucle que recorre el diccionario y va escribiendo todos los equipos en diferentes lineas con sus valores separados por comas.
                foreach (var item in LigaGlobal)
                {
                    equipo = item.Key;
                    puntuacion = item.Value.Item1;
                    jugadores = item.Value.Item2.ToList();

                    writer.Write($"{equipo},{puntuacion},");

                    for (int i = 0; i < jugadores.Count; i++)
                    {
                        writer.Write(jugadores[i] + ((i == item.Value.Item2.Length - 1) ? "" : ","));
                    }
                    writer.WriteLine();
                }
            }
        }
        static int PedirNumero(string mensaje)
        {
            //Función que pide un número y hace todas las comprobaciones de que sea un número entero valido
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
            } 
            while (!canInt);
            return numero;
        }
        static string PedirTextoSinComas(string mensaje)
        {
            //Función que pide una string y hace las comprobaciones para que no contenga comas, las comas en los nombres podrian romper el programa.
            Console.Write(mensaje);
            string texto = Console.ReadLine();
            while (texto.Contains(','))
            {
                Console.WriteLine("No puede contener comas, ya que pueden romper el programa, vuelve a intentarlo");
                Console.Write(mensaje);
                texto = Console.ReadLine();
            }
            return texto;
        }
    }
}
