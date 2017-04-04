using System;
using JetBrains.Annotations;

namespace GPOrder.Helpers
{
    /// <summary>
    /// helper pour les traces basé sur le logger natif MVC System.Diagnostics.Trace
    /// </summary>
    public class Logger
    {
        [StringFormatMethod("format")]
        public static void TraceError(Exception ex, string format, params object[] args)
        {
            TraceError("{0} | {1}", string.Format(format, args), ex);
        }

        [StringFormatMethod("format")]
        public static void TraceError(string format, params object[] args)
        {
            System.Diagnostics.Trace.TraceError("[{0}] {1}", DateTime.UtcNow, string.Format(format, args));
        }

        [StringFormatMethod("format")]
        public static void TraceWarning(Exception ex, string format, params object[] args)
        {
            TraceWarning("{0} | {1}", string.Format(format, args), ex);
        }

        [StringFormatMethod("format")]
        public static void TraceWarning(string format, params object[] args)
        {
            System.Diagnostics.Trace.TraceWarning("[{0}] {1}", DateTime.UtcNow, string.Format(format, args));
        }

        [StringFormatMethod("format")]
        public static void TraceInformation(Exception ex, string format, params object[] args)
        {
            TraceInformation("{0} | {1}", string.Format(format, args), ex);
        }

        [StringFormatMethod("format")]
        public static void TraceInformation(string format, params object[] args)
        {
            System.Diagnostics.Trace.TraceInformation("[{0}] {1}", DateTime.UtcNow, string.Format(format, args));
        }
    }
}