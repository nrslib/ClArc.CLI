using System.Collections.Generic;
using Clarc.Commands.CreateUseCase.Core.MakeUseCase;
using ClassFileGenerator;
using ClassFileGenerator.Core.Meta;
using ClassFileGenerator.Core.Meta.Words;
using ClassFileGenerator.Core.Templates;

namespace ClArc.Commands.CreateUseCase.Core.MakeUseCase {
    public class MockUseCaseMaker : IMaker {
        private readonly MainDriver classFileGenerateDriver = new MainDriver();

        public List<CreateClassData> Make(MakeUseCaseParameter param)
        {
            return new List<CreateClassData>
            {
                CreateInteractor(param)
            };
        }

        private CreateClassData CreateInteractor(MakeUseCaseParameter param)
        {
            var className = $"Mock{param.CompleteName}Interactor";
            var classNamespace = $"MockUseCase.{param.ControllerName}";

            var meta = new ClassMeta(classNamespace, className);
            meta.SetupUsing()
                .AddUsing("MockUseCase.Lib.JsonResponse")
                .AddUsing("UseCase." + param.ControllerName + "." + param.ActionName);

            meta.SetupImplements()
                .AddImplements("I" + param.CompleteName + "UseCase");

            meta.SetupFields()
                .AddField("responseGenerator",
                    field => field.SetReadOnly(true).SetType("JsonResponseGenerator"));

            meta.SetupConstructor()
                .AddConstructor(constructor => constructor
                    .SetAccessLevel(AccessLevel.Public)
                    .AddArgument("responseGenerator", "JsonResponseGenerator")
                    .AddBody("this.responseGenerator = responseGenerator;"));

            var responseClassName = param.CompleteName + "Response";
            meta.SetupMethods()
                .AddMethod("Handle", method => method
                    .SetReturnType(responseClassName)
                    .AddArgument("request", param.CompleteName + "Request")
                    .SetAccessLevel(AccessLevel.Public)
                    .AddBody($"return responseGenerator.Generate<{responseClassName}>();")
                );

            var content = classFileGenerateDriver.Create(meta, Language.CSharp);

            return new CreateClassData(
                param.ControllerName,
                className + ".cs",
                content
            );
        }
    }
}
