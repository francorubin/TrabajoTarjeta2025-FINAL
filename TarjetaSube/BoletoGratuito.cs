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
            // Si el monto es 0 (viaje gratis), no descontar
            if (monto == 0)
            {
                return true;
            }

            // Si el monto es mayor a 0 (tercer viaje en adelante), descontar normalmente
            return base.DescontarSaldo(monto);
        }

        public override bool PuedeViajar(Tiempo tiempo)
        {
            DateTime ahora = tiempo.Now();

            // Verificar si es un nuevo día
            if (primerViajeDia == null || ahora.Date != primerViajeDia.Value.Date)
            {
                // Es un nuevo día, reiniciar contador
                primerViajeDia = ahora;
                viajesDelDia = 0;
            }

            // Siempre puede viajar, pero después de 2 viajes pagará tarifa completa
            return true;
        }

        public override decimal ObtenerTarifaConLimitaciones(decimal tarifaBase, Tiempo tiempo)
        {
            DateTime ahora = tiempo.Now();

            // Verificar si es un nuevo día
            if (primerViajeDia == null || ahora.Date != primerViajeDia.Value.Date)
            {
                primerViajeDia = ahora;
                viajesDelDia = 0;
            }

            // Si ya hizo 2 viajes hoy, cobrar tarifa completa
            if (viajesDelDia >= 2)
            {
                return tarifaBase; // Tarifa completa
            }

            // Cobrar gratis (tarifa 0)
            return 0m;
        }

        public override void RegistrarViaje(Tiempo tiempo)
        {
            DateTime ahora = tiempo.Now();

            // Verificar si es un nuevo día
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
    }
}