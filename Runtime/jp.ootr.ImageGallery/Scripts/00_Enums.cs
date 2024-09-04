namespace jp.ootr.ImageGallery
{
    public enum SyncMode
    {
        Timestamp,
        DelayLocal,
        DelaySynced
    }

    public enum RestoreMode
    {
        None,
        Force,
        Timeout
    }
    
    public enum SourceType
    {
        Image,
        TextZip,
        TextZipImage
    }
}
