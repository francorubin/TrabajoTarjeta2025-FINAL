using System;
using System.Collections.Generic;

namespace TarjetaSube
{
    public class Tarjeta
    {
        protected decimal saldo;
        private const decimal LIMITE_SALDO = 40000m;
        private const decimal SALDO_NEGATIVO_PERMITIDO = -1200m;
        private static readonly List<decimal> CARGAS_ACEPTADAS = new List<decimal>
        {
            2000, 3000, 4000, 5000, 8000, 10000, 15000, 20000, 25000, 30000
        };

        public decimal Saldo
        {
            get { return saldo; }
        }

        public Tarjeta()
        {
            saldo = 0m;
        }

        public virtual bool Cargar(decimal monto)
        {
            if (!CARGAS_ACEPTADAS.Contains(monto))
            {
                return false;
            }

            decimal nuevoSaldo = saldo + monto;

            if (nuevoSaldo > LIMITE_SALDO)
            {
                return false;
            }

            saldo += monto;
            return true;
        }

        public virtual bool DescontarSaldo(decimal monto)
        {
            if (saldo - monto < SALDO_NEGATIVO_PERMITIDO)
            {
                return false;
            }

            saldo -= monto;
            return true;
        }

        public virtual decimal ObtenerTarifa(decimal tarifaBase)
        {
            return tarifaBase;
        }

        public virtual string ObtenerTipoBoleto()
        {
            return "Normal";
        }
    }
}

//Puto el que lo lea