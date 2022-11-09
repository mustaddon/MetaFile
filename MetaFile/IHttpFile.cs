namespace MetaFile;

public interface IHttpFile : IStreamFile, IHttpFileReadOnly
{
    new bool InlineDisposition { get; set; }
}

public interface IHttpFile<TMetadata> : IHttpFile, IStreamFile<TMetadata>, IHttpFileReadOnly<TMetadata>
{

}
