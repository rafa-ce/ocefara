using Ocefara.LeilaoOnline.Core;
using System.Linq;
using Xunit;

namespace Ocefara.LeilaoOnline.Tests
{
    public class LeilaoRecebeLance
    {
        [Fact]
        public void NaoPermiteNovosLancesDadoLeilaoFinalizado()
        {
            // Arrange - cenário
            var leilao = new Leilao("Van Gogh");
            var fulano = new Interessada("Fulano", leilao);

            leilao.RecebeLance(fulano, 800);
            leilao.RecebeLance(fulano, 900);

            leilao.TerminaPregao();
            
            // Act - método sob teste
            leilao.RecebeLance(fulano, 1000);

            // Assert
            var valorEsperado = 2;
            var valorObtido = leilao.Lances.Count();

            Assert.Equal(valorEsperado, valorObtido);
        }
    }
}
