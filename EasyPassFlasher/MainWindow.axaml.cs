using System;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Media;
using Avalonia.Threading;
using EasyPassFlasher.Core;

namespace EasyPassFlasher;

public partial class MainWindow : Window
{
    private EasyPassDevice _device = new();
    private CancellationDisposable _cancellationDisposable = new();
    private Dispatcher _uiDispatcher = Dispatcher.UIThread;

    public MainWindow()
    {
        InitializeComponent();
        CanResize = false;
        FlashButton.IsEnabled = false;
        
        _device.Start();
        StatusChange(false);
        _device.OnAlive.Subscribe(StatusChange, _cancellationDisposable.Token);
    }

    private void StatusChange(bool status)
    {
        _uiDispatcher.Invoke(() =>
        {
            FlashButton.IsEnabled = status;
            StatusColorBox.Background = new SolidColorBrush(status ? Colors.Chartreuse : Colors.Firebrick);
            StatusTextBox.Text = status ? "Online" : "Offline";
        });
    }


    private void OnFlashRequested_OnClick(object? sender, RoutedEventArgs e)
    {
        if (PasswordTextBox.Text is null)
            return;
        
        var result = _device.Flash(PasswordTextBox.Text.Trim()) ? "Success!" : "Failed...";
        
        TextInfo.Text = $"Flashing was {result} Please unplug the device if you want to re-flash. Also unplug the device and insert it again to restart this application.";

    }

    private void Window_OnClosed(object? sender, EventArgs e)
    {
        _cancellationDisposable.Dispose();
    }
}