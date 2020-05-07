/* 
*   NatShare
*   Copyright (c) 2020 Yusuf Olokoba.
*/

namespace NatSuite.Sharing.Internal {

    using System;
    using System.Runtime.InteropServices;

    public static class Bridge {

        private const string Assembly = @"__Internal";
        public delegate void CompletionHandler (IntPtr context, bool success);

        [DllImport(Assembly, EntryPoint = @"NSCreateSharePayload")]
        public static extern IntPtr CreateSharePayload (CompletionHandler completionHandler, IntPtr context);
        [DllImport(Assembly, EntryPoint = @"NSCreateSavePayload")]
        public static extern IntPtr CreateSavePayload (string album, CompletionHandler completionHandler, IntPtr context);
        [DllImport(Assembly, EntryPoint = @"NSCreatePrintPayload")]
        public static extern IntPtr CreatePrintPayload (bool greyscale, bool landscape, CompletionHandler completionHandler, IntPtr context);
        [DllImport(Assembly, EntryPoint = @"NSAddText")]
        public static extern void AddText (this IntPtr payload, string text);
        [DllImport(Assembly, EntryPoint = @"NSAddImage")]
        public static extern void AddImage (this IntPtr payload, IntPtr pixelBuffer, int width, int height);
        [DllImport(Assembly, EntryPoint = @"NSAddMedia")]
        public static extern void AddMedia (this IntPtr payload, string uri);
        [DllImport(Assembly, EntryPoint = @"NSCommit")]
        public static extern void Commit (this IntPtr payload);
    }
}