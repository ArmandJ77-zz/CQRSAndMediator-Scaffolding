using System;
using System.IO;
using System.Linq;
using CQRSAndMediator.Scaffolding.Enums;
using CQRSAndMediator.Scaffolding.Infrastructure;
using CQRSAndMediator.Scaffolding.Resolver;
using Microsoft.Build.Construction;

namespace CQRSAndMediator.Scaffolding.Models
{
    public sealed class DomainSettingsModel
    {
        public string Concern { get; }
        public string Operation { get; }
        public PatternDirectoryType PatternType { get; }
        public PatternFileType PatternFileType { get; }
        public string ClassName { get; }
        public string ProjectName { get; }
        public string DomainName { get; }
        public string DomainAbsolutePath { get; }
        public DomainSettingsModel(string concern, string operation, PatternDirectoryType patternType)
        {
            var solutionFile = Directory.GetFiles(@"C:\Sources\MyGithub\CQRSAndMediator-Microservice", "*.sln").FirstOrDefault();

            var solutionInfo = SolutionFile.Parse(solutionFile);

            var projectList = solutionInfo.ProjectsInOrder;

            foreach (var proj in projectList)
            {
                if (!proj.ProjectName.Contains(".Logic")
                    && !proj.ProjectName.Contains(".Domain"))
                    continue;

                Concern = concern;
                Operation = operation;
                PatternType = patternType;

                ProjectName = proj.RelativePath;
                DomainName = proj.ProjectName;
                DomainAbsolutePath = ResolveDomainAbsolutePath(proj.AbsolutePath);
                PatternFileType = PatternFileNameResolver.Resolve(patternType);
                ClassName = $"{concern}{operation}{PatternFileNameResolver.Resolve(patternType)}";
            }

            Log.Info($"ProjectName: {ProjectName}");
            Log.Info($"DomainName: {DomainName}");
            Log.Info(DomainAbsolutePath);
        }
        private static string ResolveDomainAbsolutePath(string absolutePath)
        {
            var lastIndexOf = absolutePath.LastIndexOf("\\", StringComparison.Ordinal);
            var domainAbsolutePath = absolutePath.Substring(0, lastIndexOf);
            return domainAbsolutePath;
        }
    }
}
