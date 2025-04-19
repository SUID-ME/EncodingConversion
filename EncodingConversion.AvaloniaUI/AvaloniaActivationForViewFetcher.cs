using Avalonia.Controls;
using Avalonia.VisualTree;
using Avalonia.ReactiveUI;
using ReactiveUI;
using System;
using System.Reactive.Linq;
using Avalonia;

public class AvaloniaActivationForViewFetcher : IActivationForViewFetcher
{
    public int GetAffinityForView(Type view)
    {
        return typeof(Control).IsAssignableFrom(view) ? 10 : 0;
    }

    public IObservable<bool> GetActivationForView(IActivatableView view)
    {
        if (view is not Control control)
            return Observable.Return(false);

        var attached = Observable.FromEventPattern<EventHandler<VisualTreeAttachmentEventArgs>, VisualTreeAttachmentEventArgs>(
            h => control.AttachedToVisualTree += h,
            h => control.AttachedToVisualTree -= h
        ).Select(_ => true);

        var detached = Observable.FromEventPattern<EventHandler<VisualTreeAttachmentEventArgs>, VisualTreeAttachmentEventArgs>(
            h => control.DetachedFromVisualTree += h,
            h => control.DetachedFromVisualTree -= h
        ).Select(_ => false);

        return attached.Merge(detached);
    }
}