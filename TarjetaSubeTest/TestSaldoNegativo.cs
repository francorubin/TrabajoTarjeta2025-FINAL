using NUnit.Framework;
using TarjetaSube;
using System;

namespace TarjetaSubeTest
{
    [TestFixture]
    public class TestSaldoNegativo
    {
        // ===== TESTS REQUERIDOS POR EL ENUNCIADO =====

        [Test]
        public void TestTarjetaNoPuedeQuedarConMenosDeMenos1200()
        {
            Tarjeta tarjeta = new Tarjeta();
            tarjeta.Cargar(2000);
            Colectivo colectivo = new Colectivo("K");

            // Primer viaje: 2000 - 1580 = 420
            colectivo.PagarCon(tarjeta);
            Assert.AreEqual(420, tarjeta.Saldo);

            // Segundo viaje: 420 - 1580 = -1160 (PERMITIDO)
            Boleto boleto2 = colectivo.PagarCon(tarjeta);
            Assert.IsNotNull(boleto2);
            Assert.AreEqual(-1160, tarjeta.Saldo);

            // Tercer viaje: -1160 - 1580 = -2740 (NO PERMITIDO)
            Boleto boleto3 = colectivo.PagarCon(tarjeta);
            Assert.IsNull(boleto3);
            Assert.AreEqual(-1160, tarjeta.Saldo); // Saldo no cambió
        }

        [Test]
        public void TestSaldoDescuentaCorrectamenteViajesPlus()
        {
            Tarjeta tarjeta = new Tarjeta();
            tarjeta.Cargar(2000);
            Colectivo colectivo = new Colectivo("K");

            // Hacer dos viajes para usar el viaje plus
            colectivo.PagarCon(tarjeta); // 2000 - 1580 = 420
            colectivo.PagarCon(tarjeta); // 420 - 1580 = -1160 (viaje plus usado)

            Assert.AreEqual(-1160, tarjeta.Saldo);

            // Cargar saldo y verificar que descuenta el negativo
            bool cargaExitosa = tarjeta.Cargar(3000);
            Assert.IsTrue(cargaExitosa);
            Assert.AreEqual(1840, tarjeta.Saldo); // -1160 + 3000 = 1840
        }

        // ===== TESTS ADICIONALES PARA COBERTURA =====

        [Test]
        public void TestSaldoNegativoPermitidoHasta1200()
        {
            Tarjeta tarjeta = new Tarjeta();
            tarjeta.Cargar(2000);
            Colectivo colectivo = new Colectivo("K");

            colectivo.PagarCon(tarjeta); // 420
            Boleto boleto = colectivo.PagarCon(tarjeta); // -1160

            Assert.IsNotNull(boleto);
            Assert.AreEqual(-1160, tarjeta.Saldo);
        }

        [Test]
        public void TestSaldoNegativoExactoEnLimite()
        {
            Tarjeta tarjeta = new Tarjeta();
            tarjeta.Cargar(2000);

            // Descontar exactamente hasta -1200
            bool resultado = tarjeta.DescontarSaldo(3200);
            Assert.IsTrue(resultado);
            Assert.AreEqual(-1200, tarjeta.Saldo);
        }

        [Test]
        public void TestSaldoNegativoExcedePorUnPeso()
        {
            Tarjeta tarjeta = new Tarjeta();
            tarjeta.Cargar(2000);

            // Intentar descontar hasta -1201 (NO PERMITIDO)
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

            // Saldo: -1160
            Assert.AreEqual(-1160, tarjeta.Saldo);

            // Cargar 3000
            bool resultado = tarjeta.Cargar(3000);
            Assert.IsTrue(resultado);
            Assert.AreEqual(1840, tarjeta.Saldo); // -1160 + 3000
        }

        [Test]
        public void TestMultiplesViajesConSaldoNegativo()
        {
            Tarjeta tarjeta = new Tarjeta();
            tarjeta.Cargar(3000);
            Colectivo colectivo = new Colectivo("K");

            Boleto b1 = colectivo.PagarCon(tarjeta); // 1420
            Boleto b2 = colectivo.PagarCon(tarjeta); // -160

            Assert.IsNotNull(b1);
            Assert.IsNotNull(b2);
            Assert.AreEqual(-160, tarjeta.Saldo);
        }

        [Test]
        public void TestPagarDevuelveNullSinSaldoSuficiente()
        {
            Tarjeta tarjeta = new Tarjeta();
            Colectivo colectivo = new Colectivo("K");

            // Sin cargar saldo, no puede pagar (excedería -1200)
            Boleto boleto = colectivo.PagarCon(tarjeta);
            Assert.IsNull(boleto);
        }

        [Test]
        public void TestCargaConSaldoNegativoNoExcedeLimite()
        {
            Tarjeta tarjeta = new Tarjeta();
            tarjeta.Cargar(2000);
            tarjeta.DescontarSaldo(3000); // Saldo: -1000

            bool resultado = tarjeta.Cargar(30000);
            Assert.IsTrue(resultado);
            Assert.AreEqual(29000, tarjeta.Saldo);
        }

        [Test]
        public void TestCargaConSaldoNegativoExcedeLimiteSuperior()
        {
            Tarjeta tarjeta = new Tarjeta();
            tarjeta.Cargar(30000);
            tarjeta.DescontarSaldo(20000); // Saldo: 10000

            bool resultado = tarjeta.Cargar(30000); // 10000 + 30000 = 40000
            Assert.IsTrue(resultado);
            Assert.AreEqual(40000, tarjeta.Saldo);

            // Intentar cargar más
            resultado = tarjeta.Cargar(2000); // Excedería 40000
            Assert.IsFalse(resultado);
            Assert.AreEqual(40000, tarjeta.Saldo);
        }

        [Test]
        public void TestSaldoNegativoMenosUnPesoDelLimite()
        {
            Tarjeta tarjeta = new Tarjeta();
            tarjeta.Cargar(2000);

            bool resultado = tarjeta.DescontarSaldo(3199);
            Assert.IsTrue(resultado);
            Assert.AreEqual(-1199, tarjeta.Saldo);
        }

        [Test]
        public void TestDescontarSaldoDesdeNegativo()
        {
            Tarjeta tarjeta = new Tarjeta();
            tarjeta.Cargar(2000);
            tarjeta.DescontarSaldo(2500); // Saldo: -500

            bool resultado = tarjeta.DescontarSaldo(500); // -500 - 500 = -1000
            Assert.IsTrue(resultado);
            Assert.AreEqual(-1000, tarjeta.Saldo);
        }

        [Test]
        public void TestDescontarSaldoDesdeNegativoExcedeLimite()
        {
            Tarjeta tarjeta = new Tarjeta();
            tarjeta.Cargar(2000);
            tarjeta.DescontarSaldo(3000); // Saldo: -1000

            bool resultado = tarjeta.DescontarSaldo(250); // -1000 - 250 = -1250 (NO PERMITIDO)
            Assert.IsFalse(resultado);
            Assert.AreEqual(-1000, tarjeta.Saldo);
        }

        [Test]
        public void TestCargaMultipleConSaldoNegativo()
        {
            Tarjeta tarjeta = new Tarjeta();
            tarjeta.Cargar(2000);
            Colectivo colectivo = new Colectivo("K");

            colectivo.PagarCon(tarjeta);
            colectivo.PagarCon(tarjeta); // Saldo: -1160

            tarjeta.Cargar(2000); // 840
            tarjeta.Cargar(3000); // 3840

            Assert.AreEqual(3840, tarjeta.Saldo);
        }

        [Test]
        public void TestSaldoCeroConDescuento()
        {
            Tarjeta tarjeta = new Tarjeta();
            // Saldo inicial 0
            bool resultado = tarjeta.DescontarSaldo(1200);
            Assert.IsTrue(resultado);
            Assert.AreEqual(-1200, tarjeta.Saldo);
        }

        [Test]
        public void TestViajesConsecutivosHastaLimiteNegativo()
        {
            Tarjeta tarjeta = new Tarjeta();
            tarjeta.Cargar(2000);
            Colectivo colectivo = new Colectivo("K");

            Boleto b1 = colectivo.PagarCon(tarjeta); // 420
            Assert.IsNotNull(b1);
            Assert.AreEqual(420, tarjeta.Saldo);

            Boleto b2 = colectivo.PagarCon(tarjeta); // -1160
            Assert.IsNotNull(b2);
            Assert.AreEqual(-1160, tarjeta.Saldo);

            Boleto b3 = colectivo.PagarCon(tarjeta); // -2740 NO PERMITIDO
            Assert.IsNull(b3);
            Assert.AreEqual(-1160, tarjeta.Saldo);
        }

        [Test]
        public void TestRecargaDespuesDeUsarViajePlus()
        {
            Tarjeta tarjeta = new Tarjeta();
            tarjeta.Cargar(2000);
            Colectivo colectivo = new Colectivo("K");

            colectivo.PagarCon(tarjeta);
            colectivo.PagarCon(tarjeta);
            // Saldo: -1160

            tarjeta.Cargar(5000);
            // Saldo: 3840
            Assert.AreEqual(3840, tarjeta.Saldo);

            // Puede seguir viajando normalmente
            Boleto boleto = colectivo.PagarCon(tarjeta);
            Assert.IsNotNull(boleto);
            Assert.AreEqual(2260, tarjeta.Saldo);
        }

        [Test]
        public void TestSaldoNegativoPequeno()
        {
            Tarjeta tarjeta = new Tarjeta();
            tarjeta.Cargar(2000);
            tarjeta.DescontarSaldo(2100);
            Assert.AreEqual(-100, tarjeta.Saldo);
        }

        [Test]
        public void TestDescontarExactamente1200DesdeCero()
        {
            Tarjeta tarjeta = new Tarjeta();
            bool resultado = tarjeta.DescontarSaldo(1200);
            Assert.IsTrue(resultado);
            Assert.AreEqual(-1200, tarjeta.Saldo);
        }

        [Test]
        public void TestDescontarMasDe1200DesdeCero()
        {
            Tarjeta tarjeta = new Tarjeta();
            bool resultado = tarjeta.DescontarSaldo(1201);
            Assert.IsFalse(resultado);
            Assert.AreEqual(0, tarjeta.Saldo);
        }

        [Test]
        public void TestCargaMinimaSobreSaldoNegativo()
        {
            Tarjeta tarjeta = new Tarjeta();
            tarjeta.Cargar(2000);
            tarjeta.DescontarSaldo(3000); // -1000

            bool resultado = tarjeta.Cargar(2000);
            Assert.IsTrue(resultado);
            Assert.AreEqual(1000, tarjeta.Saldo);
        }
    }
}