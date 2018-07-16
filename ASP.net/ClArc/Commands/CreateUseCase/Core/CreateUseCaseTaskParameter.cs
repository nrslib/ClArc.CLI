namespace Clarc.Commands.CreateUseCase.Core
{
    public class CreateUseCaseTaskParameter
    {
        public CreateUseCaseTaskParameter(
            string projectDirectoryFullPath,
            string webProjectFullPath,
            string controllerName,
            string actionName
        )
        {
            ProjectDirectoryFullPath = projectDirectoryFullPath;
            WebProjectFullPath = webProjectFullPath;
            ControllerName = controllerName;
            ActionName = actionName;
        }

        public string ProjectDirectoryFullPath { get; }
        public string WebProjectFullPath { get; }
        public string ControllerName { get; }
        public string ActionName { get; }
    }
}
