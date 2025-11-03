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

            // Verificar si es un nuevo día
            if (primerViajeDia == null || ahora.Date != primerViajeDia.Value.Date)
            {
                // Es un nuevo día, reiniciar contador
                primerViajeDia = ahora;
                viajesDelDia = 0;
            }

            // Si ya hizo 2 viajes hoy, puede viajar pero pagará tarifa completa
            if (viajesDelDia >= 2)
            {
                return true; // Puede viajar, pero ObtenerTarifa devolverá tarifa completa
            }

            // Verificar que hayan pasado 5 minutos desde el último viaje
            if (ultimoViaje != null)
            {
                TimeSpan diferencia = ahora - ultimoViaje.Value;
                if (diferencia.TotalMinutes < 5)
                {
                    return false; // No han pasado 5 minutos
                }
            }

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

            // Cobrar medio boleto
            return tarifaBase / 2;
        }

        public override void RegistrarViaje(Tiempo tiempo)
        {
            DateTime ahora = tiempo.Now();

            // Actualizar último viaje
            ultimoViaje = ahora;

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