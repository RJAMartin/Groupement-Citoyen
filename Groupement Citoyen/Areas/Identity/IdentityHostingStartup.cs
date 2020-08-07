using Microsoft.AspNetCore.Hosting;

[assembly: HostingStartup(typeof(Groupement_Citoyen.Areas.Identity.IdentityHostingStartup))]
namespace Groupement_Citoyen.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) =>
            {
            });
        }
    }
}