// Licensed under the Apache License, Version 2.0. 
// Copyright (c) Alex Lee. All rights reserved.

using System;
using System.Runtime.InteropServices;


namespace SmartQuant
{
	
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    struct FileHeader
    {
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 10)] 
        public string FileMarker;
        public byte Version;
        public long HeaderLength;
        public long NewKeyPosition;
        public long OKeysKeyPosition;
        public long FKeysKeyPosition;
        public int OKeyLength;
        public int FKeyLength;
        public int OKeyCount;
        public int FKeyCount;
        public byte CompressionMethod;
        public byte CompressionLevel;
    }

    public partial class DataFile
    {
    }
}

