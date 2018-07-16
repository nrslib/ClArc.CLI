namespace ClArc.Commands.CreateUseCase.Core.MakeUseCase.UseCaseRegisters {
    public class ProductUseCaseRegsiter : UseCaseRegister {
        public ProductUseCaseRegsiter(UseCaseRegisterParameter param) : base(param)
        {
        }

        protected override string OnLauncherFileName()
        {
            return "ProductDiLauncher.cs";
        }

        protected override string OnAppendUsingNamespace(string controllerName)
        {
            return $"Domain.Application.{controllerName}";
        }

        protected override string OnUsecaseClass(string completeName)
        {
            return completeName + "Interactor";
        }
    }
}
