using NUnit.Framework;
using TarjetaSube;
using System;

namespace TarjetaSubeTest
{
    [TestFixture]
    public class TestFranquiciasHorario
    {
        [Test]
        public void TestMedioBoletoNoPermiteViajarDomingo()
        {
            MedioBoleto tarjeta = new MedioBoleto();
            tarjeta.Cargar(5000);
            Colectivo colectivo = new Colectivo("K");
            TiempoFalso tiempo = new TiempoFalso(2024, 10, 13, 10, 0, 0);

            Boleto boleto = colectivo.PagarCon(tarjeta, tiempo);
            Assert.IsNull(boleto);
        }

        [Test]
        public void TestMedioBoletoNoPermiteViajarAntesDeLas6()
        {
            MedioBoleto tarjeta = new MedioBoleto();
            tarjeta.Cargar(5000);
            Colectivo colectivo = new Colectivo("K");
            TiempoFalso tiempo = new TiempoFalso(2024, 10, 14, 5, 59, 0);

            Boleto boleto = colectivo.PagarCon(tarjeta, tiempo);
            Assert.IsNull(boleto);
        }

        [Test]
        public void TestMedioBoletoPermiteViajarALas6EnPunto()
        {
            MedioBoleto tarjeta = new MedioBoleto();
            tarjeta.Cargar(5000);
            Colectivo colectivo = new Colectivo("K");
            TiempoFalso tiempo = new TiempoFalso(2024, 10, 14, 6, 0, 0);

            Boleto boleto = colectivo.PagarCon(tarjeta, tiempo);
            Assert.IsNotNull(boleto);
            Assert.AreEqual(790, boleto.Monto);
        }

        [Test]
        public void TestMedioBoletoNoPermiteViajarALas22()
        {
            MedioBoleto tarjeta = new MedioBoleto();
            tarjeta.Cargar(5000);
            Colectivo colectivo = new Colectivo("K");
            TiempoFalso tiempo = new TiempoFalso(2024, 10, 14, 22, 0, 0);

            Boleto boleto = colectivo.PagarCon(tarjeta, tiempo);
            Assert.IsNull(boleto);
        }

        [Test]
        public void TestMedioBoletoPermiteViajarALas21Y59()
        {
            MedioBoleto tarjeta = new MedioBoleto();
            tarjeta.Cargar(5000);
            Colectivo colectivo = new Colectivo("K");
            TiempoFalso tiempo = new TiempoFalso(2024, 10, 14, 21, 59, 0);

            Boleto boleto = colectivo.PagarCon(tarjeta, tiempo);
            Assert.IsNotNull(boleto);
            Assert.AreEqual(790, boleto.Monto);
        }

        [Test]
        public void TestMedioBoletoPermiteViajarLunesAViernes()
        {
            MedioBoleto tarjeta = new MedioBoleto();
            tarjeta.Cargar(5000);
            Colectivo colectivo = new Colectivo("K");

            for (int dia = 14; dia <= 18; dia++)
            {
                TiempoFalso tiempo = new TiempoFalso(2024, 10, dia, 10, 0, 0);
                Boleto boleto = colectivo.PagarCon(tarjeta, tiempo);
                Assert.IsNotNull(boleto);
            }
        }

        [Test]
        public void TestBoletoGratuitoNoPermiteViajarDomingo()
        {
            BoletoGratuito tarjeta = new BoletoGratuito();
            tarjeta.Cargar(5000);
            Colectivo colectivo = new Colectivo("K");
            TiempoFalso tiempo = new TiempoFalso(2024, 10, 13, 10, 0, 0);

            Boleto boleto = colectivo.PagarCon(tarjeta, tiempo);
            Assert.IsNull(boleto);
        }

        [Test]
        public void TestBoletoGratuitoNoPermiteViajarAntesDeLas6()
        {
            BoletoGratuito tarjeta = new BoletoGratuito();
            tarjeta.Cargar(5000);
            Colectivo colectivo = new Colectivo("K");
            TiempoFalso tiempo = new TiempoFalso(2024, 10, 14, 5, 30, 0);

            Boleto boleto = colectivo.PagarCon(tarjeta, tiempo);
            Assert.IsNull(boleto);
        }

        [Test]
        public void TestBoletoGratuitoPermiteViajarALas6()
        {
            BoletoGratuito tarjeta = new BoletoGratuito();
            tarjeta.Cargar(5000);
            Colectivo colectivo = new Colectivo("K");
            TiempoFalso tiempo = new TiempoFalso(2024, 10, 14, 6, 0, 0);

            Boleto boleto = colectivo.PagarCon(tarjeta, tiempo);
            Assert.IsNotNull(boleto);
            Assert.AreEqual(0, boleto.Monto);
        }

        [Test]
        public void TestBoletoGratuitoNoPermiteViajarALas22()
        {
            BoletoGratuito tarjeta = new BoletoGratuito();
            tarjeta.Cargar(5000);
            Colectivo colectivo = new Colectivo("K");
            TiempoFalso tiempo = new TiempoFalso(2024, 10, 14, 22, 0, 0);

            Boleto boleto = colectivo.PagarCon(tarjeta, tiempo);
            Assert.IsNull(boleto);
        }

        [Test]
        public void TestFranquiciaCompletaNoPermiteViajarDomingo()
        {
            FranquiciaCompleta tarjeta = new FranquiciaCompleta();
            tarjeta.Cargar(5000);
            Colectivo colectivo = new Colectivo("K");
            TiempoFalso tiempo = new TiempoFalso(2024, 10, 13, 10, 0, 0);

            Boleto boleto = colectivo.PagarCon(tarjeta, tiempo);
            Assert.IsNull(boleto);
        }

        [Test]
        public void TestFranquiciaCompletaNoPermiteViajarAntesDeLas6()
        {
            FranquiciaCompleta tarjeta = new FranquiciaCompleta();
            tarjeta.Cargar(5000);
            Colectivo colectivo = new Colectivo("K");
            TiempoFalso tiempo = new TiempoFalso(2024, 10, 14, 3, 0, 0);

            Boleto boleto = colectivo.PagarCon(tarjeta, tiempo);
            Assert.IsNull(boleto);
        }

        [Test]
        public void TestFranquiciaCompletaPermiteViajarALas6()
        {
            FranquiciaCompleta tarjeta = new FranquiciaCompleta();
            tarjeta.Cargar(5000);
            Colectivo colectivo = new Colectivo("K");
            TiempoFalso tiempo = new TiempoFalso(2024, 10, 14, 6, 0, 0);

            Boleto boleto = colectivo.PagarCon(tarjeta, tiempo);
            Assert.IsNotNull(boleto);
            Assert.AreEqual(0, boleto.Monto);
        }

        [Test]
        public void TestFranquiciaCompletaNoPermiteViajarALas22()
        {
            FranquiciaCompleta tarjeta = new FranquiciaCompleta();
            tarjeta.Cargar(5000);
            Colectivo colectivo = new Colectivo("K");
            TiempoFalso tiempo = new TiempoFalso(2024, 10, 14, 22, 30, 0);

            Boleto boleto = colectivo.PagarCon(tarjeta, tiempo);
            Assert.IsNull(boleto);
        }

        [Test]
        public void TestFranquiciaCompletaPermiteViajarLunesAViernes()
        {
            FranquiciaCompleta tarjeta = new FranquiciaCompleta();
            tarjeta.Cargar(5000);
            Colectivo colectivo = new Colectivo("K");

            for (int dia = 14; dia <= 18; dia++)
            {
                TiempoFalso tiempo = new TiempoFalso(2024, 10, dia, 12, 0, 0);
                Boleto boleto = colectivo.PagarCon(tarjeta, tiempo);
                Assert.IsNotNull(boleto);
            }
        }

        [Test]
        public void TestMedioBoletoNoPermiteViajarSabadoFueraDeLimite()
        {
            MedioBoleto tarjeta = new MedioBoleto();
            tarjeta.Cargar(5000);
            Colectivo colectivo = new Colectivo("K");
            TiempoFalso tiempo = new TiempoFalso(2024, 10, 19, 23, 0, 0);

            Boleto boleto = colectivo.PagarCon(tarjeta, tiempo);
            Assert.IsNull(boleto);
        }

        [Test]
        public void TestBoletoGratuitoPermiteViajarSabadoDentroDelLimite()
        {
            BoletoGratuito tarjeta = new BoletoGratuito();
            tarjeta.Cargar(5000);
            Colectivo colectivo = new Colectivo("K");
            TiempoFalso tiempo = new TiempoFalso(2024, 10, 19, 15, 0, 0);

            Boleto boleto = colectivo.PagarCon(tarjeta, tiempo);
            Assert.IsNotNull(boleto);
        }
    }
}
