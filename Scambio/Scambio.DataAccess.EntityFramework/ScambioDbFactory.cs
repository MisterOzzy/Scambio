using Scambio.DataAccess.Infrastructure;

namespace Scambio.DataAccess.EntityFramework
{
    public class ScambioDbFactory : IDbFactory<ScambioContext>
    {
        private ScambioContext _scambioContext;

        public ScambioContext Initialize()
        {
            return _scambioContext ?? (_scambioContext = new ScambioContext());
        }
    }
}