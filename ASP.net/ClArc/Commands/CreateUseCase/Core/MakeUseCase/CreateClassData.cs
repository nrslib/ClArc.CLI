namespace Clarc.Commands.CreateUseCase.Core.MakeUseCase
{
    public class CreateClassData
    {
        public CreateClassData(string relatedPath, string fileName, string content)
        {
            RelatedPath = relatedPath;
            FileName = fileName;
            Content = content;
        }

        public string RelatedPath { get; }
        public string FileName { get; }
        public string Content { get; }
    }
}
