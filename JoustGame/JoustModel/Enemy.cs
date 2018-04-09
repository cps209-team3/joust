using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JoustModel {
    public abstract class Enemy : Entity {
        public EnemyState state;
        public enum Type { Buzzard, MikRed, MikBlue, MikSilver, Egg, Pterodactyl };
    }

    public abstract class EnemyState {
        public int Angle { get; set; }
        public Enemy StateEnemy { get; set; }

        protected Dictionary<Enemy.Type, string> enemy_imgPath = new Dictionary<Enemy.Type, string>();

        public EnemyState() {
            enemy_imgPath.Add(Enemy.Type.Buzzard, "buzzard");
            enemy_imgPath.Add(Enemy.Type.Egg, "egg");
            enemy_imgPath.Add(Enemy.Type.Pterodactyl, "pterodactyl");
            enemy_imgPath.Add(Enemy.Type.MikRed, "mik_red");
            enemy_imgPath.Add(Enemy.Type.MikBlue, "mik_blue");
            enemy_imgPath.Add(Enemy.Type.MikSilver, "mik_silver");
        }

        public static EnemyState GetNextState(Enemy e) {
            Random rand = new Random(DateTime.Now.Millisecond);
            for (int i = 0; i < 999990; i++) { } // Use up time so each Enemy's random updates differently
            int chance = rand.Next(100);

            if (e is Buzzard) {
                Buzzard b = e as Buzzard;

                if (b.coords.y > 450 && b.coords.y < 525 && b.coords.x > 650 && b.coords.x < 800) return new BuzzardFleeingState() { StateEnemy = b };

                if (b.state is EnemyStandingState) {
                    if (chance % 2 == 0) {
                        if (chance % 10 < 5) return new EnemyRunningState() { Angle = 0, StateEnemy = b }; // Running right state
                        else return new EnemyRunningState() { Angle = 180, StateEnemy = b }; // Running left state
                    }
                    else {
                        return new EnemyFlappingState() { Angle = 90, StateEnemy = b }; // Flapping up state
                    }
                }
                else if (b.state is EnemyRunningState) {
                    if (chance < 2) return new EnemyStandingState() { Angle = b.state.Angle, StateEnemy = b }; // Standing state
                    else if (chance < 99) return new EnemyRunningState() { Angle = b.state.Angle, StateEnemy = b }; // Running in same direction state

                    b.state.Angle += 180;
                    if (b.state.Angle > 360) b.state.Angle -= 360;

                    return new EnemyRunningState() { Angle = b.state.Angle, StateEnemy = b }; // Running in opposite direction state
                }
                else if (b.state is EnemyFlappingState) {
                    switch (b.state.Angle) {
                        case 90:
                            if (chance / 10 < 2 && chance % 10 < 4 || b.coords.y < 10) return new EnemyFallingState() { Angle = 270, StateEnemy = b }; // Falling down state
                            else if (chance % 2 == 0) return new EnemyFlappingState() { Angle = 45, StateEnemy = b }; // Flapping right state
                            else return new EnemyFlappingState() { Angle = 135, StateEnemy = b }; // Flapping left state

                        default:
                            if (chance < 2 || b.coords.y < 10) return new EnemyFlappingState() { Angle = 90, StateEnemy = b }; // Flapping up state
                            else return new EnemyFlappingState() { Angle = b.state.Angle, StateEnemy = b };
                    }
                }
                else if (b.state is EnemyFallingState) {
                    switch (b.state.Angle) {
                        case 270:
                            if (chance % 10 < 3 || b.coords.y > 800) return new EnemyFlappingState() { Angle = 90, StateEnemy = b }; // Flapping up state
                            else if (chance % 2 == 0) return new EnemyFallingState() { Angle = 315, StateEnemy = b }; // Falling right state
                            else return new EnemyFallingState() { Angle = 225, StateEnemy = b }; // Falling left state

                        default:
                            if (chance < 3) return new EnemyFallingState() { Angle = 270, StateEnemy = b }; // Falling down state
                            else return new EnemyFallingState() { Angle = b.state.Angle, StateEnemy = b };
                    }
                }
                else if (b.state is BuzzardPickupState) {
                    BuzzardPickupState set_state = b.state as BuzzardPickupState;
                    if ((set_state.TargetEgg.coords.x - b.coords.x) > -5 && (set_state.TargetEgg.coords.x - b.coords.x) < 5 && ((set_state.TargetEgg.coords.y - 50) - b.coords.y) < 5 && ((set_state.TargetEgg.coords.y - 50) - b.coords.y) > -5) {
                        set_state.TargetEgg.mounted = true;
                        return new EnemyStandingState() { StateEnemy = b, Angle = 0 };
                    }
                    else {
                        return new BuzzardPickupState() { StateEnemy = b, Angle = b.state.Angle, TargetEgg = set_state.TargetEgg };
                    }
                }
                else {
                    return new BuzzardFleeingState() { StateEnemy = b };
                }
            }
            else if (e is Egg) {
                Egg egg = e as Egg;

                if (egg.state is EnemyFallingState) {
                    // TODO: implement when the egg lands on a platform
                    if (egg.coords.y > 800) return new EnemyStandingState() { Angle = egg.state.Angle, StateEnemy = egg }; // Standing state
                    else return new EnemyFallingState() { Angle = egg.state.Angle, StateEnemy = egg }; // Falling state
                }
                else {
                    if (egg.seconds > 800) {
                        return new EggHatchedState() { StateEnemy = egg }; // Egg has hatched state
                    }
                    else if (egg.seconds > 600) {
                        egg.seconds++;
                        return new EggHatchingState() { StateEnemy = egg }; // Egg is hatching state
                    }
                    else {
                        egg.seconds++;
                        return new EnemyStandingState() { Angle = 0, StateEnemy = egg }; // Standing state
                    }
                }
            }
            else {
                return new EnemyStandingState() { Angle = 0 };
            }

        }

        public abstract void Setup();
    }


    public class EnemyStandingState : EnemyState {
        public override void Setup() {
            if (StateEnemy is Buzzard) {
                Buzzard b = StateEnemy as Buzzard;
                b.speed = 0;
                b.angle = Angle;
                b.imagePath = "Images/Enemy/" + enemy_imgPath[Enemy.Type.Buzzard] + "_stand.png";
            }
            else if (StateEnemy is Egg) {
                Egg e = StateEnemy as Egg;
                e.speed = 0;
                e.angle = Angle;
                e.imagePath = "Images/Enemy/egg1.png";
            }
        }
    }

    public class EnemyRunningState : EnemyState {
        public override void Setup() {
            if (StateEnemy is Buzzard) {
                Buzzard b = StateEnemy as Buzzard;
                b.angle = Angle;
                switch (b.imagePath) {
                    case "Images/Enemy/mik_red_stand.png":
                        b.imagePath = "Images/Enemy/mik_red_move1.png";
                        break;
                    case "Images/Enemy/mik_red_move1.png":
                        b.imagePath = "Images/Enemy/mik_red_move2.png";
                        break;
                    case "Images/Enemy/mik_red_move2.png":
                        b.imagePath = "Images/Enemy/mik_red_move3.png";
                        break;
                    case "Images/Enemy/mik_red_move3.png":
                        b.imagePath = "Images/Enemy/mik_red_stand.png";
                        break;
                    default:
                        b.imagePath = "Images/Enemy/mik_red_stand.png";
                        break;
                }
            }
        }
    }

    public class EnemyFallingState : EnemyState {
        public override void Setup() {
            if (StateEnemy is Buzzard) {
                Buzzard b = StateEnemy as Buzzard;
                b.angle = Angle;
                b.speed += 0.05;
                b.imagePath = "Images/Enemy/mik_red_fly1.png";
            }
        }
    }

    public class EnemyFlappingState : EnemyState {
        public override void Setup() {
            if (StateEnemy is Buzzard) {
                Buzzard b = StateEnemy as Buzzard;
                b.angle = Angle;
                b.speed += 0.05;
                switch (b.imagePath) {
                    case "Images/Enemy/mik_red_fly1.png":
                        b.imagePath = "Images/Enemy/mik_red_fly2.png";
                        break;
                    default:
                        b.imagePath = "Images/Enemy/mik_red_fly1.png";
                        break;
                }
            }
        }
    }

    public class BuzzardFleeingState : EnemyState {
        public override void Setup() {
            if (StateEnemy is Buzzard) {
                Buzzard b = StateEnemy as Buzzard;
                b.speed = 5;

                b.angle = 15;

                switch (b.imagePath) {
                    case "Images/Enemy/buzzard_fly1.png":
                        b.imagePath = "Images/Enemy/buzzard_fly2.png";
                        break;
                    default:
                        b.imagePath = "Images/Enemy/buzzard_fly1.png";
                        break;
                }
            }
        }
    }

    public class BuzzardPickupState : EnemyState {
        public Egg TargetEgg { get; set; }

        public override void Setup() {
            if (StateEnemy is Buzzard) {
                Buzzard b = StateEnemy as Buzzard;

                Trace.WriteLine("(" + b.coords.x + ", " + b.coords.y + ") -> (" + TargetEgg.coords.x + ", " + TargetEgg.coords.y + ")");

                if (b.coords.x < TargetEgg.coords.x) { 
                    b.angle = 180;
                    if (b.coords.y < (TargetEgg.coords.y - 50)) {
                        b.angle = 195;
                        b.imagePath = "Images/Enemy/buzzard_fly1.png";
                    }
                    else if (b.coords.y > (TargetEgg.coords.y - 50)) {
                        b.angle = 150;
                        switch (b.imagePath) {
                            case "Images/Enemy/buzzard_fly1.png":
                                b.imagePath = "Images/Enemy/buzzard_fly2.png";
                                break;
                            default:
                                b.imagePath = "Images/Enemy/buzzard_fly1.png";
                                break;
                        }
                    }
                    else {
                        b.imagePath = "Images/Enemy/buzzard_fly1.png";
                    }
                }
                else if (b.coords.x > TargetEgg.coords.x) {
                    b.angle = 0;
                    if (b.coords.y < (TargetEgg.coords.y - 50)) {
                        b.angle = 330;
                        b.imagePath = "Images/Enemy/buzzard_fly1.png";
                    }
                    else if (b.coords.y > (TargetEgg.coords.y - 50)) {
                        b.angle = 30;
                        switch (b.imagePath) {
                            case "Images/Enemy/buzzard_fly1.png":
                                b.imagePath = "Images/Enemy/buzzard_fly2.png";
                                break;
                            default:
                                b.imagePath = "Images/Enemy/buzzard_fly1.png";
                                break;
                        }
                    }
                    else {
                        b.imagePath = "Images/Enemy/buzzard_fly1.png";
                    }
                }
            }
        }
    }

    public class EggHatchingState : EnemyState {
        public override void Setup() {
            if (StateEnemy is Egg) {
                Egg e = StateEnemy as Egg;
                e.angle = 0;
                e.speed = 0;

                switch (e.imagePath) {
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
    }

    public class EggHatchedState : EnemyState {
        public override void Setup() {
            if (StateEnemy is Egg) {
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
    }
}