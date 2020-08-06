namespace Util_.JsonHelper_
{
	public interface IOnBeforeJsonCreate
	{
		/// <summary>
		/// 파일을 만들기 직전에 호출 한다.
		/// </summary>
		void OnBeforeCreate();
	}
}
