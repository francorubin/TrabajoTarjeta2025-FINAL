using NUnit.Framework;
using TarjetaSube;
using System;

namespace TarjetaSubeTest
{
    [TestFixture]
    public class TestSaldoPendiente
    {
        // ===== TESTS REQUERIDOS POR EL ENUNCIADO =====

        [Test]
        public void TestCargaExcedeLimiteMaximoYGuardaPendiente()
        {
            Tarjeta tarjeta = new Tarjeta();
            tarjeta.Cargar(30000);
            tarjeta.Cargar(30000);

            // Saldo debe ser 56000 (límite máximo)
            Assert.AreEqual(56000, tarjeta.Saldo);

            // Saldo pendiente debe ser 4000 (60000 - 56000)
            Assert.AreEqual(4000, tarjeta.SaldoPendiente);
        }

        [Test]
        public void TestSaldoPendienteSeAcreditaDespuesDeViaje()
        {
            Tarjeta tarjeta = new Tarjeta();
            tarjeta.Cargar(30000);
            tarjeta.Cargar(30000);

            // Saldo: 56000, Pendiente: 4000
            Assert.AreEqual(56000, tarjeta.Saldo);
            Assert.AreEqual(4000, tarjeta.SaldoPendiente);

            Colectivo colectivo = new Colectivo("K");
            colectivo.PagarCon(tarjeta);

            // Después del viaje: 56000 - 1580 = 54420
            // Se acredita saldo pendiente: 54420 + 1580 = 56000
            // Pendiente: 4000 - 1580 = 2420
            Assert.AreEqual(56000, tarjeta.Saldo);
            Assert.AreEqual(2420, tarjeta.SaldoPendiente);
        }

        // ===== TESTS ADICIONALES PARA COBERTURA =====

        [Test]
        public void TestCargaMultipleSuperaLimiteMaximo()
        {
            Tarjeta tarjeta = new Tarjeta();
            tarjeta.Cargar(30000);

            // Intentar cargar 20000 (30000 + 20000 = 50000, > 40000, <= 56000) → RECHAZADA
            bool resultado1 = tarjeta.Cargar(20000);
            Assert.IsFalse(resultado1);
            Assert.AreEqual(30000, tarjeta.Saldo);

            // Intentar cargar 15000 (30000 + 15000 = 45000, > 40000, <= 56000) → RECHAZADA
            bool resultado2 = tarjeta.Cargar(15000);
            Assert.IsFalse(resultado2);
            Assert.AreEqual(30000, tarjeta.Saldo);

            // Para que funcione la funcionalidad de pendiente, necesitamos cargar algo que supere 56000
            // Cargar 30000 (30000 + 30000 = 60000 > 56000) → ACEPTADA con pendiente
            bool resultado3 = tarjeta.Cargar(30000);
            Assert.IsTrue(resultado3);
            Assert.AreEqual(56000, tarjeta.Saldo);
            Assert.AreEqual(4000, tarjeta.SaldoPendiente);
        }
        

        [Test]
        public void TestSaldoPendienteSeAcreditaProgresivamente()
        {
            Tarjeta tarjeta = new Tarjeta();
            tarjeta.Cargar(30000);
            tarjeta.Cargar(30000);
            // Saldo: 56000, Pendiente: 4000

            Colectivo colectivo = new Colectivo("K");

            // Primer viaje
            colectivo.PagarCon(tarjeta);
            Assert.AreEqual(56000, tarjeta.Saldo);
            Assert.AreEqual(2420, tarjeta.SaldoPendiente);

            // Segundo viaje
            colectivo.PagarCon(tarjeta);
            Assert.AreEqual(56000, tarjeta.Saldo);
            Assert.AreEqual(840, tarjeta.SaldoPendiente);

            // Tercer viaje (agota el pendiente)
            colectivo.PagarCon(tarjeta);
            Assert.AreEqual(55260, tarjeta.Saldo); // 56000 - 1580 + 840
            Assert.AreEqual(0, tarjeta.SaldoPendiente);
        }

        [Test]
        public void TestSaldoPendienteConVariasCargas()
        {
            Tarjeta tarjeta = new Tarjeta();

            tarjeta.Cargar(20000);
            Assert.AreEqual(20000, tarjeta.Saldo);
            Assert.AreEqual(0, tarjeta.SaldoPendiente);

            // Intentar cargar 30000 (20000 + 30000 = 50000, > 40000, <= 56000) → RECHAZADA
            bool resultado1 = tarjeta.Cargar(30000);
            Assert.IsFalse(resultado1);
            Assert.AreEqual(20000, tarjeta.Saldo);
            Assert.AreEqual(0, tarjeta.SaldoPendiente);

            // Cargar 10000 (20000 + 10000 = 30000, OK)
            bool resultado2 = tarjeta.Cargar(10000);
            Assert.IsTrue(resultado2);
            Assert.AreEqual(30000, tarjeta.Saldo);
            Assert.AreEqual(0, tarjeta.SaldoPendiente);

            // Cargar 30000 (30000 + 30000 = 60000 > 56000) → Saldo 56000, Pendiente 4000
            bool resultado3 = tarjeta.Cargar(30000);
            Assert.IsTrue(resultado3);
            Assert.AreEqual(56000, tarjeta.Saldo);
            Assert.AreEqual(4000, tarjeta.SaldoPendiente);

            // Intentar cargar 5000 (saldo ya está en 56000) → Todo a pendiente
            bool resultado4 = tarjeta.Cargar(5000);
            Assert.IsTrue(resultado4);
            Assert.AreEqual(56000, tarjeta.Saldo);
            Assert.AreEqual(9000, tarjeta.SaldoPendiente);
        }


        [Test]
        public void TestAcreditarCargaConSaldoNegativo()
        {
            Tarjeta tarjeta = new Tarjeta();
            tarjeta.Cargar(30000);
            tarjeta.Cargar(30000);
            // Saldo: 56000, Pendiente: 4000

            Colectivo colectivo = new Colectivo("K");

            // Hacer varios viajes para llegar a saldo bajo
            for (int i = 0; i < 35; i++)
            {
                colectivo.PagarCon(tarjeta);
            }

            // Verificar que todo el pendiente se acreditó
            Assert.AreEqual(0, tarjeta.SaldoPendiente);
        }

        [Test]
        public void TestSaldoPendienteSinViajes()
        {
            Tarjeta tarjeta = new Tarjeta();
            tarjeta.Cargar(30000);
            tarjeta.Cargar(30000);

            // El saldo pendiente no se acredita automáticamente sin viajes
            Assert.AreEqual(56000, tarjeta.Saldo);
            Assert.AreEqual(4000, tarjeta.SaldoPendiente);
        }

        [Test]
        public void TestAcreditarCargaSinSaldoPendiente()
        {
            Tarjeta tarjeta = new Tarjeta();
            tarjeta.Cargar(5000);

            Colectivo colectivo = new Colectivo("K");
            colectivo.PagarCon(tarjeta);

            // Saldo: 3420, Pendiente: 0
            Assert.AreEqual(3420, tarjeta.Saldo);
            Assert.AreEqual(0, tarjeta.SaldoPendiente);
        }

        [Test]
        public void TestSaldoPendienteNoSeAcreditaSiSaldoEstaEnMaximo()
        {
            Tarjeta tarjeta = new Tarjeta();
            tarjeta.Cargar(30000);

            // Intentar cargar 26000 (30000 + 26000 = 56000, NO > 56000) → RECHAZADA
            bool resultado = tarjeta.Cargar(26000);
            Assert.IsFalse(resultado);
            Assert.AreEqual(30000, tarjeta.Saldo);
            Assert.AreEqual(0, tarjeta.SaldoPendiente);

            // Llamar a AcreditarCarga manualmente
            tarjeta.AcreditarCarga();

            Assert.AreEqual(30000, tarjeta.Saldo);
            Assert.AreEqual(0, tarjeta.SaldoPendiente);
        }

        [Test]
        public void TestCargaExactaAlLimiteMaximoSinPendiente()
        {
            Tarjeta tarjeta = new Tarjeta();
            tarjeta.Cargar(30000);
            // Intentar cargar 26000 (30000 + 26000 = 56000)
            // Como 56000 NO es > 56000, cae en la condición > 40000 y RECHAZA
            bool resultado = tarjeta.Cargar(26000);

            Assert.IsFalse(resultado); // Ahora debe ser FALSE
            Assert.AreEqual(30000, tarjeta.Saldo); // Saldo no cambia
            Assert.AreEqual(0, tarjeta.SaldoPendiente);
        }

        [Test]
        public void TestSaldoPendienteConMedioBoleto()
        {
            MedioBoleto tarjeta = new MedioBoleto();
            tarjeta.Cargar(30000);
            tarjeta.Cargar(30000);

            // Saldo: 56000, Pendiente: 4000
            Assert.AreEqual(56000, tarjeta.Saldo);
            Assert.AreEqual(4000, tarjeta.SaldoPendiente);

            Colectivo colectivo = new Colectivo("K");
            TiempoFalso tiempo = new TiempoFalso();

            colectivo.PagarCon(tarjeta, tiempo);

            // Después del viaje con medio boleto (790)
            // 56000 - 790 + 790 = 56000
            Assert.AreEqual(56000, tarjeta.Saldo);
            Assert.AreEqual(3210, tarjeta.SaldoPendiente);
        }

        [Test]
        public void TestSaldoPendienteConFranquiciaCompleta()
        {
            FranquiciaCompleta tarjeta = new FranquiciaCompleta();
            tarjeta.Cargar(30000);
            tarjeta.Cargar(30000);

            // Saldo: 56000, Pendiente: 4000
            Assert.AreEqual(56000, tarjeta.Saldo);
            Assert.AreEqual(4000, tarjeta.SaldoPendiente);

            Colectivo colectivo = new Colectivo("K");
            colectivo.PagarCon(tarjeta);

            // Franquicia completa no descuenta saldo
            Assert.AreEqual(56000, tarjeta.Saldo);
            Assert.AreEqual(4000, tarjeta.SaldoPendiente);
        }

        [Test]
        public void TestSaldoPendienteConBoletoGratuito()
        {
            BoletoGratuito tarjeta = new BoletoGratuito();
            tarjeta.Cargar(30000);
            tarjeta.Cargar(30000);

            // Saldo: 56000, Pendiente: 4000
            Assert.AreEqual(56000, tarjeta.Saldo);
            Assert.AreEqual(4000, tarjeta.SaldoPendiente);

            Colectivo colectivo = new Colectivo("K");
            TiempoFalso tiempo = new TiempoFalso();

            // Primer viaje gratis
            colectivo.PagarCon(tarjeta, tiempo);
            Assert.AreEqual(56000, tarjeta.Saldo);
            Assert.AreEqual(4000, tarjeta.SaldoPendiente);

            tiempo.AgregarMinutos(10);

            // Segundo viaje gratis
            colectivo.PagarCon(tarjeta, tiempo);
            Assert.AreEqual(56000, tarjeta.Saldo);
            Assert.AreEqual(4000, tarjeta.SaldoPendiente);

            tiempo.AgregarMinutos(10);

            // Tercer viaje (paga tarifa completa)
            colectivo.PagarCon(tarjeta, tiempo);
            Assert.AreEqual(56000, tarjeta.Saldo);
            Assert.AreEqual(2420, tarjeta.SaldoPendiente);
        }

        [Test]
        public void TestCargaMinimaSuperaLimite()
        {
            Tarjeta tarjeta = new Tarjeta();
            tarjeta.Cargar(30000);
            tarjeta.Cargar(25000); // 30000 + 25000 = 55000 (> 40000, <= 56000) → RECHAZADA

            // La carga de 25000 fue rechazada, saldo sigue en 30000
            Assert.AreEqual(30000, tarjeta.Saldo);
            Assert.AreEqual(0, tarjeta.SaldoPendiente);

            // Ahora cargar 2000 (30000 + 2000 = 32000, OK)
            bool resultado = tarjeta.Cargar(2000);
            Assert.IsTrue(resultado);
            Assert.AreEqual(32000, tarjeta.Saldo);
            Assert.AreEqual(0, tarjeta.SaldoPendiente);
        }

        [Test]
        public void TestSaldoPendienteSeAcreditaHastaLimite()
        {
            Tarjeta tarjeta = new Tarjeta();
            tarjeta.Cargar(30000);
            tarjeta.Cargar(30000);
            // Saldo: 56000, Pendiente: 4000

            Colectivo colectivo = new Colectivo("K");

            // Viaje pequeño
            colectivo.PagarCon(tarjeta);
            // Saldo vuelve a 56000, pendiente: 2420

            // Intentar cargar más
            tarjeta.Cargar(5000);
            // Todo va a pendiente
            Assert.AreEqual(56000, tarjeta.Saldo);
            Assert.AreEqual(7420, tarjeta.SaldoPendiente);
        }

        [Test]
        public void TestGetterSaldoPendiente()
        {
            Tarjeta tarjeta = new Tarjeta();
            tarjeta.Cargar(30000);
            tarjeta.Cargar(30000);

            decimal pendiente = tarjeta.SaldoPendiente;
            Assert.AreEqual(4000, pendiente);
        }

        [Test]
        public void TestAcreditarCargaConEspacioJusto()
        {
            Tarjeta tarjeta = new Tarjeta();
            tarjeta.Cargar(30000);
            tarjeta.Cargar(30000);
            // Saldo: 56000, Pendiente: 4000

            // Descontar exactamente 4000
            tarjeta.DescontarSaldo(4000);

            // Saldo: 56000 - 4000 + 4000 = 56000
            Assert.AreEqual(56000, tarjeta.Saldo);
            Assert.AreEqual(0, tarjeta.SaldoPendiente);
        }

        [Test]
        public void TestCargaInvalidaNoAfectaSaldoPendiente()
        {
            Tarjeta tarjeta = new Tarjeta();
            tarjeta.Cargar(30000);

            bool resultado = tarjeta.Cargar(1000); // Carga inválida

            Assert.IsFalse(resultado);
            Assert.AreEqual(30000, tarjeta.Saldo);
            Assert.AreEqual(0, tarjeta.SaldoPendiente);
        }
    }
}
