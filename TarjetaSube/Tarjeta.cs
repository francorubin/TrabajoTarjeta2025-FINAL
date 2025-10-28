using System;
using System.Collections.Generic;

namespace TarjetaSube
{
    public class Tarjeta
    {
        private decimal saldo;
        private const decimal LIMITE_SALDO = 40000m;
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

        public bool Cargar(decimal monto)
        {
            if (!CARGAS_ACEPTADAS.Contains(monto))
            {
                return false;
            }

            if (saldo + monto > LIMITE_SALDO)
            {
                return false;
            }

            saldo += monto;
            return true;
        }

        public bool DescontarSaldo(decimal monto)
        {
            if (saldo < monto)
            {
                return false;
            }

            saldo -= monto;
            return true;
        }
    }
}