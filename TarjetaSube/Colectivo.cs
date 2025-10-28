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

            decimal saldoAnterior = tarjeta.Saldo;

            if (!tarjeta.DescontarSaldo(TARIFA_BASICA))
            {
                return null;
            }

            Boleto boleto = new Boleto(
                TARIFA_BASICA,
                tarjeta.Saldo,
                linea,
                DateTime.Now,
                "Normal"
            );

            return boleto;
        }
    }
}