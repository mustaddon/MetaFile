namespace MetaFile;

public interface IContentFileReadOnly<out TContent> : IMetaFileReadOnly
{
    TContent Content { get; }
}

public interface IContentFileReadOnly<out TContent, out TMetadata> : IContentFileReadOnly<TContent>, IMetaFileReadOnly<TMetadata>
{

}
