using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BinaryCalculator.Models
{
    public class Calculator : ICalculator
    {
        private Func<int, int, int> GetFunction(string operation)
        {
            if(string.IsNullOrEmpty(operation)) { operation = "+"; }

            switch(operation)
            {
                case "+":
                    return (a, b) => a + b;

                case "-":
                    return (a, b) => a - b;

                default:
                    throw new ArgumentException("Operation not valid");
            }
        }

        //public double Calculate(string expression)
        //{
        //    var dataTable = new DataTable();
        //    var result = dataTable.Compute(expression, string.Empty);
        //    return Convert.ToDouble(result);
        //}

        public int Calculate(int first, int last, string operation)
        {
            var function = GetFunction(operation);
            var result = function(first, last);
            return function(first, last);
        }
    }
}
