using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace TarjetaSube
{
    public class MedioBoleto : Tarjeta
    {
        public MedioBoleto() : base()
        {
        }

        public override decimal ObtenerTarifa(decimal tarifaBase)
        {
            return tarifaBase / 2;
        }

        public override string ObtenerTipoBoleto()
        {
            return "Medio Boleto";
        }
    }
}