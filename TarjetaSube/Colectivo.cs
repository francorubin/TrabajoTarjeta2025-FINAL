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
            if (tarjeta == null)
            {
                return null;
            }

         
            decimal tarifaAPagar = tarjeta.ObtenerTarifa(TARIFA_BASICA);

      
            if (!tarjeta.DescontarSaldo(tarifaAPagar))
            {
                return null;
            }

            Boleto boleto = new Boleto(
                tarifaAPagar,
                tarjeta.Saldo,
                linea,
                DateTime.Now,
                tarjeta.ObtenerTipoBoleto()
            );

            return boleto;
        }
    }
}