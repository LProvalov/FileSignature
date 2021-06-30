using System;
using System.Collections.Generic;
using System.Text;

namespace Veeam.FileSignature
{
    public static class Utility
    {
        private static void OutputExceptionMessageToConsole(Exception ex)
        {
            if (ex != null && !string.IsNullOrEmpty(ex.Message))
            {
                Console.WriteLine($"{ex.Message}");
            }
            if (ex.InnerException != null)
            {
                OutputExceptionMessageToConsole(ex.InnerException);
            }
        }

        private static void OutputStackTraceMessageToConsole(Exception ex)
        {
            if (ex != null && !string.IsNullOrEmpty(ex.StackTrace))
            {
                Console.WriteLine(ex.StackTrace);
            }
            if (ex.InnerException != null)
            {
                OutputStackTraceMessageToConsole(ex.InnerException);
            }
        }
        public static void OutputException(Exception ex)
        {
            if (ex != null)
            {
                Console.WriteLine("Exception Message:");
                OutputExceptionMessageToConsole(ex);

                Console.WriteLine("Stack trace:");
                OutputStackTraceMessageToConsole(ex);
            }
        }
    }
}
