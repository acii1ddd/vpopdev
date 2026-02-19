using Scalar.AspNetCore;
using ToDo.API.EndpointSettings;

namespace ToDo.API.ConfigurationExtensions;

public static class WebAppExtensions
{
    public static async Task ConfigureApp(this WebApplication app)
    {
        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();

            app.MapScalarApiReference(options =>
            {
                options.AddPreferredSecuritySchemes("Bearer");
                
                options.AddHttpAuthentication("Bearer", auth =>
                {
                    auth.Token = "eyJhbGciOiJ...";
                });

                options.Title = "ProductService APi";
                options.Theme = ScalarTheme.Laserwave;
                options.DefaultHttpClient = new KeyValuePair<ScalarTarget, ScalarClient>(ScalarTarget.CSharp, ScalarClient.HttpClient);
            });
        }

        app.UseExceptionHandler(opt => { });

        //app.UseAuthentication();
        //app.UseAuthorization();

        var mapGroup = app.MapGroup("/api");
        app.MapEndpoints(mapGroup);

        await app.InitDbAsync();
    }
}