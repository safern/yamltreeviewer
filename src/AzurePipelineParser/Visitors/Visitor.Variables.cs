using AzurePipelineParser.Models;
using System;
using System.Collections.Generic;
using System.Text;
using YamlDotNet.RepresentationModel;

namespace AzurePipelineParser.Visitors
{
    public partial class Visitor
    {
        public Variables VisitVariables(YamlNode node)
        {
            Variables result = new Variables();
            if (node is YamlSequenceNode sequenceNode)
            {
                foreach (var child in sequenceNode.Children)
                {
                    // special case when variables are templates
                    if (child is YamlMappingNode childAsMappingNode && childAsMappingNode.Children.ContainsKey("template"))
                    {
                        var templateReference = VisitTemplate(child, TemplateType.Variable);
                        result.Children.Add(templateReference);
                    }
                    else
                    {
                        var newChild = VisitVariable(child as YamlMappingNode);
                        result.Children.Add(newChild);
                        result.VariableList.Add(newChild);
                    }
                }
                return result;
            }
            else if (node is YamlMappingNode mappingNode)
            {
                foreach (var child in mappingNode.Children)
                {
                    var newChild = VisitVariable(child.Key.ToString(), child.Value as YamlScalarNode);
                    result.Children.Add(newChild);
                    result.VariableList.Add(newChild);
                }
                return result;
            }

            throw new Exception("Wasn't expecting a node that wasn't a sequence or an object here.");
        }

        public Variable VisitVariable(string name, YamlScalarNode? node)
        {
            if (node == null)
            {
                throw new ArgumentNullException(nameof(node));
            }
            var result = new Variable();
            result.Name = name;
            result.Value = node.ToString();
            return result;
        }

        public Variable VisitVariable(YamlMappingNode? node)
        {
            if (node == null)
            {
                throw new ArgumentNullException(nameof(node));
            }
            var result = new Variable();
            foreach (var child in node.Children)
            {
                switch (child.Key.ToString().ToLower())
                {
                    case "name":
                        result.Name = child.Value.ToString();
                        break;
                    case "value":
                        result.Value = child.Value.ToString();
                        break;
                    case "group":
                        result.Group = child.Value.ToString();
                        break;
                    case "readonly":
                        result.IsReadOnly = bool.Parse(child.Value.ToString());
                        break;
                    default:
                        throw new Exception("Unexpected key in variable.");
                }
            }
            return result;
        }
    }
}
