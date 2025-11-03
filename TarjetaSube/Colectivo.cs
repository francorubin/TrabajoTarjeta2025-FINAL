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

        // Método antiguo para compatibilidad (usa Tiempo real)
        public Boleto PagarCon(Tarjeta tarjeta)
        {
            return PagarCon(tarjeta, new Tiempo());
        }

        // Nuevo método que acepta Tiempo
        public Boleto PagarCon(Tarjeta tarjeta, Tiempo tiempo)
        {
            if (tarjeta == null)
            {
                return null;
            }

            // Verificar si la tarjeta puede viajar (limitaciones temporales)
            if (!tarjeta.PuedeViajar(tiempo))
            {
                return null;
            }

            decimal tarifaAPagar = tarjeta.ObtenerTarifaConLimitaciones(TARIFA_BASICA, tiempo);
            decimal saldoAnterior = tarjeta.Saldo;

            if (!tarjeta.DescontarSaldo(tarifaAPagar))
            {
                return null;
            }

            // Registrar el viaje en la tarjeta (para contadores de uso diario)
            tarjeta.RegistrarViaje(tiempo);

            Boleto boleto = new Boleto(
                tarifaAPagar,
                tarjeta.Saldo,
                linea,
                tiempo.Now(),
                tarjeta.ObtenerTipoBoleto(),
                tarjeta.Id,
                saldoAnterior
            );

            return boleto;
        }
    }
}