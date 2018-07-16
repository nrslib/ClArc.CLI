using System;
using System.IO;
using Clarc.Commands.CreateUseCase.Core;
using Newtonsoft.Json;
using NrsCl.Commands;
using NrsCl.Consoles;
using NrsCl.Environment;
using NrsCl.Prompts;

namespace Clarc.Commands.CreateUseCase
{
    public class CreateUseCaseCommand : ICLICommandWithInitialize
    {
        private readonly string settingFileName = $"{typeof(CreateUseCaseCommand).Name}.json";
        private CLIContext context;
        private CreateUseCaseSetting setting = new CreateUseCaseSetting();

        public void Dispose()
        {
        }

        public void Initialize(CLIContext context)
        {
            this.context = context;

            CLIConsole.WriteLine($"({GetType().Name})Loading setting file...");
            var filePath = Path.Combine(this.context.DataDirectoryFullPath, settingFileName);
            if (!File.Exists(filePath))
            {
                File.Create(filePath).Close();
                saveSetting(context);
            }
            setting = loadSetting(filePath);
        }

        public void Run()
        {
            CLIConsole.WriteSeparator();
            CLIConsole.WriteLine("Create UseCase");
            CLIConsole.WriteSeparator();

            if (!CheckSetting(context))
            {
                showAbort();
                return;
            }

            var controllerName = Prompt.Show("controller name:");
            var actionName = Prompt.Show("action name:");

            CLIConsole.WriteSeparator();
            CLIConsole.WriteLine("The following UseCase will be created:");
            CLIConsole.WriteSeparator();
            CLIConsole.WriteLine("directory:");
            CLIConsole.WriteLine("- " + setting.WebProjectDirectoryPath);
            CLIConsole.WriteLine("controller name:");
            CLIConsole.WriteLine("- " + controllerName);
            CLIConsole.WriteLine("action name:");
            CLIConsole.WriteLine("- " + actionName);
            CLIConsole.WriteSeparator();

            var yesno = Prompt.ShowYesNoPrompt("Look okay?");
            if (yesno == YesNoPrompt.Result.No)
            {
                showAbort();
                return;
            }

            var rootFullPath = new Uri(new Uri(setting.WebProjectDirectoryPath), ".").AbsolutePath;
            var param = new CreateUseCaseTaskParameter(rootFullPath, setting.WebProjectDirectoryPath, controllerName, actionName);
            var task = new CreateUseCaseTask();
            task.Run(param);

            CLIConsole.WriteLine();
            CLIConsole.WriteLine("CreateUseCase done.");
            CLIConsole.WriteLine();
        }

        private void saveSetting(CLIContext context)
        {
            var filePath = Path.Combine(context.DataDirectoryFullPath, settingFileName);
            if (!File.Exists(filePath))
            {
                throw new Exception($"file not found({filePath}).");
            }
            var json = JsonConvert.SerializeObject(setting);
            File.WriteAllText(filePath, json);
        }

        private CreateUseCaseSetting loadSetting(string filePath)
        {
            var text = File.ReadAllText(filePath);
            return JsonConvert.DeserializeObject<CreateUseCaseSetting>(text);
        }

        private void showAbort()
        {
            CLIConsole.WriteLine("aborted", ConsoleColor.Red);
        }

        private bool CheckSetting(CLIContext context)
        {
            if (setting.IsSetuped)
            {
                if (ValidSolution(setting.SolutionFullPath) && ValidProject(setting.WebProjectDirectoryPath))
                {
                    CLIConsole.WriteLine("Current target solution is " + setting.SolutionFullPath);
                    var result = Prompt.ShowYesNoPrompt("Would you want to change project directory?", YesNoPrompt.Result.No);
                    switch (result)
                    {
                        case YesNoPrompt.Result.Yes:
                            setting.Clear();
                            break;
                        case YesNoPrompt.Result.No:
                            return true;
                        default:
                            return false;
                    }
                }
                else
                {
                    setting.Clear();
                }
            }

            if (!setting.IsSetuped)
            {
                while (!setting.IsSetupSolution)
                {
                    var (isQuit, input) = Prompt.ShowQuitable(@"Type your solution by full path.");
                    if (isQuit) {
                        return false;
                    }

                    if (ValidSolution(input)) {
                        CLIConsole.WriteLine("Solution file accepted");
                        setting.SaveSolutionFullPath(input);
                        saveSetting(context);
                    } else {
                        CLIConsole.WriteLine("The file is not solution file.");
                    }
                }

                while (!setting.IsSetupWebProject)
                {
                    var (isQuit, input) = Prompt.ShowQuitable(@"Type your web project directory by full path.");
                    if (isQuit) {
                        return false;
                    }

                    if (ValidProject(input)) {
                        CLIConsole.WriteLine("Project direcotry accepted");
                        setting.SaveWebProjectFullPath(input);
                        saveSetting(context);
                    } else {
                        CLIConsole.WriteLine("The file is not web project directory.");
                    }
                }
            }
            return true;
        }

        private bool ValidSolution(string fullPath)
        {
            if (!File.Exists(fullPath))
            {
                return false;
            }

            if (Path.GetExtension(fullPath) != ".sln")
            {
                return false;
            }

            return true;
        }

        private bool ValidProject(string fullPath)
        {
            if (!Directory.Exists(fullPath))
            {
                return false;
            }

            return true;
        }
    }
}
