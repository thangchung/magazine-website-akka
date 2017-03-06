using Topshelf;

namespace Cik.Magazine.MasterService
{
    public class MasterService : ServiceControl
    {
        public bool Start(HostControl hostControl)
        {
            return true;
        }

        public bool Stop(HostControl hostControl)
        {
            return true;
        }
    }
}