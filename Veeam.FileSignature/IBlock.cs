using System.IO;

namespace Veeam.FileSignature
{
    public interface IBlock
    {
        void Calculate();
    }
}
