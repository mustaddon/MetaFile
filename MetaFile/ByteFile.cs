using System;

namespace MetaFile;

public class ByteFile : IByteFile
{
    public virtual string? Type { get; set; }
    public virtual string? Name { get; set; }
    public virtual byte[] Content { get; set; } = Array.Empty<byte>();

    public virtual long Size
    {
        get => Content.Length;
        set
        {
            var arr = new byte[value];
            Array.Copy(Content, 0, arr, 0, Math.Min(Content.Length, arr.Length));
            Content = arr;
        }
    }
}

public class ByteFile<TMetadata> : ByteFile, IByteFile<TMetadata>
{
    public virtual TMetadata? Metadata { get; set; }
}
