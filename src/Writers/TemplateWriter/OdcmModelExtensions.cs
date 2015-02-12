﻿using System;
using System.Collections.Generic;
using System.Linq;
using Vipr.Core.CodeModel;

namespace TemplateWriter
{
    public static class OdcmModelExtensions
    {
        public static bool IsCollection(this OdcmProperty odcmProperty)
        {
            return odcmProperty.Field != null && odcmProperty.Field.IsCollection;
        }


        private static OdcmNamespace GetOdcmNamespace(OdcmModel model)
        {
            return model.Namespaces.Find(x => String.Equals(x.Name,
                                              ConfigurationService.PrimaryNamespaceName,
                                              StringComparison.InvariantCultureIgnoreCase));
        }

        public static IEnumerable<OdcmClass> GetComplexTypes(this OdcmModel model)
        {
            var @namespace = GetOdcmNamespace(model);
            return @namespace.Classes.Where(x => x.Kind == OdcmClassKind.Complex);
        }

        public static IEnumerable<OdcmClass> GetEntityTypes(this OdcmModel model)
        {
            var @namespace = GetOdcmNamespace(model);
            return @namespace.Classes.Where(x => x.Kind == OdcmClassKind.Entity);
        }

        public static IEnumerable<OdcmEnum> GetEnumTypes(this OdcmModel model)
        {
            var @namespace = GetOdcmNamespace(model);
            return @namespace.Types.OfType<OdcmEnum>();
        }

        public static IEnumerable<OdcmProperty> NavigationProperties(this OdcmClass odcmClass)
        {
            return odcmClass.Properties.WhereIsNavigation();
        }

        public static IEnumerable<OdcmProperty> WhereIsNavigation(this IEnumerable<OdcmProperty> odcmProperties,
            bool isNavigation = true)
        {
            return odcmProperties.Where(p => isNavigation == (p.Type is OdcmClass
                                                              && ((OdcmClass)p.Type).Kind == OdcmClassKind.Entity));
        }

        public static bool HasActions(this OdcmClass odcmClass)
        {
            return odcmClass.Methods.Any();
        }

        public static IEnumerable<OdcmMethod> Actions(this OdcmClass odcmClass)
        {
            return odcmClass.Methods;
        }

        public static bool IsFunction(this OdcmMethod method)
        {
            return method.IsComposable; //TODO:REVIEW
        }

        public static string GetNamespace(this OdcmModel model)
        {
            var @namespace = GetOdcmNamespace(model);
            return @namespace.Name;
        }

        public static OdcmClass AsOdcmClass(this OdcmObject odcmObject)
        {
            return odcmObject as OdcmClass;
        }

        public static OdcmEnum AsOdcmEnum(this OdcmObject odcmObject)
        {
            return odcmObject as OdcmEnum;
        }
    }
}