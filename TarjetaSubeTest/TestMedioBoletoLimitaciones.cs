using NUnit.Framework;
using TarjetaSube;
using System;

namespace TarjetaSubeTest
{
    [TestFixture]
    public class TestMedioBoletoLimitaciones
    {
        // ===== TESTS REQUERIDOS POR EL ENUNCIADO =====

        [Test]
        public void TestMedioBoletoNoPermiteViajarAntesDe5Minutos()
        {
            MedioBoleto tarjeta = new MedioBoleto();
            tarjeta.Cargar(5000);
            Colectivo colectivo = new Colectivo("K");
            TiempoFalso tiempo = new TiempoFalso();

            // Primer viaje - debe funcionar
            Boleto boleto1 = colectivo.PagarCon(tarjeta, tiempo);
            Assert.IsNotNull(boleto1);
            Assert.AreEqual(790, boleto1.Monto);

            // Intentar segundo viaje inmediatamente - NO debe funcionar
            Boleto boleto2 = colectivo.PagarCon(tarjeta, tiempo);
            Assert.IsNull(boleto2);
            Assert.AreEqual(4210, tarjeta.Saldo); // Saldo no cambió

            // Avanzar 4 minutos - todavía NO debe funcionar
            tiempo.AgregarMinutos(4);
            Boleto boleto3 = colectivo.PagarCon(tarjeta, tiempo);
            Assert.IsNull(boleto3);
            Assert.AreEqual(4210, tarjeta.Saldo);

            // Avanzar 1 minuto más (total 5 minutos) - AHORA SÍ debe funcionar
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

            // Primer viaje
            Boleto boleto1 = colectivo.PagarCon(tarjeta, tiempo);
            Assert.IsNotNull(boleto1);

            // Avanzar 5 minutos
            tiempo.AgregarMinutos(5);

            // Segundo viaje - debe funcionar
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

            // Primer viaje con medio boleto - 790
            Boleto boleto1 = colectivo.PagarCon(tarjeta, tiempo);
            Assert.IsNotNull(boleto1);
            Assert.AreEqual(790, boleto1.Monto);
            Assert.AreEqual(9210, tarjeta.Saldo); // 10000 - 790

            tiempo.AgregarMinutos(10);

            // Segundo viaje con medio boleto - 790
            Boleto boleto2 = colectivo.PagarCon(tarjeta, tiempo);
            Assert.IsNotNull(boleto2);
            Assert.AreEqual(790, boleto2.Monto);
            Assert.AreEqual(8420, tarjeta.Saldo); // 9210 - 790

            tiempo.AgregarMinutos(10);

            // Tercer viaje - tarifa COMPLETA 1580
            Boleto boleto3 = colectivo.PagarCon(tarjeta, tiempo);
            Assert.IsNotNull(boleto3);
            Assert.AreEqual(1580, boleto3.Monto);
            Assert.AreEqual(6840, tarjeta.Saldo); // 8420 - 1580
        }

        [Test]
        public void TestMedioBoletoTercerViajeCobraTarifaNormal()
        {
            MedioBoleto tarjeta = new MedioBoleto();
            tarjeta.Cargar(10000);
            Colectivo colectivo = new Colectivo("K");
            TiempoFalso tiempo = new TiempoFalso();

            // Primer viaje
            colectivo.PagarCon(tarjeta, tiempo);
            tiempo.AgregarMinutos(5);

            // Segundo viaje
            colectivo.PagarCon(tarjeta, tiempo);
            tiempo.AgregarMinutos(5);

            // Tercer viaje
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

            // Primer día - 2 viajes con medio boleto
            Boleto b1 = colectivo.PagarCon(tarjeta, tiempo);
            tiempo.AgregarMinutos(10);
            Boleto b2 = colectivo.PagarCon(tarjeta, tiempo);

            Assert.AreEqual(790, b1.Monto);
            Assert.AreEqual(790, b2.Monto);

            // Avanzar al día siguiente
            tiempo.AgregarDias(1);

            // Nuevos 2 viajes con medio boleto
            Boleto b3 = colectivo.PagarCon(tarjeta, tiempo);
            tiempo.AgregarMinutos(10);
            Boleto b4 = colectivo.PagarCon(tarjeta, tiempo);

            Assert.AreEqual(790, b3.Monto);
            Assert.AreEqual(790, b4.Monto);
        }

        // ===== TESTS ADICIONALES PARA COBERTURA =====

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

            // Viaje 1 - medio boleto (790)
            Boleto b1 = colectivo.PagarCon(tarjeta, tiempo);
            Assert.AreEqual(790, b1.Monto);
            Assert.AreEqual(19210, tarjeta.Saldo);

            tiempo.AgregarMinutos(10);

            // Viaje 2 - medio boleto (790)
            Boleto b2 = colectivo.PagarCon(tarjeta, tiempo);
            Assert.AreEqual(790, b2.Monto);
            Assert.AreEqual(18420, tarjeta.Saldo);

            tiempo.AgregarMinutos(10);

            // Viaje 3 - tarifa completa (1580)
            Boleto b3 = colectivo.PagarCon(tarjeta, tiempo);
            Assert.AreEqual(1580, b3.Monto);
            Assert.AreEqual(16840, tarjeta.Saldo);

            tiempo.AgregarMinutos(10);

            // Viaje 4 - tarifa completa (1580)
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
            TiempoFalso tiempo = new TiempoFalso(2024, 10, 14, 23, 55, 0);

            // Viaje al final del día
            Boleto b1 = colectivo.PagarCon(tarjeta, tiempo);
            Assert.AreEqual(790, b1.Monto);

            // Avanzar al día siguiente (00:05 del martes)
            tiempo.AgregarMinutos(10);

            // Debe poder hacer 2 viajes con medio boleto nuevamente
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

            // Viaje 1: 2000 - 790 = 1210
            Boleto b1 = colectivo.PagarCon(tarjeta, tiempo);
            Assert.AreEqual(790, b1.Monto);
            Assert.AreEqual(1210, tarjeta.Saldo);

            tiempo.AgregarMinutos(10);

            // Viaje 2: 1210 - 790 = 420
            Boleto b2 = colectivo.PagarCon(tarjeta, tiempo);
            Assert.AreEqual(790, b2.Monto);
            Assert.AreEqual(420, tarjeta.Saldo);

            tiempo.AgregarMinutos(10);

            // Viaje 3 (tercero del día, tarifa completa): 420 - 1580 = -1160
            Boleto b3 = colectivo.PagarCon(tarjeta, tiempo);
            Assert.IsNotNull(b3);
            Assert.AreEqual(1580, b3.Monto); // Tarifa completa
            Assert.AreEqual(-1160, tarjeta.Saldo); // CORREGIDO
        }

        [Test]
        public void TestMedioBoletoVerificaTiempoEnSegundos()
        {
            MedioBoleto tarjeta = new MedioBoleto();
            tarjeta.Cargar(5000);
            Colectivo colectivo = new Colectivo("K");
            TiempoFalso tiempo = new TiempoFalso();

            colectivo.PagarCon(tarjeta, tiempo);

            // Avanzar exactamente 300 segundos (5 minutos)
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

            // Primer viaje en línea K
            Boleto b1 = colectivoK.PagarCon(tarjeta, tiempo);
            Assert.AreEqual(790, b1.Monto);
            Assert.AreEqual("K", b1.Linea);

            tiempo.AgregarMinutos(10);

            // Segundo viaje en línea 142
            Boleto b2 = colectivo142.PagarCon(tarjeta, tiempo);
            Assert.AreEqual(790, b2.Monto);
            Assert.AreEqual("142", b2.Linea);

            tiempo.AgregarMinutos(10);

            // Tercer viaje - tarifa completa
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

            // Viaje 1: 2000 - 790 = 1210
            colectivo.PagarCon(tarjeta, tiempo);
            tiempo.AgregarMinutos(10);

            // Viaje 2: 1210 - 790 = 420
            colectivo.PagarCon(tarjeta, tiempo);
            tiempo.AgregarMinutos(10);

            // Viaje 3: 420 - 1580 = -1160 (permitido)
            colectivo.PagarCon(tarjeta, tiempo);

            // Intentar cuarto viaje: -1160 - 1580 = -2740 (NO permitido, excede -1200)
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

            // Viajes hasta saldo negativo
            colectivo.PagarCon(tarjeta, tiempo); // 1210
            tiempo.AgregarMinutos(10);
            colectivo.PagarCon(tarjeta, tiempo); // 420
            tiempo.AgregarMinutos(10);
            colectivo.PagarCon(tarjeta, tiempo); // -1160

            // Recargar 5000
            tarjeta.Cargar(5000);

            // Saldo esperado: -1160 + 5000 = 3840
            Assert.AreEqual(3840, tarjeta.Saldo);
        }

        [Test]
        public void TestMedioBoletoViajesEnDiferentesDias()
        {
            MedioBoleto tarjeta = new MedioBoleto();
            tarjeta.Cargar(10000);
            Colectivo colectivo = new Colectivo("K");
            TiempoFalso tiempo = new TiempoFalso(2024, 10, 14, 8, 0, 0);

            // Día 1: 3 viajes (2 medio + 1 completo)
            colectivo.PagarCon(tarjeta, tiempo);
            tiempo.AgregarMinutos(10);
            colectivo.PagarCon(tarjeta, tiempo);
            tiempo.AgregarMinutos(10);
            colectivo.PagarCon(tarjeta, tiempo);

            // Día 2: Debe resetear y permitir 2 medios boletos
            tiempo.AgregarDias(1);

            Boleto b4 = colectivo.PagarCon(tarjeta, tiempo);
            Assert.AreEqual(790, b4.Monto); // Medio boleto del día 2

            tiempo.AgregarMinutos(10);
            Boleto b5 = colectivo.PagarCon(tarjeta, tiempo);
            Assert.AreEqual(790, b5.Monto); // Medio boleto del día 2

            tiempo.AgregarMinutos(10);
            Boleto b6 = colectivo.PagarCon(tarjeta, tiempo);
            Assert.AreEqual(1580, b6.Monto); // Tarifa completa del día 2
        }
    }
}