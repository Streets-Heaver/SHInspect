using Prism.Commands;
using SHInspect.Classes;

namespace SHInspect.ViewModels
{
    public partial class MainViewModel : ViewModelBase
    {
        public DelegateCommand AddWindowCommand { get; private set; }
        public DelegateCommand CrashWindowCommand { get; private set; }
        public DelegateCommand DeleteWindowCommand { get; private set; }
        public DelegateCommand SearchCommand { get; private set; }
        public DelegateCommand RefreshGridCommand { get; private set; }
        public DelegateCommand RefreshWindowsCommand { get; private set; }
        public DelegateCommand RefreshDetailsCommand { get; private set; }
        public DelegateCommand IsLiveCommand { get; private set; }
        public DelegateCommand CopyXPathCommand { get; private set; }
        public DelegateCommand NextResultCommand { get; private set; }
        public DelegateCommand PreviousResultCommand { get; private set; }
        public DelegateCommand GoToParentCommand { get; private set; }
        public DelegateCommand GoToRootCommand { get; private set; }
        public DelegateCommand MakeTemporaryCommand { get; private set; }
        public DelegateCommand RemoveWindowCommand { get; private set; }
        public DelegateCommand<string> CopyValueCommand { get; private set; }
        public DelegateCommand<MethodDetails> InvokeMethodCommand { get; private set; }
        public DelegateCommand FocusCommand { get; private set; }
    }
}
