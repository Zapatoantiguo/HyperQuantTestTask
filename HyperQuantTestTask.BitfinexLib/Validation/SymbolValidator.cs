
namespace HyperQuantTestTask.BitfinexLib.Validation
{
    public static class SymbolValidator 
    {
        public static bool IsValidTradingPair(string pair)
        {
            // TODO: добавить более реальную проверку
            // TODO: в перспективе - обернуть в типы возможные trade, funding и другие символы с поддержкой проверки валидности

            return pair.StartsWith("t");
        }
    }
}
