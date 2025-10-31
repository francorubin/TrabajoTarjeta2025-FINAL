using NUnit.Framework;
using TarjetaSube;
using System;
using System.IO;

namespace TarjetaSubeTest
{
    [TestFixture]
    public class TestProgram
    {
        [Test]
        public void TestIntegracionTarjetaNormalCompleta()
        {
            Tarjeta tarjeta = new Tarjeta();
            Colectivo colectivo = new Colectivo("K");

            bool carga = tarjeta.Cargar(5000);
            Assert.IsTrue(carga);
            Assert.AreEqual(5000, tarjeta.Saldo);

            Boleto boleto = colectivo.PagarCon(tarjeta);
            Assert.IsNotNull(boleto);
            Assert.AreEqual("K", boleto.Linea);
            Assert.AreEqual(1580, boleto.Monto);
            Assert.AreEqual(3420, boleto.SaldoRestante);
            Assert.AreEqual("Normal", boleto.TipoBoleto);
            Assert.IsNotNull(boleto.Fecha);
        }

        [Test]
        public void TestIntegracionMedioBoletoCompleta()
        {
            MedioBoleto tarjeta = new MedioBoleto();
            Colectivo colectivo = new Colectivo("142");

            tarjeta.Cargar(3000);
            Assert.AreEqual(3000, tarjeta.Saldo);

            Boleto boleto = colectivo.PagarCon(tarjeta);
            Assert.IsNotNull(boleto);
            Assert.AreEqual("142", boleto.Linea);
            Assert.AreEqual(790, boleto.Monto);
            Assert.AreEqual("Medio Boleto", boleto.TipoBoleto);
        }

        [Test]
        public void TestIntegracionBoletoGratuitoCompleta()
        {
            BoletoGratuito tarjeta = new BoletoGratuito();
            Colectivo colectivo = new Colectivo("133");

            tarjeta.Cargar(2000);

            Boleto boleto = colectivo.PagarCon(tarjeta);
            Assert.IsNotNull(boleto);
            Assert.AreEqual("133", boleto.Linea);
            Assert.AreEqual(0, boleto.Monto);
            Assert.AreEqual("Boleto Gratuito", boleto.TipoBoleto);
            Assert.AreEqual(2000, tarjeta.Saldo);
        }

        [Test]
        public void TestIntegracionFranquiciaCompletaSinSaldo()
        {
            FranquiciaCompleta tarjeta = new FranquiciaCompleta();
            Colectivo colectivo = new Colectivo("K");

            Boleto boleto = colectivo.PagarCon(tarjeta);
            Assert.IsNotNull(boleto);
            Assert.AreEqual(0, boleto.Monto);
            Assert.AreEqual("Franquicia Completa", boleto.TipoBoleto);
        }

        [Test]
        public void TestIntegracionCargaMultiple()
        {
            Tarjeta tarjeta = new Tarjeta();

            tarjeta.Cargar(5000);
            tarjeta.Cargar(3000);
            tarjeta.Cargar(2000);

            Assert.AreEqual(10000, tarjeta.Saldo);
        }

        [Test]
        public void TestIntegracionCargaInvalida()
        {
            Tarjeta tarjeta = new Tarjeta();

            bool resultado = tarjeta.Cargar(1000);
            Assert.IsFalse(resultado);
            Assert.AreEqual(0, tarjeta.Saldo);
        }

        [Test]
        public void TestIntegracionCargaExcedeLimite()
        {
            Tarjeta tarjeta = new Tarjeta();

            tarjeta.Cargar(30000);
            tarjeta.Cargar(10000);
            bool resultado = tarjeta.Cargar(2000);

            Assert.IsFalse(resultado);
            Assert.AreEqual(40000, tarjeta.Saldo);
        }

        [Test]
        public void TestIntegracionPagarConSaldoCeroUsandoViajePlus()
        {
            Tarjeta tarjeta = new Tarjeta();
            Colectivo colectivo = new Colectivo("K");

            Boleto boleto = colectivo.PagarCon(tarjeta);
            Assert.IsNull(boleto);
            Assert.AreEqual(0, tarjeta.Saldo);
        }

        [Test]
        public void TestIntegracionPagarExcedeSaldoNegativo()
        {
            Tarjeta tarjeta = new Tarjeta();
            Colectivo colectivo = new Colectivo("K");

            colectivo.PagarCon(tarjeta);

            Assert.AreEqual(0, tarjeta.Saldo);
        }

        [Test]
        public void TestIntegracionViajeDespuesDeRecarga()
        {
            Tarjeta tarjeta = new Tarjeta();
            Colectivo colectivo = new Colectivo("K");

            tarjeta.Cargar(3000);
            colectivo.PagarCon(tarjeta);

            tarjeta.Cargar(2000);

            Boleto boleto = colectivo.PagarCon(tarjeta);
            Assert.IsNotNull(boleto);
            Assert.AreEqual(1840, tarjeta.Saldo);
        }

        [Test]
        public void TestIntegracionCambioDeLinea()
        {
            Tarjeta tarjeta = new Tarjeta();
            tarjeta.Cargar(5000);

            Colectivo colectivoK = new Colectivo("K");
            Colectivo colectivo142 = new Colectivo("142");

            Boleto b1 = colectivoK.PagarCon(tarjeta);
            Boleto b2 = colectivo142.PagarCon(tarjeta);

            Assert.AreEqual("K", b1.Linea);
            Assert.AreEqual("142", b2.Linea);
            Assert.AreEqual(1840, tarjeta.Saldo);
        }

        [Test]
        public void TestIntegracionMultiplesTiposDeTarjeta()
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

            Boleto b1 = colectivo.PagarCon(normal);
            Boleto b2 = colectivo.PagarCon(medio);
            Boleto b3 = colectivo.PagarCon(gratuito);
            Boleto b4 = colectivo.PagarCon(franquicia);

            Assert.AreEqual(3420, normal.Saldo);
            Assert.AreEqual(4210, medio.Saldo);
            Assert.AreEqual(5000, gratuito.Saldo);
            Assert.AreEqual(5000, franquicia.Saldo);
        }

        [Test]
        public void TestIntegracionSaldoNegativoYRecarga()
        {
            Tarjeta tarjeta = new Tarjeta();
            Colectivo colectivo = new Colectivo("K");

            tarjeta.Cargar(2000);
            colectivo.PagarCon(tarjeta);
            colectivo.PagarCon(tarjeta);

            Assert.AreEqual(-1160, tarjeta.Saldo);

            tarjeta.Cargar(3000);
            Assert.AreEqual(1840, tarjeta.Saldo);
        }

        [Test]
        public void TestIntegracionTodosLosMontosDeRecarga()
        {
            Tarjeta tarjeta = new Tarjeta();

            int[] montos = { 2000, 3000, 4000, 5000, 8000, 10000, 15000, 20000, 25000, 30000 };

            foreach (int monto in montos)
            {
                Tarjeta t = new Tarjeta();
                bool resultado = t.Cargar(monto);
                Assert.IsTrue(resultado, $"Falló al cargar {monto}");
                Assert.AreEqual(monto, t.Saldo);
            }
        }

        [Test]
        public void TestIntegracionFlujoCompletoUsuario()
        {
            MedioBoleto tarjeta = new MedioBoleto();

            tarjeta.Cargar(5000);
            Assert.AreEqual(5000, tarjeta.Saldo);

            Colectivo colectivoK = new Colectivo("K");
            Boleto boleto1 = colectivoK.PagarCon(tarjeta);

            Assert.IsNotNull(boleto1);
            Assert.AreEqual("K", boleto1.Linea);
            Assert.AreEqual(790, boleto1.Monto);
            Assert.AreEqual(4210, tarjeta.Saldo);

            Assert.AreEqual(4210, tarjeta.Saldo);

            Boleto boleto2 = colectivoK.PagarCon(tarjeta);
            Assert.AreEqual(3420, tarjeta.Saldo);

            tarjeta.Cargar(3000);
            Assert.AreEqual(6420, tarjeta.Saldo);

            Boleto boleto3 = colectivoK.PagarCon(tarjeta);
            Assert.AreEqual(5630, tarjeta.Saldo);
        }

        [Test]
        public void TestIntegracionErroresComunes()
        {
            Tarjeta tarjeta = new Tarjeta();

            bool resultado1 = tarjeta.Cargar(1000);
            Assert.IsFalse(resultado1);

            bool resultado2 = tarjeta.Cargar(-5000);
            Assert.IsFalse(resultado2);

            Colectivo colectivo = new Colectivo("K");
            Boleto boleto = colectivo.PagarCon(null);
            Assert.IsNull(boleto);
        }

        [Test]
        public void TestIntegracionLimitesSaldo()
        {
            Tarjeta tarjeta = new Tarjeta();

            tarjeta.Cargar(30000);
            tarjeta.Cargar(10000);
            Assert.AreEqual(40000, tarjeta.Saldo);

            bool resultado = tarjeta.Cargar(2000);
            Assert.IsFalse(resultado);
        }

        [Test]
        public void TestIntegracionDiferentesLineas()
        {
            Tarjeta tarjeta = new Tarjeta();
            tarjeta.Cargar(10000);

            string[] lineas = { "K", "142", "133", "101", "115" };

            foreach (string linea in lineas)
            {
                Colectivo colectivo = new Colectivo(linea);
                Boleto boleto = colectivo.PagarCon(tarjeta);

                Assert.IsNotNull(boleto);
                Assert.AreEqual(linea, boleto.Linea);
                Assert.AreEqual(1580, boleto.Monto);
            }

            Assert.AreEqual(2100, tarjeta.Saldo);
        }

        [Test]
        public void TestIntegracionBoletoContenidoCompleto()
        {
            Tarjeta tarjeta = new Tarjeta();
            tarjeta.Cargar(5000);
            Colectivo colectivo = new Colectivo("K");

            DateTime antes = DateTime.Now;
            Boleto boleto = colectivo.PagarCon(tarjeta);
            DateTime despues = DateTime.Now;

            Assert.IsNotNull(boleto);
            Assert.AreEqual("K", boleto.Linea);
            Assert.AreEqual(1580, boleto.Monto);
            Assert.AreEqual(3420, boleto.SaldoRestante);
            Assert.AreEqual("Normal", boleto.TipoBoleto);
            Assert.IsNotNull(boleto.Fecha);
            Assert.GreaterOrEqual(boleto.Fecha, antes);
            Assert.LessOrEqual(boleto.Fecha, despues);
        }

        [Test]
        public void TestIntegracionMedioBoletoHastaAgotar()
        {
            MedioBoleto tarjeta = new MedioBoleto();
            tarjeta.Cargar(2000);
            Colectivo colectivo = new Colectivo("K");

            Boleto b1 = colectivo.PagarCon(tarjeta);
            Assert.IsNotNull(b1);

            Boleto b2 = colectivo.PagarCon(tarjeta);
            Assert.IsNotNull(b2);

            Boleto b3 = colectivo.PagarCon(tarjeta);
            Assert.IsNotNull(b3);

            Assert.AreEqual(-370, tarjeta.Saldo);
        }

        [Test]
        public void TestIntegracionRecargaDespuesDeViajePlus()
        {
            Tarjeta tarjeta = new Tarjeta();
            Colectivo colectivo = new Colectivo("K");

            tarjeta.Cargar(2000);
            colectivo.PagarCon(tarjeta);
            colectivo.PagarCon(tarjeta);

            decimal saldoNegativo = tarjeta.Saldo;
            Assert.AreEqual(-1160, saldoNegativo);

            tarjeta.Cargar(5000);
            Assert.AreEqual(3840, tarjeta.Saldo);

            Boleto boleto = colectivo.PagarCon(tarjeta);
            Assert.IsNotNull(boleto);
            Assert.AreEqual(2260, tarjeta.Saldo);
        }

        [Test]
        public void TestIntegracionComparacionTodasLasTarjetas()
        {
            Tarjeta normal = new Tarjeta();
            MedioBoleto medio = new MedioBoleto();
            BoletoGratuito gratuito = new BoletoGratuito();
            FranquiciaCompleta franquicia = new FranquiciaCompleta();

            normal.Cargar(10000);
            medio.Cargar(10000);
            gratuito.Cargar(10000);
            franquicia.Cargar(10000);

            Colectivo colectivo = new Colectivo("K");

            for (int i = 0; i < 5; i++)
            {
                colectivo.PagarCon(normal);
                colectivo.PagarCon(medio);
                colectivo.PagarCon(gratuito);
                colectivo.PagarCon(franquicia);
            }

            Assert.AreEqual(2100, normal.Saldo);
            Assert.AreEqual(6050, medio.Saldo);
            Assert.AreEqual(10000, gratuito.Saldo);
            Assert.AreEqual(10000, franquicia.Saldo);
        }

        [Test]
        public void TestIntegracionEscenarioRealista()
        {
            MedioBoleto tarjeta = new MedioBoleto();

            tarjeta.Cargar(5000);
            Assert.AreEqual(5000, tarjeta.Saldo);

            Colectivo colectivoIda = new Colectivo("K");
            Colectivo colectivoVuelta = new Colectivo("142");

            colectivoIda.PagarCon(tarjeta);
            colectivoVuelta.PagarCon(tarjeta);

            colectivoIda.PagarCon(tarjeta);
            colectivoVuelta.PagarCon(tarjeta);

            colectivoIda.PagarCon(tarjeta);
            colectivoVuelta.PagarCon(tarjeta);

            Boleto juevesIda = colectivoIda.PagarCon(tarjeta);
            Assert.IsNotNull(juevesIda);
            Assert.Less(tarjeta.Saldo, 0);

            tarjeta.Cargar(3000);
            colectivoVuelta.PagarCon(tarjeta);

            colectivoIda.PagarCon(tarjeta);
            colectivoVuelta.PagarCon(tarjeta);

            Assert.AreEqual(100, tarjeta.Saldo);
        }

        [Test]
        public void TestIntegracionViajesConsecutivosMismaLinea()
        {
            Tarjeta tarjeta = new Tarjeta();
            tarjeta.Cargar(10000);
            Colectivo colectivo = new Colectivo("K");

            for (int i = 0; i < 5; i++)
            {
                Boleto boleto = colectivo.PagarCon(tarjeta);
                Assert.IsNotNull(boleto);
                Assert.AreEqual(1580, boleto.Monto);
            }

            Assert.AreEqual(2100, tarjeta.Saldo);
        }

        [Test]
        public void TestIntegracionFranquiciaCompletaVariosViajes()
        {
            FranquiciaCompleta tarjeta = new FranquiciaCompleta();
            Colectivo colectivo = new Colectivo("K");

            for (int i = 0; i < 10; i++)
            {
                Boleto boleto = colectivo.PagarCon(tarjeta);
                Assert.IsNotNull(boleto);
                Assert.AreEqual(0, boleto.Monto);
            }

            Assert.AreEqual(0, tarjeta.Saldo);
        }

        [Test]
        public void TestIntegracionBoletoGratuitoVariosViajes()
        {
            BoletoGratuito tarjeta = new BoletoGratuito();
            tarjeta.Cargar(5000);
            Colectivo colectivo = new Colectivo("K");

            for (int i = 0; i < 10; i++)
            {
                Boleto boleto = colectivo.PagarCon(tarjeta);
                Assert.IsNotNull(boleto);
            }

            Assert.AreEqual(5000, tarjeta.Saldo);
        }

        [Test]
        public void TestIntegracionRecargasMultiples()
        {
            Tarjeta tarjeta = new Tarjeta();

            tarjeta.Cargar(5000);
            tarjeta.Cargar(5000);
            tarjeta.Cargar(5000);

            Assert.AreEqual(15000, tarjeta.Saldo);
        }

        [Test]
        public void TestIntegracionLimiteSuperiorExacto()
        {
            Tarjeta tarjeta = new Tarjeta();

            tarjeta.Cargar(20000);
            tarjeta.Cargar(20000);

            Assert.AreEqual(40000, tarjeta.Saldo);

            bool resultado = tarjeta.Cargar(2000);
            Assert.IsFalse(resultado);
        }
    }
}