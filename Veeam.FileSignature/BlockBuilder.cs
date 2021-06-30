using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Veeam.FileSignature
{
    public class BlockBuilder : IDisposable
    {
        private long _currentPositionInFile;
        private long _fileSize;
        private int _blockByteSize;
        private int _bytesRead = 0;
        private int _blockCount;
        private FileStream _fileStream;

        public BlockBuilder(FileInfo fileInfo, int blockByteSize)
        {
            if (blockByteSize <= 0)
            {
                throw new ArgumentException("BlockByteSize");
            }
            _blockByteSize = blockByteSize;
            if (!fileInfo.Exists)
            {
                throw new ArgumentException("File should exist");
            }
            _fileSize = fileInfo.Length;
            _currentPositionInFile = 0;
            _fileStream = fileInfo.OpenRead();
            _blockCount = 0;
        }

        public IBlock BuildNextBlock()
        {
            byte[] buffer = new byte[_blockByteSize];
            
            var blockSize = _fileStream.Read(buffer, 0, _blockByteSize);

            if (blockSize > 0)
            {
                _bytesRead += blockSize;
                return new SHA256Block(_blockCount++, buffer, blockSize);
            }

            EoF = true;
            return null;
        }

        public bool EoF { get; private set; }

        public void Dispose()
        {
            if (_fileStream != null)
            {
                _fileStream.Dispose();
            }
        }
    }
}
