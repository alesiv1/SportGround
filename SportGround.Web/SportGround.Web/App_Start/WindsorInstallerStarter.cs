using Sitecore.DependencyInjection.ContainerContexts;
using SportGround.Web.App_Start;
using SportGround.Web.App_Start.Installers;
using WebActivatorEx;

[assembly: PostApplicationStartMethod(typeof(WindsorInstallerStarter), "Start")]

namespace SportGround.Web.App_Start
{
    public class WindsorInstallerStarter
    {
        public static void Start()
        {
            //Register Controllers with WindsorContainer
            WindsorContainerContext.Instance.Install(new MvcControllersInstaller());
            //Register Services with WindsorContainer
            WindsorContainerContext.Instance.Install(new ServicesInstaller());
            //Initialise Solr with WindsorContainer
            //new WindsorSolrStartUp(WindsorContainerContext.Instance).Initialize();
        }
    }
}