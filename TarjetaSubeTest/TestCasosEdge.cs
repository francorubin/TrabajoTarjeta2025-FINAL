using NUnit.Framework;
using TarjetaSube;
using System;

namespace TarjetaSubeTest
{
    [TestFixture]
    public class TestCasosEdge
    {
        // ===== TESTS PARA MEJORAR COBERTURA =====

        [Test]
        public void TestColectivoLineaGetter()
        {
            Colectivo colectivo = new Colectivo("TestLinea");
            Assert.AreEqual("TestLinea", colectivo.Linea);
        }

        [Test]
        public void TestBoletoConstructorConTodosLosParametros()
        {
            DateTime fecha = DateTime.Now;
            Boleto boleto = new Boleto(1580m, 5000m, "K", fecha, "Normal");

            Assert.AreEqual(1580m, boleto.Monto);
            Assert.AreEqual(5000m, boleto.SaldoRestante);
            Assert.AreEqual("K", boleto.Linea);
            Assert.AreEqual(fecha, boleto.Fecha);
            Assert.AreEqual("Normal", boleto.TipoBoleto);
        }

        [Test]
        public void TestBoletoConstructorConSaldoNegativo()
        {
            DateTime fecha = DateTime.Now;
            Boleto boleto = new Boleto(1580m, -500m, "142", fecha, "Normal");

            Assert.AreEqual(1580m, boleto.Monto);
            Assert.AreEqual(-500m, boleto.SaldoRestante);
        }

        [Test]
        public void TestBoletoConstructorConMontoDecimal()
        {
            DateTime fecha = DateTime.Now;
            Boleto boleto = new Boleto(790m, 4210m, "K", fecha, "Medio Boleto");

            Assert.AreEqual(790m, boleto.Monto);
            Assert.AreEqual("Medio Boleto", boleto.TipoBoleto);
        }

        [Test]
        public void TestBoletoGettersMonto()
        {
            DateTime fecha = DateTime.Now;
            Boleto boleto = new Boleto(1580m, 5000m, "K", fecha, "Normal");
            decimal monto = boleto.Monto;
            Assert.AreEqual(1580m, monto);
        }

        [Test]
        public void TestBoletoGettersSaldoRestante()
        {
            DateTime fecha = DateTime.Now;
            Boleto boleto = new Boleto(1580m, 5000m, "K", fecha, "Normal");
            decimal saldo = boleto.SaldoRestante;
            Assert.AreEqual(5000m, saldo);
        }

        [Test]
        public void TestBoletoGettersLinea()
        {
            DateTime fecha = DateTime.Now;
            Boleto boleto = new Boleto(1580m, 5000m, "K", fecha, "Normal");
            string linea = boleto.Linea;
            Assert.AreEqual("K", linea);
        }

        [Test]
        public void TestBoletoGettersFecha()
        {
            DateTime fecha = DateTime.Now;
            Boleto boleto = new Boleto(1580m, 5000m, "K", fecha, "Normal");
            DateTime fechaBoleto = boleto.Fecha;
            Assert.AreEqual(fecha, fechaBoleto);
        }

        [Test]
        public void TestBoletoGettersTipoBoleto()
        {
            DateTime fecha = DateTime.Now;
            Boleto boleto = new Boleto(1580m, 5000m, "K", fecha, "Normal");
            string tipo = boleto.TipoBoleto;
            Assert.AreEqual("Normal", tipo);
        }

        [Test]
        public void TestTarjetaSaldoGetterDespuesDeCarga()
        {
            Tarjeta tarjeta = new Tarjeta();
            tarjeta.Cargar(5000);
            decimal saldo = tarjeta.Saldo;
            Assert.AreEqual(5000, saldo);
        }

        [Test]
        public void TestTarjetaSaldoGetterDespuesDeDescuento()
        {
            Tarjeta tarjeta = new Tarjeta();
            tarjeta.Cargar(5000);
            tarjeta.DescontarSaldo(1580);
            decimal saldo = tarjeta.Saldo;
            Assert.AreEqual(3420, saldo);
        }

        [Test]
        public void TestColectivoConLineaVacia()
        {
            Colectivo colectivo = new Colectivo("");
            Assert.AreEqual("", colectivo.Linea);
        }

        [Test]
        public void TestColectivoConLineaNumerica()
        {
            Colectivo colectivo = new Colectivo("142");
            Tarjeta tarjeta = new Tarjeta();
            tarjeta.Cargar(5000);

            Boleto boleto = colectivo.PagarCon(tarjeta);
            Assert.AreEqual("142", boleto.Linea);
        }

        [Test]
        public void TestColectivoConLineaAlfanumerica()
        {
            Colectivo colectivo = new Colectivo("K1");
            Tarjeta tarjeta = new Tarjeta();
            tarjeta.Cargar(5000);

            Boleto boleto = colectivo.PagarCon(tarjeta);
            Assert.AreEqual("K1", boleto.Linea);
        }

        [Test]
        public void TestMedioBoletoHeredaDeTarjeta()
        {
            MedioBoleto medio = new MedioBoleto();
            Assert.IsInstanceOf<Tarjeta>(medio);
        }

        [Test]
        public void TestBoletoGratuitoHeredaDeTarjeta()
        {
            BoletoGratuito gratuito = new BoletoGratuito();
            Assert.IsInstanceOf<Tarjeta>(gratuito);
        }

        [Test]
        public void TestFranquiciaCompletaHeredaDeTarjeta()
        {
            FranquiciaCompleta franquicia = new FranquiciaCompleta();
            Assert.IsInstanceOf<Tarjeta>(franquicia);
        }

        [Test]
        public void TestTarjetaCargarMontoLimiteInferior()
        {
            Tarjeta tarjeta = new Tarjeta();
            bool resultado = tarjeta.Cargar(2000);
            Assert.IsTrue(resultado);
            Assert.AreEqual(2000, tarjeta.Saldo);
        }

        [Test]
        public void TestTarjetaCargarMontoLimiteSuperior()
        {
            Tarjeta tarjeta = new Tarjeta();
            bool resultado = tarjeta.Cargar(30000);
            Assert.IsTrue(resultado);
            Assert.AreEqual(30000, tarjeta.Saldo);
        }

        [Test]
        public void TestMedioBoletoConSaldoCero()
        {
            MedioBoleto tarjeta = new MedioBoleto();
            Assert.AreEqual(0, tarjeta.Saldo);
        }

        [Test]
        public void TestBoletoGratuitoConSaldoCero()
        {
            BoletoGratuito tarjeta = new BoletoGratuito();
            Assert.AreEqual(0, tarjeta.Saldo);
        }

        [Test]
        public void TestFranquiciaCompletaConSaldoCero()
        {
            FranquiciaCompleta tarjeta = new FranquiciaCompleta();
            Assert.AreEqual(0, tarjeta.Saldo);
        }

        [Test]
        public void TestColectivoTarjetaNullDevuelveNull()
        {
            Colectivo colectivo = new Colectivo("K");
            Boleto boleto = colectivo.PagarCon(null);
            Assert.IsNull(boleto);
        }

        [Test]
        public void TestTarjetaDescontarSaldoConMontoMayorAlSaldoPeroMenorAlLimite()
        {
            Tarjeta tarjeta = new Tarjeta();
            tarjeta.Cargar(2000);
            bool resultado = tarjeta.DescontarSaldo(2500);
            Assert.IsTrue(resultado);
            Assert.AreEqual(-500, tarjeta.Saldo);
        }

        [Test]
        public void TestTodosLosTiposDeTarjetaPuedenCargarSaldo()
        {
            Tarjeta normal = new Tarjeta();
            MedioBoleto medio = new MedioBoleto();
            BoletoGratuito gratuito = new BoletoGratuito();
            FranquiciaCompleta franquicia = new FranquiciaCompleta();

            Assert.IsTrue(normal.Cargar(5000));
            Assert.IsTrue(medio.Cargar(5000));
            Assert.IsTrue(gratuito.Cargar(5000));
            Assert.IsTrue(franquicia.Cargar(5000));

            Assert.AreEqual(5000, normal.Saldo);
            Assert.AreEqual(5000, medio.Saldo);
            Assert.AreEqual(5000, gratuito.Saldo);
            Assert.AreEqual(5000, franquicia.Saldo);
        }

        [Test]
        public void TestBoletoConLineaLarga()
        {
            Colectivo colectivo = new Colectivo("Linea Muy Larga 12345");
            Tarjeta tarjeta = new Tarjeta();
            tarjeta.Cargar(5000);

            Boleto boleto = colectivo.PagarCon(tarjeta);
            Assert.AreEqual("Linea Muy Larga 12345", boleto.Linea);
        }

        [Test]
        public void TestCargarSaldoEnCero()
        {
            Tarjeta tarjeta = new Tarjeta();
            bool resultado = tarjeta.Cargar(0);
            Assert.IsFalse(resultado);
        }

        [Test]
        public void TestDescontarSaldoCero()
        {
            Tarjeta tarjeta = new Tarjeta();
            tarjeta.Cargar(5000);
            bool resultado = tarjeta.DescontarSaldo(0);
            Assert.IsTrue(resultado);
            Assert.AreEqual(5000, tarjeta.Saldo);
        }

        [Test]
        public void TestMedioBoletoDescontarSaldoDevuelveTrue()
        {
            MedioBoleto tarjeta = new MedioBoleto();
            tarjeta.Cargar(5000);
            bool resultado = tarjeta.DescontarSaldo(790);
            Assert.IsTrue(resultado);
            Assert.AreEqual(4210, tarjeta.Saldo);
        }

        [Test]
        public void TestBoletoGratuitoDescontarSaldoNoModificaSaldo()
        {
            BoletoGratuito tarjeta = new BoletoGratuito();
            tarjeta.Cargar(5000);
            tarjeta.DescontarSaldo(1580);
            Assert.AreEqual(5000, tarjeta.Saldo);
        }

        [Test]
        public void TestFranquiciaCompletaDescontarSaldoNoModificaSaldo()
        {
            FranquiciaCompleta tarjeta = new FranquiciaCompleta();
            tarjeta.Cargar(5000);
            tarjeta.DescontarSaldo(1580);
            Assert.AreEqual(5000, tarjeta.Saldo);
        }
    }
}