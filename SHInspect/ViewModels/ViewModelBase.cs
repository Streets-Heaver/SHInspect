using Prism.Commands;
using SHInspect.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;

namespace SHInspect.ViewModels
{
    public class ViewModelBase : BaseNotify
    {
        List<DelegateCommandBase> _commands;
        public ViewModelBase()
        {
            CommandManager.RequerySuggested += CommandManager_RequerySuggested;
        }

        private void CommandManager_RequerySuggested(object sender, EventArgs e)
        {
            RaiseCanExecuteChangedOnCommands();
        }

        protected void RaiseCanExecuteChangedOnCommands()
        {
            if (_commands == null)
            {
                var delegateCommands = GetDelegateCommands(this.GetType(), this);
                _commands = delegateCommands.Where(cm => cm != null).ToList();
            }

            if (_commands != null)
                _commands.ForEach(cm => cm.RaiseCanExecuteChanged());

            IEnumerable<DelegateCommandBase> GetDelegateCommands(Type type, object instance)
            {
                return type.GetProperties().Where(p => typeof(DelegateCommandBase).IsAssignableFrom(p.PropertyType)).Select(p => p.GetValue(instance) as DelegateCommandBase);
            }
        }
    }
}
