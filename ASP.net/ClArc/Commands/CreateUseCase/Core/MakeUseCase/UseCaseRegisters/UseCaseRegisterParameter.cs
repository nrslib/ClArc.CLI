namespace ClArc.Commands.CreateUseCase.Core.MakeUseCase.UseCaseRegisters {
    public class UseCaseRegisterParameter {
        public UseCaseRegisterParameter(string webProjectDirectoryFullPath, string controllerName, string actionName)
        {
            WebProjectDirectoryFullPath = webProjectDirectoryFullPath;
            ControllerName = controllerName;
            ActionName = actionName;
        }

        public string WebProjectDirectoryFullPath { get; }
        public string ControllerName { get; }
        public string ActionName { get; }
    }
}
