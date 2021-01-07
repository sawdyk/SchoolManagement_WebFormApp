using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(PassisNew.Startup))]
namespace PassisNew
{
    public partial class Startup {
        public void Configuration(IAppBuilder app) {
            ConfigureAuth(app);
        }
    }
}
