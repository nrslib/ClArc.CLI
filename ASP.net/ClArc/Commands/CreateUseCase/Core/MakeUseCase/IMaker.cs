using System.Collections.Generic;

namespace Clarc.Commands.CreateUseCase.Core.MakeUseCase
{
    public interface IMaker
    {
        List<CreateClassData> Make(MakeUseCaseParameter param);
    }
}
