using AzurePipelineParser.Models;
using System;
using System.Collections.Generic;
using System.Text;
using YamlDotNet.RepresentationModel;

namespace AzurePipelineParser.Visitors
{
    public partial class Visitor
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
                                result.Include.Add(child.Value.ToString());
                            else
                                result.Exclude.Add(child.Value.ToString());
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
