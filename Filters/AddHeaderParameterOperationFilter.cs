using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace DSF.AspNetCore.Api.Template.Filters
{
    /// <summary>
    /// Header
    /// </summary>
    public class AddHeaderParameterOperationFilter: IOperationFilter
    {
        /// <summary>
        /// apply
        /// </summary>
        /// <param name="operation"></param>
        /// <param name="context"></param>
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            if (operation.Parameters != null)
            {

                operation.Parameters.Add(new OpenApiParameter
                {
                    Name = "client-key",
                    In = ParameterLocation.Header,
                    Description = "Used to authenticate/authorize the caller. In the format of a Pre-Shared API Key",
                    Required = false,
                    Schema = new OpenApiSchema { Type = "String" }
                });

                //operation.Parameters.Add(new OpenApiParameter
                //{
                //    Name = "service-id",
                //    In = ParameterLocation.Header,
                //    Description = "Used to identify the service",
                //    Required = false,
                //    Schema = new OpenApiSchema { Type = "String" }
                //});
            }
        }
    }
}
