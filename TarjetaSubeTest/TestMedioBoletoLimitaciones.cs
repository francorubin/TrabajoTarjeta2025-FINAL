using NUnit.Framework;
using TarjetaSube;
using System;

namespace TarjetaSubeTest
{
    [TestFixture]
    public class TestMedioBoletoLimitaciones
    {
        [Test]
        public void TestMedioBoletoNoPermiteViajarAntesDe5Minutos()
        {
            MedioBoleto tarjeta = new MedioBoleto();
            tarjeta.Cargar(5000);
            Colectivo colectivo = new Colectivo("K");
            TiempoFalso tiempo = new TiempoFalso();

            Boleto boleto1 = colectivo.PagarCon(tarjeta, tiempo);
            Assert.IsNotNull(boleto1);
            Assert.AreEqual(790, boleto1.Monto);

            Boleto boleto2 = colectivo.PagarCon(tarjeta, tiempo);
            Assert.IsNull(boleto2);
            Assert.AreEqual(4210, tarjeta.Saldo);

            tiempo.AgregarMinutos(4);
            Boleto boleto3 = colectivo.PagarCon(tarjeta, tiempo);
            Assert.IsNull(boleto3);
            Assert.AreEqual(4210, tarjeta.Saldo);

            tiempo.AgregarMinutos(1);
            Boleto boleto4 = colectivo.PagarCon(tarjeta, tiempo);
            Assert.IsNotNull(boleto4);
            Assert.AreEqual(790, boleto4.Monto);
            Assert.AreEqual(3420, tarjeta.Saldo);
        }

        [Test]
        public void TestMedioBoletoPermiteViajarDespuesDe5Minutos()
        {
            MedioBoleto tarjeta = new MedioBoleto();
            tarjeta.Cargar(5000);
            Colectivo colectivo = new Colectivo("K");
            TiempoFalso tiempo = new TiempoFalso();

            Boleto boleto1 = colectivo.PagarCon(tarjeta, tiempo);
            Assert.IsNotNull(boleto1);

            tiempo.AgregarMinutos(5);

            Boleto boleto2 = colectivo.PagarCon(tarjeta, tiempo);
            Assert.IsNotNull(boleto2);
            Assert.AreEqual(790, boleto2.Monto);
        }

        [Test]
        public void TestMedioBoletoMaximo2ViajesPorDia()
        {
            MedioBoleto tarjeta = new MedioBoleto();
            tarjeta.Cargar(10000);
            Colectivo colectivo = new Colectivo("K");
            TiempoFalso tiempo = new TiempoFalso(2024, 10, 14, 8, 0, 0);

            Boleto boleto1 = colectivo.PagarCon(tarjeta, tiempo);
            Assert.IsNotNull(boleto1);
            Assert.AreEqual(790, boleto1.Monto);
            Assert.AreEqual(9210, tarjeta.Saldo);

            tiempo.AgregarMinutos(10);

            Boleto boleto2 = colectivo.PagarCon(tarjeta, tiempo);
            Assert.IsNotNull(boleto2);
            Assert.AreEqual(790, boleto2.Monto);
            Assert.AreEqual(8420, tarjeta.Saldo);

            tiempo.AgregarMinutos(10);

            Boleto boleto3 = colectivo.PagarCon(tarjeta, tiempo);
            Assert.IsNotNull(boleto3);
            Assert.AreEqual(1580, boleto3.Monto);
            Assert.AreEqual(6840, tarjeta.Saldo);
        }

        [Test]
        public void TestMedioBoletoTercerViajeCobraTarifaNormal()
        {
            MedioBoleto tarjeta = new MedioBoleto();
            tarjeta.Cargar(10000);
            Colectivo colectivo = new Colectivo("K");
            TiempoFalso tiempo = new TiempoFalso();

            colectivo.PagarCon(tarjeta, tiempo);
            tiempo.AgregarMinutos(5);

            colectivo.PagarCon(tarjeta, tiempo);
            tiempo.AgregarMinutos(5);

            Boleto boleto3 = colectivo.PagarCon(tarjeta, tiempo);
            Assert.AreEqual(1580, boleto3.Monto);
        }

        [Test]
        public void TestMedioBoletoContadorSeReiniciaDiaSiguiente()
        {
            MedioBoleto tarjeta = new MedioBoleto();
            tarjeta.Cargar(10000);
            Colectivo colectivo = new Colectivo("K");
            TiempoFalso tiempo = new TiempoFalso(2024, 10, 14, 8, 0, 0);

            Boleto b1 = colectivo.PagarCon(tarjeta, tiempo);
            tiempo.AgregarMinutos(10);
            Boleto b2 = colectivo.PagarCon(tarjeta, tiempo);

            Assert.AreEqual(790, b1.Monto);
            Assert.AreEqual(790, b2.Monto);

            tiempo.AgregarDias(1);

            Boleto b3 = colectivo.PagarCon(tarjeta, tiempo);
            tiempo.AgregarMinutos(10);
            Boleto b4 = colectivo.PagarCon(tarjeta, tiempo);

            Assert.AreEqual(790, b3.Monto);
            Assert.AreEqual(790, b4.Monto);
        }

        [Test]
        public void TestMedioBoletoExactamente5MinutosPermiteViajar()
        {
            MedioBoleto tarjeta = new MedioBoleto();
            tarjeta.Cargar(5000);
            Colectivo colectivo = new Colectivo("K");
            TiempoFalso tiempo = new TiempoFalso();

            colectivo.PagarCon(tarjeta, tiempo);
            tiempo.AgregarMinutos(5);

            Boleto boleto = colectivo.PagarCon(tarjeta, tiempo);
            Assert.IsNotNull(boleto);
        }

        [Test]
        public void TestMedioBoleto4Minutos59SegundosNoPermiteViajar()
        {
            MedioBoleto tarjeta = new MedioBoleto();
            tarjeta.Cargar(5000);
            Colectivo colectivo = new Colectivo("K");
            TiempoFalso tiempo = new TiempoFalso();

            colectivo.PagarCon(tarjeta, tiempo);
            tiempo.AgregarMinutos(4);
            tiempo.AgregarSegundos(59);

            Boleto boleto = colectivo.PagarCon(tarjeta, tiempo);
            Assert.IsNull(boleto);
        }

        [Test]
        public void TestMedioBoletoMultiplesViajes()
        {
            MedioBoleto tarjeta = new MedioBoleto();
            tarjeta.Cargar(20000);
            Colectivo colectivo = new Colectivo("K");
            TiempoFalso tiempo = new TiempoFalso();

            Boleto b1 = colectivo.PagarCon(tarjeta, tiempo);
            Assert.AreEqual(790, b1.Monto);
            Assert.AreEqual(19210, tarjeta.Saldo);

            tiempo.AgregarMinutos(10);

            Boleto b2 = colectivo.PagarCon(tarjeta, tiempo);
            Assert.AreEqual(790, b2.Monto);
            Assert.AreEqual(18420, tarjeta.Saldo);

            tiempo.AgregarMinutos(10);

            Boleto b3 = colectivo.PagarCon(tarjeta, tiempo);
            Assert.AreEqual(1580, b3.Monto);
            Assert.AreEqual(16840, tarjeta.Saldo);

            tiempo.AgregarMinutos(10);

            Boleto b4 = colectivo.PagarCon(tarjeta, tiempo);
            Assert.AreEqual(1580, b4.Monto);
            Assert.AreEqual(15260, tarjeta.Saldo);
        }

        [Test]
        public void TestMedioBoletoNuevoDiaReiniciaContador()
        {
            MedioBoleto tarjeta = new MedioBoleto();
            tarjeta.Cargar(20000);
            Colectivo colectivo = new Colectivo("K");
            TiempoFalso tiempo = new TiempoFalso(2024, 10, 14, 21, 50, 0);

            Boleto b1 = colectivo.PagarCon(tarjeta, tiempo);
            Assert.AreEqual(790, b1.Monto);

            tiempo.AgregarMinutos(600);

            Boleto b2 = colectivo.PagarCon(tarjeta, tiempo);
            Assert.AreEqual(790, b2.Monto);
        }

        [Test]
        public void TestMedioBoletoConSaldoNegativo()
        {
            MedioBoleto tarjeta = new MedioBoleto();
            tarjeta.Cargar(2000);
            Colectivo colectivo = new Colectivo("K");
            TiempoFalso tiempo = new TiempoFalso();

            Boleto b1 = colectivo.PagarCon(tarjeta, tiempo);
            Assert.AreEqual(790, b1.Monto);
            Assert.AreEqual(1210, tarjeta.Saldo);

            tiempo.AgregarMinutos(10);

            Boleto b2 = colectivo.PagarCon(tarjeta, tiempo);
            Assert.AreEqual(790, b2.Monto);
            Assert.AreEqual(420, tarjeta.Saldo);

            tiempo.AgregarMinutos(10);

            Boleto b3 = colectivo.PagarCon(tarjeta, tiempo);
            Assert.IsNotNull(b3);
            Assert.AreEqual(1580, b3.Monto);
            Assert.AreEqual(-1160, tarjeta.Saldo);
        }

        [Test]
        public void TestMedioBoletoVerificaTiempoEnSegundos()
        {
            MedioBoleto tarjeta = new MedioBoleto();
            tarjeta.Cargar(5000);
            Colectivo colectivo = new Colectivo("K");
            TiempoFalso tiempo = new TiempoFalso();

            colectivo.PagarCon(tarjeta, tiempo);

            tiempo.AgregarSegundos(300);

            Boleto boleto = colectivo.PagarCon(tarjeta, tiempo);
            Assert.IsNotNull(boleto);
        }

        [Test]
        public void TestMedioBoletoHorarioDiferente()
        {
            MedioBoleto tarjeta = new MedioBoleto();
            tarjeta.Cargar(10000);
            Colectivo colectivo = new Colectivo("K");
            TiempoFalso tiempo = new TiempoFalso(2024, 10, 14, 18, 30, 0);

            Boleto b1 = colectivo.PagarCon(tarjeta, tiempo);
            Assert.AreEqual(790, b1.Monto);

            tiempo.AgregarMinutos(10);
            Boleto b2 = colectivo.PagarCon(tarjeta, tiempo);
            Assert.AreEqual(790, b2.Monto);

            tiempo.AgregarMinutos(10);
            Boleto b3 = colectivo.PagarCon(tarjeta, tiempo);
            Assert.AreEqual(1580, b3.Monto);
        }

        [Test]
        public void TestMedioBoletoLineaDiferente()
        {
            MedioBoleto tarjeta = new MedioBoleto();
            tarjeta.Cargar(10000);
            Colectivo colectivoK = new Colectivo("K");
            Colectivo colectivo142 = new Colectivo("142");
            TiempoFalso tiempo = new TiempoFalso();

            Boleto b1 = colectivoK.PagarCon(tarjeta, tiempo);
            Assert.AreEqual(790, b1.Monto);
            Assert.AreEqual("K", b1.Linea);

            tiempo.AgregarHoras(2);

            Boleto b2 = colectivo142.PagarCon(tarjeta, tiempo);
            Assert.AreEqual(790, b2.Monto);
            Assert.AreEqual("142", b2.Linea);

            tiempo.AgregarHoras(2);

            Boleto b3 = colectivoK.PagarCon(tarjeta, tiempo);
            Assert.AreEqual(1580, b3.Monto);
        }

        [Test]
        public void TestMedioBoletoSinSaldoSuficiente()
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

            tiempo.AgregarMinutos(10);
            Boleto b4 = colectivo.PagarCon(tarjeta, tiempo);
            Assert.IsNull(b4);
        }

        [Test]
        public void TestMedioBoletoRecargaDespuesDeSaldoNegativo()
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

            tarjeta.Cargar(5000);

            Assert.AreEqual(3840, tarjeta.Saldo);
        }

        [Test]
        public void TestMedioBoletoViajesEnDiferentesDias()
        {
            MedioBoleto tarjeta = new MedioBoleto();
            tarjeta.Cargar(10000);
            Colectivo colectivo = new Colectivo("K");
            TiempoFalso tiempo = new TiempoFalso(2024, 10, 14, 8, 0, 0);

            colectivo.PagarCon(tarjeta, tiempo);
            tiempo.AgregarMinutos(10);
            colectivo.PagarCon(tarjeta, tiempo);
            tiempo.AgregarMinutos(10);
            colectivo.PagarCon(tarjeta, tiempo);

            tiempo.AgregarDias(1);

            Boleto b4 = colectivo.PagarCon(tarjeta, tiempo);
            Assert.AreEqual(790, b4.Monto);

            tiempo.AgregarMinutos(10);
            Boleto b5 = colectivo.PagarCon(tarjeta, tiempo);
            Assert.AreEqual(790, b5.Monto);

            tiempo.AgregarMinutos(10);
            Boleto b6 = colectivo.PagarCon(tarjeta, tiempo);
            Assert.AreEqual(1580, b6.Monto);
        }
    }
}