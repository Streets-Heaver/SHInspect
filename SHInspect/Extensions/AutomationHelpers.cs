using SHAutomation.Core;
using SHAutomation.Core.AutomationElements;
using SHAutomation.Core.Capturing;
using SHAutomation.Core.Identifiers;
using SHAutomation.Core.Patterns;
using SHAutomation.Core.Patterns.Infrastructure;
using SHAutomation.UIA3;
using SHInspect.Classes;
using SHInspect.Constants;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using static SHAutomation.Core.FrameworkAutomationElementBase;

namespace SHInspect.Extensions
{
    public static class AutomationHelpers
    {
        public static List<DetailBO> GetElementDetailViewModelProperties(ISHAutomationElement element, out ImageSource image)
        {
            image = null;
            List<DetailBO> details = new List<DetailBO>();
            try
            {
                var items = element.GetSupportedPropertiesDirect().Where(x => x.Id != 0 && !x.Name.Contains("Available")).ToList();

                if (items.Any(x => x.Name == SHInspectConstants.AutomationId))
                {
                    var item = items.First(x => x.Name == SHInspectConstants.AutomationId);
                    items.Remove(item);
                    var output = GetPropertyValueHandled(element, item);
                    if (output != null)
                    {
                        details.Add(new DetailBO(item.Name, output?.ToString()));
                    }
                }
                if (items.Any(x => x.Name == SHInspectConstants.Name))
                {
                    var item = items.First(x => x.Name == SHInspectConstants.Name);
                    items.Remove(item);
                    var output = GetPropertyValueHandled(element, item);
                    if (output != null)
                    {
                        details.Add(new DetailBO(item.Name, output?.ToString()));
                    }
                }
                if (items.Any(x => x.Name == SHInspectConstants.ClassName))
                {
                    var item = items.First(x => x.Name == SHInspectConstants.ClassName);
                    items.Remove(item);
                    var output = GetPropertyValueHandled(element, item);
                    if (output != null)
                    {
                        details.Add(new DetailBO(item.Name, output?.ToString()));
                    }
                }
                if (items.Any(x => x.Name == SHInspectConstants.ControlType))
                {
                    var item = items.First(x => x.Name == SHInspectConstants.ControlType);
                    var output = GetPropertyValueHandled(element, item);
                    if (output != null)
                    {
                        details.Add(new DetailBO(item.Name, output?.ToString()));
                    }
                    items.Remove(item);
                }
                if (items.Any(x => x.Name == SHInspectConstants.HelpText))
                {
                    var item = items.First(x => x.Name == SHInspectConstants.HelpText);
                    items.Remove(item);
                    var output = GetPropertyValueHandled(element, item);
                    if (output != null)
                    {
                        details.Add(new DetailBO(item.Name, output?.ToString()));
                    }
                }
                if (items.Any(x => x.Name == SHInspectConstants.IsOffscreen))
                {
                    var item = items.First(x => x.Name == SHInspectConstants.IsOffscreen);
                    items.Remove(item);
                    var output = GetPropertyValueHandled(element, item);
                    if (output != null && output is bool)
                    {
                        var outputBool = output as bool?;
                        details.Add(new DetailBO(SHInspectConstants.IsOnscreen, (!outputBool.Value).ToString()));
                    }
                }
                if (items.Any(x => x.Name == SHInspectConstants.IsEnabled))
                {
                    var item = items.First(x => x.Name == SHInspectConstants.IsEnabled);
                    items.Remove(item);
                    var output = GetPropertyValueHandled(element, item);
                    if (output != null)
                    {
                        details.Add(new DetailBO(item.Name, output?.ToString()));
                    }
                }
                if (items.Any(x => x.Name == SHInspectConstants.BoundingRectangle))
                {
                    var item = items.First(x => x.Name == SHInspectConstants.BoundingRectangle);
                    items.Remove(item);
                    var output = GetPropertyValueHandled(element, item);
                    if (output != null)
                    {
                        details.Add(new DetailBO(item.Name, output?.ToString()));
                    }

                    if (element.SupportsBoundingRectangle && !element.BoundingRectangle.IsEmpty)
                    {
                        using var capture = Capture.Element(element as SHAutomationElement);
                        var bitmapSource = CreateBitmapSourceFromGdiBitmap(capture.Bitmap);
                        image = bitmapSource;
                    }
                }
                if (items.Any(x => x.Name == SHInspectConstants.RuntimeId))
                {
                    var item = items.First(x => x.Name == SHInspectConstants.RuntimeId);
                    items.Remove(item);
                    var output = GetPropertyValueHandled(element, item);
                    if (output != null && output is int[])
                    {
                        var arr = output as int[];
                        string result = string.Join(",", arr);
                        details.Add(new DetailBO(item.Name, result));
                    }
                }
                if (items.Any(x => x.Name == SHInspectConstants.ProviderDescription))
                {
                    var item = items.First(x => x.Name == SHInspectConstants.ProviderDescription);
                    items.Remove(item);
                }
                foreach (var item in items)
                {
                    var output = GetPropertyValueHandled(element, item);
                    if (output != null)
                    {
                        details.Add(new DetailBO(item.Name, output.ToString()));
                    }
                }
            }
            catch (Exception ex) when (ex is OverflowException || ex is InvalidOperationException || ex is ArgumentException || ex is UnauthorizedAccessException)
            {
                return null;
            }
            return details;
        }
        private static object GetPropertyValueHandled(ISHAutomationElement automationElement, PropertyId propertyId)
        {
            try
            {
                object output = null;
                automationElement.FrameworkAutomationElement.TryGetPropertyValue(propertyId, out output);
                return output;
            }
            catch
            {
                return null;
            }

        }

        public static PropertyInfo[] GetPublicProperties(this Type type)
        {
            if (type.IsInterface)
            {
                var propertyInfos = new List<PropertyInfo>();

                var considered = new List<Type>();
                var queue = new Queue<Type>();
                considered.Add(type);
                queue.Enqueue(type);
                while (queue.Count > 0)
                {
                    var subType = queue.Dequeue();
                    foreach (var subInterface in subType.GetInterfaces())
                    {
                        if (considered.Contains(subInterface)) continue;

                        considered.Add(subInterface);
                        queue.Enqueue(subInterface);
                    }

                    var typeProperties = subType.GetProperties(
                        BindingFlags.FlattenHierarchy
                        | BindingFlags.Public
                        | BindingFlags.Instance);

                    var newPropertyInfos = typeProperties
                        .Where(x => !propertyInfos.Contains(x));

                    propertyInfos.InsertRange(0, newPropertyInfos);
                }

                return propertyInfos.ToArray();
            }

            return type.GetProperties(BindingFlags.FlattenHierarchy
                | BindingFlags.Public | BindingFlags.Instance);
        }

        public static List<PatternBO> GetElementDetailViewModelPatterns(ISHAutomationElement element)
        {
            try
            {
                List<PatternBO> details = new List<PatternBO>();
                var library = element.Automation.PatternLibrary;
                PropertyInfo[] allPatterns = GetPublicProperties(library.GetType());
                List<string> patternNames = allPatterns.Select(x => x.Name.Split("Pattern")[0]).ToList();
                var supportedPatterns = element.GetSupportedPatterns().Where(x => x.Id != 0);
                var automationPatterns = element.Patterns;
                var automationPatternsProperties = GetPublicProperties(typeof(IFrameworkPatterns));


                foreach (var item in patternNames)
                {
                    if (supportedPatterns.Any(x => x.Name == item))
                    {
                        var pattern = supportedPatterns.First(x => x.Name == item);
                        object output;
                        element.FrameworkAutomationElement.TryGetNativePattern(pattern, out output);

                        if (output != null)
                        {
                            details.Add(new PatternBO(item, string.Empty, true, true));
                            var patternObject = automationPatternsProperties.First(x => x.Name.Split("Pattern")[0] == pattern.Name);
                            var patternType = patternObject.GetMethod.Invoke(automationPatterns, null);
                            var patternGet = patternType.GetType().GetMethods().First(x => x.Name == "get_Pattern").Invoke(patternType, null);
                            var patternProperties = GetPublicProperties(patternGet.GetType());
                            foreach (var prop in patternProperties)
                            {
                                var returntype = prop.GetMethod.ReturnType;
                                if (returntype.Name.Contains("AutomationProperty"))
                                {
                                    details.Add(new PatternBO(prop.Name, prop.GetMethod.Invoke(patternGet, null).ToString(), true, false));
                                }
                            }
                        }
                    }
                    else
                    {
                        details.Add(new PatternBO(item, string.Empty, false, true));
                    }

                }
                return details.OrderByDescending(x => x.IsSupported).ToList();
            }
            catch (Exception ex) when (ex is OverflowException || ex is InvalidOperationException || ex is ArgumentException || ex is UnauthorizedAccessException)
            {
                return null;
            }
        }
        public static BitmapSource CreateBitmapSourceFromGdiBitmap(Bitmap bitmap)
        {
            if (bitmap == null)
                throw new ArgumentNullException("bitmap");

            var rect = new Rectangle(0, 0, bitmap.Width, bitmap.Height);

            var bitmapData = bitmap.LockBits(
                rect,
                ImageLockMode.ReadWrite,
                System.Drawing.Imaging.PixelFormat.Format32bppArgb);

            try
            {
                var size = (rect.Width * rect.Height) * 4;

                return BitmapSource.Create(
                    bitmap.Width,
                    bitmap.Height,
                    bitmap.HorizontalResolution,
                    bitmap.VerticalResolution,
                    PixelFormats.Bgra32,
                    null,
                    bitmapData.Scan0,
                    size,
                    bitmapData.Stride);
            }
            finally
            {
                bitmap.UnlockBits(bitmapData);
            }
        }


    }
}
