using NUnit.Framework;
using TarjetaSube;
using System;

namespace TarjetaSubeTest
{
    [TestFixture]
    public class TestFranquicias
    {
        [Test]
        public void TestMedioBoletoTarifaMitadDeNormal()
        {
            MedioBoleto tarjeta = new MedioBoleto();
            tarjeta.Cargar(5000);
            Colectivo colectivo = new Colectivo("K");
            TiempoFalso tiempo = new TiempoFalso(2024, 10, 14, 8, 0, 0);

            Boleto boleto = colectivo.PagarCon(tarjeta, tiempo);

            Assert.IsNotNull(boleto);
            Assert.AreEqual(790, boleto.Monto);
            Assert.AreEqual(4210, tarjeta.Saldo);
            Assert.AreEqual("Medio Boleto", boleto.TipoBoleto);
        }

        [Test]
        public void TestMedioBoletoSiempreMitadDePrecio()
        {
            MedioBoleto tarjeta = new MedioBoleto();
            tarjeta.Cargar(10000);
            Colectivo colectivo = new Colectivo("K");
            TiempoFalso tiempo = new TiempoFalso();

            Boleto b1 = colectivo.PagarCon(tarjeta, tiempo);
            tiempo.AgregarMinutos(10);
            Boleto b2 = colectivo.PagarCon(tarjeta, tiempo);

            Assert.AreEqual(790, b1.Monto);
            Assert.AreEqual(790, b2.Monto);
        }

        [Test]
        public void TestFranquiciaCompletaSiemprePuedeViajar()
        {
            FranquiciaCompleta tarjeta = new FranquiciaCompleta();
            Colectivo colectivo = new Colectivo("K");
            TiempoFalso tiempo = new TiempoFalso(2024, 10, 14, 8, 0, 0);

            for (int i = 0; i < 100; i++)
            {
                Boleto boleto = colectivo.PagarCon(tarjeta, tiempo);
                Assert.IsNotNull(boleto);
                Assert.AreEqual(0, boleto.Monto);
                tiempo.AgregarMinutos(5);
            }
        }

        [Test]
        public void TestFranquiciaCompletaSinSaldo()
        {
            FranquiciaCompleta tarjeta = new FranquiciaCompleta();
            Colectivo colectivo = new Colectivo("K");
            TiempoFalso tiempo = new TiempoFalso(2024, 10, 14, 8, 0, 0);

            Boleto boleto = colectivo.PagarCon(tarjeta, tiempo);

            Assert.IsNotNull(boleto);
            Assert.AreEqual(0, boleto.Monto);
            Assert.AreEqual(0, tarjeta.Saldo);
        }

        [Test]
        public void TestFranquiciaCompletaNoDescuentaSaldo()
        {
            FranquiciaCompleta tarjeta = new FranquiciaCompleta();
            tarjeta.Cargar(5000);
            Colectivo colectivo = new Colectivo("K");
            TiempoFalso tiempo = new TiempoFalso(2024, 10, 14, 8, 0, 0);

            colectivo.PagarCon(tarjeta, tiempo);
            colectivo.PagarCon(tarjeta, tiempo);
            colectivo.PagarCon(tarjeta, tiempo);

            Assert.AreEqual(5000, tarjeta.Saldo);
        }

        [Test]
        public void TestBoletoGratuitoPrimerosViajesGratis()
        {
            BoletoGratuito tarjeta = new BoletoGratuito();
            tarjeta.Cargar(5000);
            Colectivo colectivo = new Colectivo("K");
            TiempoFalso tiempo = new TiempoFalso(2024, 10, 14, 8, 0, 0);

            Boleto boleto = colectivo.PagarCon(tarjeta, tiempo);

            Assert.IsNotNull(boleto);
            Assert.AreEqual(0, boleto.Monto);
            Assert.AreEqual(5000, tarjeta.Saldo);
            Assert.AreEqual("Boleto Gratuito", boleto.TipoBoleto);
        }

        [Test]
        public void TestMedioBoletoDescontaSaldoCorrectamente()
        {
            MedioBoleto tarjeta = new MedioBoleto();
            tarjeta.Cargar(5000);
            Colectivo colectivo = new Colectivo("K");
            TiempoFalso tiempo = new TiempoFalso();

            Boleto b1 = colectivo.PagarCon(tarjeta, tiempo);
            Assert.AreEqual(4210, tarjeta.Saldo);

            tiempo.AgregarMinutos(10);
            Boleto b2 = colectivo.PagarCon(tarjeta, tiempo);
            Assert.AreEqual(3420, tarjeta.Saldo);
        }

        [Test]
        public void TestMedioBoletoMultiplesViajes()
        {
            MedioBoleto tarjeta = new MedioBoleto();
            tarjeta.Cargar(10000);
            Colectivo colectivo = new Colectivo("K");
            TiempoFalso tiempo = new TiempoFalso();

            for (int i = 0; i < 5; i++)
            {
                Boleto boleto = colectivo.PagarCon(tarjeta, tiempo);
                Assert.IsNotNull(boleto);
                tiempo.AgregarMinutos(10);
            }
        }

        [Test]
        public void TestBoletoGratuitoDescontaSaldoDespuesDeDosViajes()
        {
            BoletoGratuito tarjeta = new BoletoGratuito();
            tarjeta.Cargar(5000);
            Colectivo colectivo = new Colectivo("K");
            TiempoFalso tiempo = new TiempoFalso();

            colectivo.PagarCon(tarjeta, tiempo);
            tiempo.AgregarMinutos(10);
            colectivo.PagarCon(tarjeta, tiempo);
            tiempo.AgregarMinutos(10);

            Assert.AreEqual(5000, tarjeta.Saldo);

            Boleto b3 = colectivo.PagarCon(tarjeta, tiempo);
            Assert.AreEqual(1580, b3.Monto);
            Assert.AreEqual(3420, tarjeta.Saldo);
        }

        [Test]
        public void TestTodasLasFranquiciasHeredanDeTarjeta()
        {
            MedioBoleto medio = new MedioBoleto();
            BoletoGratuito gratuito = new BoletoGratuito();
            FranquiciaCompleta completa = new FranquiciaCompleta();

            Assert.IsInstanceOf<Tarjeta>(medio);
            Assert.IsInstanceOf<Tarjeta>(gratuito);
            Assert.IsInstanceOf<Tarjeta>(completa);
        }

        [Test]
        public void TestMedioBoletoTipoBoleto()
        {
            MedioBoleto tarjeta = new MedioBoleto();
            Assert.AreEqual("Medio Boleto", tarjeta.ObtenerTipoBoleto());
        }

        [Test]
        public void TestBoletoGratuitoTipoBoleto()
        {
            BoletoGratuito tarjeta = new BoletoGratuito();
            Assert.AreEqual("Boleto Gratuito", tarjeta.ObtenerTipoBoleto());
        }

        [Test]
        public void TestFranquiciaCompletaTipoBoleto()
        {
            FranquiciaCompleta tarjeta = new FranquiciaCompleta();
            Assert.AreEqual("Franquicia Completa", tarjeta.ObtenerTipoBoleto());
        }

        [Test]
        public void TestMedioBoletoConSaldoInsuficiente()
        {
            MedioBoleto tarjeta = new MedioBoleto();
            tarjeta.Cargar(2000);
            Colectivo colectivo = new Colectivo("K");
            TiempoFalso tiempo = new TiempoFalso();

            colectivo.PagarCon(tarjeta, tiempo);
            tiempo.AgregarMinutos(10);
            colectivo.PagarCon(tarjeta, tiempo);
            tiempo.AgregarMinutos(10);
            colectivo.PagarCon(tarjeta, tiempo);

            decimal saldo = tarjeta.Saldo;
            Assert.Less(saldo, 0);
        }

        [Test]
        public void TestMedioBoletoViajesPorDifernetesLineas()
        {
            MedioBoleto tarjeta = new MedioBoleto();
            tarjeta.Cargar(5000);
            Colectivo colectivoK = new Colectivo("K");
            Colectivo colectivo142 = new Colectivo("142");
            TiempoFalso tiempo = new TiempoFalso();

            Boleto b1 = colectivoK.PagarCon(tarjeta, tiempo);
            Assert.AreEqual(790, b1.Monto);

            tiempo.AgregarHoras(2);

            Boleto b2 = colectivo142.PagarCon(tarjeta, tiempo);
            Assert.AreEqual(790, b2.Monto);
        }

        [Test]
        public void TestBoletoGratuitoConSaldoNegativo()
        {
            BoletoGratuito tarjeta = new BoletoGratuito();
            tarjeta.Cargar(2000);
            Colectivo colectivo = new Colectivo("K");
            TiempoFalso tiempo = new TiempoFalso();

            colectivo.PagarCon(tarjeta, tiempo);
            tiempo.AgregarMinutos(10);
            colectivo.PagarCon(tarjeta, tiempo);
            tiempo.AgregarMinutos(10);
            colectivo.PagarCon(tarjeta, tiempo);

            Assert.AreEqual(420, tarjeta.Saldo);
        }

        [Test]
        public void TestFranquiciaCompletaViajesIlimitados()
        {
            FranquiciaCompleta tarjeta = new FranquiciaCompleta();
            Colectivo colectivo = new Colectivo("K");
            TiempoFalso tiempo = new TiempoFalso(2024, 10, 14, 8, 0, 0);

            for (int i = 0; i < 20; i++)
            {
                Boleto boleto = colectivo.PagarCon(tarjeta, tiempo);
                Assert.IsNotNull(boleto);
                Assert.AreEqual(0, boleto.Monto);
                tiempo.AgregarMinutos(5);
            }

            Assert.AreEqual(0, tarjeta.Saldo);
        }

        [Test]
        public void TestMedioBoletoGetterTarifa()
        {
            MedioBoleto tarjeta = new MedioBoleto();
            decimal tarifa = tarjeta.ObtenerTarifa(1580);
            Assert.AreEqual(790, tarifa);
        }

        [Test]
        public void TestBoletoGratuitoGetterTarifa()
        {
            BoletoGratuito tarjeta = new BoletoGratuito();
            decimal tarifa = tarjeta.ObtenerTarifa(1580);
            Assert.AreEqual(0, tarifa);
        }

        [Test]
        public void TestFranquiciaCompletaGetterTarifa()
        {
            FranquiciaCompleta tarjeta = new FranquiciaCompleta();
            decimal tarifa = tarjeta.ObtenerTarifa(1580);
            Assert.AreEqual(0, tarifa);
        }

        [Test]
        public void TestMedioBoletoIdUnico()
        {
            MedioBoleto tarjeta1 = new MedioBoleto();
            MedioBoleto tarjeta2 = new MedioBoleto();

            Assert.AreNotEqual(tarjeta1.Id, tarjeta2.Id);
        }

        [Test]
        public void TestBoletoGratuitoIdUnico()
        {
            BoletoGratuito tarjeta1 = new BoletoGratuito();
            BoletoGratuito tarjeta2 = new BoletoGratuito();

            Assert.AreNotEqual(tarjeta1.Id, tarjeta2.Id);
        }

        [Test]
        public void TestFranquiciaCompletaIdUnico()
        {
            FranquiciaCompleta tarjeta1 = new FranquiciaCompleta();
            FranquiciaCompleta tarjeta2 = new FranquiciaCompleta();

            Assert.AreNotEqual(tarjeta1.Id, tarjeta2.Id);
        }
    }
}