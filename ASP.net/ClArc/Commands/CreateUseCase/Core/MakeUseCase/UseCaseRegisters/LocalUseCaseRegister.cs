namespace ClArc.Commands.CreateUseCase.Core.MakeUseCase.UseCaseRegisters {
    public class LocalUseCaseRegister : UseCaseRegister {
        public LocalUseCaseRegister(UseCaseRegisterParameter param) : base(param) {
        }

        protected override string OnLauncherFileName() {
            return "LocalDiLauncher.cs";
        }

        protected override string OnAppendUsingNamespace(string controllerName) {
            return $"Domain.Application.{controllerName}";
        }

        protected override string OnUsecaseClass(string completeName) {
            return completeName + "Interactor";
        }
    }
}
