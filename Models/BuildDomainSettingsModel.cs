using System.IO;
using System.Linq;
using CQRSAndMediator.Scaffolding.Enums;
using CQRSAndMediator.Scaffolding.Infrastructure;
using Microsoft.Build.Construction;

namespace CQRSAndMediator.Scaffolding.Models
{
    public static class BuildDomainSettingsModel
    {
        public static DomainSettingsModel Build(string concern,string operation,PatternDirectoryType patternType)
        {
            var result = new DomainSettingsModel();

            var solutionFile = Directory.GetFiles(@"C:\Sources\MyGithub\CQRSAndMediator-Microservice", "*.sln").FirstOrDefault();

            var solutionInfo = SolutionFile.Parse(solutionFile);
            var projectList = solutionInfo.ProjectsInOrder;

            foreach (var proj in projectList)
            {
                if (!proj.ProjectName.Contains(".Logic")
                    && !proj.ProjectName.Contains(".Domain"))
                    continue;

                result
                    .SetConcern(concern)
                    .SetOperation(operation)
                    .SetPatternType(patternType)

                    .SetProjectName(proj.RelativePath)
                    .SetDomainName(proj.ProjectName)
                    .SetDomainAbsolutePath(proj.AbsolutePath)
                    .UpdateClassName()
                    ;
            }

            Log.Info($"ProjectName: {result.ProjectName}");
            Log.Info($"DomainName: {result.DomainName}");
            Log.Info(result.DomainAbsolutePath);


            return result;
        }
    }
}
