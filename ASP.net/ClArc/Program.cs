using System;
using Clarc.Commands.CreateUseCase;
using NrsCl;
using NrsCl.Commands;
using NrsCl.Environment;

namespace Clarc
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            var commands = new CLICommandsContainer
            {
                {new CLICommandConfig("UseCase"), new CreateUseCaseCommand()}
            };
            var context = new CLIContext(Environment.CurrentDirectory);
            using (var driver = new Driver(commands, context))
            {
                driver.Run();
            }
        }
    }
}