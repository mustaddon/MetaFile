namespace MetaFile;

public interface IHttpFileReadOnly : IStreamFileReadOnly
{
    bool InlineDisposition { get; }
}

public interface IHttpFileReadOnly<out TMetadata> : IHttpFileReadOnly, IStreamFileReadOnly<TMetadata>
{

}
