using System.Linq;
using System.Net.Http;
using System.Web.Http;
using NetMX.Remote.HttpAdaptor.Resources;

namespace NetMX.Remote.HttpAdaptor.Controllers
{
    public class MBeanServerController : BaseController
    {
        public MBeanServerController(IMBeanServerConnection serverConnection, string baseUrl)
            : base(serverConnection, baseUrl)
        {
        }

        public HttpResponseMessage<MBeanServerResource> Get()
        {
            var beans = _serverConnection.QueryNames(null, null)
                .Select(x => new Resources.MBeanInfo
                                 {
                                     ObjectName = x.CanonicalName,
                                     HRef = GetResourceUrl("bean", new { objectName = x.CanonicalName})
                                 });
            var resource = new MBeanServerResource
                               {
                                   Beans = beans.ToList()
                               };

            return new HttpResponseMessage<MBeanServerResource>(resource);
        }
    }
}