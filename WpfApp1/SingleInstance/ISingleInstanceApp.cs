using System.Collections.Generic;

namespace Microsoft.Shell
{
    public interface ISingleInstanceApp
    {
        bool SignalExternalCommandLineArgs(IList<string> args);
    }
}