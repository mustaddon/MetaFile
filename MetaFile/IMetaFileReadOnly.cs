using System.IO;

namespace MetaFile;

public interface IMetaFileReadOnly
{
    string? Type { get; }
    string? Name { get; }
    long Size { get; }
}

public interface IMetaFileReadOnly<out TMetadata> : IMetaFileReadOnly
{
    TMetadata? Metadata { get; }
}
