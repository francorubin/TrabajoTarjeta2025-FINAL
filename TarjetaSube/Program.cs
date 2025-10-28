using System;

namespace TarjetaSube
{
    class Program
    {
        static void Main(string[] args)
        {
            Tarjeta miTarjeta = new Tarjeta();
            Colectivo colectivo = new Colectivo("K");
            bool salir = false;

            Console.WriteLine("=== Sistema de Transporte Urbano de Rosario ===\n");

            while (!salir)
            {
                Console.WriteLine("\n--- Menu ---");
                Console.WriteLine("1. Cargar saldo");
                Console.WriteLine("2. Ver saldo");
                Console.WriteLine("3. Pagar viaje");
                Console.WriteLine("4. Salir");
                Console.Write("\nSeleccione una opcion: ");

                string opcion = Console.ReadLine();

                switch (opcion)
                {
                    case "1":
                        Console.WriteLine("\nMontos disponibles: 2000, 3000, 4000, 5000, 8000, 10000, 15000, 20000, 25000, 30000");
                        Console.Write("Ingrese el monto a cargar: ");
                        if (decimal.TryParse(Console.ReadLine(), out decimal monto))
                        {
                            if (miTarjeta.Cargar(monto))
                            {
                                Console.WriteLine($"Carga exitosa. Saldo actual: ${miTarjeta.Saldo}");
                            }
                            else
                            {
                                Console.WriteLine("Error: Monto invalido o excede el limite de saldo ($40000)");
                            }
                        }
                        else
                        {
                            Console.WriteLine("Error: Ingrese un numero valido");
                        }
                        break;

                    case "2":
                        Console.WriteLine($"\nSaldo actual: ${miTarjeta.Saldo}");
                        break;

                    case "3":
                        Console.Write("Ingrese la linea del colectivo: ");
                        string linea = Console.ReadLine();
                        colectivo = new Colectivo(linea);

                        Boleto boleto = colectivo.PagarCon(miTarjeta);

                        if (boleto != null)
                        {
                            Console.WriteLine("\n--- Boleto emitido ---");
                            Console.WriteLine($"Linea: {boleto.Linea}");
                            Console.WriteLine($"Monto: ${boleto.Monto}");
                            Console.WriteLine($"Saldo restante: ${boleto.SaldoRestante}");
                            Console.WriteLine($"Tipo: {boleto.TipoBoleto}");
                            Console.WriteLine($"Fecha: {boleto.Fecha}");
                        }
                        else
                        {
                            Console.WriteLine("Error: Saldo insuficiente");
                        }
                        break;

                    case "4":
                        salir = true;
                        Console.WriteLine("Gracias por usar el sistema");
                        break;

                    default:
                        Console.WriteLine("Opcion invalida");
                        break;
                }
            }
        }
    }
}