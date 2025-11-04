using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TarjetaSube
{
    public class FranquiciaCompleta : Tarjeta
    {
        public FranquiciaCompleta() : base()
        {
        }

        public override decimal ObtenerTarifa(decimal tarifaBase)
        {
            return 0m;
        }

        public override string ObtenerTipoBoleto()
        {
            return "Franquicia Completa";
        }

        public override bool DescontarSaldo(decimal monto)
        {
            return true;
        }

        public override decimal ObtenerTarifaConLimitaciones(decimal tarifaBase, Tiempo tiempo)
        {
            return 0m;
        }

        public override bool PuedeViajar(Tiempo tiempo)
        {
            DateTime ahora = tiempo.Now();

            if (!EstaEnHorarioPermitido(ahora))
            {
                return false;
            }

            return true;
        }

        private bool EstaEnHorarioPermitido(DateTime fecha)
        {
            if (fecha.DayOfWeek == DayOfWeek.Sunday)
            {
                return false;
            }

            TimeSpan hora = fecha.TimeOfDay;
            TimeSpan horaInicio = new TimeSpan(6, 0, 0);
            TimeSpan horaFin = new TimeSpan(22, 0, 0);

            return hora >= horaInicio && hora < horaFin;
        }
    }
}
