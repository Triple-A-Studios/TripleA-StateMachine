using System;

namespace TripleA.FSM
{
	public class FuncPredicate: IPredicate
	{
		readonly Func<bool> m_predicate;
		
		public FuncPredicate(Func<bool> predicate)
		{
			m_predicate = predicate;
		}
		
		public bool Evaluate() => m_predicate.Invoke();
	}
}