using System;
using System.Collections.Generic;
using System.IO;
using Clarc.Commands.CreateUseCase.Core.MakeUseCase;
using ClassFileGenerator;
using ClassFileGenerator.Core.Meta;
using ClassFileGenerator.Core.Meta.Words;
using ClassFileGenerator.Core.Templates;

namespace ClArc.Commands.CreateUseCase.Core.MakeUseCase {
    public class DomainUseCaseMaker : IMaker{
        private readonly MainDriver classFileGenerateDriver = new MainDriver();

        public List<CreateClassData> Make(MakeUseCaseParameter param)
        {
            return new List<CreateClassData>
            {
                CreateUseCase(param)
            };
        }

        private CreateClassData CreateUseCase(MakeUseCaseParameter param)
        {
            var className = $"{param.CompleteName}Interactor";
            var classNameSpace = $"Domain.Application.{param.ControllerName}";

            var meta = new ClassMeta(classNameSpace, className);
            meta.SetupUsing()
                .AddUsing("System")
                .AddUsing("UseCase." + param.ControllerName + "." + param.ActionName);

            meta.SetupImplements()
                .AddImplements("I" + param.CompleteName + "UseCase");

            meta.SetupMethods()
                .AddMethod("Handle", method => method
                    .SetReturnType(param.CompleteName + "Response")
                    .AddArgument("request", param.CompleteName + "Request")
                    .SetAccessLevel(AccessLevel.Public)
                );

            var content = classFileGenerateDriver.Create(meta, Language.CSharp);

            return new CreateClassData(
                Path.Combine("Application", param.ActionName),
                className + ".cs",
                content
            );
        }
    }
}
