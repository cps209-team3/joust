using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace JoustModel
{
    /// <summary>
    /// This is the Class that all enemies in the game
    /// inherit from getting an EnemyState variable.
    /// </summary>
    public abstract class Enemy : Entity
    {
        public EnemyState state;
        public StateMachine stateMachine;
    }

    /// <summary>
    /// This Class is the base class for all other enemy states. It contains
    /// a Factory Method that produces the next state.
    /// </summary>
    public class EnemyState : IState
    {
        // Properties used in child states
        public int Angle { get; set; }
        public Enemy StateEnemy { get; set; }

        // Class Constructor
        public EnemyState() { }

        /// <summary>
        /// This Factory Method produces the next state for every enemy calling it.
        /// </summary>
        /// <param name="e"> Used to determine what states are available to the enemy </param>
        /// <returns> A new EnemyState Child </returns>
        public static void GetNextState(Enemy e)
        {
            Random rand = new Random(DateTime.Now.Millisecond);
            for (int i = 0; i < 999990; i++) { }
            int chance = rand.Next(100);

            if (e is Buzzard)
            {
                /*** Buzzard States ***/
                Buzzard b = e as Buzzard;

                if (b.stateMachine.currentState is EnemyStandingState)
                {
                    if (chance % 2 == 0)
                    {
                        if (chance % 10 < 5) b.stateMachine.Change("run_right");
                        else b.stateMachine.Change("run_right");
                    }
                    else
                    {
                        b.stateMachine.Change("flap");
                    }
                }
                else if (b.stateMachine.currentState is EnemyRunningState)
                {
                    if (chance < 2)
                    {
                        b.stateMachine.Change("stand");
                    }
                    else if (chance == 99)
                    {
                        EnemyState enemySt = b.stateMachine.currentState as EnemyState;
                        if (enemySt.Angle == 0) b.stateMachine.Change("run_left");
                        else b.stateMachine.Change("run_right");
                    }
                }
                else if (b.stateMachine.currentState is EnemyFlappingState)
                {
                    EnemyState enemySt = b.stateMachine.currentState as EnemyState;
                    switch (enemySt.Angle)
                    {
                        case 90:
                            if (chance / 10 < 2 && chance % 10 < 4 || b.coords.y < 10) b.stateMachine.Change("fall");
                            else if (chance % 2 == 0) b.stateMachine.Change("flap_right");
                            else b.stateMachine.Change("flap_left");
                            break;
                        default:
                            if (chance < 2 || b.coords.y < 10) b.stateMachine.Change("flap");
                            break;
                    }
                }
                else if (b.stateMachine.currentState is EnemyFallingState)
                {
                    EnemyState enemySt = b.stateMachine.currentState as EnemyState;
                    switch (enemySt.Angle)
                    {
                        case 270:
                            if (chance % 10 < 3 || b.coords.y > 730) b.stateMachine.Change("flap");
                            else if (chance % 2 == 0) b.stateMachine.Change("fall_right");
                            else b.stateMachine.Change("fall_left");
                            break;
                        default:
                            if (chance < 3 || b.coords.y > 730) b.stateMachine.Change("fall");
                            break;
                    }
                }
                else if (b.stateMachine.currentState is BuzzardPickupState)
                {
                    BuzzardPickupState set_state = b.stateMachine.currentState as BuzzardPickupState;
                    // Determine if the Buzzard is close enough to the Mik being picked up
                    if ((set_state.TargetEgg.coords.x - b.coords.x) > -5 && (set_state.TargetEgg.coords.x - b.coords.x) < 5 && ((set_state.TargetEgg.coords.y - 50) - b.coords.y) < 5 && ((set_state.TargetEgg.coords.y - 50) - b.coords.y) > -5) {
                        // Picked up Mik
                        set_state.TargetEgg.mounted = true;
                        b.droppedEgg = false;
                        Trace.WriteLine(set_state.TargetEgg.mounted);
                        b.stateMachine.Change("stand");
                    }
                }
                else
                {
                    // If none of the previous states, set state to Fleeing
                    b.stateMachine.Change("flee");
                }
            }
            else if (e is Egg)
            {
                /*** Egg States ***/
                Egg egg = e as Egg;

                if (egg.stateMachine.currentState is EnemyFallingState)
                {
                    // *** Implement when the egg lands on a platform ***
                    if (egg.coords.y > 750) egg.stateMachine.Change("stand");
                }
                else
                {
                    if (egg.seconds > 800)
                    {
                        egg.stateMachine.Change("hatched");
                    }
                    else if (egg.seconds > 600)
                    {
                        egg.seconds++;
                        egg.stateMachine.Change("hatching");
                    }
                    else
                    {
                        egg.seconds++;
                        egg.stateMachine.Change("stand");
                    }
                }
            }
            else if (e is Pterodactyl)
            {
                /*** Pterodactyl States ***/
                Pterodactyl p = e as Pterodactyl;

                // *** Add charging state to attack nearby player ***
                if (p.coords.y > 300 && p.coords.y < 450 && p.coords.x > 600 && p.coords.x < 700) p.stateMachine.Change("charge");

                // *** Add hitbox check to destroy pterodactyl ***
                if (p.coords.y > 450 && p.coords.y < 525 && p.coords.x > 650 && p.coords.x < 800) p.stateMachine.Change("destroyed");

                if (p.stateMachine.currentState is EnemyFallingState)
                {
                    EnemyState enemySt = p.stateMachine.currentState as EnemyState;
                    switch (enemySt.Angle)
                    {
                        case 270:
                            if (chance % 10 < 3 || p.coords.y > 700) p.stateMachine.Change("flap");
                            else if (chance % 2 == 0) p.stateMachine.Change("fall_right");
                            else p.stateMachine.Change("fall_left");
                            break;
                        default:
                            if (chance < 3 || p.coords.y > 700) p.stateMachine.Change("fall");
                            break;
                    }
                }
                else if (p.stateMachine.currentState is EnemyFlappingState)
                {
                    EnemyState enemySt = p.stateMachine.currentState as EnemyState;
                    switch (enemySt.Angle)
                    {
                        case 90:
                            if (chance / 10 < 2 && chance % 10 < 4 || p.coords.y < 10) p.stateMachine.Change("fall");
                            else if (chance % 2 == 0) p.stateMachine.Change("flap_right");
                            else p.stateMachine.Change("flap_left");
                            break;
                        default:
                            if (chance < 2 || p.coords.y < 10) p.stateMachine.Change("flap");
                            break;
                    }
                }
                else if (p.stateMachine.currentState is PterodactylChargeState)
                {
                    if (p.coords.y <= 300 || p.coords.y >= 450 || p.coords.x <= 600 || p.coords.x >= 700)
                    {
                        p.stateMachine.Change("flap");
                    }
                }
            }
        }

        public virtual void Update() { }
        public virtual void HandleInput(string data) { }
        public virtual void Enter() { }
        public virtual void Exit() { }
        public virtual void CheckCollisions() { }
    }




    public class EnemyStandingState : EnemyState
    {
        StateMachine stateMachine;

        public EnemyStandingState(Enemy enemy)
        {
            StateEnemy = enemy;
            stateMachine = StateEnemy.stateMachine;
        }

        public override void Update()
        {
            if (StateEnemy is Buzzard)
            {
                Buzzard b = StateEnemy as Buzzard;
                b.speed = 0;
                b.angle = Angle;
                b.imagePath = "Images/Enemy/mik_" + b.Color + "_stand.png";
            }
            else if (StateEnemy is Egg)
            {
                Egg e = StateEnemy as Egg;
                e.speed = 0;
                e.angle = Angle;
                e.imagePath = "Images/Enemy/egg1.png";
            }
        }

        public override void HandleInput(string command)
        {
            switch (command)
            {
                case "flap":
                case "run_left":
                case "run_right":
                case "stand":
                    stateMachine.Change(command);
                    break;
                default:
                    break;
            }
        }

        public override void Enter() { }
        public override void Exit() { }
    }

    public class EnemyRunningState : EnemyState
    {
        StateMachine stateMachine;

        public EnemyRunningState(Enemy enemy)
        {
            StateEnemy = enemy;
            stateMachine = StateEnemy.stateMachine;
        }

        public override void Update()
        {
            if (StateEnemy is Buzzard)
            {
                Buzzard b = StateEnemy as Buzzard;
                b.angle = Angle;

                if (b.imagePath.EndsWith("stand.png")) b.imagePath = "Images/Enemy/mik_" + b.Color + "_move3.png";
                else if (b.imagePath.EndsWith("move3.png")) b.imagePath = "Images/Enemy/mik_" + b.Color + "_move2.png";
                else if (b.imagePath.EndsWith("move2.png")) b.imagePath = "Images/Enemy/mik_" + b.Color + "_move1.png";
                else if (b.imagePath.EndsWith("move1.png")) b.imagePath = "Images/Enemy/mik_" + b.Color + "_stand.png";
                else b.imagePath = "Images/Enemy/mik_" + b.Color + "_stand.png";
            }
        }

        public override void HandleInput(string command)
        {
            switch (command)
            {
                case "stand":
                case "run_left":
                case "run_right":
                    stateMachine.Change(command);
                    break;
                default:
                    break;
            }
        }

        public override void Enter() { }
        public override void Exit() { }
        public override void CheckCollisions() { }
    }

    public class EnemyFallingState : EnemyState
    {
        StateMachine stateMachine;

        public EnemyFallingState(Enemy enemy)
        {
            StateEnemy = enemy;
            stateMachine = StateEnemy.stateMachine;
        }

        public override void Update()
        {
            if (StateEnemy is Buzzard)
            {
                Buzzard b = StateEnemy as Buzzard;
                b.angle = Angle;
                b.speed += 0.05;
                b.imagePath = "Images/Enemy/mik_" + b.Color + "_fly1.png";
            }
            else if (StateEnemy is Pterodactyl)
            {
                Pterodactyl p = StateEnemy as Pterodactyl;
                p.angle = Angle;
                p.speed += 0.05;
                p.imagePath = "Images/Enemy/pterodactyl_fly1.png";
            }
            else if (StateEnemy is Egg)
            {
                Egg egg = StateEnemy as Egg;
                egg.angle = Angle;
                egg.speed = 5;
                egg.imagePath = "Images/Enemy/egg1.png";
            }
        }

        public override void HandleInput(string command)
        {
            switch (command)
            {
                case "fall":
                case "fall_left":
                case "fall_right":
                case "flap":
                case "stand":
                    stateMachine.Change(command);
                    break;
                default:
                    break;
            }
        }

        public override void Enter() { }
        public override void Exit() { }
    }

    public class EnemyFlappingState : EnemyState
    {
        StateMachine stateMachine;

        public EnemyFlappingState(Enemy enemy)
        {
            StateEnemy = enemy;
            stateMachine = StateEnemy.stateMachine;
        }

        public override void Update()
        {
            if (StateEnemy is Buzzard)
            {
                Buzzard b = StateEnemy as Buzzard;
                b.angle = Angle;
                b.speed += 0.05;

                if (b.imagePath.EndsWith("fly1.png")) b.imagePath = "Images/Enemy/mik_" + b.Color + "_fly2.png";
                else b.imagePath = "Images/Enemy/mik_" + b.Color + "_fly1.png";

            }
            else if (StateEnemy is Pterodactyl)
            {
                Pterodactyl p = StateEnemy as Pterodactyl;
                p.angle = Angle;
                p.speed += 0.05;

                switch (p.imagePath)
                {
                    case "Images/Enemy/pterodactyl_fly1.png":
                        p.imagePath = "Images/Enemy/pterodactyl_fly2.png";
                        break;
                    default:
                        p.imagePath = "Images/Enemy/pterodactyl_fly1.png";
                        break;
                }
            }
        }

        public override void HandleInput(string command)
        {
            switch (command)
            {
                case "fall":
                case "flap_left":
                case "flap_right":
                case "flap":
                    stateMachine.Change(command);
                    break;
                default:
                    break;
            }
        }

        public override void Enter() { }
        public override void Exit() { }
    }

    public class BuzzardFleeingState : EnemyState
    {
        StateMachine stateMachine;

        public BuzzardFleeingState(Enemy enemy)
        {
            StateEnemy = enemy;
            stateMachine = StateEnemy.stateMachine;
        }

        public override void Update()
        {
            if (StateEnemy is Buzzard)
            {
                Buzzard b = StateEnemy as Buzzard;
                b.speed = 5;

                b.angle = 15;

                switch (b.imagePath)
                {
                    case "Images/Enemy/buzzard_fly1.png":
                        b.imagePath = "Images/Enemy/buzzard_fly2.png";
                        break;
                    default:
                        b.imagePath = "Images/Enemy/buzzard_fly1.png";
                        break;
                }
            }
        }

        public override void HandleInput(string command) { }
        public override void Enter() { }
        public override void Exit() { }
    }

    public class BuzzardPickupState : EnemyState
    {
        public Egg TargetEgg { get; set; }
        StateMachine stateMachine;

        public BuzzardPickupState(Enemy enemy)
        {
            StateEnemy = enemy;
            stateMachine = StateEnemy.stateMachine;
        }

        public override void Update()
        {
            Task.Run(() => UpdateEnemy());
        }

        public void UpdateEnemy()
        {
            if (StateEnemy is Buzzard)
            {
                Buzzard b = StateEnemy as Buzzard;
                TargetEgg = b.pickupEgg;

                if (TargetEgg != null)
                {
                    if (b.coords.x < TargetEgg.coords.x)
                    {
                        b.angle = 180;
                        if (b.coords.y < (TargetEgg.coords.y - 50))
                        {
                            b.angle = 195;
                            b.imagePath = "Images/Enemy/buzzard_fly1.png";
                        }
                        else if (b.coords.y > (TargetEgg.coords.y - 50))
                        {
                            b.angle = 150;
                            switch (b.imagePath)
                            {
                                case "Images/Enemy/buzzard_fly1.png":
                                    b.imagePath = "Images/Enemy/buzzard_fly2.png";
                                    break;
                                default:
                                    b.imagePath = "Images/Enemy/buzzard_fly1.png";
                                    break;
                            }
                        }
                        else
                        {
                            b.imagePath = "Images/Enemy/buzzard_fly1.png";
                        }
                    }
                    else if (b.coords.x > TargetEgg.coords.x)
                    {
                        b.angle = 0;
                        if (b.coords.y < (TargetEgg.coords.y - 50))
                        {
                            b.angle = 330;
                            b.imagePath = "Images/Enemy/buzzard_fly1.png";
                        }
                        else if (b.coords.y > (TargetEgg.coords.y - 50))
                        {
                            b.angle = 30;
                            switch (b.imagePath)
                            {
                                case "Images/Enemy/buzzard_fly1.png":
                                    b.imagePath = "Images/Enemy/buzzard_fly2.png";
                                    break;
                                default:
                                    b.imagePath = "Images/Enemy/buzzard_fly1.png";
                                    break;
                            }
                        }
                        else
                        {
                            b.imagePath = "Images/Enemy/buzzard_fly1.png";
                        }
                    }
                }
                else
                {
                    HandleInput("flee");
                }
            }
        }

        public override void HandleInput(string command)
        {
            switch (command)
            {
                case "flee":
                case "stand":
                    stateMachine.Change(command);
                    break;
                default:
                    break;
            }
        }

        public override void Enter() { }
        public override void Exit() { }
    }

    public class EggHatchingState : EnemyState
    {
        StateMachine stateMachine;

        public EggHatchingState(Enemy enemy)
        {
            StateEnemy = enemy;
            stateMachine = StateEnemy.stateMachine;
        }

        public override void Update()
        {
            if (StateEnemy is Egg)
            {
                Egg e = StateEnemy as Egg;
                e.angle = 0;
                e.speed = 0;

                switch (e.imagePath)
                {
                    case "Images/Enemy/egg1.png":
                        e.imagePath = "Images/Enemy/egg" + e.milliseconds + ".png";
                        if (e.milliseconds == 3) e.milliseconds = 2;
                        else e.milliseconds = 3;
                        break;
                    case "Images/Enemy/egg2.png":
                    case "Images/Enemy/egg3.png":
                        e.imagePath = "Images/Enemy/egg1.png";
                        break;
                    default:
                        e.imagePath = "Images/Enemy/egg1.png";
                        break;
                }
            }
        }

        public override void HandleInput(string command) { }
        public override void Enter() { }
        public override void Exit() { }
    }

    public class EggHatchedState : EnemyState
    {
        StateMachine stateMachine;

        public EggHatchedState(Enemy enemy)
        {
            StateEnemy = enemy;
            stateMachine = StateEnemy.stateMachine;
        }

        public override void Update()
        {
            if (StateEnemy is Egg)
            {
                Egg e = StateEnemy as Egg;
                e.angle = 0;
                e.speed = 0;

                e.milliseconds++;
                if (e.milliseconds > 30) e.imagePath = "Images/Enemy/egg6.png";
                else if (e.milliseconds > 15) e.imagePath = "Images/Enemy/egg5.png";
                else e.imagePath = "Images/Enemy/egg4.png";

                if (e.milliseconds > 50) e.milliseconds = 50;
            }
        }

        public override void HandleInput(string command) { }
        public override void Enter() { }
        public override void Exit() { }
    }

    public class PterodactylDestroyedState : EnemyState
    {
        StateMachine stateMachine;

        public PterodactylDestroyedState(Enemy enemy)
        {
            StateEnemy = enemy;
            stateMachine = StateEnemy.stateMachine;
        }

        public override void Update()
        {
            if (StateEnemy is Pterodactyl)
            {
                Pterodactyl p = StateEnemy as Pterodactyl;
                p.angle = 0;
                p.speed = 0;
                switch (p.imagePath)
                {
                    case "Images/Enemy/pterodactyl.die1.png":
                        p.imagePath = "Images/Enemy/pterodactyl_die2.png";
                        break;
                    default:
                        p.imagePath = "Images/Enemy/pterodactyl_die1.png";
                        break;
                }
            }
        }

        public override void HandleInput(string command) { }
        public override void Enter() { }
        public override void Exit() { }
    }

    public class PterodactylChargeState : EnemyState
    {
        StateMachine stateMachine;

        public PterodactylChargeState(Enemy enemy)
        {
            StateEnemy = enemy;
            stateMachine = StateEnemy.stateMachine;
        }

        public override void Update()
        {
            if (StateEnemy is Pterodactyl)
            {
                Pterodactyl p = StateEnemy as Pterodactyl;
                try
                {
                    Point playerCoords = new Point(0, 0);
                    foreach (WorldObject obj in World.Instance.objects)
                    {
                        if (obj is Ostrich)
                        {
                            Ostrich o = obj as Ostrich;
                            playerCoords = o.coords;
                        }
                    }

                    double x = (p.coords.x - playerCoords.x);
                    double y = (p.coords.y - playerCoords.y);
                    p.angle = Math.Atan(x / y);

                    if (x < 0) p.angle += 180;
                    p.imagePath = "Images/Enemy/pterodactyl_charge.png";
                }
                catch (InvalidOperationException op)
                {
                    Trace.WriteLine(op.Message);
                }
            }
        }

        public override void HandleInput(string command) { }
        public override void Enter() { }
        public override void Exit() { }
    }
}