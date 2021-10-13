using System;
using System.Collections.Generic;
using System.Text;

namespace AzurePipelineParser.Models
{
    public enum PipelineNodeTypes
    {
        File,
        Pipeline,
        Stage,
        Job,
        Step,
        Template,
        Variable,
        Parameter,
        Resources,
        PipelineResource,
        ContainerResource,
        MountReadyOnly,
        RepositoryResource,
        PackageResource,
        Trigger,
        IncludeExclude,
        Pool,
        Environment,
        ContainerReference,
        Workspace,
        Uses,
        Condition
    }
}
