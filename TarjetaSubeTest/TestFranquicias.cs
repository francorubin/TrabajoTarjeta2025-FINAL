using NUnit.Framework;
using TarjetaSube;
using System;

namespace TarjetaSubeTest
{
    [TestFixture]
    public class TestFranquicias
    {
        // ===== TESTS REQUERIDOS POR EL ENUNCIADO =====

        [Test]
        public void TestMedioBoletoMontoEsMitadDelNormal()
        {
            MedioBoleto tarjeta = new MedioBoleto();
            tarjeta.Cargar(2000);
            Colectivo colectivo = new Colectivo("K");
            TiempoFalso tiempo = new TiempoFalso();

            Boleto boleto = colectivo.PagarCon(tarjeta, tiempo);

            Assert.IsNotNull(boleto);
            Assert.AreEqual(790, boleto.Monto);
            Assert.AreEqual("Medio Boleto", boleto.TipoBoleto);
            Assert.AreEqual(1210, tarjeta.Saldo);
        }

        [Test]
        public void TestFranquiciaCompletaSiemprePuedePagar()
        {
            FranquiciaCompleta tarjeta = new FranquiciaCompleta();
            Colectivo colectivo = new Colectivo("K");

            Boleto boleto = colectivo.PagarCon(tarjeta);

            Assert.IsNotNull(boleto);
            Assert.AreEqual(0, boleto.Monto);
            Assert.AreEqual("Franquicia Completa", boleto.TipoBoleto);
            Assert.AreEqual(0, tarjeta.Saldo);
        }

        // ===== TESTS DE MEDIO BOLETO =====

        [Test]
        public void TestMedioBoletoConstructor()
        {
            MedioBoleto tarjeta = new MedioBoleto();
            Assert.AreEqual(0, tarjeta.Saldo);
        }

        [Test]
        public void TestMedioBoletoObtenerTarifa()
        {
            MedioBoleto tarjeta = new MedioBoleto();
            decimal tarifa = tarjeta.ObtenerTarifa(1580);
            Assert.AreEqual(790, tarifa);
        }

        [Test]
        public void TestMedioBoletoObtenerTipoBoleto()
        {
            MedioBoleto tarjeta = new MedioBoleto();
            string tipo = tarjeta.ObtenerTipoBoleto();
            Assert.AreEqual("Medio Boleto", tipo);
        }

        [Test]
        public void TestMedioBoletoMultiplesViajes()
        {
            MedioBoleto tarjeta = new MedioBoleto();
            tarjeta.Cargar(5000);
            Colectivo colectivo = new Colectivo("K");
            TiempoFalso tiempo = new TiempoFalso();

            Boleto b1 = colectivo.PagarCon(tarjeta, tiempo);
            tiempo.AgregarMinutos(10);

            Boleto b2 = colectivo.PagarCon(tarjeta, tiempo);
            tiempo.AgregarMinutos(10);

            Boleto b3 = colectivo.PagarCon(tarjeta, tiempo);

            Assert.IsNotNull(b1);
            Assert.IsNotNull(b2);
            Assert.IsNotNull(b3);
            Assert.AreEqual(790, b1.Monto);
            Assert.AreEqual(790, b2.Monto);
            Assert.AreEqual(1580, b3.Monto); // Tercer viaje: tarifa completa
            Assert.AreEqual(1840, tarjeta.Saldo); // 5000 - 790 - 790 - 1580
        }

        [Test]
        public void TestMedioBoletoSinSaldo()
        {
            MedioBoleto tarjeta = new MedioBoleto();
            Colectivo colectivo = new Colectivo("K");
            TiempoFalso tiempo = new TiempoFalso();

            Boleto boleto = colectivo.PagarCon(tarjeta, tiempo);
            Assert.IsNotNull(boleto);
            Assert.AreEqual(-790, tarjeta.Saldo);
        }

        [Test]
        public void TestMedioBoletoConSaldoNegativo()
        {
            MedioBoleto tarjeta = new MedioBoleto();
            tarjeta.Cargar(2000);
            Colectivo colectivo = new Colectivo("K");
            TiempoFalso tiempo = new TiempoFalso();

            colectivo.PagarCon(tarjeta, tiempo); // 1210
            tiempo.AgregarMinutos(10);

            colectivo.PagarCon(tarjeta, tiempo); // 420
            tiempo.AgregarMinutos(10);

            Boleto boleto = colectivo.PagarCon(tarjeta, tiempo); // -1160 (tercero, tarifa completa)

            Assert.IsNotNull(boleto);
            Assert.AreEqual(-1160, tarjeta.Saldo);
        }

        [Test]
        public void TestMedioBoletoCargar()
        {
            MedioBoleto tarjeta = new MedioBoleto();
            bool resultado = tarjeta.Cargar(5000);
            Assert.IsTrue(resultado);
            Assert.AreEqual(5000, tarjeta.Saldo);
        }

        [Test]
        public void TestMedioBoletoDescontarSaldo()
        {
            MedioBoleto tarjeta = new MedioBoleto();
            tarjeta.Cargar(3000);
            bool resultado = tarjeta.DescontarSaldo(790);
            Assert.IsTrue(resultado);
            Assert.AreEqual(2210, tarjeta.Saldo);
        }

        [Test]
        public void TestMedioBoletoLimiteNegativo()
        {
            MedioBoleto tarjeta = new MedioBoleto();
            tarjeta.Cargar(2000);
            Colectivo colectivo = new Colectivo("K");
            TiempoFalso tiempo = new TiempoFalso();

            // Viaje 1: 2000 - 790 = 1210
            Boleto b1 = colectivo.PagarCon(tarjeta, tiempo);
            Assert.IsNotNull(b1);
            Assert.AreEqual(1210, tarjeta.Saldo);

            tiempo.AgregarMinutos(10);

            // Viaje 2: 1210 - 790 = 420
            Boleto b2 = colectivo.PagarCon(tarjeta, tiempo);
            Assert.IsNotNull(b2);
            Assert.AreEqual(420, tarjeta.Saldo);

            tiempo.AgregarMinutos(10);

            // Viaje 3 (tercero del día, tarifa completa): 420 - 1580 = -1160
            Boleto b3 = colectivo.PagarCon(tarjeta, tiempo);
            Assert.IsNotNull(b3);
            Assert.AreEqual(-1160, tarjeta.Saldo);

            tiempo.AgregarMinutos(10);

            // Viaje 4: -1160 - 1580 = -2740 (NO PERMITIDO, excede -1200)
            Boleto b4 = colectivo.PagarCon(tarjeta, tiempo);
            Assert.IsNull(b4); // Debe ser NULL porque excedería el límite
            Assert.AreEqual(-1160, tarjeta.Saldo); // Saldo no cambió
        }

        [Test]
        public void TestMedioBoletoRecargaDespuesDeSaldoNegativo()
        {
            MedioBoleto tarjeta = new MedioBoleto();
            tarjeta.Cargar(2000);
            Colectivo colectivo = new Colectivo("K");
            TiempoFalso tiempo = new TiempoFalso();

            colectivo.PagarCon(tarjeta, tiempo); // 1210
            tiempo.AgregarMinutos(10);

            colectivo.PagarCon(tarjeta, tiempo); // 420
            tiempo.AgregarMinutos(10);

            colectivo.PagarCon(tarjeta, tiempo); // -1160

            tarjeta.Cargar(5000);
            Assert.AreEqual(3840, tarjeta.Saldo); // -1160 + 5000
        }

        [Test]
        public void TestMedioBoletoViajesPorDifernetesLineas()
        {
            MedioBoleto tarjeta = new MedioBoleto();
            tarjeta.Cargar(5000);
            TiempoFalso tiempo = new TiempoFalso();

            Colectivo colectivoK = new Colectivo("K");
            Colectivo colectivo142 = new Colectivo("142");

            Boleto b1 = colectivoK.PagarCon(tarjeta, tiempo);
            tiempo.AgregarMinutos(10);

            Boleto b2 = colectivo142.PagarCon(tarjeta, tiempo);

            Assert.AreEqual("K", b1.Linea);
            Assert.AreEqual("142", b2.Linea);
            Assert.AreEqual(790, b1.Monto);
            Assert.AreEqual(790, b2.Monto);
            Assert.AreEqual(3420, tarjeta.Saldo);
        }

        // ===== TESTS DE BOLETO GRATUITO =====

        [Test]
        public void TestBoletoGratuitoConstructor()
        {
            BoletoGratuito tarjeta = new BoletoGratuito();
            Assert.AreEqual(0, tarjeta.Saldo);
        }

        [Test]
        public void TestBoletoGratuitoObtenerTarifa()
        {
            BoletoGratuito tarjeta = new BoletoGratuito();
            decimal tarifa = tarjeta.ObtenerTarifa(1580);
            Assert.AreEqual(0, tarifa);
        }

        [Test]
        public void TestBoletoGratuitoObtenerTipoBoleto()
        {
            BoletoGratuito tarjeta = new BoletoGratuito();
            string tipo = tarjeta.ObtenerTipoBoleto();
            Assert.AreEqual("Boleto Gratuito", tipo);
        }

        [Test]
        public void TestBoletoGratuitoNoDescuentaSaldo()
        {
            BoletoGratuito tarjeta = new BoletoGratuito();
            tarjeta.Cargar(2000);
            Colectivo colectivo = new Colectivo("K");

            Boleto boleto = colectivo.PagarCon(tarjeta);

            Assert.IsNotNull(boleto);
            Assert.AreEqual(0, boleto.Monto);
            Assert.AreEqual("Boleto Gratuito", boleto.TipoBoleto);
            Assert.AreEqual(2000, tarjeta.Saldo);
        }

        [Test]
        public void TestBoletoGratuitoSinSaldo()
        {
            BoletoGratuito tarjeta = new BoletoGratuito();
            Colectivo colectivo = new Colectivo("K");

            Boleto boleto = colectivo.PagarCon(tarjeta);

            Assert.IsNotNull(boleto);
            Assert.AreEqual(0, tarjeta.Saldo);
        }

        [Test]
        public void TestBoletoGratuitoMultiplesViajes()
        {
            BoletoGratuito tarjeta = new BoletoGratuito();
            tarjeta.Cargar(5000);
            Colectivo colectivo = new Colectivo("K");
            TiempoFalso tiempo = new TiempoFalso();

            // Primer viaje gratis
            colectivo.PagarCon(tarjeta, tiempo);
            Assert.AreEqual(5000, tarjeta.Saldo);

            tiempo.AgregarMinutos(10);

            // Segundo viaje gratis
            colectivo.PagarCon(tarjeta, tiempo);
            Assert.AreEqual(5000, tarjeta.Saldo);

            tiempo.AgregarMinutos(10);

            // Tercer viaje - tarifa completa
            colectivo.PagarCon(tarjeta, tiempo);
            Assert.AreEqual(3420, tarjeta.Saldo); // 5000 - 1580 = 3420
        }

        [Test]
        public void TestBoletoGratuitoCargar()
        {
            BoletoGratuito tarjeta = new BoletoGratuito();
            bool resultado = tarjeta.Cargar(3000);
            Assert.IsTrue(resultado);
            Assert.AreEqual(3000, tarjeta.Saldo);
        }

        [Test]
        public void TestBoletoGratuitoDescontarSaldo()
        {
            BoletoGratuito tarjeta = new BoletoGratuito();
            tarjeta.Cargar(3000);

            // Descontar 0 no modifica el saldo (viaje gratis)
            bool resultado = tarjeta.DescontarSaldo(0);
            Assert.IsTrue(resultado);
            Assert.AreEqual(3000, tarjeta.Saldo);

            // Descontar 1580 sí modifica el saldo (viaje con tarifa completa)
            resultado = tarjeta.DescontarSaldo(1580);
            Assert.IsTrue(resultado);
            Assert.AreEqual(1420, tarjeta.Saldo);
        }

        [Test]
        public void TestBoletoGratuitoSinSaldoCargado()
        {
            BoletoGratuito tarjeta = new BoletoGratuito();
            Colectivo colectivo = new Colectivo("K");
            TiempoFalso tiempo = new TiempoFalso();

            // Primeros 2 viajes gratis sin saldo
            Boleto b1 = colectivo.PagarCon(tarjeta, tiempo);
            Assert.IsNotNull(b1);
            Assert.AreEqual(0, tarjeta.Saldo);

            tiempo.AgregarMinutos(10);

            Boleto b2 = colectivo.PagarCon(tarjeta, tiempo);
            Assert.IsNotNull(b2);
            Assert.AreEqual(0, tarjeta.Saldo);

            tiempo.AgregarMinutos(10);

            // Tercer viaje sin saldo - NO puede viajar porque excedería -1200
            Boleto b3 = colectivo.PagarCon(tarjeta, tiempo);
            Assert.IsNull(b3); // Debe ser NULL
            Assert.AreEqual(0, tarjeta.Saldo);
        }

        [Test]
        public void TestBoletoGratuitoPorDiferentesLineas()
        {
            BoletoGratuito tarjeta = new BoletoGratuito();
            tarjeta.Cargar(5000);

            Colectivo colectivoK = new Colectivo("K");
            Colectivo colectivo142 = new Colectivo("142");

            Boleto b1 = colectivoK.PagarCon(tarjeta);
            Boleto b2 = colectivo142.PagarCon(tarjeta);

            Assert.AreEqual(0, b1.Monto);
            Assert.AreEqual(0, b2.Monto);
            Assert.AreEqual(5000, tarjeta.Saldo);
        }

        // ===== TESTS DE FRANQUICIA COMPLETA =====

        [Test]
        public void TestFranquiciaCompletaConstructor()
        {
            FranquiciaCompleta tarjeta = new FranquiciaCompleta();
            Assert.AreEqual(0, tarjeta.Saldo);
        }

        [Test]
        public void TestFranquiciaCompletaObtenerTarifa()
        {
            FranquiciaCompleta tarjeta = new FranquiciaCompleta();
            decimal tarifa = tarjeta.ObtenerTarifa(1580);
            Assert.AreEqual(0, tarifa);
        }

        [Test]
        public void TestFranquiciaCompletaObtenerTipoBoleto()
        {
            FranquiciaCompleta tarjeta = new FranquiciaCompleta();
            string tipo = tarjeta.ObtenerTipoBoleto();
            Assert.AreEqual("Franquicia Completa", tipo);
        }

        [Test]
        public void TestFranquiciaCompletaNoDescuentaSaldo()
        {
            FranquiciaCompleta tarjeta = new FranquiciaCompleta();
            tarjeta.Cargar(5000);
            Colectivo colectivo = new Colectivo("K");

            Boleto boleto = colectivo.PagarCon(tarjeta);

            Assert.IsNotNull(boleto);
            Assert.AreEqual(0, boleto.Monto);
            Assert.AreEqual(5000, tarjeta.Saldo);
        }

        [Test]
        public void TestFranquiciaCompletaMultiplesViajes()
        {
            FranquiciaCompleta tarjeta = new FranquiciaCompleta();
            Colectivo colectivo = new Colectivo("K");

            Boleto b1 = colectivo.PagarCon(tarjeta);
            Boleto b2 = colectivo.PagarCon(tarjeta);
            Boleto b3 = colectivo.PagarCon(tarjeta);

            Assert.IsNotNull(b1);
            Assert.IsNotNull(b2);
            Assert.IsNotNull(b3);
            Assert.AreEqual(0, tarjeta.Saldo);
        }

        [Test]
        public void TestFranquiciaCompletaConSaldoCargado()
        {
            FranquiciaCompleta tarjeta = new FranquiciaCompleta();
            tarjeta.Cargar(5000);
            Colectivo colectivo = new Colectivo("K");

            Boleto boleto = colectivo.PagarCon(tarjeta);

            Assert.IsNotNull(boleto);
            Assert.AreEqual(5000, tarjeta.Saldo);
        }

        [Test]
        public void TestFranquiciaCompletaCargar()
        {
            FranquiciaCompleta tarjeta = new FranquiciaCompleta();
            bool resultado = tarjeta.Cargar(2000);
            Assert.IsTrue(resultado);
            Assert.AreEqual(2000, tarjeta.Saldo);
        }

        [Test]
        public void TestFranquiciaCompletaDescontarSaldo()
        {
            FranquiciaCompleta tarjeta = new FranquiciaCompleta();
            tarjeta.Cargar(3000);
            bool resultado = tarjeta.DescontarSaldo(1580);
            Assert.IsTrue(resultado);
            Assert.AreEqual(3000, tarjeta.Saldo);
        }

        [Test]
        public void TestFranquiciaCompletaSinSaldoInicial()
        {
            FranquiciaCompleta tarjeta = new FranquiciaCompleta();
            Colectivo colectivo = new Colectivo("K");

            for (int i = 0; i < 10; i++)
            {
                Boleto boleto = colectivo.PagarCon(tarjeta);
                Assert.IsNotNull(boleto);
            }

            Assert.AreEqual(0, tarjeta.Saldo);
        }

        [Test]
        public void TestFranquiciaCompletaPorDiferentesLineas()
        {
            FranquiciaCompleta tarjeta = new FranquiciaCompleta();

            Colectivo colectivoK = new Colectivo("K");
            Colectivo colectivo142 = new Colectivo("142");
            Colectivo colectivo133 = new Colectivo("133");

            Boleto b1 = colectivoK.PagarCon(tarjeta);
            Boleto b2 = colectivo142.PagarCon(tarjeta);
            Boleto b3 = colectivo133.PagarCon(tarjeta);

            Assert.AreEqual("K", b1.Linea);
            Assert.AreEqual("142", b2.Linea);
            Assert.AreEqual("133", b3.Linea);
            Assert.AreEqual(0, tarjeta.Saldo);
        }

        // ===== TESTS COMPARATIVOS ENTRE TIPOS =====

        [Test]
        public void TestComparacionTarifasEntreTodasLasTarjetas()
        {
            Tarjeta normal = new Tarjeta();
            MedioBoleto medio = new MedioBoleto();
            BoletoGratuito gratuito = new BoletoGratuito();
            FranquiciaCompleta franquicia = new FranquiciaCompleta();

            normal.Cargar(5000);
            medio.Cargar(5000);
            gratuito.Cargar(5000);
            franquicia.Cargar(5000);

            Colectivo colectivo = new Colectivo("K");
            TiempoFalso tiempo = new TiempoFalso();

            Boleto b1 = colectivo.PagarCon(normal);
            Boleto b2 = colectivo.PagarCon(medio, tiempo);
            Boleto b3 = colectivo.PagarCon(gratuito);
            Boleto b4 = colectivo.PagarCon(franquicia);

            Assert.AreEqual(1580, b1.Monto);
            Assert.AreEqual(790, b2.Monto);
            Assert.AreEqual(0, b3.Monto);
            Assert.AreEqual(0, b4.Monto);

            Assert.AreEqual(3420, normal.Saldo);
            Assert.AreEqual(4210, medio.Saldo);
            Assert.AreEqual(5000, gratuito.Saldo);
            Assert.AreEqual(5000, franquicia.Saldo);
        }

        [Test]
        public void TestTiposDeBoletoCorrectosParaCadaTarjeta()
        {
            Tarjeta normal = new Tarjeta();
            MedioBoleto medio = new MedioBoleto();
            BoletoGratuito gratuito = new BoletoGratuito();
            FranquiciaCompleta franquicia = new FranquiciaCompleta();

            Assert.AreEqual("Normal", normal.ObtenerTipoBoleto());
            Assert.AreEqual("Medio Boleto", medio.ObtenerTipoBoleto());
            Assert.AreEqual("Boleto Gratuito", gratuito.ObtenerTipoBoleto());
            Assert.AreEqual("Franquicia Completa", franquicia.ObtenerTipoBoleto());
        }

        [Test]
        public void TestTarifasCorrectasParaCadaTarjeta()
        {
            Tarjeta normal = new Tarjeta();
            MedioBoleto medio = new MedioBoleto();
            BoletoGratuito gratuito = new BoletoGratuito();
            FranquiciaCompleta franquicia = new FranquiciaCompleta();

            Assert.AreEqual(1580, normal.ObtenerTarifa(1580));
            Assert.AreEqual(790, medio.ObtenerTarifa(1580));
            Assert.AreEqual(0, gratuito.ObtenerTarifa(1580));
            Assert.AreEqual(0, franquicia.ObtenerTarifa(1580));
        }

        [Test]
        public void TestMedioBoletoConCargasMultiples()
        {
            MedioBoleto tarjeta = new MedioBoleto();
            tarjeta.Cargar(2000);
            tarjeta.Cargar(3000);
            tarjeta.Cargar(5000);

            Assert.AreEqual(10000, tarjeta.Saldo);

            Colectivo colectivo = new Colectivo("K");
            TiempoFalso tiempo = new TiempoFalso();

            colectivo.PagarCon(tarjeta, tiempo);

            Assert.AreEqual(9210, tarjeta.Saldo);
        }

        [Test]
        public void TestBoletoGratuitoConLimiteMaximoDeSaldo()
        {
            BoletoGratuito tarjeta = new BoletoGratuito();
            tarjeta.Cargar(30000);
            tarjeta.Cargar(10000);

            Assert.AreEqual(40000, tarjeta.Saldo);

            Colectivo colectivo = new Colectivo("K");
            colectivo.PagarCon(tarjeta);

            Assert.AreEqual(40000, tarjeta.Saldo);
        }

        [Test]
        public void TestFranquiciaCompletaConCargaInvalida()
        {
            FranquiciaCompleta tarjeta = new FranquiciaCompleta();
            bool resultado = tarjeta.Cargar(1000);

            Assert.IsFalse(resultado);
            Assert.AreEqual(0, tarjeta.Saldo);
        }

        [Test]
        public void TestMedioBoletoSaldoRestanteEnBoleto()
        {
            MedioBoleto tarjeta = new MedioBoleto();
            tarjeta.Cargar(5000);
            Colectivo colectivo = new Colectivo("K");
            TiempoFalso tiempo = new TiempoFalso();

            Boleto boleto = colectivo.PagarCon(tarjeta, tiempo);

            Assert.AreEqual(4210, boleto.SaldoRestante);
            Assert.AreEqual(4210, tarjeta.Saldo);
        }
    }
}