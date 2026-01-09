using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Code.Scripts.Game.UI
{
    [Serializable]
    public class StepSlider : Slider
    {
        [SerializeField] public float StepsIncrement = .1f;

        public override void OnMove(AxisEventData eventData)
        {
            if(direction is Direction.RightToLeft or Direction.LeftToRight &&
               eventData.moveDir is MoveDirection.Left or MoveDirection.Right)
            {
                this.value += eventData.moveDir == MoveDirection.Left ? -StepsIncrement : StepsIncrement;
                return;
            }

            if (direction is Direction.BottomToTop or Direction.TopToBottom &&
                eventData.moveDir is MoveDirection.Down or MoveDirection.Up)
            {
                this.value += eventData.moveDir == MoveDirection.Down ? -StepsIncrement : StepsIncrement;
                return;
            }

            base.OnMove(eventData);
        }

    }
}