using System;

namespace TarjetaSube
{
    public class Boleto
    {
        private decimal monto;
        private decimal saldoRestante;
        private string linea;
        private DateTime fecha;
        private string tipoBoleto;

        public decimal Monto
        {
            get { return monto; }
        }

        public decimal SaldoRestante
        {
            get { return saldoRestante; }
        }

        public string Linea
        {
            get { return linea; }
        }

        public DateTime Fecha
        {
            get { return fecha; }
        }

        public string TipoBoleto
        {
            get { return tipoBoleto; }
        }

        public Boleto(decimal montoAbonado, decimal saldo, string lineaColectivo, DateTime fechaViaje, string tipo)
        {
            monto = montoAbonado;
            saldoRestante = saldo;
            linea = lineaColectivo;
            fecha = fechaViaje;
            tipoBoleto = tipo;
        }
    }
}