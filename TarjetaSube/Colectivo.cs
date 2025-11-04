using System;

namespace TarjetaSube
{
    public class Colectivo
    {
        private const decimal TARIFA_BASICA = 1580m;
        private string linea;

        public string Linea
        {
            get { return linea; }
        }

        public Colectivo(string lineaColectivo)
        {
            linea = lineaColectivo;
        }

        public Boleto PagarCon(Tarjeta tarjeta)
        {
            return PagarCon(tarjeta, new Tiempo());
        }

        public Boleto PagarCon(Tarjeta tarjeta, Tiempo tiempo)
        {
            if (tarjeta == null)
            {
                return null;
            }

            if (!tarjeta.PuedeViajar(tiempo))
            {
                return null;
            }

            bool esTrasbordo = tarjeta.EsTrasbordo(linea, tiempo);
            decimal tarifaAPagar = 0m;

            if (!esTrasbordo)
            {
                tarifaAPagar = tarjeta.ObtenerTarifaConLimitaciones(TARIFA_BASICA, tiempo);
            }
            else
            {
                // CORREGIDO: Cuando es trasbordo, obtener la tarifa para verificar si la tarjeta
                // aún tiene descuentos disponibles (ej: MedioBoleto 2 viajes, BoletoGratuito 2 gratis)
                decimal tarifaSinTrasbordo = tarjeta.ObtenerTarifaConLimitaciones(TARIFA_BASICA, tiempo);

                // Si la tarifa sin trasbordo es 0 (FranquiciaCompleta o BoletoGratuito con viajes gratis),
                // el trasbordo también es gratis
                if (tarifaSinTrasbordo == 0m)
                {
                    tarifaAPagar = 0m;
                }
                // Si la tarifa sin trasbordo es la tarifa completa (ya no tiene descuentos del día),
                // NO aplica trasbordo gratis, cobra la tarifa completa
                else if (tarifaSinTrasbordo >= TARIFA_BASICA)
                {
                    tarifaAPagar = tarifaSinTrasbordo;
                    esTrasbordo = false; // Ya no es trasbordo porque no tiene descuentos
                }
                // Si tiene descuento (MedioBoleto), el trasbordo es gratis
                else
                {
                    tarifaAPagar = 0m;
                }
            }

            decimal saldoAnterior = tarjeta.Saldo;

            if (!tarjeta.DescontarSaldo(tarifaAPagar))
            {
                return null;
            }

            tarjeta.RegistrarViaje(tiempo);
            tarjeta.RegistrarViajeParaTrasbordo(linea, tiempo, esTrasbordo);

            Boleto boleto = new Boleto(
                tarifaAPagar,
                tarjeta.Saldo,
                linea,
                tiempo.Now(),
                tarjeta.ObtenerTipoBoleto(),
                tarjeta.Id,
                saldoAnterior,
                esTrasbordo
            );

            return boleto;
        }
    }
}