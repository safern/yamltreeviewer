using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.JSInterop;
using System.Net.Http.Json;
using yamltreeviewer.Shared;

namespace yamltreeviewer.Pages
{
    public partial class Index : IAsyncDisposable
    {
        private YamlNodeModel? _root = null;
        private bool _loadingTree = false;
        private bool _shouldShowTree = false;
        private InputFile? fileInput;
        private ElementReference? clickContainerElement;
        private IJSObjectReference? _clickContainerInstance;
        private IBrowserFile? _currentFile = null;

        [Inject]
        public HttpClient? Http { get; set; }

        [Inject]
        public IJSRuntime? JS { get; set; }

        protected override async Task OnInitializedAsync()
        {
            _loadingTree = true;
            _root = await Http!.GetFromJsonAsync<YamlNodeModel>("sample-data/yaml-tree.json");
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                // Initialize the drop zone
                _clickContainerInstance = await JS!.InvokeAsync<IJSObjectReference>("interopFunctions.initializeClickElement", clickContainerElement, fileInput!.Element);
            }
        }


        private void OnInputFileChanged(InputFileChangeEventArgs e)
        {
            if (e.File.ContentType != "application/x-zip-compressed")
            {
                // TODO show error.
                return;
            }

            _currentFile = e.File;

        }

        public async ValueTask DisposeAsync()
        {
            if (_clickContainerInstance != null)
            {
                await _clickContainerInstance.InvokeVoidAsync("dispose");
                await _clickContainerInstance.DisposeAsync();
            }
        }
    }
}
