namespace MetaFile;

public interface IByteFileReadOnly : IContentFileReadOnly<byte[]>
{

}

public interface IByteFileReadOnly<out TMetadata> : IByteFileReadOnly, IContentFileReadOnly<byte[], TMetadata>
{

}
