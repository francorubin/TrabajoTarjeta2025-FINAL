using NUnit.Framework;
using TarjetaSube;
using System;

namespace TarjetaSubeTest
{
    [TestFixture]
    public class TestTrasbordos
    {
        [Test]
        public void TestTrasbordoEntreDosLineasDistintas()
        {
            Tarjeta tarjeta = new Tarjeta();
            tarjeta.Cargar(10000);
            Colectivo colectivoK = new Colectivo("K");
            Colectivo colectivo142 = new Colectivo("142");
            TiempoFalso tiempo = new TiempoFalso(2024, 10, 14, 8, 0, 0);

            Boleto b1 = colectivoK.PagarCon(tarjeta, tiempo);
            Assert.AreEqual(1580, b1.Monto);
            Assert.AreEqual(8420, tarjeta.Saldo);

            tiempo.AgregarMinutos(30);

            Boleto b2 = colectivo142.PagarCon(tarjeta, tiempo);
            Assert.AreEqual(0, b2.Monto);
            Assert.AreEqual(8420, tarjeta.Saldo);
        }

        [Test]
        public void TestTrasbordoMismaLineaNoAplica()
        {
            Tarjeta tarjeta = new Tarjeta();
            tarjeta.Cargar(10000);
            Colectivo colectivo = new Colectivo("K");
            TiempoFalso tiempo = new TiempoFalso(2024, 10, 14, 8, 0, 0);

            Boleto b1 = colectivo.PagarCon(tarjeta, tiempo);
            Assert.AreEqual(1580, b1.Monto);

            tiempo.AgregarMinutos(30);

            Boleto b2 = colectivo.PagarCon(tarjeta, tiempo);
            Assert.AreEqual(1580, b2.Monto);
        }

        [Test]
        public void TestTrasbordoDespuesDe1HoraNoAplica()
        {
            Tarjeta tarjeta = new Tarjeta();
            tarjeta.Cargar(10000);
            Colectivo colectivoK = new Colectivo("K");
            Colectivo colectivo142 = new Colectivo("142");
            TiempoFalso tiempo = new TiempoFalso(2024, 10, 14, 8, 0, 0);

            Boleto b1 = colectivoK.PagarCon(tarjeta, tiempo);
            Assert.AreEqual(1580, b1.Monto);

            tiempo.AgregarMinutos(61);

            Boleto b2 = colectivo142.PagarCon(tarjeta, tiempo);
            Assert.AreEqual(1580, b2.Monto);
        }

        [Test]
        public void TestTrasbordoExactamente1HoraAplica()
        {
            Tarjeta tarjeta = new Tarjeta();
            tarjeta.Cargar(10000);
            Colectivo colectivoK = new Colectivo("K");
            Colectivo colectivo142 = new Colectivo("142");
            TiempoFalso tiempo = new TiempoFalso(2024, 10, 14, 8, 0, 0);

            Boleto b1 = colectivoK.PagarCon(tarjeta, tiempo);
            Assert.AreEqual(1580, b1.Monto);

            tiempo.AgregarMinutos(60);

            Boleto b2 = colectivo142.PagarCon(tarjeta, tiempo);
            Assert.AreEqual(0, b2.Monto);
        }

        [Test]
        public void TestTrasbordoDomingoNoAplica()
        {
            Tarjeta tarjeta = new Tarjeta();
            tarjeta.Cargar(10000);
            Colectivo colectivoK = new Colectivo("K");
            Colectivo colectivo142 = new Colectivo("142");
            TiempoFalso tiempo = new TiempoFalso(2024, 10, 13, 10, 0, 0);

            Boleto b1 = colectivoK.PagarCon(tarjeta, tiempo);
            Assert.AreEqual(1580, b1.Monto);

            tiempo.AgregarMinutos(30);

            Boleto b2 = colectivo142.PagarCon(tarjeta, tiempo);
            Assert.AreEqual(1580, b2.Monto);
        }

        [Test]
        public void TestTrasbordoAntesDe7AmNoAplica()
        {
            Tarjeta tarjeta = new Tarjeta();
            tarjeta.Cargar(10000);
            Colectivo colectivoK = new Colectivo("K");
            Colectivo colectivo142 = new Colectivo("142");
            TiempoFalso tiempo = new TiempoFalso(2024, 10, 14, 6, 30, 0);

            Boleto b1 = colectivoK.PagarCon(tarjeta, tiempo);
            Assert.AreEqual(1580, b1.Monto);

            tiempo.AgregarMinutos(20);

            Boleto b2 = colectivo142.PagarCon(tarjeta, tiempo);
            Assert.AreEqual(1580, b2.Monto);
        }

        [Test]
        public void TestTrasbordoDespuesDe22NoAplica()
        {
            Tarjeta tarjeta = new Tarjeta();
            tarjeta.Cargar(10000);
            Colectivo colectivoK = new Colectivo("K");
            Colectivo colectivo142 = new Colectivo("142");
            TiempoFalso tiempo = new TiempoFalso(2024, 10, 14, 21, 50, 0);

            Boleto b1 = colectivoK.PagarCon(tarjeta, tiempo);
            Assert.AreEqual(1580, b1.Monto);

            tiempo.AgregarMinutos(20);

            Boleto b2 = colectivo142.PagarCon(tarjeta, tiempo);
            Assert.AreEqual(1580, b2.Monto);
        }

        [Test]
        public void TestTrasbordoLunesAplica()
        {
            Tarjeta tarjeta = new Tarjeta();
            tarjeta.Cargar(10000);
            Colectivo colectivoK = new Colectivo("K");
            Colectivo colectivo142 = new Colectivo("142");
            TiempoFalso tiempo = new TiempoFalso(2024, 10, 14, 10, 0, 0);

            colectivoK.PagarCon(tarjeta, tiempo);
            tiempo.AgregarMinutos(30);

            bool esTrasbordo = tarjeta.EsTrasbordo("142", tiempo);
            Assert.IsTrue(esTrasbordo);
        }

        [Test]
        public void TestTrasbordoSabadoAplica()
        {
            Tarjeta tarjeta = new Tarjeta();
            tarjeta.Cargar(10000);
            Colectivo colectivoK = new Colectivo("K");
            Colectivo colectivo142 = new Colectivo("142");
            TiempoFalso tiempo = new TiempoFalso(2024, 10, 19, 10, 0, 0);

            colectivoK.PagarCon(tarjeta, tiempo);
            tiempo.AgregarMinutos(30);

            bool esTrasbordo = tarjeta.EsTrasbordo("142", tiempo);
            Assert.IsTrue(esTrasbordo);
        }

        [Test]
        public void TestMultiplesTrasbordos()
        {
            Tarjeta tarjeta = new Tarjeta();
            tarjeta.Cargar(10000);
            Colectivo colectivoK = new Colectivo("K");
            Colectivo colectivo142 = new Colectivo("142");
            Colectivo colectivo133 = new Colectivo("133");
            TiempoFalso tiempo = new TiempoFalso(2024, 10, 14, 8, 0, 0);

            Boleto b1 = colectivoK.PagarCon(tarjeta, tiempo);
            Assert.AreEqual(1580, b1.Monto);

            tiempo.AgregarMinutos(20);

            Boleto b2 = colectivo142.PagarCon(tarjeta, tiempo);
            Assert.AreEqual(0, b2.Monto);

            tiempo.AgregarMinutos(20);

            Boleto b3 = colectivo133.PagarCon(tarjeta, tiempo);
            Assert.AreEqual(0, b3.Monto);
        }

        [Test]
        public void TestTrasbordoNoAplicaSiPrimerViajeNull()
        {
            Tarjeta tarjeta = new Tarjeta();
            tarjeta.Cargar(10000);
            Colectivo colectivo = new Colectivo("K");
            TiempoFalso tiempo = new TiempoFalso(2024, 10, 14, 8, 0, 0);

            bool esTrasbordo = tarjeta.EsTrasbordo("K", tiempo);
            Assert.IsFalse(esTrasbordo);
        }

        [Test]
        public void TestTrasbordoConMedioBoleto()
        {
            MedioBoleto tarjeta = new MedioBoleto();
            tarjeta.Cargar(10000);
            Colectivo colectivoK = new Colectivo("K");
            Colectivo colectivo142 = new Colectivo("142");
            TiempoFalso tiempo = new TiempoFalso(2024, 10, 14, 8, 0, 0);

            Boleto b1 = colectivoK.PagarCon(tarjeta, tiempo);
            Assert.AreEqual(790, b1.Monto);

            tiempo.AgregarMinutos(30);

            Boleto b2 = colectivo142.PagarCon(tarjeta, tiempo);
            Assert.AreEqual(0, b2.Monto);
        }

        [Test]
        public void TestTrasbordoConFranquiciaCompleta()
        {
            FranquiciaCompleta tarjeta = new FranquiciaCompleta();
            Colectivo colectivoK = new Colectivo("K");
            Colectivo colectivo142 = new Colectivo("142");
            TiempoFalso tiempo = new TiempoFalso(2024, 10, 14, 8, 0, 0);

            Boleto b1 = colectivoK.PagarCon(tarjeta, tiempo);
            Assert.AreEqual(0, b1.Monto);

            tiempo.AgregarMinutos(30);

            Boleto b2 = colectivo142.PagarCon(tarjeta, tiempo);
            Assert.AreEqual(0, b2.Monto);
        }

        [Test]
        public void TestTrasbordoConBoletoGratuito()
        {
            BoletoGratuito tarjeta = new BoletoGratuito();
            tarjeta.Cargar(10000);
            Colectivo colectivoK = new Colectivo("K");
            Colectivo colectivo142 = new Colectivo("142");
            TiempoFalso tiempo = new TiempoFalso(2024, 10, 14, 8, 0, 0);

            Boleto b1 = colectivoK.PagarCon(tarjeta, tiempo);
            Assert.AreEqual(0, b1.Monto);

            tiempo.AgregarMinutos(30);

            Boleto b2 = colectivo142.PagarCon(tarjeta, tiempo);
            Assert.AreEqual(0, b2.Monto);
        }

        [Test]
        public void TestTrasbordoDespuesDeTrasbordoResetea()
        {
            Tarjeta tarjeta = new Tarjeta();
            tarjeta.Cargar(10000);
            Colectivo colectivoK = new Colectivo("K");
            Colectivo colectivo142 = new Colectivo("142");
            TiempoFalso tiempo = new TiempoFalso(2024, 10, 14, 8, 0, 0);

            Boleto b1 = colectivoK.PagarCon(tarjeta, tiempo);
            Assert.AreEqual(1580, b1.Monto);

            tiempo.AgregarMinutos(20);

            Boleto b2 = colectivo142.PagarCon(tarjeta, tiempo);
            Assert.AreEqual(0, b2.Monto);

            tiempo.AgregarMinutos(20);

            Boleto b3 = colectivoK.PagarCon(tarjeta, tiempo);
            Assert.AreEqual(0, b3.Monto);
        }

        [Test]
        public void TestTrasbordoA7AmExactoAplica()
        {
            Tarjeta tarjeta = new Tarjeta();
            tarjeta.Cargar(10000);
            Colectivo colectivoK = new Colectivo("K");
            Colectivo colectivo142 = new Colectivo("142");
            TiempoFalso tiempo = new TiempoFalso(2024, 10, 14, 6, 30, 0);

            Boleto b1 = colectivoK.PagarCon(tarjeta, tiempo);
            Assert.AreEqual(1580, b1.Monto);

            tiempo.AgregarMinutos(30);

            Boleto b2 = colectivo142.PagarCon(tarjeta, tiempo);
            Assert.AreEqual(0, b2.Monto);
        }

        [Test]
        public void TestTrasbordoA21_59Aplica()
        {
            Tarjeta tarjeta = new Tarjeta();
            tarjeta.Cargar(10000);
            Colectivo colectivoK = new Colectivo("K");
            Colectivo colectivo142 = new Colectivo("142");
            TiempoFalso tiempo = new TiempoFalso(2024, 10, 14, 21, 30, 0);

            Boleto b1 = colectivoK.PagarCon(tarjeta, tiempo);
            Assert.AreEqual(1580, b1.Monto);

            tiempo.AgregarMinutos(25);

            Boleto b2 = colectivo142.PagarCon(tarjeta, tiempo);
            Assert.AreEqual(0, b2.Monto);
        }

        [Test]
        public void TestTrasbordoSinSaldoSuficiente()
        {
            Tarjeta tarjeta = new Tarjeta();
            tarjeta.Cargar(2000);
            Colectivo colectivoK = new Colectivo("K");
            Colectivo colectivo142 = new Colectivo("142");
            TiempoFalso tiempo = new TiempoFalso(2024, 10, 14, 8, 0, 0);

            Boleto b1 = colectivoK.PagarCon(tarjeta, tiempo);
            Assert.AreEqual(1580, b1.Monto);
            Assert.AreEqual(420, tarjeta.Saldo);

            tiempo.AgregarMinutos(30);

            Boleto b2 = colectivo142.PagarCon(tarjeta, tiempo);
            Assert.IsNotNull(b2);
            Assert.AreEqual(0, b2.Monto);
            Assert.AreEqual(420, tarjeta.Saldo);
        }

        [Test]
        public void TestTrasbordoMantieneSaldoPendiente()
        {
            Tarjeta tarjeta = new Tarjeta();
            tarjeta.Cargar(30000);
            tarjeta.Cargar(30000);
            Colectivo colectivoK = new Colectivo("K");
            Colectivo colectivo142 = new Colectivo("142");
            TiempoFalso tiempo = new TiempoFalso(2024, 10, 14, 8, 0, 0);

            Assert.AreEqual(56000, tarjeta.Saldo);
            Assert.AreEqual(4000, tarjeta.SaldoPendiente);

            Boleto b1 = colectivoK.PagarCon(tarjeta, tiempo);
            Assert.AreEqual(1580, b1.Monto);

            tiempo.AgregarMinutos(30);

            Boleto b2 = colectivo142.PagarCon(tarjeta, tiempo);
            Assert.AreEqual(0, b2.Monto);
            Assert.AreEqual(56000, tarjeta.Saldo);
        }

        [Test]
        public void TestTrasbordoVolverALineaOriginalDespuesDeTrasbordo()
        {
            Tarjeta tarjeta = new Tarjeta();
            tarjeta.Cargar(10000);
            Colectivo colectivoK = new Colectivo("K");
            Colectivo colectivo142 = new Colectivo("142");
            TiempoFalso tiempo = new TiempoFalso(2024, 10, 14, 8, 0, 0);

            Boleto b1 = colectivoK.PagarCon(tarjeta, tiempo);
            Assert.AreEqual(1580, b1.Monto);

            tiempo.AgregarMinutos(20);

            Boleto b2 = colectivo142.PagarCon(tarjeta, tiempo);
            Assert.AreEqual(0, b2.Monto);

            tiempo.AgregarMinutos(20);

            Boleto b3 = colectivoK.PagarCon(tarjeta, tiempo);
            Assert.AreEqual(0, b3.Monto);
        }

        [Test]
        public void TestTrasbordoViernesAplica()
        {
            Tarjeta tarjeta = new Tarjeta();
            tarjeta.Cargar(10000);
            Colectivo colectivoK = new Colectivo("K");
            Colectivo colectivo142 = new Colectivo("142");
            TiempoFalso tiempo = new TiempoFalso(2024, 10, 18, 10, 0, 0);

            colectivoK.PagarCon(tarjeta, tiempo);
            tiempo.AgregarMinutos(30);

            bool esTrasbordo = tarjeta.EsTrasbordo("142", tiempo);
            Assert.IsTrue(esTrasbordo);
        }

        [Test]
        public void TestTrasbordoMartesAplica()
        {
            Tarjeta tarjeta = new Tarjeta();
            tarjeta.Cargar(10000);
            Colectivo colectivoK = new Colectivo("K");
            TiempoFalso tiempo = new TiempoFalso(2024, 10, 15, 10, 0, 0);

            colectivoK.PagarCon(tarjeta, tiempo);
            tiempo.AgregarMinutos(30);

            bool esTrasbordo = tarjeta.EsTrasbordo("142", tiempo);
            Assert.IsTrue(esTrasbordo);
        }

        [Test]
        public void TestTrasbordoCon59MinutosAplica()
        {
            Tarjeta tarjeta = new Tarjeta();
            tarjeta.Cargar(10000);
            Colectivo colectivoK = new Colectivo("K");
            TiempoFalso tiempo = new TiempoFalso(2024, 10, 14, 8, 0, 0);

            colectivoK.PagarCon(tarjeta, tiempo);
            tiempo.AgregarMinutos(59);

            bool esTrasbordo = tarjeta.EsTrasbordo("142", tiempo);
            Assert.IsTrue(esTrasbordo);
        }

        [Test]
        public void TestTrasbordoGetterEnBoleto()
        {
            Tarjeta tarjeta = new Tarjeta();
            tarjeta.Cargar(10000);
            Colectivo colectivoK = new Colectivo("K");
            Colectivo colectivo142 = new Colectivo("142");
            TiempoFalso tiempo = new TiempoFalso(2024, 10, 14, 8, 0, 0);

            Boleto b1 = colectivoK.PagarCon(tarjeta, tiempo);
            Assert.IsFalse(b1.EsTrasbordo);

            tiempo.AgregarMinutos(30);

            Boleto b2 = colectivo142.PagarCon(tarjeta, tiempo);
            Assert.IsTrue(b2.EsTrasbordo);
        }

        [Test]
        public void TestTrasbordoNoAplicaSiLineaEsNull()
        {
            Tarjeta tarjeta = new Tarjeta();
            TiempoFalso tiempo = new TiempoFalso(2024, 10, 14, 8, 0, 0);

            bool esTrasbordo = tarjeta.EsTrasbordo("K", tiempo);
            Assert.IsFalse(esTrasbordo);
        }

        [Test]
        public void TestTrasbordoTresDiferentesLineasEnUnaHora()
        {
            Tarjeta tarjeta = new Tarjeta();
            tarjeta.Cargar(10000);
            Colectivo colectivoK = new Colectivo("K");
            Colectivo colectivo142 = new Colectivo("142");
            Colectivo colectivo133 = new Colectivo("133");
            Colectivo colectivo115 = new Colectivo("115");
            TiempoFalso tiempo = new TiempoFalso(2024, 10, 14, 8, 0, 0);

            Boleto b1 = colectivoK.PagarCon(tarjeta, tiempo);
            Assert.AreEqual(1580, b1.Monto);
            Assert.IsFalse(b1.EsTrasbordo);

            tiempo.AgregarMinutos(15);

            Boleto b2 = colectivo142.PagarCon(tarjeta, tiempo);
            Assert.AreEqual(0, b2.Monto);
            Assert.IsTrue(b2.EsTrasbordo);

            tiempo.AgregarMinutos(15);

            Boleto b3 = colectivo133.PagarCon(tarjeta, tiempo);
            Assert.AreEqual(0, b3.Monto);
            Assert.IsTrue(b3.EsTrasbordo);

            tiempo.AgregarMinutos(15);

            Boleto b4 = colectivo115.PagarCon(tarjeta, tiempo);
            Assert.AreEqual(0, b4.Monto);
            Assert.IsTrue(b4.EsTrasbordo);
        }

        [Test]
        public void TestTrasbordoConDiferentesTarjetas()
        {
            Tarjeta normal = new Tarjeta();
            MedioBoleto medio = new MedioBoleto();
            BoletoGratuito gratuito = new BoletoGratuito();
            FranquiciaCompleta franquicia = new FranquiciaCompleta();

            normal.Cargar(10000);
            medio.Cargar(10000);
            gratuito.Cargar(10000);

            Colectivo colectivoK = new Colectivo("K");
            Colectivo colectivo142 = new Colectivo("142");
            TiempoFalso tiempo = new TiempoFalso(2024, 10, 14, 8, 0, 0);

            colectivoK.PagarCon(normal, tiempo);
            tiempo.AgregarMinutos(30);
            Boleto b1 = colectivo142.PagarCon(normal, tiempo);
            Assert.IsTrue(b1.EsTrasbordo);

            tiempo = new TiempoFalso(2024, 10, 14, 8, 0, 0);
            colectivoK.PagarCon(medio, tiempo);
            tiempo.AgregarMinutos(30);
            Boleto b2 = colectivo142.PagarCon(medio, tiempo);
            Assert.IsTrue(b2.EsTrasbordo);

            tiempo = new TiempoFalso(2024, 10, 14, 8, 0, 0);
            colectivoK.PagarCon(gratuito, tiempo);
            tiempo.AgregarMinutos(30);
            Boleto b3 = colectivo142.PagarCon(gratuito, tiempo);
            Assert.IsTrue(b3.EsTrasbordo);

            tiempo = new TiempoFalso(2024, 10, 14, 8, 0, 0);
            colectivoK.PagarCon(franquicia, tiempo);
            tiempo.AgregarMinutos(30);
            Boleto b4 = colectivo142.PagarCon(franquicia, tiempo);
            Assert.IsTrue(b4.EsTrasbordo);
        }
    }
}