using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace TD.WPF.Behaviors;

/// <summary>
/// Attached property that forwards mouse-wheel input from a child control (e.g. a nested
/// <see cref="DataGrid"/> or <see cref="ListBox"/>) to the nearest ancestor
/// <see cref="ScrollViewer"/>, so the outer scroll viewer scrolls even when the cursor is
/// over a child whose own scroll viewer would otherwise swallow the wheel event.
/// </summary>
public static class BubbleScroll
{
    public static readonly DependencyProperty EnabledProperty =
        DependencyProperty.RegisterAttached(
            "Enabled", typeof(bool), typeof(BubbleScroll),
            new PropertyMetadata(false, OnEnabledChanged));

    public static void SetEnabled(UIElement element, bool value) => element.SetValue(EnabledProperty, value);
    public static bool GetEnabled(UIElement element) => (bool)element.GetValue(EnabledProperty);

    private static void OnEnabledChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is not UIElement element)
            return;

        if (e.NewValue is true)
            element.PreviewMouseWheel += OnPreviewMouseWheel;
        else
            element.PreviewMouseWheel -= OnPreviewMouseWheel;
    }

    private static void OnPreviewMouseWheel(object sender, MouseWheelEventArgs e)
    {
        if (sender is not UIElement element || e.Handled)
            return;

        e.Handled = true;

        var args = new MouseWheelEventArgs(e.MouseDevice, e.Timestamp, e.Delta)
        {
            RoutedEvent = UIElement.MouseWheelEvent,
            Source = sender
        };

        FindParentScrollViewer(element)?.RaiseEvent(args);
    }

    private static ScrollViewer? FindParentScrollViewer(DependencyObject child)
    {
        var current = VisualTreeHelper.GetParent(child);
        while (current is not null)
        {
            if (current is ScrollViewer scrollViewer)
                return scrollViewer;
            current = VisualTreeHelper.GetParent(current);
        }
        return null;
    }
}
