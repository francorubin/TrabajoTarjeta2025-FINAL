using NUnit.Framework;
using TarjetaSube;

namespace TarjetaSubeTest
{
    [TestFixture]
    public class TestFranquicias
    {

        [Test]
        public void TestMedioBoletoMontoEsMitadDelNormal()
        {
            MedioBoleto tarjeta = new MedioBoleto();
            tarjeta.Cargar(2000);
            Colectivo colectivo = new Colectivo("K");

            Boleto boleto = colectivo.PagarCon(tarjeta);

            Assert.IsNotNull(boleto);
            Assert.AreEqual(790, boleto.Monto); // 1580 / 2
            Assert.AreEqual("Medio Boleto", boleto.TipoBoleto);
            Assert.AreEqual(1210, tarjeta.Saldo); // 2000 - 790
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

        [Test]
        public void TestMedioBoletoMultiplesViajes()
        {
            MedioBoleto tarjeta = new MedioBoleto();
            tarjeta.Cargar(5000);
            Colectivo colectivo = new Colectivo("K");

            Boleto b1 = colectivo.PagarCon(tarjeta);
            Boleto b2 = colectivo.PagarCon(tarjeta);
            Boleto b3 = colectivo.PagarCon(tarjeta);

            Assert.IsNotNull(b1);
            Assert.IsNotNull(b2);
            Assert.IsNotNull(b3);
            Assert.AreEqual(790, b1.Monto);
            Assert.AreEqual(790, b2.Monto);
            Assert.AreEqual(790, b3.Monto);
            Assert.AreEqual(2630, tarjeta.Saldo); // 5000 - 790*3
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
            Assert.AreEqual(2000, tarjeta.Saldo); // No se descuenta
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
        public void TestMedioBoletoConSaldoNegativo()
        {
            MedioBoleto tarjeta = new MedioBoleto();
            tarjeta.Cargar(2000);
            Colectivo colectivo = new Colectivo("K");

            colectivo.PagarCon(tarjeta); // 1210
            colectivo.PagarCon(tarjeta); // 420
            Boleto boleto = colectivo.PagarCon(tarjeta); // -370

            Assert.IsNotNull(boleto);
            Assert.AreEqual(-370, tarjeta.Saldo);
        }

        [Test]
        public void TestFranquiciaCompletaConSaldoCargado()
        {
            FranquiciaCompleta tarjeta = new FranquiciaCompleta();
            tarjeta.Cargar(5000);
            Colectivo colectivo = new Colectivo("K");

            Boleto boleto = colectivo.PagarCon(tarjeta);

            Assert.IsNotNull(boleto);
            Assert.AreEqual(5000, tarjeta.Saldo); // No descuenta
        }
    }
}