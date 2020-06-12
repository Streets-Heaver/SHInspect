using SHAutomation.Core;
using SHAutomation.Core.AutomationElements;
using SHAutomation.Core.Conditions;
using SHAutomation.Core.Definitions;
using SHInspect.Classes;
using SHInspect.Extensions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Media;

namespace SHInspect.Classes
{
    public class WindowBO : BaseNotify
    {
        public string Name { get; set; }
        public string AutomationId { get; set; }
        public string Identifier { get; set; }
        public ISHAutomationElement AutomationElement { get; set; }
        public bool IsCurrent { get; set; }


        public WindowBO(ISHAutomationElement automationElement, bool isCurrent)
        {
            AutomationElement = automationElement;
            IsCurrent = isCurrent;
            UpdateProperties();
            Identifier = !string.IsNullOrEmpty(AutomationId) ? AutomationId : Name;
        }
        public WindowBO(string identifier, bool isCurrent)
        {
            IsCurrent = isCurrent;
            Identifier = identifier;
        }
        public void UpdateProperties()
        {
            if (AutomationElement != null)
            {
                Name = StringExtensions.NormalizeString(AutomationElement.Properties.Name.ValueOrDefault);
                AutomationId = StringExtensions.NormalizeString(AutomationElement.Properties.AutomationId.ValueOrDefault);
                Identifier = !string.IsNullOrEmpty(AutomationId) ? AutomationId : Name;
            }
        }
      
    }
}
