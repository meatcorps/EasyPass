// See https://aka.ms/new-console-template for more information
// // item is {VendorID: 4617, ProductID: 50525}

 //Create logger factory that will pick up all logs and output them in the debug output window

using EasyPassFlasher.Core;

if (args.Length == 0)
{
    Console.WriteLine("No argument detected. Use this cli tool if you hate GUI's. Max password length is 28. Will automatically be trimmed from start and end spaces. Few examples:\n --flash-password LoremIpum\n --status");
    return;
}

var device = new EasyPassDevice();

device.Start();
await Task.Delay(1000);

if (!device.OnlineAndReady)
{
    Console.WriteLine("Offline");
    device.Dispose();
    return;
}

if (args[0] == "--flash-password")
{
    var argList = args.ToList();
    argList.RemoveAt(0);

    var password = string.Join(' ', argList.ToArray()).Trim();

    if (password.Length > 28)
        password = password.Substring(0, 27);

    if (device.Flash(password))
        Console.WriteLine("Success");
    else
        Console.WriteLine("Failed or timeout");
}

if (args[0] == "--status")
{
    Console.WriteLine("Online");
}
    
    

device.Dispose();