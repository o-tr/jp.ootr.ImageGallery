namespace jp.ootr.ImageGallery
{
    public class ImageGallery : LogicDelaySynced
    {
        public override string GetClassName()
        {
            return "jp.ootr.ImageGallery.ImageGallery";
        }

        public override string GetName()
        {
            return "ImageGallery";
        }

        public override bool IsCastableDevice()
        {
            return false;
        }

        public override void OnPreloadComplete()
        {
            if (Urls.Length == 0)
            {
                return;
            }
            switch (syncMode)
            {
                case SyncMode.Timestamp:
                    InitTimestamp();
                    break;
                case SyncMode.DelayLocal:
                    InitDelayLocal();
                    break;
                case SyncMode.DelaySynced:
                    InitDelaySynced();
                    break;
            }
        }
    }
}
