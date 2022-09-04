using Ocefara.LeilaoOnline.Core;
using System;
using Xunit;

namespace Ocefara.LeilaoOnline.Tests
{
    public class LanceCtor
    {
        [Fact]
        public void RetornaArgumentExceptionDadoValorNegativo()
        {
            // Arranje
            var valorNegativo = -100;

            // Assert
            var excecaoObtida = Assert.Throws<ArgumentException>(
                // Act
                () => new Lance(null, valorNegativo)
            );

            var mensagemEsperada = "Valor do lance não pode ser menor que zero";
            Assert.Equal(mensagemEsperada, excecaoObtida.Message);
        }
    }
}
