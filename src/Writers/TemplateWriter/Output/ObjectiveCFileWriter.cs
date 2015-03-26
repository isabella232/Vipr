﻿using System.IO;
using System.Text;
using TemplateWriter.Templates;
using TemplateWriter.Settings;
using Vipr.Core.CodeModel;

namespace TemplateWriter.Output
{
    class ObjectiveCFileWriter : BaseFileWriter
    {
        public ObjectiveCFileWriter(OdcmModel model, TemplateWriterSettings configuration)
            : base(model, configuration)
        {
        }

        public new string FileExtension { get; set; }

        public override void WriteText(Template template, string fileName, string text)
        {
            var destPath = string.Format("{0}{1}", ConfigurationService.Settings.OutputDirectory, Path.DirectorySeparatorChar);

            var identifier = FileName(template, fileName);

            FileExtension = template.ResourceName.Contains("header") ? ".h" : ".m";

            // var fullPath = Path.Combine(destPath, destPath);

            if (!DirectoryExists(destPath))
                CreateDirectory(destPath);

            var fullPath = Path.Combine(destPath, template.FolderName);

            if (!DirectoryExists(fullPath))
                CreateDirectory(fullPath);

            var filePath = Path.Combine(fullPath, string.Format("{0}{1}", identifier, FileExtension));

            using (var writer = new StreamWriter(filePath, false, Encoding.ASCII))
            {
                writer.Write(text);
            }
        }

        protected override string FileName(Template template, string identifier)
        {
            return template.Name.Contains("Entity")
                ? (template.FolderName == "odata"
                    ? template.Name.Replace("Entity", identifier)
                    : identifier)
                    : identifier;
        }
    }
}