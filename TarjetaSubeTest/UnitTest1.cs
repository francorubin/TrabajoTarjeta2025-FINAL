using NUnit.Framework;
using TarjetaSube;
using System;

namespace TarjetaSubeTest
{
    [TestFixture]
    public class TarjetaTests
    {
        [Test]
        public void TestConstructorTarjetaSaldoInicial()
        {
            Tarjeta tarjeta = new Tarjeta();
            Assert.AreEqual(0, tarjeta.Saldo);
        }

        [Test]
        public void TestCargaSaldo2000()
        {
            Tarjeta tarjeta = new Tarjeta();
            bool resultado = tarjeta.Cargar(2000);
            Assert.IsTrue(resultado);
            Assert.AreEqual(2000, tarjeta.Saldo);
        }

        [Test]
        public void TestCargaSaldo3000()
        {
            Tarjeta tarjeta = new Tarjeta();
            bool resultado = tarjeta.Cargar(3000);
            Assert.IsTrue(resultado);
            Assert.AreEqual(3000, tarjeta.Saldo);
        }

        [Test]
        public void TestCargaSaldo4000()
        {
            Tarjeta tarjeta = new Tarjeta();
            bool resultado = tarjeta.Cargar(4000);
            Assert.IsTrue(resultado);
            Assert.AreEqual(4000, tarjeta.Saldo);
        }

        [Test]
        public void TestCargaSaldo5000()
        {
            Tarjeta tarjeta = new Tarjeta();
            bool resultado = tarjeta.Cargar(5000);
            Assert.IsTrue(resultado);
            Assert.AreEqual(5000, tarjeta.Saldo);
        }

        [Test]
        public void TestCargaSaldo8000()
        {
            Tarjeta tarjeta = new Tarjeta();
            bool resultado = tarjeta.Cargar(8000);
            Assert.IsTrue(resultado);
            Assert.AreEqual(8000, tarjeta.Saldo);
        }

        [Test]
        public void TestCargaSaldo10000()
        {
            Tarjeta tarjeta = new Tarjeta();
            bool resultado = tarjeta.Cargar(10000);
            Assert.IsTrue(resultado);
            Assert.AreEqual(10000, tarjeta.Saldo);
        }

        [Test]
        public void TestCargaSaldo15000()
        {
            Tarjeta tarjeta = new Tarjeta();
            bool resultado = tarjeta.Cargar(15000);
            Assert.IsTrue(resultado);
            Assert.AreEqual(15000, tarjeta.Saldo);
        }

        [Test]
        public void TestCargaSaldo20000()
        {
            Tarjeta tarjeta = new Tarjeta();
            bool resultado = tarjeta.Cargar(20000);
            Assert.IsTrue(resultado);
            Assert.AreEqual(20000, tarjeta.Saldo);
        }

        [Test]
        public void TestCargaSaldo25000()
        {
            Tarjeta tarjeta = new Tarjeta();
            bool resultado = tarjeta.Cargar(25000);
            Assert.IsTrue(resultado);
            Assert.AreEqual(25000, tarjeta.Saldo);
        }

        [Test]
        public void TestCargaSaldo30000()
        {
            Tarjeta tarjeta = new Tarjeta();
            bool resultado = tarjeta.Cargar(30000);
            Assert.IsTrue(resultado);
            Assert.AreEqual(30000, tarjeta.Saldo);
        }

        [Test]
        public void TestCargaMontoInvalido1000()
        {
            Tarjeta tarjeta = new Tarjeta();
            bool resultado = tarjeta.Cargar(1000);
            Assert.IsFalse(resultado);
            Assert.AreEqual(0, tarjeta.Saldo);
        }

        [Test]
        public void TestCargaMontoInvalido500()
        {
            Tarjeta tarjeta = new Tarjeta();
            bool resultado = tarjeta.Cargar(500);
            Assert.IsFalse(resultado);
            Assert.AreEqual(0, tarjeta.Saldo);
        }

        [Test]
        public void TestCargaMontoInvalido7000()
        {
            Tarjeta tarjeta = new Tarjeta();
            bool resultado = tarjeta.Cargar(7000);
            Assert.IsFalse(resultado);
            Assert.AreEqual(0, tarjeta.Saldo);
        }

        [Test]
        public void TestCargaMontoNegativo()
        {
            Tarjeta tarjeta = new Tarjeta();
            bool resultado = tarjeta.Cargar(-5000);
            Assert.IsFalse(resultado);
            Assert.AreEqual(0, tarjeta.Saldo);
        }

        [Test]
        public void TestCargaMultiple()
        {
            Tarjeta tarjeta = new Tarjeta();
            tarjeta.Cargar(5000);
            tarjeta.Cargar(3000);
            tarjeta.Cargar(2000);
            Assert.AreEqual(10000, tarjeta.Saldo);
        }

        [Test]
        public void TestLimiteDeSaldoExacto()
        {
            Tarjeta tarjeta = new Tarjeta();
            tarjeta.Cargar(30000);
            bool resultado = tarjeta.Cargar(10000);
            Assert.IsTrue(resultado);
            Assert.AreEqual(40000, tarjeta.Saldo);
        }

        [Test]
        public void TestLimiteDeSaldoExcedido()
        {
            Tarjeta tarjeta = new Tarjeta();
            tarjeta.Cargar(30000);
            tarjeta.Cargar(10000);
            bool resultado = tarjeta.Cargar(2000);
            Assert.IsFalse(resultado);
            Assert.AreEqual(40000, tarjeta.Saldo);
        }

        [Test]
        public void TestLimiteDeSaldoExcedidoPorUnPeso()
        {
            Tarjeta tarjeta = new Tarjeta();
            tarjeta.Cargar(25000);
            tarjeta.Cargar(15000);
            bool resultado = tarjeta.Cargar(2000);
            Assert.IsFalse(resultado);
            Assert.AreEqual(40000, tarjeta.Saldo);
        }

        [Test]
        public void TestDescontarSaldoConSaldoSuficiente()
        {
            Tarjeta tarjeta = new Tarjeta();
            tarjeta.Cargar(5000);
            bool resultado = tarjeta.DescontarSaldo(1580);
            Assert.IsTrue(resultado);
            Assert.AreEqual(3420, tarjeta.Saldo);
        }

        [Test]
        public void TestDescontarSaldoSinSaldoSuficiente()
        {
            Tarjeta tarjeta = new Tarjeta();
            tarjeta.Cargar(2000);
            bool resultado = tarjeta.DescontarSaldo(3000);
            Assert.IsFalse(resultado);
            Assert.AreEqual(2000, tarjeta.Saldo);
        }

        [Test]
        public void TestDescontarSaldoExacto()
        {
            Tarjeta tarjeta = new Tarjeta();
            tarjeta.Cargar(5000);
            bool resultado = tarjeta.DescontarSaldo(5000);
            Assert.IsTrue(resultado);
            Assert.AreEqual(0, tarjeta.Saldo);
        }

        [Test]
        public void TestDescontarSaldoConSaldoCero()
        {
            Tarjeta tarjeta = new Tarjeta();
            bool resultado = tarjeta.DescontarSaldo(1580);
            Assert.IsFalse(resultado);
            Assert.AreEqual(0, tarjeta.Saldo);
        }

        [Test]
        public void TestPagarBoletoLineaK()
        {
            Tarjeta tarjeta = new Tarjeta();
            tarjeta.Cargar(5000);
            Colectivo colectivo = new Colectivo("K");
            Boleto boleto = colectivo.PagarCon(tarjeta);

            Assert.IsNotNull(boleto);
            Assert.AreEqual(1580, boleto.Monto);
            Assert.AreEqual(3420, tarjeta.Saldo);
            Assert.AreEqual(3420, boleto.SaldoRestante);
            Assert.AreEqual("K", boleto.Linea);
            Assert.AreEqual("Normal", boleto.TipoBoleto);
        }

        [Test]
        public void TestPagarBoletoLinea142()
        {
            Tarjeta tarjeta = new Tarjeta();
            tarjeta.Cargar(5000);
            Colectivo colectivo = new Colectivo("142");
            Boleto boleto = colectivo.PagarCon(tarjeta);

            Assert.IsNotNull(boleto);
            Assert.AreEqual("142", boleto.Linea);
        }

        [Test]
        public void TestPagarSinSaldoSuficiente()
        {
            Tarjeta tarjeta = new Tarjeta();
            tarjeta.Cargar(2000);
            Colectivo colectivo = new Colectivo("K");

            colectivo.PagarCon(tarjeta);
            Boleto boleto = colectivo.PagarCon(tarjeta);

            Assert.IsNull(boleto);
            Assert.AreEqual(420, tarjeta.Saldo);
        }

        [Test]
        public void TestPagarConTarjetaNull()
        {
            Colectivo colectivo = new Colectivo("K");
            Boleto boleto = colectivo.PagarCon(null);
            Assert.IsNull(boleto);
        }

        [Test]
        public void TestPagarConSaldoJusto()
        {
            Tarjeta tarjeta = new Tarjeta();
            tarjeta.Cargar(2000);
            Colectivo colectivo = new Colectivo("K");

            Boleto boleto = colectivo.PagarCon(tarjeta);

            Assert.IsNotNull(boleto);
            Assert.AreEqual(420, tarjeta.Saldo);
            Assert.AreEqual(1580, boleto.Monto);
        }

        [Test]
        public void TestColectivoGuardaLineaCorrectamente()
        {
            Colectivo colectivo = new Colectivo("133");
            Assert.AreEqual("133", colectivo.Linea);
        }

        [Test]
        public void TestBoletoGuardaFechaCorrectamente()
        {
            Tarjeta tarjeta = new Tarjeta();
            tarjeta.Cargar(5000);
            Colectivo colectivo = new Colectivo("K");

            DateTime antes = DateTime.Now;
            Boleto boleto = colectivo.PagarCon(tarjeta);
            DateTime despues = DateTime.Now;

            Assert.IsNotNull(boleto);
            Assert.GreaterOrEqual(boleto.Fecha, antes);
            Assert.LessOrEqual(boleto.Fecha, despues);
        }

        [Test]
        public void TestMultiplesPagos()
        {
            Tarjeta tarjeta = new Tarjeta();
            tarjeta.Cargar(10000);
            Colectivo colectivo = new Colectivo("K");

            Boleto boleto1 = colectivo.PagarCon(tarjeta);
            Boleto boleto2 = colectivo.PagarCon(tarjeta);
            Boleto boleto3 = colectivo.PagarCon(tarjeta);

            Assert.IsNotNull(boleto1);
            Assert.IsNotNull(boleto2);
            Assert.IsNotNull(boleto3);
            Assert.AreEqual(5260, tarjeta.Saldo);
        }

        [Test]
        public void TestBoletoTieneTodosLosDatos()
        {
            Tarjeta tarjeta = new Tarjeta();
            tarjeta.Cargar(5000);
            Colectivo colectivo = new Colectivo("K");
            Boleto boleto = colectivo.PagarCon(tarjeta);

            Assert.IsNotNull(boleto);
            Assert.IsNotNull(boleto.Linea);
            Assert.Greater(boleto.Monto, 0);
            Assert.GreaterOrEqual(boleto.SaldoRestante, 0);
            Assert.IsNotNull(boleto.TipoBoleto);
            Assert.IsNotNull(boleto.Fecha);
        }

        [Test]
        public void TestCargaDespuesDePagar()
        {
            Tarjeta tarjeta = new Tarjeta();
            tarjeta.Cargar(3000);
            Colectivo colectivo = new Colectivo("K");

            colectivo.PagarCon(tarjeta);
            Assert.AreEqual(1420, tarjeta.Saldo);

            tarjeta.Cargar(5000);
            Assert.AreEqual(6420, tarjeta.Saldo);
        }

        [Test]
        public void TestSaldoNoCambiaSiFallaPago()
        {
            Tarjeta tarjeta = new Tarjeta();
            tarjeta.Cargar(2000);
            Colectivo colectivo = new Colectivo("K");

            colectivo.PagarCon(tarjeta);
            decimal saldoAntes = tarjeta.Saldo;

            Boleto boleto = colectivo.PagarCon(tarjeta);

            Assert.IsNull(boleto);
            Assert.AreEqual(saldoAntes, tarjeta.Saldo);
        }
    }
}