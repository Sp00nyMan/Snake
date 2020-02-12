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

        public List<BodyPart> body = new List<BodyPart>((int) START_SNAKE_LENGHT);
        public  bool Alive {get; set; } = true;
        public  bool AteAllFood {get; set; } = true;

        public Snake(uint fieldSize)
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
                int BPx = (int)(fieldSize / 2 + xOffset * i - 1);
                int BPy = (int)(fieldSize / 2 + yOffset * i - 1);
                body.Add(new BodyPart( BPx,BPy));
            }
        }

        public bool Update(uint fieldSize, Food food, ref Diractions? newDir)
        {
            BodyPart headCopy = new BodyPart(body[0]);

            Move(ref headCopy,newDir);
            if(CollidedAWall(fieldSize, headCopy) || CollidedItself(headCopy))
                return Alive = false;
            MoveSnake(headCopy);
            Expand(headCopy, food);
            if (body.Count == fieldSize * fieldSize)
                return false;
            return Alive;
        }

        private void Move(ref BodyPart head, Diractions? dir)
        {
            int xOffset = 0, yOffset = 0;
            if (dir == null)
                dir = direction;
            switch (dir)
            {
                case Diractions.up:
                    if(direction == Diractions.down) //Tried to move up while moving down
                        goto case Diractions.down;
                    yOffset = -1;
                    direction = Diractions.up;
                    break;
                case Diractions.down:
                    if (direction == Diractions.up) //Tried to move down while moving up
                        goto case Diractions.up;
                    yOffset = 1;
                    direction = Diractions.down;
                    break;
                case Diractions.left:
                    if (direction == Diractions.right) //Tried to move left while moving right
                        goto case Diractions.right;
                    xOffset = -1;
                    direction = Diractions.left;
                    break;
                case Diractions.right:
                    if (direction == Diractions.left) //Tried to move right while moving left
                        goto case Diractions.left;
                    xOffset = 1;
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
                if (head == body[i])
                    return true;
            }
            return false;
        }
        private void MoveSnake(BodyPart newHead)
        {
            for (int i = this.body.Count - 1; i > 0; i--)
                this.body[i].move(body[i - 1]);
            this.body[0].move(newHead);
        }
        private void Expand(BodyPart head, Food food)
        {
            if (head == food)
            {
                AteAllFood = true;
                body.Add(new BodyPart(body[body.Count - 1]));
            }
        }

        public static bool operator ==(Snake snake, Food food)
        {
            foreach (var part in snake.body)
            {
                if (part == food)
                    return true;
            }
            return false;
        }
        public static bool operator !=(Snake snake, Food food)
        {
            return !(snake == food);
        }
    }

    public class BodyPart
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

        public void move(BodyPart other)
        {
            x = other.x;
            y = other.y;
        }

        public static bool operator ==(BodyPart bp1, BodyPart bp2)
        {
            if (bp1.x == bp2.x && bp1.y == bp2.y)
                return true;
            return false;
        }

        public static bool operator !=(BodyPart bp1, BodyPart bp2)
        {
            return !(bp1 == bp2);
        }

        public static bool operator ==(BodyPart part, Food food)
        {
            if (part.x == food.x && part.y == food.y)
                return true;
            return false;
        }
        public static bool operator !=(BodyPart part, Food food)
        {
            return !(part == food);
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
        public int x;
        public int y;

        public Food(int x, int y)
        {
            this.x = x;
            this.y = y;
        }
    }
}
