using System;

namespace JoustModel
{
    public class StandState : IState
    {
        StateMachine stateMachine;
        Ostrich ostrich;

        public StandState(Ostrich ostrich)
        {
            this.ostrich = ostrich;
            this.stateMachine = ostrich.stateMachine;
        }

        public void Update()
        {
            ostrich.MoveLeftRight();
            ostrich.WrapAround();
        }

        public void HandleInput(string command)
        {
            switch (command)
            {
                case "flap":
                    stateMachine.Change("flap");
                    break;
                case "left":
                    ostrich.nSpeed = 600;
                    ostrich.nAngle = 180;
                    break;
                case "right":
                    ostrich.nSpeed = 600;
                    ostrich.nAngle = 0;
                    break;
                default:
                    break;
            }
        }

        public void Enter() { }
        public void Exit() { }

        public override string ToString()
        {
            return "stand";
        }


        public void CheckCollisions()
        {
            bool collisionDetected = false;
            foreach (WorldObject wo in World.Instance.objects)
            {       // Don't collide with itself! and check for collision
                if (wo.ToString() != ostrich.ToString() && (ostrich.coords.x < wo.coords.x + wo.width && ostrich.coords.x + ostrich.width > wo.coords.x && ostrich.coords.y  < wo.coords.y + wo.height && ostrich.height + ostrich.coords.y + 1 > wo.coords.y))
                {
                    collisionDetected = true;
                }
            }
            if (collisionDetected != true)
            {
                stateMachine.Change("fall"); // no collision
            }
            else
            {
                // do something else
            }    
        }
    }
}