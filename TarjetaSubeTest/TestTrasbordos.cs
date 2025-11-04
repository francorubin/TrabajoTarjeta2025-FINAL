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
            TiempoFalso tiempo = new TiempoFalso(2024, 11, 4, 10, 0, 0);

            Boleto boleto1 = colectivoK.PagarCon(tarjeta, tiempo);
            Assert.IsNotNull(boleto1);
            Assert.AreEqual(1580, boleto1.Monto);
            Assert.IsFalse(boleto1.EsTrasbordo);
            Assert.AreEqual(8420, tarjeta.Saldo);

            tiempo.AgregarMinutos(30);

            Boleto boleto2 = colectivo142.PagarCon(tarjeta, tiempo);
            Assert.IsNotNull(boleto2);
            Assert.AreEqual(0, boleto2.Monto);
            Assert.IsTrue(boleto2.EsTrasbordo);
            Assert.AreEqual(8420, tarjeta.Saldo);
        }

        [Test]
        public void TestTrasbordoMismaLineaNoAplica()
        {
            Tarjeta tarjeta = new Tarjeta();
            tarjeta.Cargar(10000);
            Colectivo colectivoK = new Colectivo("K");
            TiempoFalso tiempo = new TiempoFalso(2024, 11, 4, 10, 0, 0);

            Boleto boleto1 = colectivoK.PagarCon(tarjeta, tiempo);
            Assert.AreEqual(1580, boleto1.Monto);
            Assert.IsFalse(boleto1.EsTrasbordo);

            tiempo.AgregarMinutos(30);

            Boleto boleto2 = colectivoK.PagarCon(tarjeta, tiempo);
            Assert.AreEqual(1580, boleto2.Monto);
            Assert.IsFalse(boleto2.EsTrasbordo);
            Assert.AreEqual(6840, tarjeta.Saldo);
        }

        [Test]
        public void TestTrasbordoDespuesDe1HoraNoAplica()
        {
            Tarjeta tarjeta = new Tarjeta();
            tarjeta.Cargar(10000);
            Colectivo colectivoK = new Colectivo("K");
            Colectivo colectivo142 = new Colectivo("142");
            TiempoFalso tiempo = new TiempoFalso(2024, 11, 4, 10, 0, 0);

            Boleto boleto1 = colectivoK.PagarCon(tarjeta, tiempo);
            Assert.AreEqual(1580, boleto1.Monto);
            Assert.IsFalse(boleto1.EsTrasbordo);

            tiempo.AgregarMinutos(61);

            Boleto boleto2 = colectivo142.PagarCon(tarjeta, tiempo);
            Assert.AreEqual(1580, boleto2.Monto);
            Assert.IsFalse(boleto2.EsTrasbordo);
            Assert.AreEqual(6840, tarjeta.Saldo);
        }

        [Test]
        public void TestTrasbordoExactamente1HoraAplica()
        {
            Tarjeta tarjeta = new Tarjeta();
            tarjeta.Cargar(10000);
            Colectivo colectivoK = new Colectivo("K");
            Colectivo colectivo142 = new Colectivo("142");
            TiempoFalso tiempo = new TiempoFalso(2024, 11, 4, 10, 0, 0);

            Boleto boleto1 = colectivoK.PagarCon(tarjeta, tiempo);
            Assert.AreEqual(1580, boleto1.Monto);

            tiempo.AgregarMinutos(60);

            Boleto boleto2 = colectivo142.PagarCon(tarjeta, tiempo);
            Assert.AreEqual(0, boleto2.Monto);
            Assert.IsTrue(boleto2.EsTrasbordo);
        }

        [Test]
        public void TestTrasbordoDomingoNoAplica()
        {
            Tarjeta tarjeta = new Tarjeta();
            tarjeta.Cargar(10000);
            Colectivo colectivoK = new Colectivo("K");
            Colectivo colectivo142 = new Colectivo("142");
            TiempoFalso tiempo = new TiempoFalso(2024, 11, 3, 10, 0, 0);

            Boleto boleto1 = colectivoK.PagarCon(tarjeta, tiempo);
            Assert.AreEqual(1580, boleto1.Monto);

            tiempo.AgregarMinutos(30);

            Boleto boleto2 = colectivo142.PagarCon(tarjeta, tiempo);
            Assert.AreEqual(1580, boleto2.Monto);
            Assert.IsFalse(boleto2.EsTrasbordo);
        }

        [Test]
        public void TestTrasbordoAntesDe7AmNoAplica()
        {
            Tarjeta tarjeta = new Tarjeta();
            tarjeta.Cargar(10000);
            Colectivo colectivoK = new Colectivo("K");
            Colectivo colectivo142 = new Colectivo("142");
            TiempoFalso tiempo = new TiempoFalso(2024, 11, 4, 6, 30, 0);

            Boleto boleto1 = colectivoK.PagarCon(tarjeta, tiempo);
            Assert.AreEqual(1580, boleto1.Monto);

            tiempo.AgregarMinutos(30);

            Boleto boleto2 = colectivo142.PagarCon(tarjeta, tiempo);
            Assert.AreEqual(1580, boleto2.Monto);
            Assert.IsFalse(boleto2.EsTrasbordo);
        }

        [Test]
        public void TestTrasbordoDespuesDe22HsNoAplica()
        {
            Tarjeta tarjeta = new Tarjeta();
            tarjeta.Cargar(10000);
            Colectivo colectivoK = new Colectivo("K");
            Colectivo colectivo142 = new Colectivo("142");
            TiempoFalso tiempo = new TiempoFalso(2024, 11, 4, 21, 50, 0);

            Boleto boleto1 = colectivoK.PagarCon(tarjeta, tiempo);
            Assert.AreEqual(1580, boleto1.Monto);

            tiempo.AgregarMinutos(30);

            Boleto boleto2 = colectivo142.PagarCon(tarjeta, tiempo);
            Assert.AreEqual(1580, boleto2.Monto);
            Assert.IsFalse(boleto2.EsTrasbordo);
        }

        [Test]
        public void TestTrasbordoLunesAplica()
        {
            Tarjeta tarjeta = new Tarjeta();
            tarjeta.Cargar(10000);
            Colectivo colectivoK = new Colectivo("K");
            Colectivo colectivo142 = new Colectivo("142");
            TiempoFalso tiempo = new TiempoFalso(2024, 11, 4, 10, 0, 0);

            Boleto boleto1 = colectivoK.PagarCon(tarjeta, tiempo);
            tiempo.AgregarMinutos(30);
            Boleto boleto2 = colectivo142.PagarCon(tarjeta, tiempo);

            Assert.IsTrue(boleto2.EsTrasbordo);
        }

        [Test]
        public void TestTrasbordoSabadoAplica()
        {
            Tarjeta tarjeta = new Tarjeta();
            tarjeta.Cargar(10000);
            Colectivo colectivoK = new Colectivo("K");
            Colectivo colectivo142 = new Colectivo("142");
            TiempoFalso tiempo = new TiempoFalso(2024, 11, 9, 10, 0, 0);

            Boleto boleto1 = colectivoK.PagarCon(tarjeta, tiempo);
            tiempo.AgregarMinutos(30);
            Boleto boleto2 = colectivo142.PagarCon(tarjeta, tiempo);

            Assert.IsTrue(boleto2.EsTrasbordo);
        }

        [Test]
        public void TestMultiplesTrasbordos()
        {
            Tarjeta tarjeta = new Tarjeta();
            tarjeta.Cargar(10000);
            Colectivo colectivoK = new Colectivo("K");
            Colectivo colectivo142 = new Colectivo("142");
            Colectivo colectivo133 = new Colectivo("133");
            TiempoFalso tiempo = new TiempoFalso(2024, 11, 4, 10, 0, 0);

            Boleto boleto1 = colectivoK.PagarCon(tarjeta, tiempo);
            Assert.AreEqual(1580, boleto1.Monto);
            Assert.IsFalse(boleto1.EsTrasbordo);

            tiempo.AgregarMinutos(20);

            Boleto boleto2 = colectivo142.PagarCon(tarjeta, tiempo);
            Assert.AreEqual(0, boleto2.Monto);
            Assert.IsTrue(boleto2.EsTrasbordo);

            tiempo.AgregarMinutos(20);

            Boleto boleto3 = colectivo133.PagarCon(tarjeta, tiempo);
            Assert.AreEqual(0, boleto3.Monto);
            Assert.IsTrue(boleto3.EsTrasbordo);

            Assert.AreEqual(8420, tarjeta.Saldo);
        }

        [Test]
        public void TestTrasbordoConMedioBoleto()
        {
            MedioBoleto tarjeta = new MedioBoleto();
            tarjeta.Cargar(10000);
            Colectivo colectivoK = new Colectivo("K");
            Colectivo colectivo142 = new Colectivo("142");
            TiempoFalso tiempo = new TiempoFalso(2024, 11, 4, 10, 0, 0);

            Boleto boleto1 = colectivoK.PagarCon(tarjeta, tiempo);
            Assert.AreEqual(790, boleto1.Monto);
            Assert.IsFalse(boleto1.EsTrasbordo);

            tiempo.AgregarMinutos(30);

            Boleto boleto2 = colectivo142.PagarCon(tarjeta, tiempo);
            Assert.AreEqual(0, boleto2.Monto);
            Assert.IsTrue(boleto2.EsTrasbordo);

            Assert.AreEqual(9210, tarjeta.Saldo);
        }

        [Test]
        public void TestTrasbordoConBoletoGratuito()
        {
            BoletoGratuito tarjeta = new BoletoGratuito();
            tarjeta.Cargar(10000);
            Colectivo colectivoK = new Colectivo("K");
            Colectivo colectivo142 = new Colectivo("142");
            TiempoFalso tiempo = new TiempoFalso(2024, 11, 4, 10, 0, 0);

            Boleto boleto1 = colectivoK.PagarCon(tarjeta, tiempo);
            Assert.AreEqual(0, boleto1.Monto);
            Assert.IsFalse(boleto1.EsTrasbordo);

            tiempo.AgregarMinutos(30);

            Boleto boleto2 = colectivo142.PagarCon(tarjeta, tiempo);
            Assert.AreEqual(0, boleto2.Monto);
            Assert.IsTrue(boleto2.EsTrasbordo);

            Assert.AreEqual(10000, tarjeta.Saldo);
        }

        [Test]
        public void TestTrasbordoConFranquiciaCompleta()
        {
            FranquiciaCompleta tarjeta = new FranquiciaCompleta();
            tarjeta.Cargar(10000);
            Colectivo colectivoK = new Colectivo("K");
            Colectivo colectivo142 = new Colectivo("142");
            TiempoFalso tiempo = new TiempoFalso(2024, 11, 4, 10, 0, 0);

            Boleto boleto1 = colectivoK.PagarCon(tarjeta, tiempo);
            Assert.AreEqual(0, boleto1.Monto);
            Assert.IsFalse(boleto1.EsTrasbordo);

            tiempo.AgregarMinutos(30);

            Boleto boleto2 = colectivo142.PagarCon(tarjeta, tiempo);
            Assert.AreEqual(0, boleto2.Monto);
            Assert.IsTrue(boleto2.EsTrasbordo);

            Assert.AreEqual(10000, tarjeta.Saldo);
        }

        [Test]
        public void TestTrasbordoDespuesDeTrasbordoResetea()
        {
            Tarjeta tarjeta = new Tarjeta();
            tarjeta.Cargar(10000);
            Colectivo colectivoK = new Colectivo("K");
            Colectivo colectivo142 = new Colectivo("142");
            TiempoFalso tiempo = new TiempoFalso(2024, 11, 4, 10, 0, 0);

            Boleto boleto1 = colectivoK.PagarCon(tarjeta, tiempo);
            Assert.AreEqual(1580, boleto1.Monto);
            Assert.IsFalse(boleto1.EsTrasbordo);

            tiempo.AgregarMinutos(30);

            Boleto boleto2 = colectivo142.PagarCon(tarjeta, tiempo);
            Assert.AreEqual(0, boleto2.Monto);
            Assert.IsTrue(boleto2.EsTrasbordo);

            tiempo.AgregarMinutos(70);

            Boleto boleto3 = colectivoK.PagarCon(tarjeta, tiempo);
            Assert.AreEqual(1580, boleto3.Monto);
            Assert.IsFalse(boleto3.EsTrasbordo);
        }

        [Test]
        public void TestTrasbordoA7AmExactoAplica()
        {
            Tarjeta tarjeta = new Tarjeta();
            tarjeta.Cargar(10000);
            Colectivo colectivoK = new Colectivo("K");
            Colectivo colectivo142 = new Colectivo("142");
            TiempoFalso tiempo = new TiempoFalso(2024, 11, 4, 6, 50, 0);

            Boleto boleto1 = colectivoK.PagarCon(tarjeta, tiempo);
            Assert.AreEqual(1580, boleto1.Monto);

            tiempo.AgregarMinutos(20);

            Boleto boleto2 = colectivo142.PagarCon(tarjeta, tiempo);
            Assert.AreEqual(0, boleto2.Monto);
            Assert.IsTrue(boleto2.EsTrasbordo);
        }

        [Test]
        public void TestTrasbordoA21_59Aplica()
        {
            Tarjeta tarjeta = new Tarjeta();
            tarjeta.Cargar(10000);
            Colectivo colectivoK = new Colectivo("K");
            Colectivo colectivo142 = new Colectivo("142");
            TiempoFalso tiempo = new TiempoFalso(2024, 11, 4, 21, 30, 0);

            Boleto boleto1 = colectivoK.PagarCon(tarjeta, tiempo);
            Assert.AreEqual(1580, boleto1.Monto);

            tiempo.AgregarMinutos(20);

            Boleto boleto2 = colectivo142.PagarCon(tarjeta, tiempo);
            Assert.AreEqual(0, boleto2.Monto);
            Assert.IsTrue(boleto2.EsTrasbordo);
        }

        [Test]
        public void TestTrasbordoSinSaldoSuficiente()
        {
            Tarjeta tarjeta = new Tarjeta();
            tarjeta.Cargar(2000);
            Colectivo colectivoK = new Colectivo("K");
            Colectivo colectivo142 = new Colectivo("142");
            TiempoFalso tiempo = new TiempoFalso(2024, 11, 4, 10, 0, 0);

            Boleto boleto1 = colectivoK.PagarCon(tarjeta, tiempo);
            Assert.AreEqual(1580, boleto1.Monto);
            Assert.AreEqual(420, tarjeta.Saldo);

            tiempo.AgregarMinutos(30);

            Boleto boleto2 = colectivo142.PagarCon(tarjeta, tiempo);
            Assert.IsNotNull(boleto2);
            Assert.AreEqual(0, boleto2.Monto);
            Assert.IsTrue(boleto2.EsTrasbordo);
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
            TiempoFalso tiempo = new TiempoFalso(2024, 11, 4, 10, 0, 0);

            Assert.AreEqual(56000, tarjeta.Saldo);
            Assert.AreEqual(4000, tarjeta.SaldoPendiente);

            Boleto boleto1 = colectivoK.PagarCon(tarjeta, tiempo);
            Assert.AreEqual(1580, boleto1.Monto);

            tiempo.AgregarMinutos(30);

            Boleto boleto2 = colectivo142.PagarCon(tarjeta, tiempo);
            Assert.AreEqual(0, boleto2.Monto);
            Assert.IsTrue(boleto2.EsTrasbordo);
            Assert.AreEqual(56000, tarjeta.Saldo);
        }

        [Test]
        public void TestTrasbordoVolverALineaOriginalDespuesDeTrasbordo()
        {
            Tarjeta tarjeta = new Tarjeta();
            tarjeta.Cargar(10000);
            Colectivo colectivoK = new Colectivo("K");
            Colectivo colectivo142 = new Colectivo("142");
            TiempoFalso tiempo = new TiempoFalso(2024, 11, 4, 10, 0, 0);

            Boleto boleto1 = colectivoK.PagarCon(tarjeta, tiempo);
            Assert.AreEqual(1580, boleto1.Monto);
            Assert.IsFalse(boleto1.EsTrasbordo);

            tiempo.AgregarMinutos(20);

            Boleto boleto2 = colectivo142.PagarCon(tarjeta, tiempo);
            Assert.AreEqual(0, boleto2.Monto);
            Assert.IsTrue(boleto2.EsTrasbordo);

            tiempo.AgregarMinutos(20);

            Boleto boleto3 = colectivoK.PagarCon(tarjeta, tiempo);
            Assert.AreEqual(0, boleto3.Monto);
            Assert.IsTrue(boleto3.EsTrasbordo);
        }

        [Test]
        public void TestBoletoSinTrasbordoTieneEsTrasbordoFalse()
        {
            Tarjeta tarjeta = new Tarjeta();
            tarjeta.Cargar(10000);
            Colectivo colectivo = new Colectivo("K");
            TiempoFalso tiempo = new TiempoFalso(2024, 11, 4, 10, 0, 0);

            Boleto boleto = colectivo.PagarCon(tarjeta, tiempo);
            Assert.IsFalse(boleto.EsTrasbordo);
        }

        [Test]
        public void TestTrasbordoViernesAplica()
        {
            Tarjeta tarjeta = new Tarjeta();
            tarjeta.Cargar(10000);
            Colectivo colectivoK = new Colectivo("K");
            Colectivo colectivo142 = new Colectivo("142");
            TiempoFalso tiempo = new TiempoFalso(2024, 11, 8, 10, 0, 0);

            Boleto boleto1 = colectivoK.PagarCon(tarjeta, tiempo);
            tiempo.AgregarMinutos(30);
            Boleto boleto2 = colectivo142.PagarCon(tarjeta, tiempo);

            Assert.IsTrue(boleto2.EsTrasbordo);
        }

        [Test]
        public void TestPrimerViajeDelDiaNuncaEsTrasbordo()
        {
            Tarjeta tarjeta = new Tarjeta();
            tarjeta.Cargar(10000);
            Colectivo colectivo = new Colectivo("K");
            TiempoFalso tiempo = new TiempoFalso(2024, 11, 4, 10, 0, 0);

            Boleto boleto = colectivo.PagarCon(tarjeta, tiempo);
            Assert.IsFalse(boleto.EsTrasbordo);
            Assert.AreEqual(1580, boleto.Monto);
        }

        [Test]
        public void TestTrasbordoConDescuentoUsoFrecuente()
        {
            Tarjeta tarjeta = new Tarjeta();
            tarjeta.Cargar(30000);
            tarjeta.Cargar(30000);
            Colectivo colectivoK = new Colectivo("K");
            Colectivo colectivo142 = new Colectivo("142");
            TiempoFalso tiempo = new TiempoFalso(2024, 11, 1, 10, 0, 0);

            for (int i = 0; i < 29; i++)
            {
                colectivoK.PagarCon(tarjeta, tiempo);
                tiempo.AgregarMinutos(70);
            }

            Boleto boleto30 = colectivoK.PagarCon(tarjeta, tiempo);
            Assert.AreEqual(1264, boleto30.Monto);
            Assert.IsFalse(boleto30.EsTrasbordo);

            tiempo.AgregarMinutos(30);

            Boleto boletoTrasbordo = colectivo142.PagarCon(tarjeta, tiempo);
            Assert.AreEqual(0, boletoTrasbordo.Monto);
            Assert.IsTrue(boletoTrasbordo.EsTrasbordo);
        }

        [Test]
        public void TestTrasbordoCon59MinutosAplica()
        {
            Tarjeta tarjeta = new Tarjeta();
            tarjeta.Cargar(10000);
            Colectivo colectivoK = new Colectivo("K");
            Colectivo colectivo142 = new Colectivo("142");
            TiempoFalso tiempo = new TiempoFalso(2024, 11, 4, 10, 0, 0);

            Boleto boleto1 = colectivoK.PagarCon(tarjeta, tiempo);
            tiempo.AgregarMinutos(59);

            Boleto boleto2 = colectivo142.PagarCon(tarjeta, tiempo);
            Assert.IsTrue(boleto2.EsTrasbordo);
        }

        [Test]
        public void TestTrasbordoGetterEnBoleto()
        {
            Tarjeta tarjeta = new Tarjeta();
            tarjeta.Cargar(10000);
            Colectivo colectivoK = new Colectivo("K");
            Colectivo colectivo142 = new Colectivo("142");
            TiempoFalso tiempo = new TiempoFalso(2024, 11, 4, 10, 0, 0);

            Boleto boleto1 = colectivoK.PagarCon(tarjeta, tiempo);
            bool esTrasbordo1 = boleto1.EsTrasbordo;
            Assert.IsFalse(esTrasbordo1);

            tiempo.AgregarMinutos(30);

            Boleto boleto2 = colectivo142.PagarCon(tarjeta, tiempo);
            bool esTrasbordo2 = boleto2.EsTrasbordo;
            Assert.IsTrue(esTrasbordo2);
        }
    }
}