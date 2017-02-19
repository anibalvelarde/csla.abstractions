using Csla.DataPortalClient;
using System.ServiceModel;

namespace Csla.Abstractions.Proxy
{
    public class WcfProxyBasic : WcfProxy
    {
        protected override ChannelFactory<IWcfPortal> GetChannelFactory()
        {
            ChannelFactory<IWcfPortal> factory;

            factory = new ChannelFactory<IWcfPortal>("CslaAbstractionsEndPoint");

            return factory;
        }

    }
}
