using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ocefara.LeilaoOnline.Core
{
    public class MaiorValor : IModalidadeAvaliacao
    {
        public Lance Avalia(Leilao leilao)
        {
            return leilao.Lances
                   .DefaultIfEmpty(new Lance(null, 0))
                   .OrderBy(l => l.Valor).LastOrDefault();
        }
    }
}
