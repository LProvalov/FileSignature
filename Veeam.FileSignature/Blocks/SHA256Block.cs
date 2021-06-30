using System;
using System.Security.Cryptography;
using System.Text;

namespace Veeam.FileSignature
{
    public class SHA256Block : IBlock
    {
        private int _blockNumber;
        private byte[] _hashValue;
        private byte[] _data;
        private int _blockSize;

        public SHA256Block(int blockNumber, byte[] data, int blockSize)
        {
            _blockNumber = blockNumber;
            _data = data;
            _blockSize = blockSize;
        }

        public void Calculate()
        {
            if (_data != null)
            {
                using (SHA256 sha256 = SHA256.Create())
                {
                    try
                    {
                        _hashValue = sha256.ComputeHash(_data, 0, _blockSize);
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

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            foreach (var b in _hashValue)
            {
                sb.Append(b.ToString("X2"));
            }
            return $"{_blockNumber}: {sb.ToString()}/n";
        }
    }
}
