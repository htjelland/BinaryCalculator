using BinaryCalculator.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity;

namespace BinaryCalculator
{
    public static class UnityRegistration
    {
        public static IUnityContainer RegisterInstances(this UnityContainer container)
        {
            container.RegisterType<ICalculator, Calculator>();
            return container;
        }
    }
}
