using Microsoft.AspNetCore.OpenApi;
using Microsoft.OpenApi.Models;

namespace TodoHub.API.OperationTransformers;

public class AddRequiredHeaderParameter : IOpenApiOperationTransformer
{
    public Task TransformAsync(OpenApiOperation operation, OpenApiOperationTransformerContext context,CancellationToken cancellationToken)
    {
        if (operation.Tags.Any(t => "Users".Equals(t.Name)))
            return Task.CompletedTask;
        
        operation.Parameters ??= [];
        operation.Parameters.Add(new OpenApiParameter
        {
            Name = "x-user-id",
            In = ParameterLocation.Header,
            Required = true,
            Description = "User ID header present todo collection owner ID"
        });
        
        return Task.CompletedTask;
    }
}