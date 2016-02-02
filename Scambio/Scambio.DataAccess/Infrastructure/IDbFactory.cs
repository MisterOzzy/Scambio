namespace Scambio.DataAccess.Infrastructure
{
    public interface IDbFactory<out TContext>
    {
        TContext Initialize();
    }
}