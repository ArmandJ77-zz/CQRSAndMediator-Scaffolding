using CQRSAndMediator.Scaffolding.Enums;
using CQRSAndMediator.Scaffolding.Resolver;
using Microsoft.Build.Construction;
using System;
using System.IO;
using System.Linq;

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
        public GroupByType GroupingStrategy { get; set; }

        public DomainSettingsModel(string concern, string operation, PatternDirectoryType patternType, GroupByType groupBy)
        {
            var solutionFile = Directory.GetFiles(Directory.GetCurrentDirectory(), "*.sln").FirstOrDefault();
            var solutionInfo = SolutionFile.Parse(solutionFile);

            var projectList = solutionInfo.ProjectsInOrder;

            var proj =
                projectList.FirstOrDefault(x => x.ProjectName.Equals("Logic") || x.ProjectName.Equals("Domain"));

            if (proj == null)
                throw new Exception("Missing domain or logic project in solution");


            Concern = concern;
            Operation = operation;
            PatternType = patternType;

            ProjectName = proj.RelativePath;
            DomainName = proj.ProjectName;
            DomainAbsolutePath = ResolveDomainAbsolutePath(proj.AbsolutePath);
            PatternFileType = PatternFileNameResolver.Resolve(patternType);
            ClassName = $"{concern}{operation}{PatternFileNameResolver.Resolve(patternType)}";
            GroupingStrategy = groupBy;

            //LogUtility.Info($"ProjectName: {ProjectName}");
            //LogUtility.Info($"DomainName: {DomainName}");
            //LogUtility.Info($"DomainAbsolutePath: {DomainAbsolutePath}");
        }

        private static string ResolveDomainAbsolutePath(string absolutePath)
        {
            var lastIndexOf = absolutePath.LastIndexOf("\\", StringComparison.Ordinal);
            var domainAbsolutePath = absolutePath.Substring(0, lastIndexOf);
            return domainAbsolutePath;
        }
    }
}
