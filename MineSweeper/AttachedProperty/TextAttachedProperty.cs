using System.Windows;
using System.Windows.Controls;

namespace MineSweeper
{
    /// <summary>
    /// Focuses (keyboard focus) this element on load
    /// </summary>
    public class IsFocusedProperty : BaseAttachedProperty<IsFocusedProperty, bool>
    {
        public override void OnValueChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            if (!(sender is Control control))
                return;

            // Focus this control once loaded
            control.Loaded += (s, ee) => control.Focus();
        }
    }
}
