using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using ClArc.Lib;

namespace ClArc.Commands.CreateUseCase.Core.MakeUseCase.UseCaseRegisters {
    public abstract class UseCaseRegister
    {
        private const string Indent = "            ";
        private readonly UseCaseRegisterParameter param;

        protected UseCaseRegister(UseCaseRegisterParameter param) {
            this.param = param;
        }

        private string controllerName => param.ControllerName;
        private string actionName => param.ActionName;

        public void Run() {
            var file = Path.Combine(
                param.WebProjectDirectoryFullPath,
                "ClArc",
                "DI",
                OnLauncherFileName()
            );
            var lines = File.ReadAllLines(file);
            var (front, middle, after) = TextSplitter.DivideIntoThree(
                lines,
                line => line.TrimStart().StartsWith("#region ClArc START"),
                line => line.TrimStart().StartsWith("#endregion ClArc END"),
                false
            );

            var adjustedFront = AddUsing(front);
            IEnumerable<string> newMiddle;
            if (middle.Any(x => x.TrimStart().StartsWith($"#region {param.ControllerName}"))) {
                var usecaseLines = CreateAddedUsecaseLines(middle);
                newMiddle = usecaseLines;
            } else {
                var appendAfterMiddle = new List<string>
                {
                    "",
                    $"{Indent}#region {param.ControllerName}",
                    _registerUsecaseText(),
                    $"{Indent}#endregion"
                };
                newMiddle = middle.Concat(appendAfterMiddle);
            }
            var splitedMiddles = TextSplitter.Divide(newMiddle, x => x.Contains("#region"), false, true);
            var middles = splitedMiddles.OrderBy(x => x.First()).Select(x => x.Where(text => !string.IsNullOrWhiteSpace(text)));
            var newMids = TextJoiner.Join(middles, "");

            var result = adjustedFront.Concat(newMids).Concat(after);
            File.WriteAllLines(file, result, Encoding.UTF8);
        }

        protected abstract string OnLauncherFileName();
        protected abstract string OnAppendUsingNamespace(string controllerName);
        protected abstract string OnUsecaseClass(string completeName);

        private IEnumerable<string> AddUsing(IEnumerable<string> arg) {
            var (usings, rest) = TextSplitter.DivideInto2(
                arg,
                text => text == "",
                false
            );

            var implementsNamespaceUsing = $"using {OnAppendUsingNamespace(controllerName)};";
            var normalUsing = $"using UseCase.{controllerName}.{actionName};";

            var adjustedUsings = usings.ToList();

            if (adjustedUsings.All(x => !x.Contains(implementsNamespaceUsing))) {
                adjustedUsings.Add(implementsNamespaceUsing);
            }
            if (adjustedUsings.All(x => !x.Contains(normalUsing))) {
                adjustedUsings.Add(normalUsing);
            }

            adjustedUsings.Sort();

            return adjustedUsings.Concat(rest);
        }

        private IEnumerable<string> CreateAddedUsecaseLines(IEnumerable<string> source) {
            return RecursiveAddRegisterUseCase(source, new List<string>());
        }

        private List<string> RecursiveAddRegisterUseCase(IEnumerable<string> source, List<string> acc) {
            var (front, middle, rear) = TextSplitter.DivideIntoThree(
                source,
                line => line.TrimStart().StartsWith("#region"),
                line => line.TrimStart().StartsWith("#endregion")
            );

            IEnumerable<string> changedMiddle = middle;
            var middleHead = middle.FirstOrDefault();
            var isTarget = middleHead != null && middleHead.Contains(controllerName);
            if (isTarget) {
                changedMiddle = InvokeAddRegisterUseCase(middle);
            }
            acc.AddRange(front.Concat(changedMiddle));
            if (rear.Any()) {
                return RecursiveAddRegisterUseCase(rear, acc);
            } else {
                return acc;
            }
        }

        private IEnumerable<string> InvokeAddRegisterUseCase(IEnumerable<string> source) {
            var (front, middle, rear) = TextSplitter.DivideIntoThree(
                source,
                line => line.TrimStart().StartsWith("#region"),
                line => line.TrimStart().StartsWith("#endregion"),
                false
            );
            var prefix = controllerName + actionName;
            var reg = new Regex(@"<(?<request>.*),(?<usecaseInterface>.*),(?<usecaseImplements>.*)>");
            var hit = false;
            foreach (var m in middle) {
                var matched = reg.Match(m);
                if (matched.Success) {
                    var request = matched.Groups["request"].Value;
                    if (request.StartsWith(prefix)) {
                        hit = true;
                    }
                }
            }

            if (hit) {
                return source;
            } else {
                var addedMiddle = new List<string>(middle)
                {
                    _registerUsecaseText()
                };
                addedMiddle.Sort();
                return front
                    .Concat(addedMiddle)
                    .Concat(rear);
            }
        }

        private string _registerUsecaseText() {
            var prefix = controllerName + actionName;
            return $"{Indent}builder.RegisterUseCase<{prefix}Request, I{prefix}UseCase, {OnUsecaseClass(prefix)}>();";
        }
    }
}
