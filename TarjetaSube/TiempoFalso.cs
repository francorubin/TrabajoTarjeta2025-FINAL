using System;

namespace TarjetaSube
{
    public class TiempoFalso : Tiempo
    {
        private DateTime tiempo;

        public TiempoFalso()
        {
            // CORREGIDO: Inicia en una fecha específica (lunes 14 de octubre de 2024 a las 08:00:00)
            // Cambio de 00:00:00 a 08:00:00 para que esté dentro del horario permitido (6-22)
            // y para que los tests que avanzan muchas horas no salgan del rango
            tiempo = new DateTime(2024, 10, 14, 8, 0, 0);
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