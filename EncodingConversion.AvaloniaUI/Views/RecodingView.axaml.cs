using Avalonia.Controls;
using Avalonia.ReactiveUI;
using EncodingConversion.AvaloniaUI.ViewModels;
using ReactiveUI;
using System;
using System.Reactive.Disposables;

namespace EncodingConversion.AvaloniaUI.Views;

internal partial class RecodingView : ReactiveUserControl<RecordingVM>
{
    public RecodingView()
    {
        InitializeComponent();
        SubscribeToInteraction();
    }

    private void SubscribeToInteraction()
    {
        this.WhenActivated(disposables =>
        {
            if (DataContext == null || DataContext is not RecordingVM vm)
            {
                return;
            }

            vm.ShowFilePiker.RegisterHandler(async context =>
            {
                // Берём топ левел приложения.
                TopLevel topLevel = TopLevel.GetTopLevel(this) ?? throw new NullReferenceException("Не удалось получить TopLevel.");

                // Открываем диалог.
                var dirs = await topLevel.StorageProvider.OpenFolderPickerAsync(new());

                context.SetOutput(dirs);
            }).DisposeWith(disposables);
        });
    }
}