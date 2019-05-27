using Microsoft.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Pluralsight.Owin.Demo.MiddlerWare
{
    public class DebugMiddleWareOption
    {
        public Action<IOwinContext> OnIncomingRequest { get; set; }

        public Action<IOwinContext> OnOutgoingRequest { get; set; }
    }
}