using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BinaryCalculator.Models
{
    public interface ICalculator
    {
        int Calculate(int first, int last, string operation);
    }
}
