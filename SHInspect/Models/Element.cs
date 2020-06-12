using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Automation;

namespace SHInspect.Models
{
    public class Element
    {
        public Element()
        {
            Children = new List<Element>();
        }

        public string Name { get; set; }
        public string AutomationId { get; set; }
        public string Text { get; set; }
        public ControlType ControlType { get; set; }
        public List<Element> Children { get; }
    }
}
