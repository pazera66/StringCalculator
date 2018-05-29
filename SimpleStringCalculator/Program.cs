using System;
using System.Globalization;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using SimpleStringCalculator.Interfaces;
using SimpleStringCalculator.Implementation;

namespace SimpleStringCalculator
{
    class Program
    {
        static void Main(string[] args)
        {
            var serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);
            var serviceProvider = serviceCollection.BuildServiceProvider();

            var calculator1 = serviceProvider.GetRequiredService<IStringCalculator1>();
            var calculator2 = serviceProvider.GetRequiredService<IStringCalculator2>();

            string resultFromCalc1 = calculator1.PerformCalculations(args[0]).ToString(CultureInfo.InvariantCulture);
            string resultFromCalc2;
            try
            {
                resultFromCalc2 = calculator2.PerformCalculations(args[0]).ToString(CultureInfo.InvariantCulture);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

            StringBuilder builder = new StringBuilder();
            builder.Append("Result calculated by Calculator1 is: ");
            builder.Append(resultFromCalc1);
            builder.AppendLine();
            builder.Append("Result calculated by Calculator2 is: ");
            builder.Append(resultFromCalc2);
            Console.WriteLine(builder.ToString());
        }

        private static void ConfigureServices(ServiceCollection services)
        {
            services.AddSingleton<IHelpers, Helpers>();
            services.AddSingleton<IStringCalculator1, StringCalculator1>();
            services.AddSingleton<IStringCalculator2, StringCalculator2>();
        }
    }
}
