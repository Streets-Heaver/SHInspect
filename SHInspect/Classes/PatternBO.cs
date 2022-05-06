using SHInspect.Enums;

namespace SHInspect.Classes
{
    public class PatternBO : BaseNotify
    {
        public PatternBO(string key, string value,bool isSupported, PatternType patternType)
        {
            Key = key;
            Value = value;
            IsSupported = isSupported;
            PatternType = patternType;
        }
        public PatternBO(string key, MethodDetails method, bool isSupported, PatternType patternType)
        {
            Key = key;
            Method = method;
            IsSupported = isSupported;
            PatternType = patternType;
        }
        private MethodDetails _method;

        public MethodDetails Method
        {
            get { return _method; }
            set { _method = value; }
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
        private PatternType _patternType;

        public PatternType PatternType
        {
            get { return _patternType; }
            set
            {
                _patternType = value;
                RaisePropertyChanged();
            }
        }

        public bool Equals(PatternBO other)
        {
            return other != null && Key == other.Key && Value == other.Value && PatternType == other.PatternType && IsSupported == other.IsSupported;
        }

        public override int GetHashCode()
        {
            return (Key+Value).GetHashCode();
        }
    }
}
