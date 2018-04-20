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
            ostrich.CheckEnemyCollision(ostrich.CheckCollision());
        }
    }
}