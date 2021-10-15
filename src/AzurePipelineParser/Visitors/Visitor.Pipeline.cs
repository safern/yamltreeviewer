using AzurePipelineParser.Models;
using System;
using System.Collections.Generic;
using System.Text;
using YamlDotNet.RepresentationModel;

namespace AzurePipelineParser.Visitors
{
    public partial class Visitor
    {
        public Pipeline VisitPipeline(YamlMappingNode node)
        {
            Pipeline result = new Pipeline();

            foreach (var child in node.Children)
            {
                switch (child.Key.ToString().ToLowerInvariant())
                {
                    case "name":
                        result.Name = child.Value.ToString();
                        break;
                    case "resources":
                        var resources = VisitResources(child.Value as YamlMappingNode);
                        result.Children.Add(resources);
                        result.Resources = resources;
                        break;
                    case "variables":
                        var variables = VisitVariables(child.Value);
                        result.Children.Add(variables);
                        result.Variables = variables;
                        break;
                    case "trigger":
                        var trigger = VisitPushTrigger(child.Value);
                        result.Children.Add(trigger);
                        result.Trigger = trigger;
                        break;
                    case "pr":
                        var prTrigger = VisitPullRequestTrigger(child.Value as YamlMappingNode);
                        result.Children.Add(prTrigger);
                        result.Pr = prTrigger;
                        break;
                    case "stages":
                        throw new NotImplementedException();
                        break;
                    case "jobs":
                        throw new NotImplementedException();
                        break;
                    case "steps":
                        throw new NotImplementedException();
                        break;
                    default:
                        throw new Exception("Unknown child found");
                }
            }

            return result;
        }
    }
}
