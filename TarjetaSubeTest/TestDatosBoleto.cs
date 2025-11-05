using NUnit.Framework;
using TarjetaSube;
using System;

namespace TarjetaSubeTest
{
    [TestFixture]
    public class TestDatosBoleto
    {
        [Test]
        public void TestBoletoTieneIdTarjeta()
        {
            Tarjeta tarjeta = new Tarjeta();
            tarjeta.Cargar(5000);
            Colectivo colectivo = new Colectivo("K");

            Boleto boleto = colectivo.PagarCon(tarjeta);

            Assert.IsNotNull(boleto);
            Assert.AreEqual(tarjeta.Id, boleto.IdTarjeta);
        }

        [Test]
        public void TestBoletoMuestraMontoTotalConSaldoPositivo()
        {
            Tarjeta tarjeta = new Tarjeta();
            tarjeta.Cargar(5000);
            Colectivo colectivo = new Colectivo("K");

            Boleto boleto = colectivo.PagarCon(tarjeta);

            Assert.AreEqual(1580, boleto.TotalAbonado);
            Assert.AreEqual(1580, boleto.Monto);
        }

        [Test]
        public void TestBoletoMuestraMontoTotalConSaldoNegativo()
        {
            Tarjeta tarjeta = new Tarjeta();
            tarjeta.Cargar(2000);
            Colectivo colectivo = new Colectivo("K");

            colectivo.PagarCon(tarjeta);
            Boleto boleto2 = colectivo.PagarCon(tarjeta);

            Assert.AreEqual(1580, boleto2.Monto);

            Assert.AreEqual(1580, boleto2.TotalAbonado);
        }

        [Test]
        public void TestBoletoTieneTodasLasPropiedades()
        {
            Tarjeta tarjeta = new Tarjeta();
            tarjeta.Cargar(5000);
            Colectivo colectivo = new Colectivo("142");

            Boleto boleto = colectivo.PagarCon(tarjeta);

            Assert.IsNotNull(boleto.IdTarjeta);
            Assert.IsNotNull(boleto.Linea);
            Assert.AreEqual("142", boleto.Linea);
            Assert.AreEqual("Normal", boleto.TipoBoleto);
            Assert.Greater(boleto.Monto, 0);
            Assert.Greater(boleto.TotalAbonado, 0);
        }

        [Test]
        public void TestBoletoMedioBoletoTieneTipoCorrecto()
        {
            MedioBoleto tarjeta = new MedioBoleto();
            tarjeta.Cargar(5000);
            Colectivo colectivo = new Colectivo("K");
            TiempoFalso tiempo = new TiempoFalso(2024, 10, 14, 8, 0, 0);

            Boleto boleto = colectivo.PagarCon(tarjeta, tiempo);

            Assert.AreEqual("Medio Boleto", boleto.TipoBoleto);
            Assert.AreEqual(790, boleto.Monto);
        }

        [Test]
        public void TestBoletoFranquiciaCompletaTieneTipoCorrecto()
        {
            FranquiciaCompleta tarjeta = new FranquiciaCompleta();
            tarjeta.Cargar(5000);
            Colectivo colectivo = new Colectivo("K");
            TiempoFalso tiempo = new TiempoFalso(2024, 10, 14, 8, 0, 0);

            Boleto boleto = colectivo.PagarCon(tarjeta, tiempo);

            Assert.AreEqual("Franquicia Completa", boleto.TipoBoleto);
            Assert.AreEqual(0, boleto.Monto);
        }

        [Test]
        public void TestBoletoBoletoGratuitoTieneTipoCorrecto()
        {
            BoletoGratuito tarjeta = new BoletoGratuito();
            tarjeta.Cargar(5000);
            Colectivo colectivo = new Colectivo("K");
            TiempoFalso tiempo = new TiempoFalso(2024, 10, 14, 8, 0, 0);

            Boleto boleto = colectivo.PagarCon(tarjeta, tiempo);

            Assert.AreEqual("Boleto Gratuito", boleto.TipoBoleto);
            Assert.AreEqual(0, boleto.Monto);
        }
    }
}