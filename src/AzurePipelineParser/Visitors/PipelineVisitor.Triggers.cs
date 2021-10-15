using AzurePipelineParser.Models;
using System;
using System.Collections.Generic;
using System.Text;
using YamlDotNet.RepresentationModel;

namespace AzurePipelineParser.Visitors
{
    public partial class PipelineVisitor
    {
        public PushTrigger VisitPushTrigger(YamlNode? node)
        {
            if (node == null)
                throw new ArgumentNullException(nameof(node));

            PushTrigger pushTrigger = new PushTrigger();

            if (node is YamlSequenceNode nodeAsSequence)
            {
                foreach (var child in nodeAsSequence.Children)
                {
                    pushTrigger.BranchNames.Add(child.ToString());
                }
            }
            else if (node is YamlMappingNode nodeAsMapping)
            {
                foreach (var child in nodeAsMapping.Children)
                {
                    switch (child.Key.ToString().ToLower())
                    {
                        case "batch":
                            pushTrigger.Batch = bool.Parse(child.Value.ToString());
                            break;
                        case "branches":
                            var branch = VisitIncludeExclude(child.Value as YamlMappingNode);
                            pushTrigger.Branches = branch;
                            pushTrigger.Children.Add(branch);
                            break;
                        case "tags":
                            var tag = VisitIncludeExclude(child.Value as YamlMappingNode);
                            pushTrigger.Tags = tag;
                            pushTrigger.Children.Add(tag);
                            break;
                        case "paths":
                            var path = VisitIncludeExclude(child.Value as YamlMappingNode);
                            pushTrigger.Paths = path;
                            pushTrigger.Children.Add(path);
                            break;
                    }
                }
            } 
            else if (node is YamlScalarNode nodeAsScalar)
            {
                if (nodeAsScalar.ToString().Equals("none", StringComparison.InvariantCultureIgnoreCase))
                    pushTrigger.Disabled = true;
            }

            return pushTrigger;
        }

        public PullRequestTrigger VisitPullRequestTrigger(YamlNode? node)
        {
            if (node == null)
                throw new ArgumentNullException(nameof(node));

            PullRequestTrigger prTrigger = new();

            if (node is YamlSequenceNode nodeAsSequence)
            {
                foreach (var child in nodeAsSequence.Children)
                {
                    prTrigger.BranchNames.Add(child.ToString());
                }
            }
            else if (node is YamlMappingNode nodeAsMapping)
            {
                foreach (var child in nodeAsMapping.Children)
                {
                    switch (child.Key.ToString().ToLower())
                    {
                        case "autocancel":
                            prTrigger.AutoCancel = bool.Parse(child.Value.ToString());
                            break;
                        case "drafts":
                            prTrigger.Drafts = bool.Parse(child.Value.ToString());
                            break;
                        case "branches":
                            var branch = VisitIncludeExclude(child.Value as YamlMappingNode);
                            prTrigger.Branches = branch;
                            prTrigger.Children.Add(branch);
                            break;
                        case "paths":
                            var path = VisitIncludeExclude(child.Value as YamlMappingNode);
                            prTrigger.Paths = path;
                            prTrigger.Children.Add(path);
                            break;
                    }
                }
            }
            else if (node is YamlScalarNode nodeAsScalar)
            {
                if (nodeAsScalar.ToString().Equals("none", StringComparison.InvariantCultureIgnoreCase))
                    prTrigger.Disabled = true;
            }

            return prTrigger;
        }

        public PipelineTrigger VisitPipelineTrigger(YamlNode? node)
        {
            if (node is YamlScalarNode scalarNode)
            {
                PipelineTrigger result = new();
                if (scalarNode.ToString().Equals("true", StringComparison.InvariantCultureIgnoreCase))
                    result.TriggerOnPipelineCompletion = true;
                return result;
            }
            else if (node is YamlMappingNode mappingNode)
            {
                var result = new PipelineTrigger();
                foreach (var child in mappingNode.Children)
                {
                    switch (child.Key.ToString().ToLower())
                    {
                        case "branches":
                            var branches = VisitIncludeExclude(child.Value as YamlMappingNode);
                            result.Branches = branches;
                            result.Children.Add(branches);
                            break;
                        case "tags":
                            var tagsSequence = child.Value as YamlSequenceNode;
                            if (tagsSequence == null)
                                throw new ArgumentNullException(nameof(tagsSequence));

                            foreach (var tag in tagsSequence.Children)
                            {
                                result.Tags.Add(tag.ToString());
                            }
                            break;
                        case "stages":
                            var stageSequence = child.Value as YamlSequenceNode;
                            if (stageSequence == null)
                                throw new ArgumentNullException(nameof(stageSequence));

                            foreach (var stage in stageSequence.Children)
                            {
                                result.Stages.Add(stage.ToString());
                            }
                            break;
                        default:
                            throw new Exception("Unexpected property on pipeline trigger.");
                    }
                }

                return result;
            }
            else
                throw new ArgumentNullException(nameof(node));
        }

        public IncludeExclude VisitIncludeExclude(YamlMappingNode? node)
        {
            if (node == null)
                throw new ArgumentNullException(nameof(node));

            IncludeExclude result = new();

            foreach (var child in node.Children)
            {
                switch (child.Key.ToString().ToLower())
                {
                    case "include":
                    case "exclude":
                        var nodeAsSequence = child.Value as YamlSequenceNode;
                        if (nodeAsSequence == null)
                            throw new ArgumentNullException(nameof(nodeAsSequence));
                        foreach (var str in nodeAsSequence.Children)
                        {
                            if (child.Key.ToString().ToLower() == "include")
                                result.Include.Add(str.ToString());
                            else
                                result.Exclude.Add(str.ToString());
                        }
                        break;
                    default:
                        throw new Exception("Expected include or exclude property.");
                }
            }

            return result;
        }
    }
}
