using System;
using System.IO;

namespace MetaFile;

public interface IStreamFileReadOnly : IContentFileReadOnly<Stream>, IDisposable
{

}

public interface IStreamFileReadOnly<out TMetadata> : IStreamFileReadOnly, IContentFileReadOnly<Stream, TMetadata>
{

}
