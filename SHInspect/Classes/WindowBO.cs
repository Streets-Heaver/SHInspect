using SHAutomation.Core.AutomationElements;
using SHInspect.Extensions;

namespace SHInspect.Classes
{
    public class WindowBO : BaseNotify
    {
        public string Name { get; set; }
        public string AutomationId { get; set; }
        public string Identifier { get; set; }
        public ISHAutomationElement AutomationElement { get; set; }
        public bool IsCurrent { get; set; }
        private bool isTemporary;

        public bool IsTemporary
        {
            get { return isTemporary; }
            set {
                isTemporary = value;
                RaisePropertyChanged();
            }
        }

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
