using System;
using System.IO;

namespace MetaFile;

public class StreamFile : IStreamFile
{
    public virtual string? Type { get; set; }
    public virtual string? Name { get; set; }
    public virtual Stream Content { get; set; } = Stream.Null;

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

public class StreamFile<TMetadata> : StreamFile, IStreamFile<TMetadata>
{
    public virtual TMetadata? Metadata { get; set; }
}
