using System;
using jp.ootr.common;
using jp.ootr.ImageDeviceController;
using UnityEngine;
using VRC.SDKBase;

namespace jp.ootr.ImageGallery
{
    public class LogicInit : BaseClass
    {
        [SerializeField] public SourceType[] registeredTypes = new SourceType[0];
        [SerializeField] public string[] registeredUrls = new string[0];
        [SerializeField] public string[] registeredFileNames = new string[0];
        [SerializeField] public VRCUrl[] vrcUrls = new VRCUrl[0];
        
        protected string[] Urls;
        protected string[] FileNames;
        private string[] _toLoadUrls;


        public override void InitController()
        {
            base.InitController();
            PushVRCUrls();
            Urls = new string[0];
            FileNames = new string[0];
            _toLoadUrls = new string[0];
            var sourceLength = registeredTypes.Length;
            for (var i = 0; i < sourceLength; i++)
            {
                switch (registeredTypes[i])
                {
                    case SourceType.Image:
                        Urls = Urls.Append(registeredUrls[i]);
                        FileNames = FileNames.Append(registeredUrls[i]);
                        break;
                    case SourceType.TextZipImage:
                        Urls = Urls.Append(registeredUrls[i]);
                        FileNames = FileNames.Append(registeredFileNames[i]);
                        break;
                    case SourceType.TextZip:
                        Urls = Urls.Append(registeredUrls[i]);
                        _toLoadUrls = _toLoadUrls.Append(registeredUrls[i]);
                        FileNames = FileNames.Append("zip-temp://");
                        LLIFetchImage(registeredUrls[i], URLType.TextZip);
                        break;
                }
            }
        }

        private void PushVRCUrls()
        {
            foreach (var url in vrcUrls)
            {
                controller.UsAddUrlLocal(url);
            }
        }

        public override void OnFilesLoadSuccess(string source, string[] fileNames)
        {
            base.OnFilesLoadSuccess(source, fileNames);
            if (Urls.Has(source))
            {
                foreach (var fileName in fileNames)
                {
                    if (!fileNames.Has(fileName)) continue;
                    controller.CcGetTexture(source,fileName);
                }
            }
            if (!_toLoadUrls.Has(source)) return;
            {
                var index = Array.IndexOf(_toLoadUrls, source);
                _toLoadUrls = _toLoadUrls.Remove(index);
            }
            {
                var index = Array.IndexOf(Urls, source);
                FileNames = FileNames.Replace(fileNames, index);
                Debug.Log($"{string.Join(",",fileNames)} / {string.Join(",",FileNames)}");
                var newUrls = new string[fileNames.Length];
                for (var i = 0; i < fileNames.Length; i++) newUrls[i] = source;

                Urls = Urls.Replace(newUrls, index);
            }

            if (_toLoadUrls.Length > 0) return;
            SendCustomEventDelayedFrames(nameof(OnPreloadComplete), 1);
        }

        public virtual void OnPreloadComplete()
        {
        }
    }
}
