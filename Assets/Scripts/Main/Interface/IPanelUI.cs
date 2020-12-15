namespace DefaultNamespace
{
    /// <summary>
    /// 데이터가 변경돼야 하는 패널에 사용합니다.
    /// </summary>
    public interface IPanelUI
    {
        /// <summary>
        /// 패널에 표시되는 모든 데이터를 업데이트함.
        /// </summary>
        void ShowPanelData();
    }
}