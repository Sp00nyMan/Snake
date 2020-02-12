using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snake.Snake
{
    public class Snake
    {
        private const uint START_SNAKE_LENGHT = 4;
        private Diractions direction = Diractions.left;

        private List<BodyPart> body = new List<BodyPart>((int) START_SNAKE_LENGHT);
        private  bool Alive {get; set; } = true;
        private  bool AteAllFood {get; set; } = true;

        public Snake(uint fielSize)
        {
            int xOffset = 0, yOffset = 0;

            switch(direction)
            {
                case Diractions.up:
                    yOffset = 1;
                    break;
                case Diractions.down:
                    yOffset = -1;
                    break;
                case Diractions.left:
                    xOffset = 1;
                    break;
                case Diractions.right:
                    xOffset = -1;
                    break;
            }

            for (int i = 0; i < START_SNAKE_LENGHT; i++)
            {
                int BPx = (int)(fielSize / 2 + xOffset * i - 1);
                int BPy = (int)(fielSize / 2 + yOffset * i - 1);
                body.Add(new BodyPart( BPx,BPy));
            }
        }

        public bool Update(uint fieldSize, Food food, Diractions? newDir)
        {
            BodyPart headCopy = new BodyPart(body[0]);

            Move(headCopy,newDir);
            if(CollidedAWall(fieldSize, headCopy))
                return Alive = false;
            return Alive;
        }

        private void Move(BodyPart head, Diractions? dir)
        {
            int xOffset = 0, yOffset = 0;
            if (dir == null)
                dir = direction;
            switch (dir)
            {
                case Diractions.up:
                    if(direction == Diractions.down) //Tried to move up while moving down
                        goto case Diractions.down;
                    yOffset = 1;
                    direction = Diractions.up;
                    break;
                case Diractions.down:
                    if (direction == Diractions.up) //Tried to move down while moving up
                        goto case Diractions.up;
                    yOffset = -1;
                    direction = Diractions.down;
                    break;
                case Diractions.left:
                    if (direction == Diractions.right) //Tried to move left while moving right
                        goto case Diractions.right;
                    xOffset = 1;
                    direction = Diractions.left;
                    break;
                case Diractions.right:
                    if (direction == Diractions.left) //Tried to move right while moving left
                        goto case Diractions.left;
                    xOffset = -1;
                    direction = Diractions.right;
                    break;
            }

            head.move(xOffset, yOffset);
        }

        private bool CollidedAWall(uint fieldSize, BodyPart head)
        {
            if(head.x < 0 || head.x >= fieldSize || head.y < 0 || head.y >= fieldSize)
                return true;
            return false;
        }
        private bool CollidedItself(BodyPart head)
        {
            for (int i = 1; i < body.Count; i++)
            {
                
            }
            return false;
        }
    }

    struct BodyPart
    {
        public int x;
        public int y;

        public BodyPart(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        public BodyPart(BodyPart other)
        {
            x = other.x;
            y = other.y;
        }

        public void move(int xOffset, int yOffset)
        {
            x += xOffset;
            y += yOffset;
        }

        public void move(ref BodyPart other)
        {
            x = other.x;
            y = other.y;
        }
    }

    public enum Diractions
    {
        up,
        down,
        left,
        right
    }

    public struct Food
    {

    }
}
