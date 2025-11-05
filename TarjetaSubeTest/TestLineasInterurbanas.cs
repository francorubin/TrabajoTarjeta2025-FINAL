using NUnit.Framework;
using TarjetaSube;
using System;

namespace TarjetaSubeTest
{
    [TestFixture]
    public class TestLineasInterurbanas
    {
        [Test]
        public void TestColectivoInterurbanoTarifaBase()
        {
            Tarjeta tarjeta = new Tarjeta();
            tarjeta.Cargar(10000);
            ColectivoInterurbano colectivo = new ColectivoInterurbano("Galvez");

            Boleto boleto = colectivo.PagarCon(tarjeta);

            Assert.IsNotNull(boleto);
            Assert.AreEqual(3000, boleto.Monto);
            Assert.AreEqual(7000, tarjeta.Saldo);
            Assert.AreEqual("Galvez", boleto.Linea);
        }

        [Test]
        public void TestColectivoInterurbanoConMedioBoleto()
        {
            MedioBoleto tarjeta = new MedioBoleto();
            tarjeta.Cargar(10000);
            ColectivoInterurbano colectivo = new ColectivoInterurbano("Baigorria");
            TiempoFalso tiempo = new TiempoFalso(2024, 10, 14, 10, 0, 0);

            Boleto boleto = colectivo.PagarCon(tarjeta, tiempo);

            Assert.IsNotNull(boleto);
            Assert.AreEqual(1500, boleto.Monto);
            Assert.AreEqual(8500, tarjeta.Saldo);
            Assert.AreEqual("Medio Boleto", boleto.TipoBoleto);
        }

        [Test]
        public void TestColectivoInterurbanoConFranquiciaCompleta()
        {
            FranquiciaCompleta tarjeta = new FranquiciaCompleta();
            tarjeta.Cargar(5000);
            ColectivoInterurbano colectivo = new ColectivoInterurbano("Galvez");
            TiempoFalso tiempo = new TiempoFalso(2024, 10, 14, 8, 0, 0);

            Boleto boleto = colectivo.PagarCon(tarjeta, tiempo);

            Assert.IsNotNull(boleto);
            Assert.AreEqual(0, boleto.Monto);
            Assert.AreEqual(5000, tarjeta.Saldo);
            Assert.AreEqual("Franquicia Completa", boleto.TipoBoleto);
        }

        [Test]
        public void TestColectivoInterurbanoConBoletoGratuito()
        {
            BoletoGratuito tarjeta = new BoletoGratuito();
            tarjeta.Cargar(10000);
            ColectivoInterurbano colectivo = new ColectivoInterurbano("Baigorria");
            TiempoFalso tiempo = new TiempoFalso(2024, 10, 14, 9, 0, 0);

            Boleto boleto1 = colectivo.PagarCon(tarjeta, tiempo);
            Assert.IsNotNull(boleto1);
            Assert.AreEqual(0, boleto1.Monto);
            Assert.AreEqual(10000, tarjeta.Saldo);

            tiempo.AgregarMinutos(10);
            Boleto boleto2 = colectivo.PagarCon(tarjeta, tiempo);
            Assert.IsNotNull(boleto2);
            Assert.AreEqual(0, boleto2.Monto);
            Assert.AreEqual(10000, tarjeta.Saldo);

            tiempo.AgregarMinutos(10);
            Boleto boleto3 = colectivo.PagarCon(tarjeta, tiempo);
            Assert.IsNotNull(boleto3);
            Assert.AreEqual(3000, boleto3.Monto);
            Assert.AreEqual(7000, tarjeta.Saldo);
        }

        [Test]
        public void TestColectivoInterurbanoMedioBoleto2ViajesPorDia()
        {
            MedioBoleto tarjeta = new MedioBoleto();
            tarjeta.Cargar(20000);
            ColectivoInterurbano colectivo = new ColectivoInterurbano("Galvez");
            TiempoFalso tiempo = new TiempoFalso(2024, 10, 14, 8, 0, 0);

            Boleto boleto1 = colectivo.PagarCon(tarjeta, tiempo);
            Assert.AreEqual(1500, boleto1.Monto);
            Assert.AreEqual(18500, tarjeta.Saldo);

            tiempo.AgregarMinutos(10);
            Boleto boleto2 = colectivo.PagarCon(tarjeta, tiempo);
            Assert.AreEqual(1500, boleto2.Monto);
            Assert.AreEqual(17000, tarjeta.Saldo);

            tiempo.AgregarMinutos(10);
            Boleto boleto3 = colectivo.PagarCon(tarjeta, tiempo);
            Assert.AreEqual(3000, boleto3.Monto);
            Assert.AreEqual(14000, tarjeta.Saldo);
        }

        [Test]
        public void TestColectivoInterurbanoMedioBoletoLimitacion5Minutos()
        {
            MedioBoleto tarjeta = new MedioBoleto();
            tarjeta.Cargar(10000);
            ColectivoInterurbano colectivo = new ColectivoInterurbano("Baigorria");
            TiempoFalso tiempo = new TiempoFalso(2024, 10, 14, 10, 0, 0);

            Boleto boleto1 = colectivo.PagarCon(tarjeta, tiempo);
            Assert.IsNotNull(boleto1);
            Assert.AreEqual(1500, boleto1.Monto);

            tiempo.AgregarMinutos(4);
            Boleto boleto2 = colectivo.PagarCon(tarjeta, tiempo);
            Assert.IsNull(boleto2);

            tiempo.AgregarMinutos(1);
            Boleto boleto3 = colectivo.PagarCon(tarjeta, tiempo);
            Assert.IsNotNull(boleto3);
            Assert.AreEqual(1500, boleto3.Monto);
        }

        [Test]
        public void TestColectivoInterurbanoSinSaldoSuficiente()
        {
            Tarjeta tarjeta = new Tarjeta();
            tarjeta.Cargar(2000);
            ColectivoInterurbano colectivo = new ColectivoInterurbano("Galvez");

            Boleto boleto = colectivo.PagarCon(tarjeta);
            Assert.IsNotNull(boleto);
            Assert.AreEqual(-1000, tarjeta.Saldo);

            Boleto boleto2 = colectivo.PagarCon(tarjeta);
            Assert.IsNull(boleto2);
            Assert.AreEqual(-1000, tarjeta.Saldo);
        }

        [Test]
        public void TestColectivoInterurbanoConSaldoNegativo()
        {
            Tarjeta tarjeta = new Tarjeta();
            tarjeta.Cargar(3000);
            ColectivoInterurbano colectivo = new ColectivoInterurbano("Galvez");

            Boleto boleto = colectivo.PagarCon(tarjeta);
            Assert.IsNotNull(boleto);
            Assert.AreEqual(0, tarjeta.Saldo);

            Boleto boleto2 = colectivo.PagarCon(tarjeta);
            Assert.IsNull(boleto2);
        }

        [Test]
        public void TestColectivoInterurbanoConTarjetaNull()
        {
            ColectivoInterurbano colectivo = new ColectivoInterurbano("Baigorria");
            Boleto boleto = colectivo.PagarCon(null);
            Assert.IsNull(boleto);
        }

        [Test]
        public void TestColectivoInterurbanoLineaCorrecta()
        {
            ColectivoInterurbano colectivo = new ColectivoInterurbano("Granadero Baigorria");
            Tarjeta tarjeta = new Tarjeta();
            tarjeta.Cargar(5000);

            Boleto boleto = colectivo.PagarCon(tarjeta);
            Assert.AreEqual("Granadero Baigorria", boleto.Linea);
        }

        [Test]
        public void TestColectivoInterurbanoMultiplesViajes()
        {
            Tarjeta tarjeta = new Tarjeta();
            tarjeta.Cargar(30000);
            ColectivoInterurbano colectivo = new ColectivoInterurbano("Galvez");

            Boleto b1 = colectivo.PagarCon(tarjeta);
            Boleto b2 = colectivo.PagarCon(tarjeta);
            Boleto b3 = colectivo.PagarCon(tarjeta);

            Assert.IsNotNull(b1);
            Assert.IsNotNull(b2);
            Assert.IsNotNull(b3);
            Assert.AreEqual(21000, tarjeta.Saldo);
        }

        [Test]
        public void TestColectivoInterurbanoConDescuentoUsoFrecuente()
        {
            Tarjeta tarjeta = new Tarjeta();
            tarjeta.Cargar(30000);
            tarjeta.Cargar(30000);
            tarjeta.Cargar(30000);
            ColectivoInterurbano colectivo = new ColectivoInterurbano("Galvez");
            TiempoFalso tiempo = new TiempoFalso(2024, 11, 1, 8, 0, 0);

            for (int i = 0; i < 29; i++)
            {
                Boleto boleto = colectivo.PagarCon(tarjeta, tiempo);
                Assert.AreEqual(3000, boleto.Monto);
                tiempo.AgregarMinutos(10);
            }

            Boleto boleto30 = colectivo.PagarCon(tarjeta, tiempo);
            Assert.AreEqual(2400, boleto30.Monto);
        }

        [Test]
        public void TestColectivoInterurbanoFechaBoleto()
        {
            Tarjeta tarjeta = new Tarjeta();
            tarjeta.Cargar(10000);
            ColectivoInterurbano colectivo = new ColectivoInterurbano("Baigorria");
            TiempoFalso tiempo = new TiempoFalso(2024, 10, 14, 15, 30, 0);

            DateTime antes = tiempo.Now();
            Boleto boleto = colectivo.PagarCon(tarjeta, tiempo);
            DateTime despues = tiempo.Now();

            Assert.IsNotNull(boleto);
            Assert.GreaterOrEqual(boleto.Fecha, antes);
            Assert.LessOrEqual(boleto.Fecha, despues);
        }

        [Test]
        public void TestColectivoInterurbanoIdTarjeta()
        {
            Tarjeta tarjeta = new Tarjeta();
            tarjeta.Cargar(10000);
            ColectivoInterurbano colectivo = new ColectivoInterurbano("Galvez");

            Boleto boleto = colectivo.PagarCon(tarjeta);
            Assert.AreEqual(tarjeta.Id, boleto.IdTarjeta);
        }

        [Test]
        public void TestColectivoInterurbanoTotalAbonado()
        {
            Tarjeta tarjeta = new Tarjeta();
            tarjeta.Cargar(10000);
            ColectivoInterurbano colectivo = new ColectivoInterurbano("Baigorria");

            Boleto boleto = colectivo.PagarCon(tarjeta);
            Assert.AreEqual(3000, boleto.TotalAbonado);
        }

        [Test]
        public void TestColectivoInterurbanoHerenciaDeColectivo()
        {
            ColectivoInterurbano colectivo = new ColectivoInterurbano("Galvez");
            Assert.IsInstanceOf<Colectivo>(colectivo);
        }

        [Test]
        public void TestColectivoInterurbanoYUrbanoConMismaTarjeta()
        {
            Tarjeta tarjeta = new Tarjeta();
            tarjeta.Cargar(20000);
            Colectivo urbano = new Colectivo("K");
            ColectivoInterurbano interurbano = new ColectivoInterurbano("Galvez");

            Boleto b1 = urbano.PagarCon(tarjeta);
            Assert.AreEqual(1580, b1.Monto);
            Assert.AreEqual(18420, tarjeta.Saldo);

            Boleto b2 = interurbano.PagarCon(tarjeta);
            Assert.AreEqual(3000, b2.Monto);
            Assert.AreEqual(15420, tarjeta.Saldo);
        }

        [Test]
        public void TestColectivoInterurbanoBoletoGratuitoReiniciaContadorDiario()
        {
            BoletoGratuito tarjeta = new BoletoGratuito();
            tarjeta.Cargar(20000);
            ColectivoInterurbano colectivo = new ColectivoInterurbano("Galvez");
            TiempoFalso tiempo = new TiempoFalso(2024, 10, 14, 8, 0, 0);

            colectivo.PagarCon(tarjeta, tiempo);
            tiempo.AgregarMinutos(10);
            colectivo.PagarCon(tarjeta, tiempo);
            tiempo.AgregarMinutos(10);
            colectivo.PagarCon(tarjeta, tiempo);

            tiempo.AgregarDias(1);

            Boleto boleto = colectivo.PagarCon(tarjeta, tiempo);
            Assert.AreEqual(0, boleto.Monto);
        }

        [Test]
        public void TestColectivoInterurbanoMedioBoletoContadorDiario()
        {
            MedioBoleto tarjeta = new MedioBoleto();
            tarjeta.Cargar(30000);
            ColectivoInterurbano colectivo = new ColectivoInterurbano("Baigorria");
            TiempoFalso tiempo = new TiempoFalso(2024, 10, 14, 8, 0, 0);

            colectivo.PagarCon(tarjeta, tiempo);
            tiempo.AgregarMinutos(10);
            colectivo.PagarCon(tarjeta, tiempo);
            tiempo.AgregarMinutos(10);
            colectivo.PagarCon(tarjeta, tiempo);

            tiempo.AgregarDias(1);

            Boleto boleto = colectivo.PagarCon(tarjeta, tiempo);
            Assert.AreEqual(1500, boleto.Monto);
        }

        [Test]
        public void TestColectivoInterurbanoConSaldoPendiente()
        {
            Tarjeta tarjeta = new Tarjeta();
            tarjeta.Cargar(30000);
            tarjeta.Cargar(30000);
            ColectivoInterurbano colectivo = new ColectivoInterurbano("Galvez");

            Assert.AreEqual(56000, tarjeta.Saldo);
            Assert.AreEqual(4000, tarjeta.SaldoPendiente);

            Boleto boleto = colectivo.PagarCon(tarjeta);
            Assert.IsNotNull(boleto);
            Assert.AreEqual(3000, boleto.Monto);
            Assert.AreEqual(56000, tarjeta.Saldo);
            Assert.AreEqual(1000, tarjeta.SaldoPendiente);
        }
    }
}