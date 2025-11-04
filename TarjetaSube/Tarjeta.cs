using System;
using System.Collections.Generic;

namespace TarjetaSube
{
    public class Tarjeta
    {
        protected decimal saldo;
        protected decimal saldoPendiente;
        private Guid id;
        private const decimal LIMITE_SALDO = 40000m;
        private const decimal LIMITE_SALDO_MAXIMO = 56000m;
        private const decimal SALDO_NEGATIVO_PERMITIDO = -1200m;
        private static readonly List<decimal> CARGAS_ACEPTADAS = new List<decimal>
        {
            2000, 3000, 4000, 5000, 8000, 10000, 15000, 20000, 25000, 30000
        };

        public decimal Saldo
        {
            get { return saldo; }
        }

        public decimal SaldoPendiente
        {
            get { return saldoPendiente; }
        }

        public Guid Id
        {
            get { return id; }
        }

        public Tarjeta()
        {
            saldo = 0m;
            saldoPendiente = 0m;
            id = Guid.NewGuid();
        }

        public virtual bool Cargar(decimal monto)
        {
            if (!CARGAS_ACEPTADAS.Contains(monto))
            {
                return false;
            }

            decimal nuevoSaldo = saldo + monto;

            // Si el nuevo saldo supera el límite máximo de 56000
            if (nuevoSaldo > LIMITE_SALDO_MAXIMO)
            {
                decimal excedente = nuevoSaldo - LIMITE_SALDO_MAXIMO;
                saldo = LIMITE_SALDO_MAXIMO;
                saldoPendiente += excedente;
            }
            else if (nuevoSaldo > LIMITE_SALDO)
            {
                // Si supera 40000 pero no 56000, rechazar (comportamiento original)
                return false;
            }
            else
            {
                saldo += monto;
            }

            return true;
        }

        public virtual bool DescontarSaldo(decimal monto)
        {
            if (saldo - monto < SALDO_NEGATIVO_PERMITIDO)
            {
                return false;
            }

            saldo -= monto;

            // Después de descontar, intentar acreditar saldo pendiente
            AcreditarCarga();

            return true;
        }

        public virtual void AcreditarCarga()
        {
            if (saldoPendiente > 0)
            {
                decimal espacioDisponible = LIMITE_SALDO_MAXIMO - saldo;

                if (espacioDisponible > 0)
                {
                    decimal montoAAcreditar = Math.Min(saldoPendiente, espacioDisponible);
                    saldo += montoAAcreditar;
                    saldoPendiente -= montoAAcreditar;
                }
            }
        }

        public virtual decimal ObtenerTarifa(decimal tarifaBase)
        {
            return tarifaBase;
        }

        public virtual string ObtenerTipoBoleto()
        {
            return "Normal";
        }

        // Nuevos métodos para manejo de limitaciones temporales
        public virtual bool PuedeViajar(Tiempo tiempo)
        {
            // Por defecto, cualquier tarjeta puede viajar sin restricciones de tiempo
            return true;
        }

        public virtual decimal ObtenerTarifaConLimitaciones(decimal tarifaBase, Tiempo tiempo)
        {
            // Por defecto, usa el método ObtenerTarifa normal
            return ObtenerTarifa(tarifaBase);
        }

        public virtual void RegistrarViaje(Tiempo tiempo)
        {
            // Por defecto, no hace nada. Las clases hijas lo sobrescriben si necesitan
        }
    }
}
