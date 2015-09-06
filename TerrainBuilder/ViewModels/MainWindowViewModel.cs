using Microsoft.WindowsAPICodePack.Dialogs;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.IO;
using TerrainBuilder.Models;
using TerrainBuilder.Parsers;

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

        public List<string> Imports
        {
            get { return _imports; }
            set { this.RaiseAndSetIfChanged(ref _imports, value); }
        }
        private List<string> _imports;

        public string TemplatesDirectoryPath
        {
            get { return _templatesDirectoryPath; }
            set { this.RaiseAndSetIfChanged(ref _templatesDirectoryPath, value); }
        }
        private string _templatesDirectoryPath;

        public List<TemplateLibraryFile> Templates
        {
            get { return _templates; }
            set { this.RaiseAndSetIfChanged(ref _templates, value); }
        }
        private List<TemplateLibraryFile> _templates;

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
            LoadImportFileCommand.Subscribe(_ => this.LoadImportFileCommandExecute());

            ChooseTemplatesDirectoryPathCommand = ReactiveCommand.Create();
            ChooseTemplatesDirectoryPathCommand.Subscribe(_ => this.ChooseTemplatesDirectoryPathCommandExecute());

            LoadTemplatesCommand = ReactiveCommand.Create(this.WhenAny(vm => vm.TemplatesDirectoryPath, fp => !string.IsNullOrWhiteSpace(fp.Value)));
            LoadTemplatesCommand.Subscribe(_ => this.LoadTemplatesCommandExecute());

            CalculateCommand = ReactiveCommand.Create(this.WhenAny(
                vm => vm.Imports,
                vm => vm.Templates,
                (imports, templates) => imports.Value != null && templates.Value != null
            ));
            CalculateCommand.Subscribe(_ => this.CalculateCommandExecute());
        }

        private void ChooseImportFilePathCommandExecute()
        {
            var dialog = new CommonOpenFileDialog {
                EnsureFileExists = true,
            };

            if (this.ImportFilePath != null)
            {
                dialog.InitialDirectory = Path.GetDirectoryName(this.ImportFilePath);
            }

            if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                this.ImportFilePath = dialog.FileName;
            }
        }

        private void LoadImportFileCommandExecute()
        {
            var parser = new ImportFileParser(this.ImportFilePath);
            Imports = parser.Parse();
        }

        private void ChooseTemplatesDirectoryPathCommandExecute()
        {
            var dialog = new CommonOpenFileDialog {
                EnsurePathExists = true,
                IsFolderPicker = true,
                Title = "Select your TemplateLibs directory",
            };

            if (this.TemplatesDirectoryPath != null)
            {
                dialog.InitialDirectory = this.TemplatesDirectoryPath;
            }

            if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                this.TemplatesDirectoryPath = dialog.FileName;
            }
        }

        private void LoadTemplatesCommandExecute()
        {
            var parser = new TemplateLibrariesParser(this.TemplatesDirectoryPath);
            Templates = parser.Parse();
        }

        private void CalculateCommandExecute()
        {
            
        }
    }
}
