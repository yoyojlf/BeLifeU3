using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NegocioBL
{
    public abstract class Tarificador
    {
        public abstract double CalcularPrimaBase(double Base);
    }
}
