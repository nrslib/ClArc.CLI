using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Clarc.Commands.CreateUseCase.Core.MakeUseCase;
using ClArc.Commands.CreateUseCase.Core.MakeUseCase;
using ClArc.Commands.CreateUseCase.Core.MakeUseCase.UseCaseRegisters;
using Ionic.Zip;

namespace Clarc.Commands.CreateUseCase.Core
{
    public class CreateUseCaseTask
    {
        public void Run(CreateUseCaseTaskParameter taskParam)
        {
            var projectDirectoryFullPath = taskParam.ProjectDirectoryFullPath;

            var usecaseParam = new MakeUseCaseParameter(taskParam.ControllerName, taskParam.ActionName);

            CreateUsecaseTo("UseCase", projectDirectoryFullPath, usecaseParam, new UseCaseMaker());
            CreateUsecaseTo("MockUseCase", projectDirectoryFullPath, usecaseParam, new MockUseCaseMaker());
            CreateUsecaseTo("Domain", projectDirectoryFullPath, usecaseParam, new DomainUseCaseMaker());

            var registerParameter = new UseCaseRegisterParameter(taskParam.WebProjectFullPath, taskParam.ControllerName, taskParam.ActionName);
            RegisterUseCase(registerParameter);

            CreateJsonResponseFile(taskParam);
        }
        
        private void CreateUsecaseTo(string projectName, string projectDirectoryFullPath, MakeUseCaseParameter param, params IMaker[] makers)
        {
            var classes = makers.SelectMany(x => x.Make(param)).ToArray();
            var usecaseFullPath = Path.Combine(projectDirectoryFullPath, projectName);
            prepareTemplates(projectDirectoryFullPath, projectName);
            prepareDirectory(usecaseFullPath, classes);
            createClassFile(usecaseFullPath, classes);
        }

        private void prepareTemplates(string rootFullPath, string projectName)
        {
            var outputDirectoryFullPath = Path.Combine(rootFullPath, projectName);
            if (Directory.Exists(outputDirectoryFullPath))
            {
                return;
            }

            var zipFileFullPath = Path.Combine(Environment.CurrentDirectory, "Templates", projectName + ".zip");

            using (var zip = new ZipFile(zipFileFullPath))
            {
                zip.ExtractAll(rootFullPath);
            }
        }

        private void prepareDirectory(string projectDirectoryFullPath, CreateClassData[] data)
        {
            var directories = data.Select(x => Path.Combine(projectDirectoryFullPath, x.RelatedPath));
            foreach (var path in directories.Where(x => !Directory.Exists(x)))
            {
                Directory.CreateDirectory(path);
            }
        }

        private void createClassFile(string projectDirectoryFullPath, CreateClassData[] args)
        {
            foreach (var data in args)
            {
                var filepath = Path.Combine(projectDirectoryFullPath, data.RelatedPath, data.FileName);
                File.Create(filepath).Close();
                File.WriteAllText(filepath, data.Content);
            }
        }

        private void RegisterUseCase(UseCaseRegisterParameter param)
        {
            invokePrepareClArc(param.WebProjectDirectoryFullPath);

            var registers = new List<UseCaseRegister>
            {
                new DebugUseCaseRegister(param),
                new LocalUseCaseRegister(param),
                new ProductUseCaseRegsiter(param)
            };

            registers.ForEach(x => x.Run());
        }

        private void invokePrepareClArc(string webProjectFullPath)
        {
            var outputDirectoryFullPath = Path.Combine(webProjectFullPath, "ClArc");
            if (Directory.Exists(outputDirectoryFullPath)) {
                return;
            }
            var zipFileFullPath = Path.Combine(Environment.CurrentDirectory, "Templates", "ClArc.zip");

            using (var zip = new ZipFile(zipFileFullPath)) {
                zip.ExtractAll(webProjectFullPath);
            }
        }

        private void CreateJsonResponseFile(CreateUseCaseTaskParameter taskParam)
        {
            var outputDirectoryFullPath = Path.Combine(taskParam.WebProjectFullPath, "Debug", "JsonResponses");
            var fileName = taskParam.ControllerName + taskParam.ActionName + ".jsons";
            var fileFullPath = Path.Combine(outputDirectoryFullPath, fileName);
            if (!Directory.Exists(outputDirectoryFullPath))
            {
                Directory.CreateDirectory(outputDirectoryFullPath);
            }
            File.Create(fileFullPath).Close();
        }
    }
}
