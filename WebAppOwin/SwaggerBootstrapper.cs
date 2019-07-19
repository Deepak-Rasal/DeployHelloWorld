using Swashbuckle.Application;
using Swashbuckle.Swagger;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Web.Http;
using System.Web.Http.Description;

namespace WebAppOwin
{

    public static class SwaggerBootstrapper
    {
        public static void Register(HttpConfiguration config)
        {
            GlobalConfiguration.Configuration.EnableSwagger("docs/{apiVersion}/swagger", x =>
            {
                EnableSwagger(x);
            }).EnableSwaggerUi(swaggerUIConfig => EnableSwaggerUi(swaggerUIConfig));
        }
        private static void EnableSwaggerUi(SwaggerUiConfig swaggerUIConfig)
        {
            var assembly = typeof(SwaggerBootstrapper).Assembly;
            var resourceNamespace = typeof(SwaggerBootstrapper).Namespace;
            swaggerUIConfig.CustomAsset("index", assembly, string.Format("{0}.swagger.content.swagger.html", resourceNamespace));
            swaggerUIConfig.CustomAsset("ext/wkLogo", assembly, string.Format("{0}.swagger.content.wk_logo.png", resourceNamespace));
            swaggerUIConfig.EnableDiscoveryUrlSelector();
        }

        private static void EnableSwagger(SwaggerDocsConfig swaggerDocsConfig)
        {
            swaggerDocsConfig.SingleApiVersion("v1.0", "WK Admin ASE Service v1.0");
            swaggerDocsConfig.OperationFilter<AddAuthorizationHeader>();
            swaggerDocsConfig.OperationFilter<MultipleOperationsWithSameVerbFilter>();
            swaggerDocsConfig.PrettyPrint();
            swaggerDocsConfig.RootUrl(GetRootUrl);
            swaggerDocsConfig.Schemes(GetSchemes());
            swaggerDocsConfig.DescribeAllEnumsAsStrings();
        }

        static string GetHeaderValue(HttpRequestMessage request, string key)
        {
            IEnumerable<string> values;
            request.Headers.TryGetValues(key, out values);
            return values == null || values.Count() == 0 ? null : values.First();
        }

        private static string GetRootUrl(HttpRequestMessage req)
        {
            var root = ConfigurationManager.AppSettings["rootUrl"];
            var scheme = GetHeaderValue(req, "X-Forwarded-Proto") ?? req.RequestUri.Scheme;
            return scheme + ":" + root;
        }

        private static string[] GetSchemes()
        {
#if DEBUG
            return new[] { "http", "https" };

#else

            return new[] { "https" };
#endif      
        }
    }

    /// <summary>
    /// Add authorization header field
    /// </summary>
    internal class AddAuthorizationHeader : IOperationFilter
    {
        /// <summary>
        /// Adds an authorization header to the given operation in Swagger.
        /// </summary>
        /// <param name="operation">The Swashbuckle operation.</param>
        /// <param name="schemaRegistry">The Swashbuckle schema registry.</param>
        /// <param name="apiDescription">The Swashbuckle api description.</param>
        public void Apply(Operation operation, SchemaRegistry schemaRegistry, ApiDescription apiDescription)
        {
            if (operation == null) return;

            if (operation.parameters == null)
            {
                operation.parameters = new List<Parameter>();
            }

            var parameter = new Parameter
            {
                description = "The authorization header [Bearer: {access token}]",
                @in = "header",
                name = "Authorization",
                required = false,
                type = "string"
            };

            if (apiDescription.ActionDescriptor.GetCustomAttributes<AllowAnonymousAttribute>().Any())
            {
                parameter.required = false;
            }
            operation.parameters.Add(parameter);
        }
    }

    internal class MultipleOperationsWithSameVerbFilter : IOperationFilter
    {
        public void Apply(
            Operation operation,
            SchemaRegistry schemaRegistry,
            ApiDescription apiDescription)
        {
            string refsSwaggerIds = string.Empty;
            if (operation.parameters != null)
            {
                foreach (var x in operation.parameters)
                {
                    if (!string.IsNullOrEmpty(x.schema?.@ref) && !string.IsNullOrEmpty(x.schema?.@ref.Split('/').LastOrDefault()))
                    {
                        refsSwaggerIds += $"_{x.schema?.@ref.Split('/').LastOrDefault()}";
                    }
                    else
                    {
                        refsSwaggerIds += $"_{x.name}";
                    }
                }
            }
            operation.operationId += !string.IsNullOrEmpty(refsSwaggerIds)
                ? refsSwaggerIds + apiDescription.RelativePath?.Replace("/", "_").Replace("{", "").Replace("}", "") :
                 apiDescription.RelativePath?.Replace("/", "_").Replace("{", "").Replace("}", "");

        }
    }
}