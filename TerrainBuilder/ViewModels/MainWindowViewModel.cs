using Microsoft.WindowsAPICodePack.Dialogs;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

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
            string[] lines = File.ReadAllLines(this.ImportFilePath);
            List<string> objects = new List<string>();
            Regex objectRegex = new Regex("^\"(.*)\";");

            foreach (var line in lines)
            {
                var match = objectRegex.Match(line);
                if (match.Success)
                {
                    objects.Add(match.Groups[1].Value);
                }
            }

            Imports = objects;
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
            Templates = new List<object>();
        }

        private void CalculateCommandExecute()
        {
            
        }
    }
}
