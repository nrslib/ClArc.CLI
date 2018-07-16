using System;

namespace Clarc.Commands.CreateUseCase
{
    [Serializable]
    public class CreateUseCaseSetting
    {
        public string SolutionFullPath { get; set; }
        public string WebProjectDirectoryPath { get; set; }
        public bool IsSetuped => IsSetupSolution && IsSetupWebProject;
        public bool IsSetupSolution => SolutionFullPath != null;
        public bool IsSetupWebProject => WebProjectDirectoryPath != null;

        public void Clear()
        {
            SolutionFullPath = null;
            WebProjectDirectoryPath = null;
        }

        public void SaveSolutionFullPath(string path)
        {
            SolutionFullPath = path;
        }

        public void SaveWebProjectFullPath(string path)
        {
            WebProjectDirectoryPath = path;
        }
    }
}
