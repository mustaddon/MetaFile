namespace MetaFile;

public interface IContentFile<TContent> : IMetaFile, IContentFileReadOnly<TContent>
{
    new TContent Content { get; set; }
}

public interface IContentFile<TContent, TMetadata> : IContentFile<TContent>, IMetaFile<TMetadata>, IContentFileReadOnly<TContent, TMetadata>
{

}
