using jp.ootr.ImageDeviceController.CommonDevice;
using UnityEngine;

namespace jp.ootr.ImageGallery
{
    public class BaseClass : CommonDevice
    {
        [SerializeField] protected ImageScreen.ImageScreen targetScreen;
        [SerializeField] [Range(1, 3600)] protected int seekInterval = 20;
        [SerializeField] protected int restoreTimeout = 60;
        [SerializeField] protected RestoreMode restoreMode = RestoreMode.Timeout;
        [SerializeField] protected SyncMode syncMode = SyncMode.Timestamp;
        protected ulong LastCastTime = 0;
    }
}
