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
        private Guid idTarjeta;
        private decimal totalAbonado;

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

        public Guid IdTarjeta
        {
            get { return idTarjeta; }
        }

        public decimal TotalAbonado
        {
            get { return totalAbonado; }
        }

        public Boleto(decimal montoViaje, decimal saldoActual, string lineaColectivo, DateTime fechaViaje, string tipo, Guid id, decimal saldoAnterior)
        {
            monto = montoViaje;
            saldoRestante = saldoActual;
            linea = lineaColectivo;
            fecha = fechaViaje;
            tipoBoleto = tipo;
            idTarjeta = id;
            totalAbonado = saldoAnterior - saldoActual;
        }
    }
}