using Ocefara.LeilaoOnline.Core;
using Xunit;

namespace Ocefara.LeilaoOnline.Tests
{
    public class LeilaoTestes
    {
        [Theory]
        [InlineData(1200, new double[] { 800, 900, 1000, 1200})]
        [InlineData(1000, new double[] { 800, 900, 1000, 990 })]
        [InlineData(800, new double[] { 800 })]
        private void LeilaoComLances(double valorEsperado, double[] lances)
        { 
            // Arrange - cenário
            var leilao = new Leilao("Van Gogh");
            var fulano = new Interessada("Fulano", leilao);
            
            foreach(var lance in lances)
                leilao.RecebeLance(fulano, lance);
            
            // Act - método sob teste
            leilao.TerminaPregao();

            // Assert
            var valorObtido = leilao.Ganhador.Valor;

            Assert.Equal(valorEsperado, valorObtido);
        }

        [Fact]
        private void LeilaoSemLances()
        {
            // Arrange - cenário
            var leilao = new Leilao("Van Gogh");

            // Act - método sob teste
            leilao.TerminaPregao();

            // Assert
            var valorEsperado = 0;
            var valorObtido = leilao.Ganhador.Valor;

            Assert.Equal(valorEsperado, valorObtido);
        }
    }
}
