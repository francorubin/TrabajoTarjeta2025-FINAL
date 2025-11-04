using System;

namespace TarjetaSube
{
    public class FranquiciaCompleta : Tarjeta
    {
        public FranquiciaCompleta() : base()
        {
        }

        public override decimal ObtenerTarifa(decimal tarifaBase)
        {
            return 0m;
        }

        public override string ObtenerTipoBoleto()
        {
            return "Franquicia Completa";
        }

        public override bool DescontarSaldo(decimal monto)
        {
            return true;
        }

        public override decimal ObtenerTarifaConLimitaciones(decimal tarifaBase, Tiempo tiempo)
        {
            return 0m;
        }

        // Implementación de la restricción de horarios (Iteración 4)
        public override bool PuedeViajar(Tiempo tiempo)
        {
            DateTime ahora = tiempo.Now();
            if (!EstaEnHorarioPermitido(ahora))
            {
                return false;
            }
            return true;
        }

        private bool EstaEnHorarioPermitido(DateTime fecha)
        {
            // No se puede viajar los domingos
            if (fecha.DayOfWeek == DayOfWeek.Sunday)
            {
                return false;
            }

            // Solo lunes a sábado de 6 a 22
            TimeSpan hora = fecha.TimeOfDay;
            TimeSpan horaInicio = new TimeSpan(6, 0, 0);
            TimeSpan horaFin = new TimeSpan(22, 0, 0);

            return hora >= horaInicio && hora < horaFin;
        }

        // Override para que FranquiciaCompleta NO cuente viajes del mes
        // (no tiene descuento por uso frecuente)
        public override void RegistrarViaje(Tiempo tiempo)
        {
            // No registrar viajes para contador mensual
            // FranquiciaCompleta no usa descuentos por uso frecuente
        }

        public override void RegistrarViajeParaTrasbordo(string lineaColectivo, Tiempo tiempo, bool esTrasbordo)
        {
            base.RegistrarViajeParaTrasbordo(lineaColectivo, tiempo, esTrasbordo);
        }
    }
}