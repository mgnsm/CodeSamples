using Microsoft.Toolkit.Mvvm.Input;
using System.Collections.Generic;
using System.Linq;

namespace BindingToParentElementSample
{
    public class ViewModel
    {
        public IEnumerable<int> Items { get; } = Enumerable.Range(1, 3);

        public RelayCommand<object> Command { get; } = new RelayCommand<object>(item => 
        {
            /* do nothing ... */
        });
    }
}