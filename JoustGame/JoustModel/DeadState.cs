namespace JoustModel
{
    public class DeadState : IState
    {
        StateMachine stateMachine;
        Ostrich ostrich;

        public DeadState(Ostrich ostrich)
        {
            this.ostrich = ostrich;
            this.stateMachine = ostrich.stateMachine;
        }

        public void Update()
        {
            ostrich.nSpeed = 600;
            ostrich.nAngle = 0;
            if (ostrich.coords.x > 1500)
            {
                stateMachine.Change("spawn");
            }
        }

        public void HandleInput(string command) { }
        public void Enter() { }
        public void Exit() { }

        public override string ToString()
        {
            return "dead";
        }
    }
}