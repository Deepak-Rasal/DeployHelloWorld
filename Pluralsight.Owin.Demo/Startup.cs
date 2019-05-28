using Owin;
using Pluralsight.Owin.Demo.MiddlerWare;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;

namespace Pluralsight.Owin.Demo
{
    public class Startup
    {
        public static void Configuration(IAppBuilder app)
        {
            app.UseDebugMiddlerware(new DebugMiddleWareOption
            {

                OnIncomingRequest = (ctx) =>
                {
                    var watch = new Stopwatch();
                    watch.Start();
                    ctx.Environment["DebugStopWatch"] = watch;
                },


                OnOutgoingRequest = (ctx) =>
                {
                    var watch = (Stopwatch)ctx.Environment["DebugStopWatch"];
                    watch.Stop();
                    Debug.WriteLine("Request Time : " + watch.ElapsedMilliseconds);

                }
            });

            app.Use(async (ctx, next) =>
            {

               await ctx.Response.WriteAsync("<html><head></head><body><b>Hello World from Git</b></body></html>");


            });
        }
    }
}