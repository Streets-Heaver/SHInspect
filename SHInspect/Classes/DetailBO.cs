namespace SHInspect.Classes
{
    public class DetailBO : BaseNotify
    {
        public DetailBO(string key, string value)
        {
            Key = key;
            Value = value;
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

        public bool Equals(DetailBO other)
        {
            return other != null && Key == other.Key && Value == other.Value;
        }

        public override int GetHashCode()
        {
            return (Key+Value).GetHashCode();
        }
    }
}
