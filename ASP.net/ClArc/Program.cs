using System;
using Clarc.Commands.CreateUseCase;
using ClArc.Commands.CreateDefaultWebProject;
using NrsCl;
using NrsCl.Commands;
using NrsCl.Consoles;
using NrsCl.Environment;

namespace Clarc
{
    class Program
    {
        static void Main(string[] args)
        {
            CLIConsole.WriteLine("Welcome to ClArc");
            CLIConsole.WriteLine();
            var commands = new CLICommandsContainer
            {
                { new CLICommandConfig("GenerateDefaultWebProject"), new CreateDefaultWebProjectCommand() },
                { new CLICommandConfig("UseCase"), new CreateUseCaseCommand() },
            };
            var context = new CLIContext(Environment.CurrentDirectory);
            using (var driver = new Driver(commands, context))
            {
                driver.Run();
            }
        }
    }
}