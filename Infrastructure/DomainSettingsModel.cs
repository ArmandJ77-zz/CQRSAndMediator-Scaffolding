using CQRSAndMediator.Scaffolding.Enums;
using CQRSAndMediator.Scaffolding.Resolver;
using System;

namespace CQRSAndMediator.Scaffolding.Infrastructure
{
    public sealed class DomainSettingsModel
    {
        public string Concern { get; set; }
        public string Operation { get; set; }
        public PatternDirectoryType PatternType { get; set; }
        public PatternFileType PatternFileType { get; set; }
        public string ClassName { get; set; }
        public string ProjectName { get; set; }
        public string DomainName { get; private set; }
        public string DomainAbsolutePath { get; private set; }

        public DomainSettingsModel UpdateClassName()
        {
            this.PatternFileType = PatternFileNameResolver.Resolve(this.PatternType);
            this.ClassName = $"{this.Concern}{this.Operation}{this.PatternFileType}";
            return this;
        }

        public DomainSettingsModel SetConcern(string concern)
        {
            this.Concern = concern;
            return this;
        }
        public DomainSettingsModel SetOperation(string operation)
        {
            this.Operation = operation;
            return this;
        }

        public DomainSettingsModel SetPatternType(PatternDirectoryType type)
        {
            this.PatternType = type;
            return this;
        }

        public DomainSettingsModel SetProjectName(string name)
        {
            this.ProjectName = name;
            return this;
        }

        public DomainSettingsModel SetDomainName(string name)
        {
            this.DomainName = name;
            return this;
        }

        public DomainSettingsModel SetDomainAbsolutePath(string absolutePath)
        {
            var lastIndexOf = absolutePath.LastIndexOf("\\", StringComparison.Ordinal);
            var domainAbsolutePath = absolutePath.Substring(0, lastIndexOf);
            this.DomainAbsolutePath = domainAbsolutePath;
            return this;
        }
    }
}
