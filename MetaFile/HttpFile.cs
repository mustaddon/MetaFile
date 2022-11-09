using System;
using System.IO;

namespace MetaFile;

public class HttpFile : IHttpFile
{
    public virtual string? Type { get; set; }
    public virtual string? Name { get; set; }
    public virtual Stream Content { get; set; } = Stream.Null;
    public virtual bool InlineDisposition { get; set; }
    public virtual long Size
    {
        get => Content.Length;
        set { if (Content != Stream.Null) Content.SetLength(value); }
    }

    public virtual void Dispose()
    {
        Content?.Dispose();
        GC.SuppressFinalize(this);
    }
}

public class HttpFile<TMetadata> : HttpFile, IHttpFile<TMetadata>
{
    public virtual TMetadata? Metadata { get; set; }
}
