using Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Pluralsight.Owin.Demo.MiddlerWare
{
    public static class DebugMiddlerwareExtensions
    {

        public static void UseDebugMiddlerware(this IAppBuilder app, DebugMiddleWareOption option)
        {
            if (option == null)
                option = new DebugMiddleWareOption();

            app.Use<DebugMiddleWare>(option);
        }

    }
}