namespace DefaultNamespace
{
    public interface IInstanceReceiver
    {
        /// <summary>
        /// 필드에 저장할 변수를 전달함.
        /// </summary>
        /// <param name="obj">필드에 저장할 변수</param>
        void SetInstance(object obj);
    }
}