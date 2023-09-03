using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Reactive.Threading.Tasks;
using HidLibrary;

namespace EasyPassFlasher.Core;

public class EasyPassDevice : IDisposable
{
    public bool OnlineAndReady
        => _hidDevice is not null;

    public IObservable<bool> OnAlive 
        => _onAliveSubject.AsObservable();

    private Subject<bool> _onAliveSubject = new();
    private HidDevice? _hidDevice = null;
    private CancellationDisposable _cancellationDisposable = new();

    public void Start()
    {
        CheckConnection(0);
        Observable
            .Interval(TimeSpan.FromSeconds(1))
            .Subscribe(CheckConnection, _cancellationDisposable.Token);
    }

    public bool Flash(string password)
    {
        if (_hidDevice is null)
            return false;
        
        var status = false;

        var payload = new HidDeviceData(HidDeviceData.ReadStatus.WaitTimedOut);

        var task = Task.Run(() =>
        {
            _hidDevice?.Write(password.ToHidByte(), 2000);
            payload = _hidDevice?.Read();
        });

        Task.WhenAny(task, Task.Delay(2000)).Wait();
        
        _hidDevice?.Dispose();
        _hidDevice = null;
        _cancellationDisposable.Dispose();
        _onAliveSubject.OnNext(false);
        
        return payload.Status == HidDeviceData.ReadStatus.Success;
    }

    private void CheckConnection(long obj)
    {
        if (_hidDevice is not null && (!_hidDevice.IsConnected || !_hidDevice.IsOpen))
        {
            _hidDevice.Dispose();
            _hidDevice = null;
            _onAliveSubject.OnNext(false);
            return;
        }
        
        var items = HidDevices.Enumerate();

        foreach (var device in items)
        {
            if (device.Attributes.VendorHexId.Equals("0x4249") && device.Attributes.ProductHexId.Equals("0x4287"))
            {
                device.OpenDevice();

                if (device.IsConnected && device.IsOpen)
                {
                    _hidDevice = device;
                    _onAliveSubject.OnNext(true);
                    return;
                }
                
                device.CloseDevice();
                device.Dispose();
                _hidDevice = null;
            }
        }
    }

    public void Dispose()
    {
        _hidDevice?.Dispose();
        _onAliveSubject.Dispose();
        
        if (!_cancellationDisposable.IsDisposed)
            _cancellationDisposable.Dispose();
    }
}