using System;

namespace TarjetaSube
{
    public class TiempoFalso : Tiempo
    {
        private DateTime tiempo;

        public TiempoFalso()
        {
            // Inicia en una fecha específica (lunes 14 de octubre de 2024 a las 00:00:00)
            tiempo = new DateTime(2024, 10, 14, 0, 0, 0);
        }

        public TiempoFalso(int anio, int mes, int dia, int hora, int minuto, int segundo)
        {
            tiempo = new DateTime(anio, mes, dia, hora, minuto, segundo);
        }

        public override DateTime Now()
        {
            return tiempo;
        }

        public void AgregarDias(int cantidad)
        {
            tiempo = tiempo.AddDays(cantidad);
        }

        public void AgregarMinutos(int cantidad)
        {
            tiempo = tiempo.AddMinutes(cantidad);
        }

        public void AgregarSegundos(int cantidad)
        {
            tiempo = tiempo.AddSeconds(cantidad);
        }

        public void AgregarHoras(int cantidad)
        {
            tiempo = tiempo.AddHours(cantidad);
        }
    }
}
