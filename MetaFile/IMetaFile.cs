namespace MetaFile;

public interface IMetaFile : IMetaFileReadOnly
{
    new string? Type { get; set; }
    new string? Name { get; set; }
    new long Size { get; set; }
}

public interface IMetaFile<TMetadata> : IMetaFile, IMetaFileReadOnly<TMetadata>
{
    new TMetadata? Metadata { get; set; }
}
