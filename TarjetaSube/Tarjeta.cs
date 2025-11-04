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

        private int viajesDelMes;
        private DateTime? primerViajeMes;
        private DateTime? ultimoViajeParaTrasbordo;
        private string ultimaLineaViaje;

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

        public int ViajesDelMes
        {
            get { return viajesDelMes; }
        }

        public Tarjeta()
        {
            saldo = 0m;
            saldoPendiente = 0m;
            id = Guid.NewGuid();
            viajesDelMes = 0;
            primerViajeMes = null;
            ultimoViajeParaTrasbordo = null;
            ultimaLineaViaje = null;
        }

        public virtual bool Cargar(decimal monto)
        {
            if (!CARGAS_ACEPTADAS.Contains(monto))
            {
                return false;
            }

            decimal nuevoSaldo = saldo + monto;

            if (nuevoSaldo > LIMITE_SALDO_MAXIMO)
            {
                decimal excedente = nuevoSaldo - LIMITE_SALDO_MAXIMO;
                saldo = LIMITE_SALDO_MAXIMO;
                saldoPendiente += excedente;
            }
            else if (nuevoSaldo > LIMITE_SALDO)
            {
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

        public virtual bool PuedeViajar(Tiempo tiempo)
        {
            return true;
        }

        public virtual decimal ObtenerTarifaConLimitaciones(decimal tarifaBase, Tiempo tiempo)
        {
            ActualizarContadorMensual(tiempo);
            return AplicarDescuentoUsoFrecuente(tarifaBase, viajesDelMes);
        }

        public virtual void RegistrarViaje(Tiempo tiempo)
        {
            ActualizarContadorMensual(tiempo);
            viajesDelMes++;
        }

        public virtual bool EsTrasbordo(string lineaColectivo, Tiempo tiempo)
        {
            if (ultimoViajeParaTrasbordo == null || ultimaLineaViaje == null)
            {
                return false;
            }

            if (lineaColectivo == ultimaLineaViaje)
            {
                return false;
            }

            DateTime ahora = tiempo.Now();
            DayOfWeek dia = ahora.DayOfWeek;

            if (dia == DayOfWeek.Sunday)
            {
                return false;
            }

            if (ahora.Hour < 7 || ahora.Hour >= 22)
            {
                return false;
            }

            TimeSpan diferencia = ahora - ultimoViajeParaTrasbordo.Value;
            if (diferencia.TotalHours <= 1)
            {
                return true;
            }

            return false;
        }

        public virtual void RegistrarViajeParaTrasbordo(string lineaColectivo, Tiempo tiempo, bool esTrasbordo)
        {
            if (!esTrasbordo)
            {
                ultimoViajeParaTrasbordo = tiempo.Now();
            }

            ultimaLineaViaje = lineaColectivo;
        }

        protected void ActualizarContadorMensual(Tiempo tiempo)
        {
            DateTime ahora = tiempo.Now();

            if (primerViajeMes == null || ahora.Month != primerViajeMes.Value.Month || ahora.Year != primerViajeMes.Value.Year)
            {
                primerViajeMes = ahora;
                viajesDelMes = 0;
            }
        }

        protected decimal AplicarDescuentoUsoFrecuente(decimal tarifaBase, int numeroViaje)
        {
            if (numeroViaje >= 29 && numeroViaje < 59)
            {
                return tarifaBase * 0.80m;
            }
            else if (numeroViaje >= 59 && numeroViaje < 80)
            {
                return tarifaBase * 0.75m;
            }
            else
            {
                return tarifaBase;
            }
        }
    }
}
