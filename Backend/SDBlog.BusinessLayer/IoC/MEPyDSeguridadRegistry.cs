using StructureMap;

namespace SDBlog.BusinessLayer.IoC
{
    public class MEPyDSeguridadRegistry : Registry
    {
        public MEPyDSeguridadRegistry()
        {
            Scan(o =>
            {
                //o.TheCallingAssembly();
                o.Assembly("MEPyDBase.BusinessLayer");
                //o.Assembly("Seguridad.Services");
                o.WithDefaultConventions();
            });
        }
    }
}