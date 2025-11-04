using System;

namespace TarjetaSube
{
    public class ColectivoInterurbano : Colectivo
    {
        private const decimal TARIFA_INTERURBANA = 3000m;

        public ColectivoInterurbano(string lineaColectivo) : base(lineaColectivo)
        {
        }

        public new Boleto PagarCon(Tarjeta tarjeta)
        {
            return PagarCon(tarjeta, new Tiempo());
        }

        public new Boleto PagarCon(Tarjeta tarjeta, Tiempo tiempo)
        {
            if (tarjeta == null)
            {
                return null;
            }

            if (!tarjeta.PuedeViajar(tiempo))
            {
                return null;
            }

            decimal tarifaAPagar = tarjeta.ObtenerTarifaConLimitaciones(TARIFA_INTERURBANA, tiempo);
            decimal saldoAnterior = tarjeta.Saldo;

            if (!tarjeta.DescontarSaldo(tarifaAPagar))
            {
                return null;
            }

            tarjeta.RegistrarViaje(tiempo);

            Boleto boleto = new Boleto(
                tarifaAPagar,
                tarjeta.Saldo,
                Linea,
                tiempo.Now(),
                tarjeta.ObtenerTipoBoleto(),
                tarjeta.Id,
                saldoAnterior
            );

            return boleto;
        }
    }
}