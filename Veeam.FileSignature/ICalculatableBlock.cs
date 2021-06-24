using System.IO;

namespace Veeam.FileSignature
{
    internal interface ICalculatableBlock
    {
        void Calculate();
    }
}
