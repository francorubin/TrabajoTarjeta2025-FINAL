using NUnit.Framework;
using TarjetaSube;
using System;

namespace TarjetaSubeTest
{
    [TestFixture]
    public class TestSaldoNegativo
    {
        [Test]
        public void TestTarjetaNoPuedeQuedarConMenosDeMenos1200()
        {
            Tarjeta tarjeta = new Tarjeta();
            tarjeta.Cargar(2000);
            Colectivo colectivo = new Colectivo("K");

            colectivo.PagarCon(tarjeta);
            Assert.AreEqual(420, tarjeta.Saldo);

            Boleto boleto2 = colectivo.PagarCon(tarjeta);
            Assert.IsNotNull(boleto2);
            Assert.AreEqual(-1160, tarjeta.Saldo);

            Boleto boleto3 = colectivo.PagarCon(tarjeta);
            Assert.IsNull(boleto3);
            Assert.AreEqual(-1160, tarjeta.Saldo); 
        }

        [Test]
        public void TestSaldoDescuentaCorrectamenteViajesPlus()
        {
            Tarjeta tarjeta = new Tarjeta();
            tarjeta.Cargar(2000);
            Colectivo colectivo = new Colectivo("K");

            colectivo.PagarCon(tarjeta); 
            colectivo.PagarCon(tarjeta); 

            Assert.AreEqual(-1160, tarjeta.Saldo);

            bool cargaExitosa = tarjeta.Cargar(3000);
            Assert.IsTrue(cargaExitosa);
            Assert.AreEqual(1840, tarjeta.Saldo); 
        }

        [Test]
        public void TestSaldoNegativoPermitidoHasta1200()
        {
            Tarjeta tarjeta = new Tarjeta();
            tarjeta.Cargar(2000);
            Colectivo colectivo = new Colectivo("K");

            colectivo.PagarCon(tarjeta); 
            Boleto boleto = colectivo.PagarCon(tarjeta); 

            Assert.IsNotNull(boleto);
            Assert.AreEqual(-1160, tarjeta.Saldo);
        }

        [Test]
        public void TestSaldoNegativoExactoEnLimite()
        {
            Tarjeta tarjeta = new Tarjeta();
            tarjeta.Cargar(2000);

            bool resultado = tarjeta.DescontarSaldo(3200);
            Assert.IsTrue(resultado);
            Assert.AreEqual(-1200, tarjeta.Saldo);
        }

        [Test]
        public void TestSaldoNegativoExcedePorUnPeso()
        {
            Tarjeta tarjeta = new Tarjeta();
            tarjeta.Cargar(2000);

            bool resultado = tarjeta.DescontarSaldo(3201);
            Assert.IsFalse(resultado);
            Assert.AreEqual(2000, tarjeta.Saldo);
        }

        [Test]
        public void TestCargaConSaldoNegativoDescuentaCorrectamente()
        {
            Tarjeta tarjeta = new Tarjeta();
            tarjeta.Cargar(2000);
            Colectivo colectivo = new Colectivo("K");

            colectivo.PagarCon(tarjeta);
            colectivo.PagarCon(tarjeta);

            Assert.AreEqual(-1160, tarjeta.Saldo);

            bool resultado = tarjeta.Cargar(3000);
            Assert.IsTrue(resultado);
            Assert.AreEqual(1840, tarjeta.Saldo); 
        }

        [Test]
        public void TestMultiplesViajesConSaldoNegativo()
        {
            Tarjeta tarjeta = new Tarjeta();
            tarjeta.Cargar(3000);
            Colectivo colectivo = new Colectivo("K");

            Boleto b1 = colectivo.PagarCon(tarjeta); 
            Boleto b2 = colectivo.PagarCon(tarjeta); 

            Assert.IsNotNull(b1);
            Assert.IsNotNull(b2);
            Assert.AreEqual(-160, tarjeta.Saldo);
        }

        [Test]
        public void TestPagarDevuelveNullSinSaldoSuficiente()
        {
            Tarjeta tarjeta = new Tarjeta();
            Colectivo colectivo = new Colectivo("K");

            Boleto boleto = colectivo.PagarCon(tarjeta);
            Assert.IsNull(boleto);
        }
    }
}