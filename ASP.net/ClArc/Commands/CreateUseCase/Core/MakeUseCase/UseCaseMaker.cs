using System.Collections.Generic;
using System.IO;
using ClassFileGenerator;
using ClassFileGenerator.Core.Meta;
using ClassFileGenerator.Core.Templates;

namespace Clarc.Commands.CreateUseCase.Core.MakeUseCase
{
    public class UseCaseMaker : IMaker
    {
        private const string usingNameSpace = "UseCase.Core";
        private readonly MainDriver classFileGenerateDriver = new MainDriver();

        public List<CreateClassData> Make(MakeUseCaseParameter param)
        {
            var data = new List<CreateClassData>
            {
                CreateUseCase(param),
                CreateRquest(param),
                CreateResponse(param),
            };

            return data;
        }

        private CreateClassData CreateUseCase(MakeUseCaseParameter param)
        {
            var interfaceName = "I" + param.CompleteName + "UseCase";
            var classNameSpace = nameSpace(param);

            var meta = new InterfaceMeta(classNameSpace, interfaceName);
            meta.SetupUsing().AddUsing(usingNameSpace);
            meta.SetupImplements()
                .AddImplements("IUseCase", x => x.AddGeneric(requestName(param), responseName(param)));

            var content = classFileGenerateDriver.Create(meta, Language.CSharp);

            return new CreateClassData(
                Path.Combine(param.ControllerName, param.ActionName),
                interfaceName + ".cs",
                content
            );
        }

        private CreateClassData CreateRquest(MakeUseCaseParameter param)
        {
            var className = requestName(param);
            var classNameSpace = nameSpace(param);

            var meta = new ClassMeta(classNameSpace, className);
            meta.SetupUsing().AddUsing(usingNameSpace);
            meta.SetupImplements()
                .AddImplements("IRequest", x => x.AddGeneric(responseName(param)));

            var content = classFileGenerateDriver.Create(meta, Language.CSharp);

            return new CreateClassData(
                Path.Combine(param.ControllerName, param.ActionName),
                className + ".cs",
                content
            );
        }

        private CreateClassData CreateResponse(MakeUseCaseParameter param) {
            var className = responseName(param);
            var classNameSpace = nameSpace(param);

            var meta = new ClassMeta(classNameSpace, className);
            meta.SetupUsing().AddUsing(usingNameSpace);
            meta.SetupImplements().AddImplements("IResponse");

            var content = classFileGenerateDriver.Create(meta, Language.CSharp);

            return new CreateClassData(
                Path.Combine(param.ControllerName, param.ActionName),
                className + ".cs",
                content
            );
        }

        private string nameSpace(MakeUseCaseParameter param) => $"UseCase.{param.ControllerName}.{param.ActionName}";
        private string requestName(MakeUseCaseParameter param) => $"{param.CompleteName}Request";
        private string responseName(MakeUseCaseParameter param) => $"{param.CompleteName}Response";
    }
}
