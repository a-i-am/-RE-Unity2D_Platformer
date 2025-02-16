/// <summary>
/// Update를 위임하기 위한 인터페이스. Update를 사용하는 모든 오브젝트에 붙이면 된다
/// </summary>
public interface IUpdateableObject
{
    ///상속받은 클래스들의 Enable, Disble 함수에서 초기화해줘야 한다
    //private void OnEnable()
    //{
    //    UpdateLogic.Instance.RegisterUpdateablObject(this);
    //}

    //private void OnDisable()
    //{
    //    UpdateLogic.Instance.DeregisterUpdateableObject(this);
    //}

    /// <summary>
    /// Update 함수 코드를 이곳에 넣어서 사용한다
    /// </summary>
    void OnUpdate();
}
