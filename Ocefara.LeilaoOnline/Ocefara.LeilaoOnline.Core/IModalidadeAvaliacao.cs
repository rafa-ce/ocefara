using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ocefara.LeilaoOnline.Core
{
    public interface IModalidadeAvaliacao
    {
        Lance Avalia(Leilao leilao);
    }
}
