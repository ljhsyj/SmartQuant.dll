using System;
using System.IO;

namespace SmartQuant
{
    public class NetDataFile : DataFile
    {
        public NetDataFile(string name, string host, StreamerManager streamerManager = null)
            : base(name, streamerManager)
        {
        }

        public override void Open(FileMode mode = FileMode.OpenOrCreate)
        {
        }

        protected override bool OpenFileStream(string name, FileMode mode)
        {
            throw new NotImplementedException();
        }

        protected override void CloseFileStream()
        {
        }

        protected internal override void ReadBuffer(byte[] buffer, long position, int length)
        {
        }

        protected internal override void WriteBuffer(byte[] buffer, long position, int length)
        {
        }

        public override void Flush()
        {
        }
    }
}

