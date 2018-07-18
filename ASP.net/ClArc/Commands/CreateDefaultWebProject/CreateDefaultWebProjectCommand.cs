using System;
using System.IO;
using Ionic.Zip;
using NrsCl.Commands;
using NrsCl.Consoles;
using NrsCl.Prompts;

namespace ClArc.Commands.CreateDefaultWebProject {
    public class CreateDefaultWebProjectCommand : ICLICommand {
        public void Dispose()
        {
        }

        public void Run()
        {
            CLIConsole.WriteSeparator();
            CLIConsole.WriteLine("Create default web project");
            CLIConsole.WriteSeparator();


            string directory;
            while (true)
            {
                var  (isQuit, input) = Prompt.ShowQuitable("directory:");
                if (isQuit)
                {
                    CLIConsole.WriteLine("aborted", ConsoleColor.Red);
                    return;
                }

                if (Directory.Exists(input))
                {
                    directory = input;
                    break;
                }

                CLIConsole.WriteLine("directory not found.");
            }

            CLIConsole.WriteSeparator();
            CLIConsole.WriteLine("The following UseCase will be created:");
            CLIConsole.WriteSeparator();
            CLIConsole.WriteLine("directory:");
            CLIConsole.WriteLine("- " + directory);

            var yesno = Prompt.ShowYesNoPrompt("Look okay?");
            if (yesno == YesNoPrompt.Result.Yes)
            {
                createWebProject(directory);
                CLIConsole.WriteLine("done.");
            }
            else
            {
                CLIConsole.WriteLine("end.");
            }
            CLIConsole.WriteLine();
        }

        private void createWebProject(string directory)
        {
            var zipFileFullPath = Path.Combine(Environment.CurrentDirectory, "Templates", "Application.zip");
            
            using (var zip = new ZipFile(zipFileFullPath)) {
                zip.ExtractAll(directory);
            }
        }
    }
}
