﻿using System;
using System.Collections.Generic;
using System.Linq;
using TemplateWriter;
using Vipr.CLI.Extensions;

namespace Vipr.CLI
{
    public class TemplateAssemblyReader : ITemplateAssemblyReader
    {
        private readonly IConfigArguments _arguments;

        public TemplateAssemblyReader(IConfigArguments arguments)
        {
            _arguments = arguments;
        }

        public IList<Template> Read(Type targetType)
        {
            var resourceNames = targetType.Assembly.GetManifestResourceNames();
            var baseString = string.Format("{0}.Base", _arguments.BuilderArguments.Language);
            return resourceNames.Select(x =>
            {
                var splits = x.Split('.');
                var name = splits.ElementAt(splits.Count() - 2);
                return new Template(name, x)
                {
                    Name = name,
                    ResourceName = x,
                    IsBase = x.Contains(baseString, StringComparison.InvariantCultureIgnoreCase)
                };
            }).ToList();
        }
    }
}