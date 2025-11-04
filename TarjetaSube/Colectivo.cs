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

            if (esTrasbordo)
            {
                tarifaAPagar = 0m;
            }
            else
            {
                tarifaAPagar = tarjeta.ObtenerTarifaConLimitaciones(TARIFA_BASICA, tiempo);
            }

            decimal saldoAnterior = tarjeta.Saldo;

            if (!tarjeta.DescontarSaldo(tarifaAPagar))
            {
                return null;
            }

            if (!esTrasbordo)
            {
                tarjeta.RegistrarViaje(tiempo);
            }
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