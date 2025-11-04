using System;

namespace TarjetaSube
{
    public class MedioBoleto : Tarjeta
    {
        private DateTime? ultimoViaje;
        private DateTime? primerViajeDia;
        private int viajesDelDia;

        public MedioBoleto() : base()
        {
            ultimoViaje = null;
            primerViajeDia = null;
            viajesDelDia = 0;
        }

        public override decimal ObtenerTarifa(decimal tarifaBase)
        {
            return tarifaBase / 2;
        }

        public override string ObtenerTipoBoleto()
        {
            return "Medio Boleto";
        }

        public override bool PuedeViajar(Tiempo tiempo)
        {
            DateTime ahora = tiempo.Now();

            if (!EstaEnHorarioPermitido(ahora))
            {
                return false;
            }

            if (primerViajeDia == null || ahora.Date != primerViajeDia.Value.Date)
            {
                primerViajeDia = ahora;
                viajesDelDia = 0;
            }

            if (viajesDelDia >= 2)
            {
                return true;
            }

            if (ultimoViaje != null)
            {
                TimeSpan diferencia = ahora - ultimoViaje.Value;
                if (diferencia.TotalMinutes < 5)
                {
                    return false;
                }
            }

            return true;
        }

        public override decimal ObtenerTarifaConLimitaciones(decimal tarifaBase, Tiempo tiempo)
        {
            DateTime ahora = tiempo.Now();

            if (primerViajeDia == null || ahora.Date != primerViajeDia.Value.Date)
            {
                primerViajeDia = ahora;
                viajesDelDia = 0;
            }

            if (viajesDelDia >= 2)
            {
                return tarifaBase;
            }

            return tarifaBase / 2;
        }

        public override void RegistrarViaje(Tiempo tiempo)
        {
            DateTime ahora = tiempo.Now();

            ultimoViaje = ahora;

            if (primerViajeDia == null || ahora.Date != primerViajeDia.Value.Date)
            {
                primerViajeDia = ahora;
                viajesDelDia = 1;
            }
            else
            {
                viajesDelDia++;
            }
        }

        private bool EstaEnHorarioPermitido(DateTime fecha)
        {
            if (fecha.DayOfWeek == DayOfWeek.Sunday)
            {
                return false;
            }

            TimeSpan hora = fecha.TimeOfDay;
            TimeSpan horaInicio = new TimeSpan(6, 0, 0);
            TimeSpan horaFin = new TimeSpan(22, 0, 0);

            return hora >= horaInicio && hora < horaFin;
        }
    }
}
