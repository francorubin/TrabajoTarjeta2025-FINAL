using System;

namespace TarjetaSube
{
    public class BoletoGratuito : Tarjeta
    {
        private DateTime? primerViajeDia;
        private int viajesDelDia;

        public BoletoGratuito() : base()
        {
            primerViajeDia = null;
            viajesDelDia = 0;
        }

        public override decimal ObtenerTarifa(decimal tarifaBase)
        {
            return 0m;
        }

        public override string ObtenerTipoBoleto()
        {
            return "Boleto Gratuito";
        }

        public override bool DescontarSaldo(decimal monto)
        {
            if (monto == 0)
            {
                return true;
            }

            return base.DescontarSaldo(monto);
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

            return 0m;
        }

        public override void RegistrarViaje(Tiempo tiempo)
        {
            DateTime ahora = tiempo.Now();

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
