using Microsoft.AspNetCore.Components;

namespace yamltreeviewer.Shared
{
    public partial class YamlNode
    {
        private YamlNodeModel _node = YamlNodeModel.Default;

        [Parameter]
        public YamlNodeModel Node
        { 
            get => _node; 
            set
            {
                bool isDocument = value.Kind == NodeKind.Main;
                ChildrenExpanded = isDocument;
                CanToggle = !isDocument;
                _node = value;
            }
        }

        internal bool ChildrenExpanded { get; set; }

        internal bool CanToggle { get; set; } = true;

        internal string ExpandedCollapsedClass => ChildrenExpanded ? "expanded" : "collapsed";

        internal void Toggle()
        {
            if (!CanToggle) return;
            ChildrenExpanded = !ChildrenExpanded;
        }

        internal string GetIconClass()
        {
            return Node.Kind switch
            {
                NodeKind.Template => "bi bi-file-earmark-code template-icon",
                NodeKind.Main => "bi bi-file-earmark-code document-icon",
                NodeKind.Jobs => "bi bi-square-fill job-icon",
                _ => string.Empty,
            };
        }
    }

    public class YamlNodeModel
    {
        public static YamlNodeModel Default => new YamlNodeModel();
        public NodeKind Kind { get; set; }
        public string? Value { get; set; }
        public YamlNodeModel[] Children { get; set; } = Array.Empty<YamlNodeModel>();
    }

    public enum NodeKind
    {
        Main = 0,
        Template = 1,
        Jobs = 2
    }
}
