using Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Diagnostics;

using AppFunc = System.Func<
    System.Collections.Generic.IDictionary<string, object>,
    System.Threading.Tasks.Task
>;
using Microsoft.Owin;

namespace Pluralsight.Owin.Demo.MiddlerWare
{
    public class DebugMiddleWare
    {
        AppFunc _next;
        DebugMiddleWareOption _options;

        public DebugMiddleWare(AppFunc next, DebugMiddleWareOption options)
        {
            _next = next;
            _options = options;

            if (_options.OnIncomingRequest == null)
                _options.OnIncomingRequest = (ctx) => { Debug.WriteLine("Incoming Request" + ctx.Request.Path); };

            if (_options.OnOutgoingRequest == null)
                _options.OnOutgoingRequest = (ctx) => { Debug.WriteLine("Outgoing Request" + ctx.Request.Path); };
        }

        public async Task Invoke(IDictionary<string, object> environment)
        {
            var ctx = new OwinContext(environment);


            _options.OnIncomingRequest(ctx);

            await _next(environment);

            _options.OnOutgoingRequest(ctx);
        }
    }
}