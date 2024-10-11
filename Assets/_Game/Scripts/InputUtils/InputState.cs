using UnityEngine;

namespace InputUtils
{
    public class InputState
    {
        public Vector2 move;
        public Vector2 look;
        public bool jump;
        public bool sprint;
        public bool fire;
        public bool matchstick;
        public bool escape;
        public bool restart;

        public bool analogMovement; // Don't know what is this for

        public void Reset()
        {
            move = Vector2.zero;
            look = Vector2.zero;
            jump = false;
            sprint = false;
            fire = false;
            matchstick = false;
            escape = false;
            restart = false;
        }
    }
}