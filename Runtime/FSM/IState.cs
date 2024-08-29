namespace TripleA.FSM
{
	public interface IState
	{
		void OnEnter();
		void OnUpdate();
		void OnFixedUpdate();
		void OnLateUpdate();
		void OnExit();
	}
}