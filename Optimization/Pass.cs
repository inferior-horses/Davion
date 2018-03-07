using IR;

namespace Optimization
{
    public interface Pass
    {
        void Opt(Function function);
    }
}