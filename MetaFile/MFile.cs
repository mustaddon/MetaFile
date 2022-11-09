namespace MetaFile;

public class MFile : IMetaFile
{
    public virtual string? Type { get; set; }
    public virtual string? Name { get; set; }
    public virtual long Size { get; set; }
}

public class MFile<TMetadata> : MFile, IMetaFile<TMetadata>
{
    public virtual TMetadata? Metadata { get; set; }
}
