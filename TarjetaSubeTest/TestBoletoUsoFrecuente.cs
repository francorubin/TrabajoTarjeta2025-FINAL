using NUnit.Framework;
using TarjetaSube;
using System;

namespace TarjetaSubeTest
{
    [TestFixture]
    public class TestBoletoUsoFrecuente
    {
        [Test]
        public void TestViajes1A29TarifaNormal()
        {
            Tarjeta tarjeta = new Tarjeta();
            tarjeta.Cargar(30000);
            tarjeta.Cargar(30000);
            Colectivo colectivo = new Colectivo("K");
            TiempoFalso tiempo = new TiempoFalso(2024, 11, 1, 8, 0, 0);

            for (int i = 0; i < 29; i++)
            {
                Boleto boleto = colectivo.PagarCon(tarjeta, tiempo);
                Assert.IsNotNull(boleto);
                Assert.AreEqual(1580, boleto.Monto);
                tiempo.AgregarMinutos(10);
            }

            decimal saldoEsperado = 60000 - (1580 * 29);
            Assert.AreEqual(saldoEsperado, tarjeta.Saldo);
        }

        [Test]
        public void TestViaje30Tiene20PorCientoDescuento()
        {
            Tarjeta tarjeta = new Tarjeta();
            tarjeta.Cargar(30000);
            tarjeta.Cargar(30000);
            Colectivo colectivo = new Colectivo("K");
            TiempoFalso tiempo = new TiempoFalso(2024, 11, 1, 8, 0, 0);

            for (int i = 0; i < 29; i++)
            {
                colectivo.PagarCon(tarjeta, tiempo);
                tiempo.AgregarMinutos(10);
            }

            Boleto boleto30 = colectivo.PagarCon(tarjeta, tiempo);
            Assert.IsNotNull(boleto30);
            Assert.AreEqual(1264, boleto30.Monto);
        }

        [Test]
        public void TestViajes30A59Tienen20PorCientoDescuento()
        {
            Tarjeta tarjeta = new Tarjeta();
            tarjeta.Cargar(30000);
            tarjeta.Cargar(30000);
            tarjeta.Cargar(30000);
            Colectivo colectivo = new Colectivo("K");
            TiempoFalso tiempo = new TiempoFalso(2024, 11, 1, 8, 0, 0);

            for (int i = 0; i < 29; i++)
            {
                colectivo.PagarCon(tarjeta, tiempo);
                tiempo.AgregarMinutos(10);
            }

            decimal saldoAntes = tarjeta.Saldo;

            for (int i = 30; i <= 59; i++)
            {
                Boleto boleto = colectivo.PagarCon(tarjeta, tiempo);
                Assert.IsNotNull(boleto);
                Assert.AreEqual(1264, boleto.Monto);
                tiempo.AgregarMinutos(10);
            }

            decimal descuentoEsperado = saldoAntes - (1264 * 30);
            Assert.AreEqual(descuentoEsperado, tarjeta.Saldo);
        }

        [Test]
        public void TestViaje60Tiene25PorCientoDescuento()
        {
            Tarjeta tarjeta = new Tarjeta();
            tarjeta.Cargar(30000);
            tarjeta.Cargar(30000);
            tarjeta.Cargar(30000);
            Colectivo colectivo = new Colectivo("K");
            TiempoFalso tiempo = new TiempoFalso(2024, 11, 1, 8, 0, 0);

            for (int i = 0; i < 59; i++)
            {
                colectivo.PagarCon(tarjeta, tiempo);
                tiempo.AgregarMinutos(10);
            }

            Boleto boleto60 = colectivo.PagarCon(tarjeta, tiempo);
            Assert.IsNotNull(boleto60);
            Assert.AreEqual(1185, boleto60.Monto);
        }

        [Test]
        public void TestViajes60A80Tienen25PorCientoDescuento()
        {
            Tarjeta tarjeta = new Tarjeta();
            tarjeta.Cargar(30000);
            tarjeta.Cargar(30000);
            tarjeta.Cargar(30000);
            tarjeta.Cargar(30000);
            Colectivo colectivo = new Colectivo("K");
            TiempoFalso tiempo = new TiempoFalso(2024, 11, 1, 8, 0, 0);

            for (int i = 0; i < 59; i++)
            {
                colectivo.PagarCon(tarjeta, tiempo);
                tiempo.AgregarMinutos(10);
            }

            decimal saldoAntes = tarjeta.Saldo;

            for (int i = 60; i <= 80; i++)
            {
                Boleto boleto = colectivo.PagarCon(tarjeta, tiempo);
                Assert.IsNotNull(boleto);
                Assert.AreEqual(1185, boleto.Monto);
                tiempo.AgregarMinutos(10);
            }

            decimal descuentoEsperado = saldoAntes - (1185 * 21);
            Assert.AreEqual(descuentoEsperado, tarjeta.Saldo);
        }

        [Test]
        public void TestViaje81VuelveTarifaNormal()
        {
            Tarjeta tarjeta = new Tarjeta();
            tarjeta.Cargar(30000);
            tarjeta.Cargar(30000);
            tarjeta.Cargar(30000);
            tarjeta.Cargar(30000);
            Colectivo colectivo = new Colectivo("K");
            TiempoFalso tiempo = new TiempoFalso(2024, 11, 1, 8, 0, 0);

            for (int i = 0; i < 80; i++)
            {
                colectivo.PagarCon(tarjeta, tiempo);
                tiempo.AgregarMinutos(10);
            }

            Boleto boleto81 = colectivo.PagarCon(tarjeta, tiempo);
            Assert.IsNotNull(boleto81);
            Assert.AreEqual(1580, boleto81.Monto);
        }

        [Test]
        public void TestViajes81EnAdelanteTarifaNormal()
        {
            Tarjeta tarjeta = new Tarjeta();
            tarjeta.Cargar(30000);
            tarjeta.Cargar(30000);
            tarjeta.Cargar(30000);
            tarjeta.Cargar(30000);
            tarjeta.Cargar(30000);
            Colectivo colectivo = new Colectivo("K");
            TiempoFalso tiempo = new TiempoFalso(2024, 11, 1, 8, 0, 0);

            for (int i = 0; i < 80; i++)
            {
                colectivo.PagarCon(tarjeta, tiempo);
                tiempo.AgregarMinutos(10);
            }

            for (int i = 81; i <= 90; i++)
            {
                Boleto boleto = colectivo.PagarCon(tarjeta, tiempo);
                Assert.IsNotNull(boleto);
                Assert.AreEqual(1580, boleto.Monto);
                tiempo.AgregarMinutos(10);
            }
        }

        [Test]
        public void TestContadorSeReiniciaEnNuevoMes()
        {
            Tarjeta tarjeta = new Tarjeta();
            tarjeta.Cargar(30000);
            tarjeta.Cargar(30000);
            tarjeta.Cargar(30000);
            Colectivo colectivo = new Colectivo("K");
            TiempoFalso tiempo = new TiempoFalso(2024, 10, 31, 8, 0, 0);

            for (int i = 0; i < 40; i++)
            {
                colectivo.PagarCon(tarjeta, tiempo);
                tiempo.AgregarMinutos(10);
            }

            tiempo.AgregarDias(1);

            Boleto boletoNuevoMes = colectivo.PagarCon(tarjeta, tiempo);
            Assert.IsNotNull(boletoNuevoMes);
            Assert.AreEqual(1580, boletoNuevoMes.Monto);
        }

        [Test]
        public void TestSoloAplicaATarjetasNormales()
        {
            MedioBoleto medio = new MedioBoleto();
            medio.Cargar(30000);
            medio.Cargar(30000);
            medio.Cargar(30000);
            Colectivo colectivo = new Colectivo("K");
            TiempoFalso tiempo = new TiempoFalso(2024, 11, 1, 8, 0, 0);

            for (int i = 0; i < 2; i++)
            {
                Boleto boleto = colectivo.PagarCon(medio, tiempo);
                Assert.IsNotNull(boleto);
                Assert.AreEqual(790, boleto.Monto);
                tiempo.AgregarMinutos(10);
            }

            for (int i = 2; i < 40; i++)
            {
                Boleto boleto = colectivo.PagarCon(medio, tiempo);
                Assert.IsNotNull(boleto);
                Assert.AreEqual(1580, boleto.Monto);
                tiempo.AgregarMinutos(10);
            }
        }

        [Test]
        public void TestContadorMensualConCambioDeAnio()
        {
            Tarjeta tarjeta = new Tarjeta();
            tarjeta.Cargar(30000);
            tarjeta.Cargar(30000);
            tarjeta.Cargar(30000);
            Colectivo colectivo = new Colectivo("K");
            TiempoFalso tiempo = new TiempoFalso(2024, 12, 31, 8, 0, 0);

            for (int i = 0; i < 40; i++)
            {
                colectivo.PagarCon(tarjeta, tiempo);
                tiempo.AgregarMinutos(10);
            }

            tiempo.AgregarDias(1);

            Boleto boletoNuevoAnio = colectivo.PagarCon(tarjeta, tiempo);
            Assert.IsNotNull(boletoNuevoAnio);
            Assert.AreEqual(1580, boletoNuevoAnio.Monto);
        }

        [Test]
        public void TestDescuentosAcumulativosEnUnMes()
        {
            Tarjeta tarjeta = new Tarjeta();
            tarjeta.Cargar(30000);
            tarjeta.Cargar(30000);
            tarjeta.Cargar(30000);
            tarjeta.Cargar(30000);
            tarjeta.Cargar(30000);
            Colectivo colectivo = new Colectivo("K");
            TiempoFalso tiempo = new TiempoFalso(2024, 11, 1, 8, 0, 0);

            decimal totalGastado = 0;

            for (int i = 1; i <= 90; i++)
            {
                Boleto boleto = colectivo.PagarCon(tarjeta, tiempo);
                Assert.IsNotNull(boleto);
                totalGastado += boleto.Monto;
                tiempo.AgregarMinutos(10);
            }

            decimal esperado = (1580 * 29) + (1264 * 30) + (1185 * 21) + (1580 * 10);
            Assert.AreEqual(esperado, totalGastado);
        }

        [Test]
        public void TestViajesDelMesGetter()
        {
            Tarjeta tarjeta = new Tarjeta();
            tarjeta.Cargar(30000);
            Colectivo colectivo = new Colectivo("K");
            TiempoFalso tiempo = new TiempoFalso(2024, 11, 1, 8, 0, 0);

            Assert.AreEqual(0, tarjeta.ViajesDelMes);

            colectivo.PagarCon(tarjeta, tiempo);
            Assert.AreEqual(1, tarjeta.ViajesDelMes);

            tiempo.AgregarMinutos(10);
            colectivo.PagarCon(tarjeta, tiempo);
            Assert.AreEqual(2, tarjeta.ViajesDelMes);
        }

        [Test]
        public void TestFranquiciaCompletaNoTieneDescuentoUsoFrecuente()
        {
            FranquiciaCompleta franquicia = new FranquiciaCompleta();
            Colectivo colectivo = new Colectivo("K");
            TiempoFalso tiempo = new TiempoFalso(2024, 11, 1, 8, 0, 0);

            for (int i = 0; i < 100; i++)
            {
                Boleto boleto = colectivo.PagarCon(franquicia, tiempo);
                Assert.IsNotNull(boleto);
                Assert.AreEqual(0, boleto.Monto);
                tiempo.AgregarMinutos(10);
            }
        }

        [Test]
        public void TestBoletoGratuitoNoTieneDescuentoUsoFrecuente()
        {
            BoletoGratuito gratuito = new BoletoGratuito();
            gratuito.Cargar(30000);
            gratuito.Cargar(30000);
            Colectivo colectivo = new Colectivo("K");
            TiempoFalso tiempo = new TiempoFalso(2024, 11, 1, 8, 0, 0);

            for (int i = 0; i < 5; i++)
            {
                Boleto boleto = colectivo.PagarCon(gratuito, tiempo);
                Assert.IsNotNull(boleto);

                if (i < 2)
                {
                    Assert.AreEqual(0, boleto.Monto);
                }
                else
                {
                    Assert.AreEqual(1580, boleto.Monto);
                }

                tiempo.AgregarMinutos(10);
            }
        }

        [Test]
        public void TestPrimerViajeNuevoMes()
        {
            Tarjeta tarjeta = new Tarjeta();
            tarjeta.Cargar(30000);
            Colectivo colectivo = new Colectivo("K");
            TiempoFalso tiempo = new TiempoFalso(2024, 11, 1, 8, 0, 0);

            Boleto boleto = colectivo.PagarCon(tarjeta, tiempo);
            Assert.IsNotNull(boleto);
            Assert.AreEqual(1580, boleto.Monto);
            Assert.AreEqual(1, tarjeta.ViajesDelMes);
        }

        [Test]
        public void TestTransicionEntreTranamos()
        {
            Tarjeta tarjeta = new Tarjeta();
            tarjeta.Cargar(30000);
            tarjeta.Cargar(30000);
            tarjeta.Cargar(30000);
            tarjeta.Cargar(30000);
            Colectivo colectivo = new Colectivo("K");
            TiempoFalso tiempo = new TiempoFalso(2024, 11, 1, 8, 0, 0);

            for (int i = 0; i < 28; i++)
            {
                colectivo.PagarCon(tarjeta, tiempo);
                tiempo.AgregarMinutos(10);
            }

            Boleto b29 = colectivo.PagarCon(tarjeta, tiempo);
            Assert.AreEqual(1580, b29.Monto);

            tiempo.AgregarMinutos(10);
            Boleto b30 = colectivo.PagarCon(tarjeta, tiempo);
            Assert.AreEqual(1264, b30.Monto);

            for (int i = 0; i < 29; i++)
            {
                colectivo.PagarCon(tarjeta, tiempo);
                tiempo.AgregarMinutos(10);
            }

            Boleto b60 = colectivo.PagarCon(tarjeta, tiempo);
            Assert.AreEqual(1185, b60.Monto);

            for (int i = 0; i < 20; i++)
            {
                colectivo.PagarCon(tarjeta, tiempo);
                tiempo.AgregarMinutos(10);
            }

            Boleto b81 = colectivo.PagarCon(tarjeta, tiempo);
            Assert.AreEqual(1580, b81.Monto);
        }
    }
}