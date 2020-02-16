using System.Collections.Generic;

namespace Snake.Snake
{
	public class Snake
	{
		private const uint START_SNAKE_LENGHT = 4;
		private Diractions direction = Diractions.Left;

		public List<BodyPart> body = new List<BodyPart>((int) START_SNAKE_LENGHT);
		public bool Alive { get; set; } = true;
		public bool AteAllFood { get; set; } = true;

		public Snake(uint fieldSize)
		{
			int xOffset = 0, yOffset = 0;

			switch (direction)
			{
				case Diractions.Up:
					yOffset = 1;
					break;
				case Diractions.Down:
					yOffset = -1;
					break;
				case Diractions.Left:
					xOffset = 1;
					break;
				case Diractions.Right:
					xOffset = -1;
					break;
			}

			for (int i = 0; i < START_SNAKE_LENGHT; i++)
			{
				int BPx = (int) (fieldSize / 2 + xOffset * i - 1);
				int BPy = (int) (fieldSize / 2 + yOffset * i - 1);
				body.Add(new BodyPart(BPx, BPy));
			}
		}

		public bool Update(uint fieldSize, Food food, ref Diractions? newDir)
		{
			BodyPart headCopy = new BodyPart(body[0]);

			Move(ref headCopy, newDir);
			if (CollidedAWall(fieldSize, ref headCopy) || CollidedItself(headCopy))
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
				case Diractions.Up:
					if (direction == Diractions.Down) //Tried to move up while moving down
						goto case Diractions.Down;
					yOffset = -1;
					direction = Diractions.Up;
					break;
				case Diractions.Down:
					if (direction == Diractions.Up) //Tried to move down while moving up
						goto case Diractions.Up;
					yOffset = 1;
					direction = Diractions.Down;
					break;
				case Diractions.Left:
					if (direction == Diractions.Right) //Tried to move left while moving right
						goto case Diractions.Right;
					xOffset = -1;
					direction = Diractions.Left;
					break;
				case Diractions.Right:
					if (direction == Diractions.Left) //Tried to move right while moving left
						goto case Diractions.Left;
					xOffset = 1;
					direction = Diractions.Right;
					break;
			}

			head.move(xOffset, yOffset);
		}

		private bool CollidedAWall(uint fieldSize, ref BodyPart head)
		{
			if(head.x < 0)
				head.move(new BodyPart((int) fieldSize - 1, head.y));
			else if (head.x >= fieldSize)
				head.move(new BodyPart(0, head.y));
			else if (head.y < 0)
				head.move(new BodyPart(head.x, (int) fieldSize - 1));
			else if (head.y >= fieldSize)
				head.move(new BodyPart(head.x, 0));
/*			if (head.x < 0 || head.x >= fieldSize || head.y < 0 || head.y >= fieldSize)
				return true;*/
			return false;
		}

		private bool CollidedItself(BodyPart head)
		{
			for (int i = 4; i < body.Count; i++)
				if (head == body[i])
					return true;
			return false;
		}

		private void MoveSnake(BodyPart newHead)
		{
			for (int i = body.Count - 1; i > 0; i--)
				body[i].move(body[i - 1]);
			body[0].move(newHead);
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
				if (part == food)
					return true;
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
		Up,
		Down,
		Left,
		Right
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