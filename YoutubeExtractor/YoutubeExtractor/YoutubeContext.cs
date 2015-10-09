﻿using System;
using System.IO;
using YoutubeExtractor.Interface;

namespace YoutubeExtractor {
    public class YoutubeContext {
        public event EventHandler<YoutubeDownloadStateChangedArgs> ProgresStateChanged;
        public event EventHandler<RetryableProcessFailed> DownloadFailed;

        /// <summary>
        ///     The url to the youtube video.
        /// </summary>
        public string Url { get; set; } 

        /// <summary>
        ///     The chosen video info.
        /// </summary>
        public VideoInfo VideoInfo { get; set; } 

        /// <summary>
        ///     Path of the audio extracted from the video.
        /// </summary>
        public FileInfo AudioPath { get; set; }

        /// <summary>
        ///     The base directory used to store the audio path and thumbnail. sort of a cache dir.
        ///     Default is the directory of the exe.
        /// </summary>
        public DirectoryInfo BaseDirectory { get; set; }

        /// <summary>
        ///     The path to the downloaded video. Availability limited when using AudioDownloader, so it might be null.
        /// </summary>
        public FileInfo VideoPath { get; set; }

        /// <summary>
        ///     When downloading the video, this field is filled with the amount of bytes that are left to download.
        /// </summary>
        public int? BytesToDownload { get; set; }

        /// <summary>
        ///     The video title
        /// </summary>
        public string Title => this.VideoInfo?.Title ?? "";

        public YoutubeContext() { }
        
        /// <param name="url">The url to the youtube video</param>
        public YoutubeContext(string url) {
            Url = DownloadUrlResolver.NormalizeYoutubeUrl(url);
        }
        
        /// <summary>
        ///     Later determined if its for audio or video.
        /// </summary>
        internal string savepath { get; set; }

        /// <summary>
        ///     Save path to extracted audio
        /// </summary>
        internal string savableFilename => Path.Combine(BaseDirectory?.FullName??"", YoutubeUrlTo.SaveName(Title)+VideoInfo?.AudioExtension);

        internal YoutubeDownloadStateChangedArgs OnProgresStateChanged(object sender, YoutubeDownloadStateChangedArgs e) {
            ProgresStateChanged?.Invoke(sender, e);
            return e;
        }


        internal YoutubeDownloadStateChangedArgs OnProgresStateChanged(YoutubeDownloadStateChangedArgs e) {
            ProgresStateChanged?.Invoke(this, e);
            return e;
        }

        internal YoutubeDownloadStateChangedArgs OnProgresStateChanged(YoutubeStage stage) {
            return OnProgresStateChanged(new YoutubeDownloadStateChangedArgs() {Stage=stage});
        }

        internal YoutubeDownloadStateChangedArgs OnProgresStateChanged(YoutubeStage stage, double precentage) {
            return OnProgresStateChanged(new YoutubeDownloadStateChangedArgs() {Stage=stage, Precentage = precentage});
        }

        internal void OnDownloadFailed(RetryableProcessFailed e) {
            DownloadFailed?.Invoke(this, e);
        }


    }
}