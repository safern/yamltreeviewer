using AzurePipelineParser.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using YamlDotNet.RepresentationModel;

namespace AzurePipelineParser.Visitors
{
    public partial class PipelineVisitor
    {
        public PipelineVisitor(string yamlPath)
            : this(yamlPath, Path.GetDirectoryName(yamlPath)) { }

        public PipelineVisitor(string currentYamlPath, string yamlRootPath)
        {
            CurrentYamlPath = currentYamlPath;
            YamlRootPath = yamlRootPath;
        }

        public string CurrentYamlPath { get; set; }

        public string YamlRootPath { get; set; }

        public Pipeline? Visit()
        {
            var input = File.ReadAllText(CurrentYamlPath);
            var reader = new StringReader(input);

            var yaml = new YamlStream();
            yaml.Load(reader);

            var rootNode = yaml.Documents[0].RootNode as YamlMappingNode;
            if (rootNode != null)
                return VisitPipeline(rootNode);
            else
                return null;
        }

        // Each of the following Visitors will need to get implemented and moved to a specific file. For now, we just have them for
        // unit test purposes.

        #region Temporary Dummy Visitors
        public TemplateReference VisitTemplate(YamlNode node, TemplateType templateType)
        {
            return new TemplateReference();
        }
        #endregion
    }

    public enum TemplateType
    {
        Stage,
        Job,
        Step,
        Variable
    }
}
