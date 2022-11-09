using System.IO;

namespace MetaFile;

public interface IStreamFile : IContentFile<Stream>, IStreamFileReadOnly
{

}

public interface IStreamFile<TMetadata> : IStreamFile, IContentFile<Stream, TMetadata>, IStreamFileReadOnly<TMetadata>
{

}
