using System;
using Game.Foes.FSM.States;

namespace Game.Foes.FSM
{
    public class FiniteStateMachine {
        
        public bool IsActive = true;
        
        private BaseState _currentState;
        private Xenolith _xenolith;

        public FiniteStateMachine(Xenolith xenolith) {
            this._xenolith = xenolith;
        }

        public void Update() {
            if (!IsActive) return;
            if (_currentState == null) return;

            
            // Check transition from current state
            switch (_currentState) {
                case SelectTarget:
                    if (_xenolith.Target) ChangeState(new ReachNearestTarget(_xenolith));
                    break;
                case AttackTarget:
                    if (!_xenolith.Target) ChangeState(new SelectTarget(_xenolith));
                    if (!_xenolith.TargetOuterReach) ChangeState(new ReachNearestTarget(_xenolith));
                    break;
                case ReachNearestTarget:
                    if (_xenolith.Target && _xenolith.TargetInnerReach) ChangeState(new AttackTarget(_xenolith));
                    if (!_xenolith.Target) ChangeState(new SelectTarget(_xenolith));
                    break;
                default:
                    throw new Exception($"{_currentState.GetType().Name} is not a valid state");
            }
            
            // Execute current state
            _currentState.UpdateState();
        }

        public void ChangeState(BaseState newState) {
            if (_currentState != null) {
                _currentState.ExitState();
            }
            _currentState = newState;
            if (_currentState != null) {
                _currentState.EnterState();
            }
        }

        public override string ToString() {
            return _xenolith.gameObject.name + " is in " + _currentState.GetType().Name;
        }
    }
}
