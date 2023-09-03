namespace EasyPassFlasher.Core;

public static class StringExtensions
{
    public static byte[] ToHidByte(this string self, int length = 32)
    {
        var returnBytes = new List<byte>();

        self += "\n";
        
        returnBytes.Add(1);
        
        // Flash protection. Not really a secret but more like a safeguard for random unwanted messages by other utils.
        returnBytes.Add(0x45);
        returnBytes.Add(0x46);
        
        returnBytes.AddRange(self
            .ToCharArray()
            .Select(Convert.ToByte));

        while (returnBytes.Count <= length)
            returnBytes.Add(1);

        return returnBytes.ToArray();
    }
}