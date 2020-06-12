using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Media;

namespace SHInspect.Classes
{
    public class PatternBO : BaseNotify
    {
        public PatternBO(string key, string value,bool isSupported, bool isHeader)
        {
            Key = key;
            Value = value;
            IsSupported = isSupported;
            IsHeader = isHeader;
        }

        private string _key;

        public string Key
        {
            get { return _key; }
            set
            {
                _key = value;
                RaisePropertyChanged();
            }
        }
        private string _value;

        public string Value
        {
            get { return _value; }
            set
            {
                _value = value;
                RaisePropertyChanged();
            }
        }
        private bool _isSupported;

        public bool IsSupported
        {
            get { return _isSupported; }
            set
            {
                _isSupported = value;
                RaisePropertyChanged();
            }
        }
        private bool _isHeader;

        public bool IsHeader
        {
            get { return _isHeader; }
            set
            {
                _isHeader = value;
                RaisePropertyChanged();
            }
        }

        
        public bool Equals(PatternBO other)
        {
            return other != null && Key == other.Key && Value == other.Value && IsHeader == other.IsHeader && IsSupported == other.IsSupported;
        }

        public override int GetHashCode()
        {
            return (Key+Value).GetHashCode();
        }
    }
}
