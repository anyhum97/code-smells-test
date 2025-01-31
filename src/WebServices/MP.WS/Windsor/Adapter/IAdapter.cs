namespace MP.WS.Windsor.Adapter
{
    /// <summary>
    /// интерфейс адаптера преобразования из одного типа в другой
    /// </summary>
    /// <typeparam name="TFrom"></typeparam>
    /// <typeparam name="TTo"></typeparam>
    public interface IAdapter<in TFrom, out TTo>
    {
        /// <summary>
        /// ф-я  преобразования
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        TTo Convert(TFrom input);
    }

    /// <summary>
    /// интерфейс адаптера преобразования из одного типа в другой
    /// </summary>
    public interface IAdapter<in TFrom1, in TFrom2, out TTo>
    {
        /// <summary>
        /// ф-я  преобразования
        /// </summary>
        /// <returns></returns>
        TTo Convert(TFrom1 input1, TFrom2 input2);
    }

    /// <summary>
    /// интерфейс адаптера преобразования из одного типа в другой
    /// </summary>
    public interface IAdapter<in TFrom1, in TFrom2, in TFrom3, out TTo>
    {
        /// <summary>
        /// ф-я  преобразования
        /// </summary>
        /// <returns></returns>
        TTo Convert(TFrom1 input1, TFrom2 input2, TFrom3 input3);
    }
}