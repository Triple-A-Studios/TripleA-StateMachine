namespace TripleA.FSM
{
	public abstract class BaseState : IState
	{
		public virtual void OnEnter()
		{
			// noop
		}

		public virtual void OnUpdate()
		{
			// noop
		}

		public virtual void OnFixedUpdate()
		{
			//noop
		}

		public virtual void OnLateUpdate()
		{
			//noop
		}

		public virtual void OnExit()
		{
			//noop
		}
	}
}