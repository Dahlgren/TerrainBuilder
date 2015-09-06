using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace TerrainBuilder.ViewModels
{
    public class MainWindowViewModel : ReactiveObject
    {
        public string ImportFilePath
        {
            get { return _importFilePath; }
            set { this.RaiseAndSetIfChanged(ref _importFilePath, value); }
        }
        private string _importFilePath;

        public List<object> Imports
        {
            get { return _imports; }
            set { this.RaiseAndSetIfChanged(ref _imports, value); }
        }
        private List<object> _imports;

        public string TemplatesDirectoryPath
        {
            get { return _templatesDirectoryPath; }
            set { this.RaiseAndSetIfChanged(ref _templatesDirectoryPath, value); }
        }
        private string _templatesDirectoryPath;

        public List<object> Templates
        {
            get { return _templates; }
            set { this.RaiseAndSetIfChanged(ref _templates, value); }
        }
        private List<object> _templates;

        public ReactiveCommand<object> ChooseImportFilePathCommand { get; private set; }
        public ReactiveCommand<object> LoadImportFileCommand { get; private set; }

        public ReactiveCommand<object> ChooseTemplatesDirectoryPathCommand { get; private set; }
        public ReactiveCommand<object> LoadTemplatesCommand { get; private set; }

        public ReactiveCommand<object> CalculateCommand { get; private set; }

        public MainWindowViewModel()
        {
            ChooseImportFilePathCommand = ReactiveCommand.Create();
            ChooseImportFilePathCommand.Subscribe(_ => this.ChooseImportFilePathCommandExecute());

            LoadImportFileCommand = ReactiveCommand.Create(this.WhenAny(vm => vm.ImportFilePath, fp => !string.IsNullOrWhiteSpace(fp.Value)));
            LoadImportFileCommand.Subscribe(_ => this.LoadTemplatesCommandExecute());

            ChooseTemplatesDirectoryPathCommand = ReactiveCommand.Create();
            ChooseTemplatesDirectoryPathCommand.Subscribe(_ => this.ChooseTemplatesDirectoryPathCommandExecute());

            LoadTemplatesCommand = ReactiveCommand.Create(this.WhenAny(vm => vm.TemplatesDirectoryPath, fp => !string.IsNullOrWhiteSpace(fp.Value)));
            LoadTemplatesCommand.Subscribe(_ => this.LoadImportFileCommandExecute());

            CalculateCommand = ReactiveCommand.Create(this.WhenAny(
                vm => vm.Imports,
                vm => vm.Templates,
                (imports, templates) => imports.Value != null && templates.Value != null
            ));
            CalculateCommand.Subscribe(_ => this.CalculateCommandExecute());
        }

        private void ChooseImportFilePathCommandExecute()
        {
            var dialog = new OpenFileDialog();
            var result = dialog.ShowDialog();
            if (result == DialogResult.OK)
            {
                ImportFilePath = dialog.FileName;
            }
        }

        private void LoadImportFileCommandExecute()
        {
            Imports = new List<object>();
        }

        private void ChooseTemplatesDirectoryPathCommandExecute()
        {
            var dialog = new FolderBrowserDialog();
            dialog.Description = "Select your TemplateLibs directory";
            dialog.SelectedPath = this.TemplatesDirectoryPath;
            var result = dialog.ShowDialog();
            if (result == DialogResult.OK)
            {
                TemplatesDirectoryPath = dialog.SelectedPath;
            }
        }

        private void LoadTemplatesCommandExecute()
        {
            Templates = new List<object>();
        }

        private void CalculateCommandExecute()
        {
            
        }
    }
}
