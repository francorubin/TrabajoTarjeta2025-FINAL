using NUnit.Framework;
using TarjetaSube;
using System;

namespace TarjetaSubeTest
{
    [TestFixture]
    public class TestBoletoGratuitoLimitaciones
    {
        [Test]
        public void TestBoletoGratuitoMaximo2ViajesGratisPorDia()
        {
            BoletoGratuito tarjeta = new BoletoGratuito();
            tarjeta.Cargar(10000);
            Colectivo colectivo = new Colectivo("K");
            TiempoFalso tiempo = new TiempoFalso(2024, 10, 14, 8, 0, 0);

            // Primer viaje gratis
            Boleto boleto1 = colectivo.PagarCon(tarjeta, tiempo);
            Assert.IsNotNull(boleto1);
            Assert.AreEqual(0, boleto1.Monto);
            Assert.AreEqual(10000, tarjeta.Saldo);

            tiempo.AgregarMinutos(10);

            // Segundo viaje gratis
            Boleto boleto2 = colectivo.PagarCon(tarjeta, tiempo);
            Assert.IsNotNull(boleto2);
            Assert.AreEqual(0, boleto2.Monto);
            Assert.AreEqual(10000, tarjeta.Saldo);

            tiempo.AgregarMinutos(10);

            // Tercer viaje - tarifa COMPLETA 1580
            Boleto boleto3 = colectivo.PagarCon(tarjeta, tiempo);
            Assert.IsNotNull(boleto3);
            Assert.AreEqual(1580, boleto3.Monto);
            Assert.AreEqual(8420, tarjeta.Saldo);
        }

        [Test]
        public void TestBoletoGratuitoTercerViajeCobraTarifaNormal()
        {
            BoletoGratuito tarjeta = new BoletoGratuito();
            tarjeta.Cargar(10000);
            Colectivo colectivo = new Colectivo("K");
            TiempoFalso tiempo = new TiempoFalso(2024, 10, 14, 8, 0, 0);

            // Primer viaje
            colectivo.PagarCon(tarjeta, tiempo);
            tiempo.AgregarMinutos(5);

            // Segundo viaje
            colectivo.PagarCon(tarjeta, tiempo);
            tiempo.AgregarMinutos(5);

            // Tercer viaje
            Boleto boleto3 = colectivo.PagarCon(tarjeta, tiempo);
            Assert.AreEqual(1580, boleto3.Monto);
            Assert.AreEqual(8420, tarjeta.Saldo);
        }

        [Test]
        public void TestBoletoGratuitoContadorSeReiniciaDiaSiguiente()
        {
            BoletoGratuito tarjeta = new BoletoGratuito();
            tarjeta.Cargar(10000);
            Colectivo colectivo = new Colectivo("K");
            TiempoFalso tiempo = new TiempoFalso(2024, 10, 14, 8, 0, 0);

            // Día 1: 2 viajes gratis
            Boleto b1 = colectivo.PagarCon(tarjeta, tiempo);
            tiempo.AgregarMinutos(10);
            Boleto b2 = colectivo.PagarCon(tarjeta, tiempo);

            Assert.AreEqual(0, b1.Monto);
            Assert.AreEqual(0, b2.Monto);

            // Avanzar al día siguiente
            tiempo.AgregarDias(1);

            // Nuevos 2 viajes gratis
            Boleto b3 = colectivo.PagarCon(tarjeta, tiempo);
            tiempo.AgregarMinutos(10);
            Boleto b4 = colectivo.PagarCon(tarjeta, tiempo);

            Assert.AreEqual(0, b3.Monto);
            Assert.AreEqual(0, b4.Monto);
        }

        [Test]
        public void TestBoletoGratuitoMultiplesViajes()
        {
            BoletoGratuito tarjeta = new BoletoGratuito();
            tarjeta.Cargar(20000);
            Colectivo colectivo = new Colectivo("K");
            TiempoFalso tiempo = new TiempoFalso(2024, 10, 14, 8, 0, 0);

            // Viaje 1 - gratis
            Boleto b1 = colectivo.PagarCon(tarjeta, tiempo);
            Assert.AreEqual(0, b1.Monto);
            Assert.AreEqual(20000, tarjeta.Saldo);

            tiempo.AgregarMinutos(10);

            // Viaje 2 - gratis
            Boleto b2 = colectivo.PagarCon(tarjeta, tiempo);
            Assert.AreEqual(0, b2.Monto);
            Assert.AreEqual(20000, tarjeta.Saldo);

            tiempo.AgregarMinutos(10);

            // Viaje 3 - tarifa completa (1580)
            Boleto b3 = colectivo.PagarCon(tarjeta, tiempo);
            Assert.AreEqual(1580, b3.Monto);
            Assert.AreEqual(18420, tarjeta.Saldo);

            tiempo.AgregarMinutos(10);

            // Viaje 4 - tarifa completa (1580)
            Boleto b4 = colectivo.PagarCon(tarjeta, tiempo);
            Assert.AreEqual(1580, b4.Monto);
            Assert.AreEqual(16840, tarjeta.Saldo);
        }

        [Test]
        public void TestBoletoGratuitoViajesEnDiferentesDias()
        {
            BoletoGratuito tarjeta = new BoletoGratuito();
            tarjeta.Cargar(10000);
            Colectivo colectivo = new Colectivo("K");
            TiempoFalso tiempo = new TiempoFalso(2024, 10, 14, 8, 0, 0);

            // Día 1: 3 viajes (2 gratis + 1 completo)
            colectivo.PagarCon(tarjeta, tiempo);
            tiempo.AgregarMinutos(10);
            colectivo.PagarCon(tarjeta, tiempo);
            tiempo.AgregarMinutos(10);
            colectivo.PagarCon(tarjeta, tiempo);

            // Día 2: Debe resetear y permitir 2 viajes gratis
            tiempo.AgregarDias(1);

            Boleto b4 = colectivo.PagarCon(tarjeta, tiempo);
            Assert.AreEqual(0, b4.Monto);

            tiempo.AgregarMinutos(10);
            Boleto b5 = colectivo.PagarCon(tarjeta, tiempo);
            Assert.AreEqual(0, b5.Monto);

            tiempo.AgregarMinutos(10);
            Boleto b6 = colectivo.PagarCon(tarjeta, tiempo);
            Assert.AreEqual(1580, b6.Monto);
        }

        [Test]
        public void TestBoletoGratuitoSinSaldoTercerViaje()
        {
            BoletoGratuito tarjeta = new BoletoGratuito();
            tarjeta.Cargar(2000);
            Colectivo colectivo = new Colectivo("K");
            TiempoFalso tiempo = new TiempoFalso(2024, 10, 14, 8, 0, 0);

            // Viaje 1: gratis - saldo: 2000
            Boleto b1 = colectivo.PagarCon(tarjeta, tiempo);
            Assert.AreEqual(0, b1.Monto);
            Assert.AreEqual(2000, tarjeta.Saldo);

            tiempo.AgregarMinutos(10);

            // Viaje 2: gratis - saldo: 2000
            Boleto b2 = colectivo.PagarCon(tarjeta, tiempo);
            Assert.AreEqual(0, b2.Monto);
            Assert.AreEqual(2000, tarjeta.Saldo);

            tiempo.AgregarMinutos(10);

            // Viaje 3: tarifa completa - saldo: 420
            Boleto b3 = colectivo.PagarCon(tarjeta, tiempo);
            Assert.IsNotNull(b3);
            Assert.AreEqual(1580, b3.Monto);
            Assert.AreEqual(420, tarjeta.Saldo);
        }

        [Test]
        public void TestBoletoGratuitoLineaDiferente()
        {
            BoletoGratuito tarjeta = new BoletoGratuito();
            tarjeta.Cargar(10000);
            Colectivo colectivoK = new Colectivo("K");
            Colectivo colectivo142 = new Colectivo("142");
            TiempoFalso tiempo = new TiempoFalso(2024, 10, 14, 8, 0, 0);

            // Primer viaje gratis en línea K
            Boleto b1 = colectivoK.PagarCon(tarjeta, tiempo);
            Assert.AreEqual(0, b1.Monto);
            Assert.AreEqual("K", b1.Linea);

            tiempo.AgregarMinutos(10);

            // Segundo viaje gratis en línea 142
            Boleto b2 = colectivo142.PagarCon(tarjeta, tiempo);
            Assert.AreEqual(0, b2.Monto);
            Assert.AreEqual("142", b2.Linea);

            tiempo.AgregarMinutos(10);

            // Tercer viaje - tarifa completa
            Boleto b3 = colectivoK.PagarCon(tarjeta, tiempo);
            Assert.AreEqual(1580, b3.Monto);
        }
    }
}