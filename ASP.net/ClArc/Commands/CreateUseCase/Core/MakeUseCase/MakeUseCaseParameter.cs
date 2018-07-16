namespace Clarc.Commands.CreateUseCase.Core.MakeUseCase
{
    public class MakeUseCaseParameter
    {
        public MakeUseCaseParameter(string controllerName, string actionName)
        {
            ControllerName = controllerName;
            ActionName = actionName;
            CompleteName = ControllerName + ActionName;
        }

        public string ControllerName { get; }
        public string ActionName { get; }
        public string CompleteName { get; }
    }
}
