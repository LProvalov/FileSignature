using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Veeam.FileSignature
{
    internal class SHA256Block : ICalculatableBlock
    {
        private int _blockNumber;
        private byte[] _hashValue;
        private Stream _dataStream;

        public SHA256Block(int blockNumber, Stream dataStream)
        {
            _blockNumber = blockNumber;
            _dataStream = dataStream;
        }

        public void Calculate()
        {
            if (_dataStream.CanRead)
            {

                using (SHA256 sha256 = SHA256.Create())
                {
                    try
                    {
                        _hashValue = sha256.ComputeHash(_dataStream);
                    }
                    catch (Exception ex)
                    {
                        throw new Exception($"", ex);
                    }
                }
            }
            else
            {
                throw new Exception($"{_blockNumber}, Can't read data stream.");
            }
        }
    }
}
