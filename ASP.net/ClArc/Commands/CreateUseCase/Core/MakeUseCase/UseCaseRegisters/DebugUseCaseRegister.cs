namespace ClArc.Commands.CreateUseCase.Core.MakeUseCase.UseCaseRegisters {
    public class DebugUseCaseRegister : UseCaseRegister {
        public DebugUseCaseRegister(UseCaseRegisterParameter param) : base(param)
        {
        }

        protected override string OnLauncherFileName()
        {
            return "DebugDiLauncher.cs";
        }

        protected override string OnAppendUsingNamespace(string controllerName)
        {
            return $"MockUseCase.{controllerName}";
        }

        protected override string OnUsecaseClass(string completeName)
        {
            return "Mock" + completeName + "Interactor";
        }
    }
}
