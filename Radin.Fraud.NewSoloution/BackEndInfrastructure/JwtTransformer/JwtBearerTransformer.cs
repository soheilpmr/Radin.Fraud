namespace IDGF.Core
{
    using Microsoft.AspNetCore.OpenApi;
    using Microsoft.OpenApi;

    public class JwtBearerTransformer : IOpenApiDocumentTransformer
    {
        public Task TransformAsync(OpenApiDocument document, OpenApiDocumentTransformerContext context, CancellationToken cancellationToken)
        {
            document.Components ??= new OpenApiComponents();

            document.Components.SecuritySchemes = document.Components.SecuritySchemes ?? new Dictionary<string, IOpenApiSecurityScheme>();

            document.Components.SecuritySchemes["Bearer"] = new OpenApiSecurityScheme
            {
                Type = SecuritySchemeType.Http,
                Scheme = "bearer",
                BearerFormat = "JWT",
                Description = "paste your token here"
            };

            document.Security ??= new List<OpenApiSecurityRequirement>();

            document.Security.Add(new OpenApiSecurityRequirement
                {
                    { new OpenApiSecuritySchemeReference("Bearer"), new List<string>() }
                });

            return Task.CompletedTask;
        }
    }

}
